using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using CallWall.Web;

namespace CallWall.Google.Providers.Contacts
{
    public sealed class GoogleContactProfileTranslator : IGoogleContactProfileTranslator
    {
        private static readonly XmlNamespaceManager _ns;

        static GoogleContactProfileTranslator()
        {
            _ns = new XmlNamespaceManager(new NameTable());
            _ns.AddNamespace("x", "http://www.w3.org/2005/Atom");
            _ns.AddNamespace("openSearch", "http://a9.com/-/spec/opensearch/1.1/");
            _ns.AddNamespace("gContact", "http://schemas.google.com/contact/2008");
            _ns.AddNamespace("batch", "http://schemas.google.com/gdata/batch");
            _ns.AddNamespace("gd", "http://schemas.google.com/g/2005");
        }

        public GoogleContactProfileTranslator()
        {
        }

        public IGoogleContactProfile Translate(string response, string accessToken)
        {
            var xDoc = XDocument.Parse(response);
            if (xDoc.Root == null)
                return null;
            var xContactEntry = xDoc.Root.Element(ToXName("x", "entry"));
            if (xContactEntry == null)
                return null;


            var title = XPathString(xContactEntry, "x:title", _ns);
            var fullName = XPathString(xContactEntry, "gd:name/gd:fullName", _ns);
            var dateOfBirth = xContactEntry.Elements(ToXName("gContact", "birthday"))
                .Select(x => (DateTime?)x.Attribute("when"))
                .FirstOrDefault();

            var avatars = xContactEntry.Elements(ToXName("x", "link"))
                .Where(x => x.Attribute("rel") != null
                            && x.Attribute("rel").Value == "http://schemas.google.com/contacts/2008/rel#photo"
                            && x.Attribute("type") != null
                            && x.Attribute("type").Value == "image/*"
                            && x.Attribute("href") != null)
                .Select(x => x.Attribute("href"))
                .Where(att => att != null)
                .Select(att =>
                            {
                                var hrp = new HttpRequestParameters(att.Value);
                                hrp.QueryStringParameters.Add("access_token", accessToken);
                                return hrp.ConstructUri();
                            });

            //<gd:email rel='http://schemas.google.com/g/2005#home' address='danrowe1978@gmail.com' primary='true'/>
            var emails = from xElement in xContactEntry.XPathSelectElements("gd:email", _ns)
                         select new ContactAssociation(ToContactAssociation(xElement.Attribute("rel")), xElement.Attribute("address").Value);


            //<gd:phoneNumber rel='http://schemas.google.com/g/2005#mobile' uri='tel:+33-6-43-06-76-58' primary='true'>+33  6 4306 7658</gd:phoneNumber>
            var phoneNumbers = from xElement in xContactEntry.XPathSelectElements("gd:phoneNumber", _ns)
                               select new ContactAssociation(ToContactAssociation(xElement.Attribute("rel")), xElement.Value);

            /*<gd:organization rel='http://schemas.google.com/g/2005#work'><gd:orgName>Technip</gd:orgName></gd:organization>*/
            var organizations = from xElement in xContactEntry.XPathSelectElements("gd:organization", _ns)
                                select new ContactAssociation(ToContactAssociation(xElement.Attribute("rel")), xElement.XPathSelectElement("gd:orgName", _ns).Value);


            //<gContact:relation rel='partner'>Anne</gContact:relation>
            var relationships = from xElement in xContactEntry.XPathSelectElements("gContact:relation", _ns)
                                select new ContactAssociation(ToContactAssociation(xElement.Attribute("rel")), xElement.Value);

            var groupUris = from xElement in xContactEntry.XPathSelectElements("gContact:groupMembershipInfo", _ns)
                            let hrefAttribute = xElement.Attribute("href")
                            where hrefAttribute != null
                            select new Uri(hrefAttribute.Value);

            var result = new GoogleContactProfile(title, fullName, dateOfBirth, avatars, emails, phoneNumbers, organizations, relationships, groupUris);
            return result;
        }

        public IGoogleContactProfile AddTags(IGoogleContactProfile contactProfile, string response)
        {
            var xDoc = XDocument.Parse(response);
            var xGroupFeed = xDoc.Root;
            if (xGroupFeed == null)
                return contactProfile;

            var groups =
                (
                    from groupEntry in xGroupFeed.Elements(ToXName("x", "entry"))
                    let id = groupEntry.Element(ToXName("x", "id"))
                    let title = groupEntry.Element(ToXName("x", "title"))
                    where id != null && title != null && !string.IsNullOrWhiteSpace(title.Value)
                    select new { Id = id.Value, Title = title.Value.Replace("System Group: ", string.Empty) }
                ).ToDictionary(g => g.Id, g => g.Title);


            foreach (var groupUri in contactProfile.GroupUris)
            {
                string tag;
                if (groups.TryGetValue(groupUri.ToString(), out tag))
                {
                    contactProfile.AddTag(tag);
                }
            }

            return contactProfile;
        }

        private static XName ToXName(string prefix, string name)
        {
            var xNamespace = _ns.LookupNamespace(prefix);
            if (xNamespace == null)
                throw new InvalidOperationException(prefix + " namespace prefix is not valid");
            return XName.Get(name, xNamespace);
        }

        private static string XPathString(XNode source, string expression, IXmlNamespaceResolver ns)
        {
            var result = source.XPathSelectElement(expression, ns);
            return result == null ? null : result.Value;
        }

        private static string ToContactAssociation(XAttribute relAttribute)
        {
            //Could be a look
            if (relAttribute == null || relAttribute.Value == null)
                return "Other";
            var hashIndex = relAttribute.Value.LastIndexOf("#", StringComparison.Ordinal);
            var association = relAttribute.Value.Substring(hashIndex + 1);
            return PascalCase(association);
        }

        private static string PascalCase(string input)
        {
            var head = input.Substring(0, 1).ToUpperInvariant();
            var tail = input.Substring(1);
            return head + tail;
        }
    }
}
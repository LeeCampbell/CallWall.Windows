using NUnit.Framework;

namespace CallWall.Windows.Google.UnitTests.Providers
{
    public abstract class Given_a_constructed_GoogleContactProfileTranslator
    {
        public sealed class When_Translating_a_fully_populated_response : Given_a_constructed_GoogleContactProfileTranslator
        {
            [Test]
            public void Should_set_the_Title()
            {
                Assert.Inconclusive("Test not yet implemented");
            }
            [Test]
            public void Should_set_the_FullName()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_set_the_DateOfBirth()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_return_contact_image_as_an_Avatar()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_populate_the_emails_collection()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_populate_the_PhoneNumbers_collection()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_populate_the_Organizations_collection()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_populate_the_Relationshelps_collection()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

        }

        
    }

    internal static class ContactResponseFactory
    {
        public static string ErynnesResponse()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<feed xmlns='http://www.w3.org/2005/Atom' xmlns:openSearch='http://a9.com/-/spec/opensearch/1.1/' xmlns:gContact='http://schemas.google.com/contact/2008' xmlns:batch='http://schemas.google.com/gdata/batch' xmlns:gd='http://schemas.google.com/g/2005' gd:etag='W/&quot;D0cARn07eyt7I2A9WhNaGU8.&quot;'>
  <id>lee.ryan.campbell@gmail.com</id>
  <updated>2013-02-03T20:57:27.303Z</updated>
  <category scheme='http://schemas.google.com/g/2005#kind' term='http://schemas.google.com/contact/2008#contact'/>
  <title>Gumble, Lee-Lee the Panda and the Grumpy Guy at Work's Contacts</title>
  <link rel='alternate' type='text/html' href='http://www.google.com/'/>
  <link rel='http://schemas.google.com/g/2005#feed' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full'/>
  <link rel='http://schemas.google.com/g/2005#post' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full'/>
  <link rel='http://schemas.google.com/g/2005#batch' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/batch'/>
  <link rel='self' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full?q=%2B44+7554+257819&amp;max-results=25'/>
  <author>
    <name>Gumble, Lee-Lee the Panda and the Grumpy Guy at Work</name>
    <email>lee.ryan.campbell@gmail.com</email>
  </author>
  <generator version='1.0' uri='http://www.google.com/m8/feeds'>Contacts</generator>
  <openSearch:totalResults>1</openSearch:totalResults>
  <openSearch:startIndex>1</openSearch:startIndex>
  <openSearch:itemsPerPage>25</openSearch:itemsPerPage>
  <entry gd:etag='&quot;Qns-ezVSLyt7I2A9WhNaGEgOQgc.&quot;'>
    <id>http://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/base/aa</id>
    <updated>2013-02-03T01:22:13.553Z</updated>
    <app:edited xmlns:app='http://www.w3.org/2007/app'>2013-02-03T01:22:13.553Z</app:edited>
    <category scheme='http://schemas.google.com/g/2005#kind' term='http://schemas.google.com/contact/2008#contact'/>
    <title>Erynne Campbell</title>
    <link rel='http://schemas.google.com/contacts/2008/rel#photo' type='image/*' href='https://www.google.com/m8/feeds/photos/media/lee.ryan.campbell%40gmail.com/aa' gd:etag='&quot;fTJYO146bCt7I2B2LhQMTS5VNkFDNHh_PE8.&quot;'/>
    <link rel='self' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/aa'/>
    <link rel='edit' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/aa'/>
    <gd:name>
      <gd:fullName>Erynne Campbell</gd:fullName>
      <gd:givenName>Erynne</gd:givenName>
      <gd:familyName>Campbell</gd:familyName>
    </gd:name>
    <gContact:birthday when='1979-04-20'/>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='Erynne.Amm@gmail.com' primary='true'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='Erynne.Campbell@googlemail.com'/>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='Erynne.Campbell@googlemail.com'/>
    <gd:email label='Obsolete' address='erynne.campbell@gmail.com'/>
    <gd:phoneNumber rel='http://schemas.google.com/g/2005#mobile' uri='tel:+61-417-910-632'>+61417910632</gd:phoneNumber>
    <gd:phoneNumber rel='http://schemas.google.com/g/2005#mobile' uri='tel:+44-7554-257819' primary='true'>+447554257819</gd:phoneNumber>
    <gd:structuredPostalAddress rel='http://schemas.google.com/g/2005#home'>
      <gd:formattedAddress>
        6/31 Warwick Rd
        Earl's Court, London SW5 9UL
      </gd:formattedAddress>
      <gd:street>6/31 Warwick Rd</gd:street>
      <gd:postcode>SW5 9UL</gd:postcode>
      <gd:city>Earl's Court</gd:city>
      <gd:region>London</gd:region>
    </gd:structuredPostalAddress>
    <gContact:groupMembershipInfo deleted='false' href='http://www.google.com/m8/feeds/groups/lee.ryan.campbell%40gmail.com/base/6'/>
    <gContact:groupMembershipInfo deleted='false' href='http://www.google.com/m8/feeds/groups/lee.ryan.campbell%40gmail.com/base/e'/>
    <gContact:groupMembershipInfo deleted='false' href='http://www.google.com/m8/feeds/groups/lee.ryan.campbell%40gmail.com/base/3e436b730aeb8cd8'/>
  </entry>
</feed>
";
        }

        //When user is Lee Campbell
        public static string DefaultUserFullResponse()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<feed xmlns='http://www.w3.org/2005/Atom' 
      xmlns:openSearch='http://a9.com/-/spec/opensearch/1.1/' 
      xmlns:gContact='http://schemas.google.com/contact/2008' 
      xmlns:batch='http://schemas.google.com/gdata/batch' 
      xmlns:gd='http://schemas.google.com/g/2005' 
      gd:etag='W/&quot;CEQNQHgyeCt7I2A9WhFSEEs.&quot;'>
  <id>lee.ryan.campbell@gmail.com</id>
  <updated>2013-06-12T18:39:51.690Z</updated>
  <category scheme='http://schemas.google.com/g/2005#kind' term='http://schemas.google.com/contact/2008#contact'/>
  <title>Lee Campbell's Contacts</title>
  <link rel='alternate' type='text/html' href='http://www.google.com/'/>
  <link rel='http://schemas.google.com/g/2005#feed' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full'/>
  <link rel='http://schemas.google.com/g/2005#post' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full'/>
  <link rel='http://schemas.google.com/g/2005#batch' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/batch'/>
  <link rel='self' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full?q=lee.ryan.campbell%40gmail.com&amp;max-results=25'/>
  <author>
    <name>Lee Campbell</name>
    <email>lee.ryan.campbell@gmail.com</email>
  </author>
  <generator version='1.0' uri='http://www.google.com/m8/feeds'>Contacts</generator>
  <openSearch:totalResults>1</openSearch:totalResults>
  <openSearch:startIndex>1</openSearch:startIndex>
  <openSearch:itemsPerPage>25</openSearch:itemsPerPage>
  <entry gd:etag='&quot;RXs7fDdaKyt7I2A9WhFTFEwOQAI.&quot;'>
    <id>http://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/base/2ed8d2af8b2b72ef</id>
    <updated>2013-06-05T06:46:04.504Z</updated>
    <app:edited xmlns:app='http://www.w3.org/2007/app'>2013-06-05T06:46:04.504Z</app:edited>
    <category scheme='http://schemas.google.com/g/2005#kind' term='http://schemas.google.com/contact/2008#contact'/>
    <title>Lee Campbell</title>
    <link rel='http://schemas.google.com/contacts/2008/rel#photo' type='image/*' href='https://www.google.com/m8/feeds/photos/media/lee.ryan.campbell%40gmail.com/2ed8d2af8b2b72ef' gd:etag='&quot;TmhdYGwyWit7I2BUORMhFi9XEGNIKHp6YjI.&quot;'/>
    <link rel='self' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/2ed8d2af8b2b72ef'/>
    <link rel='edit' type='application/atom+xml' href='https://www.google.com/m8/feeds/contacts/lee.ryan.campbell%40gmail.com/full/2ed8d2af8b2b72ef'/>
    <gd:name>
      <gd:fullName>Lee Campbell</gd:fullName>
      <gd:givenName>Lee</gd:givenName>
      <gd:familyName>Campbell</gd:familyName>
    </gd:name>
    <gContact:birthday when='1979-12-27'/>
    <gd:organization primary='true' rel='http://schemas.google.com/g/2005#other'>
      <gd:orgName>Lab 49</gd:orgName>
    </gd:organization>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='lee.ryan.campbell@gmail.com' primary='true'/>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='leeryancampbell@gmail.com'/>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='leec@artemiswest.com'/>
    <gd:email rel='http://schemas.google.com/g/2005#other' address='lee.campbell@sgcib.com'/>
    <gd:phoneNumber rel='http://schemas.google.com/g/2005#work'>07584148498</gd:phoneNumber>
    <gd:structuredPostalAddress rel='http://schemas.google.com/g/2005#home'>
      <gd:formattedAddress>
        6/31 warwick road
        Earl's Court
        London SW5 9UL
        Uk
      </gd:formattedAddress>
      <gd:street>
        6/31 warwick road
        Earl's Court
      </gd:street>
      <gd:postcode>SW5 9UL</gd:postcode>
      <gd:city>London</gd:city>
      <gd:country>Uk</gd:country>
    </gd:structuredPostalAddress>
    <gContact:website href='http://leecampbell.blogspot.com' primary='true' rel='home'/>
    <gContact:groupMembershipInfo deleted='false' href='http://www.google.com/m8/feeds/groups/lee.ryan.campbell%40gmail.com/base/6'/>
  </entry>
</feed>
";
        }
    }
}

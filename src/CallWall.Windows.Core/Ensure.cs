using System;
using System.IO.Packaging;

namespace CallWall.Windows
{
    public static class Ensure
    {
        public static void PackUriIsRegistered()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                //UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
                var _ = PackUriHelper.UriSchemePack;    //NoOp to make the PackUriHelper type run its static ctor/cctor.
            }
        }
    }
}
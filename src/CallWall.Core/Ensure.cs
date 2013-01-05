using System;

namespace CallWall
{
    public static class Ensure
    {
        public static void PackUriIsRegistered()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace CallWall.Google.Authorization
{
    //TODO: Add DebugDisplay to Session -LC
    public sealed class Session : ISession
    {
        private readonly string _accessToken;
        private readonly string _refreshToken;
        private readonly DateTimeOffset _expires;
        private readonly ISet<Uri> _authorizedResources;

        public Session(string accessToken, string refreshToken, TimeSpan accessPeriod, DateTimeOffset requested, IEnumerable<Uri> authorizedResources )
            :this(accessToken, refreshToken, requested + accessPeriod, authorizedResources)
        {
        }

        public Session(string accessToken, string refreshToken, DateTimeOffset expires, IEnumerable<Uri> authorizedResources)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _expires = expires;
            _authorizedResources = new HashSet<Uri>(authorizedResources);
        }

        public string AccessToken { get { return _accessToken; } }

        public string RefreshToken { get { return _refreshToken; } }

        public DateTimeOffset Expires { get { return _expires; } }

        public bool HasExpired()
        {
            return DateTimeOffset.Now > _expires;
        }

        public ISet<Uri> AuthorizedResources
        {
            get { return _authorizedResources; }
        }

        //TODO: Add AuthorizedUris to the ToString
        public override string ToString()
        {
            return string.Format("Session {{ AccessToken : '{0}', RefreshToken : '{1}', Expires : '{2:o}'}}", AccessToken, RefreshToken, Expires);
        }
    }
}
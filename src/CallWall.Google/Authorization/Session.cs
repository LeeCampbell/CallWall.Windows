using System;

namespace CallWall.Google.Authorization
{
    public interface ISession
    {
        string AccessToken { get; }
        string RefreshToken { get; }
        DateTimeOffset Expires { get; }
        bool HasExpired();
    }

    public sealed class Session : ISession
    {
        private readonly string _accessToken;
        private readonly string _refreshToken;
        private readonly DateTimeOffset _expires;

        public Session(string accessToken, string refreshToken, TimeSpan accessPeriod, DateTimeOffset requested)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _expires = requested + accessPeriod;
        }

        public Session(string accessToken, string refreshToken, DateTimeOffset expires)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _expires = expires;
        }

        public string AccessToken { get { return _accessToken; } }

        public string RefreshToken { get { return _refreshToken; } }

        public DateTimeOffset Expires { get { return _expires; } }

        public bool HasExpired()
        {
            return DateTimeOffset.Now > _expires;
        }

        public override string ToString()
        {
            return string.Format("Session {{ AccessToken : '{0}', RefreshToken : '{1}', Expires : '{2:o}'}}", AccessToken, RefreshToken, Expires);
        }
    }
}
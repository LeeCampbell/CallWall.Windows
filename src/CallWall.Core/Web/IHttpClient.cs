using System;

namespace CallWall.Web
{
    public interface IHttpClient
    {
        IObservable<string> GetResponse(HttpRequestParameters request);
    }
}
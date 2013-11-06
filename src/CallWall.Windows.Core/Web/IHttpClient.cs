using System;

namespace CallWall.Windows.Web
{
    public interface IHttpClient
    {
        IObservable<string> GetResponse(HttpRequestParameters request);
    }
}
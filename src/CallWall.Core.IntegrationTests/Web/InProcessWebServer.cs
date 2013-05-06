using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace CallWall.Core.IntegrationTests.Web
{
    //http://www.asp.net/web-api/overview/hosting-aspnet-web-api/self-host-a-web-api
    internal sealed class InProcessWebServer : IDisposable
    {
        private HttpSelfHostServer _server;
        public void Start(string baseAddress)
        {
            var config = new HttpSelfHostConfiguration(baseAddress);

            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            _server = new HttpSelfHostServer(config);

            _server.OpenAsync().Wait();
        }

        public void Dispose()
        {
            if (_server != null)
                _server.Dispose();
        }
    }
}

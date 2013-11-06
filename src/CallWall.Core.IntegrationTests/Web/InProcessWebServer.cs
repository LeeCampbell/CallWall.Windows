using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace CallWall.Core.IntegrationTests.Web
{
    //http://www.asp.net/web-api/overview/hosting-aspnet-web-api/self-host-a-web-api
    /*
     * The key part from this is you need to either run VS in elevated mode, or run a command.
     * 
     * 
     *      (Optional) Add an HTTP URL Namespace Reservation
     *
     *      This application listens to http://localhost:8080/. By default, listening at a particular HTTP address requires administrator privileges. When you run the tutorial, therefore, you may get this error: "HTTP could not register URL http://+:8080/" There are two ways to avoid this error:
     *
     *      Run Visual Studio with elevated administrator permissions, or
     *      Use Netsh.exe to give your account permissions to reserve the URL.
     *      To use Netsh.exe, open a command prompt with administrator privileges and enter the following command:following command:
     *
     *      netsh http add urlacl url=http://+:8080/ user=machine\username
     *      where machine\username is your user account.
     *
     *      When you are finished self-hosting, be sure to delete the reservation:
     *
     *      netsh http delete urlacl url=http://+:8080/
     */
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

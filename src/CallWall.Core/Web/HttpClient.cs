using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace CallWall.Web
{
    public sealed class HttpClient : IHttpClient
    {
        private readonly ILogger _logger;

        public HttpClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger();
        }

        public IObservable<string> GetResponse(HttpRequestParameters requestParameters)
        {
            return Observable.Create<string>(
                o =>
                {
                    var request = requestParameters.CreateRequest(_logger);
                    var q = from response in request.GetResponseAsync().ToObservable()
                            let responseStream = response.GetResponseStream()
                            let reader = new StreamReader(responseStream)
                            from responseContent in reader.ReadToEndAsync().ToObservable()
                            select responseContent;

                    return q.Subscribe(o);
                });
        }
    }
}
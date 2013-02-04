using System;
using System.IO;
using System.Linq;
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
                    //TODO: Add a correlationId to this logging.
                    _logger.Trace(requestParameters.ToString());
                    var request = requestParameters.CreateRequest(_logger);
                    var q = from response in request.GetResponseAsync()
                                .ToObservable()
                                .Do(r=>_logger.Trace(r.Headers.ToString()))
                            let responseStream = response.GetResponseStream()
                            let reader = new StreamReader(responseStream)
                            from responseContent in reader.ReadToEndAsync().ToObservable()
                            select responseContent;

                    return q.Do(r=>_logger.Trace(r))
                            .Subscribe(o);
                });
        }
    }
}
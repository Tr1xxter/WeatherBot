using System.Net;
using Newtonsoft.Json;
using Vostok.Logging.Abstractions;

namespace WeatherBot.Domain.Web.Helpers;

public static class WebHelper
{
    public static T? MakeRequest<T>(string uri, ILog log)
    {
        try
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
            var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();

            string response;
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(response);
        }
        catch (Exception exception)
        {
            log.Error(exception);
            return default;
        }
    }
}

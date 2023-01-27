using System.Net;
using Newtonsoft.Json;

namespace WeatherBot.Domain.Weather.Helpers;

public class WebHelper
{
    public static T? MakeRequest<T>(string url)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        string response;
        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
        }

        return JsonConvert.DeserializeObject<T>(response);
    }
}

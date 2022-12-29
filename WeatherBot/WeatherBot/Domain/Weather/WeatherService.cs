using System.Net;
using System.Runtime.InteropServices.Marshalling;
using WeatherBot.Configuration;

namespace WeatherBot.Domain.Weather;

public class WeatherService
{
    private readonly SecretsConfig _secretsConfig;

    public WeatherService(SecretsConfig secretsConfig)
    {
        _secretsConfig = secretsConfig;
    }

    private string countryCode = "ISO 3166-2:RU";
    private string cityName = "Yekaterinburg";
    private string limit = "5";
    private string urlCoord =>
        $"http://api.openweathermap.org/geo/1.0/direct?q={cityName},{countryCode}&limit={limit}&appid={_secretsConfig.WeatherApiKey}";

    private HttpWebRequest httpWebRequest => (HttpWebRequest)WebRequest.Create(urlCoord);
     HttpWebResponse httpWebResponse => (HttpWebResponse)httpWebRequest.GetResponse();

    private string response;

    public void GetWeather()
    {
        using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
            Console.WriteLine(response);
        }
    }
    
    //TODO: посмотреть видео с 4:30 и сделать возврат response нормальным 
    
    //TODO: пока получаю только координаты, надо еще все сделать после для самой погоды по аналогии 
    
    //TODO: сделать отдельный класс координаты и класс города для получения координаты 


    //private string urlWeather = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={ApiKey}";
}
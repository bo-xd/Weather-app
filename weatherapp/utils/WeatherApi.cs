using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace weatherapp.utils
{
    internal class WeatherApi
    {
        private static string link = "https://api.weatherapi.com/v1/forecast.json?key=";
        private static string apikey = "d55f85261308460abf574709251709";

        public class WeatherInfo
        {
            public string temp_c { get; set; }
            public string temp_f { get; set; }
        }

        public static async Task<WeatherInfo?> GetWeatherInfo()
        {
            var weatherlocation = await LocationApi.GetLocationAsync();
            var location = "&q=" + weatherlocation.City;

            var url = link + apikey + location;

            var response = await RequestLib.SendRequest(url);
            if (response == null)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            // Parse Weather info
            var tempC = RequestLib.parsejsonDouble(json, "current.temp_c").ToString();
            var tempF = RequestLib.parsejsonDouble(json, "current.temp_f").ToString();

            return new WeatherInfo
            {
                temp_c = tempC,
                temp_f = tempF
            };
        }
    }
}
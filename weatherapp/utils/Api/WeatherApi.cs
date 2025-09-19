using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace weatherapp.utils.Api
{
    public class WeatherApi
    {
        private static string link = "https://api.weatherapi.com/v1/forecast.json?key=";
        private static string apikey = "d55f85261308460abf574709251709";

        public class WeatherInfo
        {
            public string temp_c { get; set; }
            public string temp_f { get; set; }
            public string condition { get; set; }
            public string iconUrl { get; set; }
            public string location { get; set; }
            public double maxtemp_c { get; set; }
            public double mintemp_c { get; set; }
            public double maxtemp_f { get; set; }
            public double mintemp_f { get; set; }
            public List<HourlyForecast> hourlyForecast { get; set; }
        }

        public class HourlyForecast
        {
            public string time { get; set; }
            public double temp_c { get; set; }
            public double temp_f { get; set; }
            public string condition { get; set; }
            public string iconUrl { get; set; }
            public int chance_of_rain { get; set; }
        }

        public static async Task<WeatherInfo?> GetWeatherInfo()
        {
            var weatherlocation = await LocationApi.GetLocation();
            var location = "&q=" + weatherlocation.City;

            var url = link + apikey + location + "&days=2";

            var response = await RequestUtil.SendRequest(url);
            if (response == null)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var tempC = JsonUtil.parsejson<double>(json, "current.temp_c").ToString();
            var tempF = JsonUtil.parsejson<double>(json, "current.temp_f").ToString();
            var condition = JsonUtil.parsejson<string>(json, "current.condition.text");
            var iconUrl = JsonUtil.parsejson<string>(json, "current.condition.icon");
            var locationName = JsonUtil.parsejson<string>(json, "location.name");

            var maxTempC = JsonUtil.parsejson<double>(json, "forecast.forecastday.0.day.maxtemp_c");
            var minTempC = JsonUtil.parsejson<double>(json, "forecast.forecastday.0.day.mintemp_c");
            var maxTempF = JsonUtil.parsejson<double>(json, "forecast.forecastday.0.day.maxtemp_f");
            var minTempF = JsonUtil.parsejson<double>(json, "forecast.forecastday.0.day.mintemp_f");

            var hourlyForecast = new List<HourlyForecast>();
            var currentHour = DateTime.Now.Hour;
            var hoursAdded = 0;
            
            System.Diagnostics.Debug.WriteLine($"WeatherApi: Current hour is {currentHour}");
            
            for (int i = currentHour; i < 24 && hoursAdded < 6; i++)
            {
                try
                {
                    var hourTime = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.time");
                    var hourTempC = JsonUtil.parsejson<double>(json, $"forecast.forecastday.0.hour.{i}.temp_c");
                    var hourTempF = JsonUtil.parsejson<double>(json, $"forecast.forecastday.0.hour.{i}.temp_f");
                    var hourCondition = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.condition.text");
                    var hourIconUrl = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.condition.icon");
                    var chanceOfRain = JsonUtil.parsejson<int>(json, $"forecast.forecastday.0.hour.{i}.chance_of_rain");

                    hourlyForecast.Add(new HourlyForecast
                    {
                        time = hourTime,
                        temp_c = hourTempC,
                        temp_f = hourTempF,
                        condition = hourCondition,
                        iconUrl = hourIconUrl,
                        chance_of_rain = chanceOfRain
                    });
                    hoursAdded++;
                    System.Diagnostics.Debug.WriteLine($"WeatherApi: Added today hour {i}, total: {hoursAdded}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error getting today hour {i}: {ex.Message}");
                    break;
                }
            }

            if (hoursAdded < 6)
            {
                var remainingHours = 6 - hoursAdded;
                System.Diagnostics.Debug.WriteLine($"WeatherApi: Need {remainingHours} more hours from tomorrow");
                
                for (int i = 0; i < remainingHours && i < 24; i++)
                {
                    try
                    {
                        var hourTime = JsonUtil.parsejson<string>(json, $"forecast.forecastday.1.hour.{i}.time");
                        var hourTempC = JsonUtil.parsejson<double>(json, $"forecast.forecastday.1.hour.{i}.temp_c");
                        var hourTempF = JsonUtil.parsejson<double>(json, $"forecast.forecastday.1.hour.{i}.temp_f");
                        var hourCondition = JsonUtil.parsejson<string>(json, $"forecast.forecastday.1.hour.{i}.condition.text");
                        var hourIconUrl = JsonUtil.parsejson<string>(json, $"forecast.forecastday.1.hour.{i}.condition.icon");
                        var chanceOfRain = JsonUtil.parsejson<int>(json, $"forecast.forecastday.1.hour.{i}.chance_of_rain");

                        hourlyForecast.Add(new HourlyForecast
                        {
                            time = hourTime,
                            temp_c = hourTempC,
                            temp_f = hourTempF,
                            condition = hourCondition,
                            iconUrl = hourIconUrl,
                            chance_of_rain = chanceOfRain
                        });
                        hoursAdded++;
                        System.Diagnostics.Debug.WriteLine($"WeatherApi: Added tomorrow hour {i}, total: {hoursAdded}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error getting tomorrow hour {i}: {ex.Message}");
                        break;
                    }
                }
            }

            if (hourlyForecast.Count < 3)
            {
                System.Diagnostics.Debug.WriteLine($"WeatherApi: Not enough hours ({hourlyForecast.Count}), using backup strategy");
                hourlyForecast.Clear();
                
                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        var hourTime = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.time");
                        var hourTempC = JsonUtil.parsejson<double>(json, $"forecast.forecastday.0.hour.{i}.temp_c");
                        var hourTempF = JsonUtil.parsejson<double>(json, $"forecast.forecastday.0.hour.{i}.temp_f");
                        var hourCondition = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.condition.text");
                        var hourIconUrl = JsonUtil.parsejson<string>(json, $"forecast.forecastday.0.hour.{i}.condition.icon");
                        var chanceOfRain = JsonUtil.parsejson<int>(json, $"forecast.forecastday.0.hour.{i}.chance_of_rain");

                        hourlyForecast.Add(new HourlyForecast
                        {
                            time = hourTime,
                            temp_c = hourTempC,
                            temp_f = hourTempF,
                            condition = hourCondition,
                            iconUrl = hourIconUrl,
                            chance_of_rain = chanceOfRain
                        });
                        System.Diagnostics.Debug.WriteLine($"WeatherApi: Added backup hour {i}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error getting backup hour {i}: {ex.Message}");
                        break;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"WeatherApi: Retrieved {hourlyForecast.Count} hourly forecast items");

            return new WeatherInfo
            {
                temp_c = tempC,
                temp_f = tempF,
                condition = condition,
                iconUrl = iconUrl,
                location = locationName,
                maxtemp_c = maxTempC,
                mintemp_c = minTempC,
                maxtemp_f = maxTempF,
                mintemp_f = minTempF,
                hourlyForecast = hourlyForecast
            };
        }
    }
}
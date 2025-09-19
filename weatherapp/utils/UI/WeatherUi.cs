using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherapp.utils.UI
{
    public class WeatherUi
    {
        public static string GetWeatherIconPath(string condition, string IconUrl = "")
        {
            var lowerCondition = condition?.ToLower() ?? "";
            bool isNight = IconUrl.Contains("/night/");
            return lowerCondition switch
            {
                var con when con.Contains("sunny") || con.Contains("clear") => isNight ? "/Resources/CloudIcons/clearnight.png" : "/Resources/CloudIcons/Clear.png",
                var con when con.Contains("partly cloudy") || con.Contains("partially cloudy") => isNight ? "/Resources/CloudIcons/partycloudynight.png" : "/Resources/CloudIcons/partlycloudy.png",
                var con when con.Contains("cloudy") || con.Contains("overcast") => "/Resources/CloudIcons/Cloudy.png",
                var con when con.Contains("rain") || con.Contains("shower") || con.Contains("moderate rain") || con.Contains("heavy rain") => "/Resources/CloudIcons/Rain.png",
                var con when con.Contains("drizzle") || con.Contains("light rain") || con.Contains("patchy rain nearby") || con.Contains("light drizzle") => isNight ? "/Resources/CloudIcons/Drizzlenight.png" : "/Resources/CloudIcons/Drizzle.png",
                var con when con.Contains("heavy snow") || con.Contains("blizzard") => "/Resources/CloudIcons/Heavysnow.png",
                var con when con.Contains("snow") || con.Contains("light snow") || con.Contains("moderate snow") => "/Resources/CloudIcons/Snow.png",
                var con when con.Contains("thunderstorm") || con.Contains("thunder") || con.Contains("thundery") => "/Resources/CloudIcons/Thunderstorm.png",
                var con when con.Contains("fog") || con.Contains("mist") => "/Resources/CloudIcons/Fog.png",
                var con when con.Contains("haze") => "/Resources/CloudIcons/Haze.png",
                var con when con.Contains("freezing") || con.Contains("sleet") || con.Contains("ice pellets") => "/Resources/CloudIcons/Freezingrain.png",
                var con when con.Contains("windy") || con.Contains("wind") => "/Resources/CloudIcons/Windy.png",
                _ => isNight ? "/Resources/CloudIcons/clearnight.png" : "/Resources/CloudIcons/Clear.png"
            };
        }

        public static string GetWeatherSummary(string condition)
        {
            return condition.ToLower() switch
            {
                var con when con.Contains("sunny") || con.Contains("clear") => "Sunny conditions will continue throughout the day. Perfect weather for outdoor activities.",
                var con when con.Contains("partly cloudy") || con.Contains("partially cloudy") => "Partly cloudy skies with intervals of sunshine.",
                var con when con.Contains("cloudy") || con.Contains("overcast") => "Cloudy skies expected with comfortable temperatures.",
                var con when con.Contains("rain") || con.Contains("shower") => "Rain is expected. Consider bringing an umbrella if going outside.",
                var con when con.Contains("drizzle") || con.Contains("light rain") => "Light rain or drizzle possible. You may want a light jacket.",
                var con when con.Contains("heavy rain") || con.Contains("moderate rain") => "Heavy rain expected. Take precautions for possible flooding.",
                var con when con.Contains("snow") || con.Contains("light snow") || con.Contains("moderate snow") => "Snow conditions expected. Drive carefully and dress warmly.",
                var con when con.Contains("heavy snow") || con.Contains("blizzard") => "Heavy snow or blizzard conditions. Travel may be hazardous.",
                var con when con.Contains("thunderstorm") || con.Contains("thunder") || con.Contains("thundery") => "Thunderstorms possible. Stay indoors during severe weather.",
                var con when con.Contains("fog") || con.Contains("mist") => "Visibility may be reduced due to foggy conditions.",
                var con when con.Contains("haze") => "Hazy conditions may affect visibility and air quality.",
                var con when con.Contains("freezing") || con.Contains("sleet") || con.Contains("ice pellets") => "Freezing rain or sleet possible. Surfaces may be slippery.",
                var con when con.Contains("windy") || con.Contains("wind") => "Windy conditions expected. Secure loose objects outdoors.",
                _ => $"{condition} conditions expected for today."
            };
        }

        public static string setWeatherBg(string condition)
        {
            return condition.ToLower() switch
            {
                var con when con.Contains("Sunny") || con.Contains("clear") => "/Resources/Weatherbg/sky.png",
                var con when con.Contains("heavy rain") || con.Contains("thunderstorm") || con.Contains("heavy rain") => "/Resources/Weatherbg/Thunder.jpg",
                _ => "/Resources/weatherbg/Cloudy.png"
            };
        }

    }
}

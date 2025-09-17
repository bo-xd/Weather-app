using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace weatherapp.utils
{
    internal class LocationApi
    {
        public class LocationResult
        {
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
        }

        public static async Task<LocationResult?> GetLocationAsync()
        {
            var response = await RequestLib.SendRequest("http://ip-api.com/json");
            var json = await response.Content.ReadAsStringAsync();

            // Parse Location info
            var City = RequestLib.parsejsonString(json, "city");
            var Region = RequestLib.parsejsonString(json, "regionName");
            var Country = RequestLib.parsejsonString(json, "country");
            var Lat = RequestLib.parsejsonDouble(json, "lat");
            var Lon = RequestLib.parsejsonDouble(json, "lon");

            return new LocationResult
            {
                City = City,
                Region = Region,
                Country = Country,
                Lat = Lat,
                Lon = Lon
            };
        }
    }
}

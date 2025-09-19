using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using weatherapp.utils.Api;

namespace weatherapp.utils
{
    public class LocationApi
    {
        public class LocationResult
        {
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
        }

        public static async Task<LocationResult?> GetLocation()
        {
            var response = await RequestUtil.SendRequest("https://api.ipbase.com/v1/json/");
            var json = await response.Content.ReadAsStringAsync();

            var City = JsonUtil.parsejson<string>(json, "city");
            var Region = JsonUtil.parsejson<string>(json, "region_name");
            var Country = JsonUtil.parsejson<string>(json, "country_name");
            var Lat = JsonUtil.parsejson<double>(json, "latitude");
            var Lon = JsonUtil.parsejson<double>(json, "longitude");

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
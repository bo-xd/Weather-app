using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace weatherapp.utils
{
    internal class RequestLib
    {
        public static async Task<HttpResponseMessage> SendRequest(string link)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(link);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return response;
        }

        public static JsonElement parsejson(string json, string property)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;
            foreach (var part in property.Split('.'))
            {
                if (element.TryGetProperty(part, out var next))
                {
                    element = next;
                }
                else
                {
                    throw new KeyNotFoundException($"Property '{property}' not found in JSON.");
                }
            }
            return element;
        }

        public static string parsejsonString(string json, string property)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;
            foreach (var part in property.Split('.'))
            {
                if (element.TryGetProperty(part, out var next))
                {
                    element = next;
                }
                else
                {
                    throw new KeyNotFoundException($"Property '{property}' not found in JSON.");
                }
            }
            return element.GetString();
        }

        public static double parsejsonDouble(string json, string property)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;
            foreach (var part in property.Split('.'))
            {
                if (element.TryGetProperty(part, out var next))
                {
                    element = next;
                }
                else
                {
                    throw new KeyNotFoundException($"Property '{property}' not found in JSON.");
                }
            }
            return element.GetDouble();
        }
    }
}

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
    public class RequestUtil
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
    }
}

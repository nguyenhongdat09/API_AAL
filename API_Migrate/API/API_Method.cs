using API_Migrate.Config;
using API_Migrate.Model;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace API_Migrate.API
{
    internal class API_Method
    {
        public API_Method() {
            
        }
        private  Logging logging = new Logging();
        private string controller;
        public Logging Logging { get => logging; set => logging = value; }
        public string Controller { get => controller; 
            set { 
                controller = value;
                logging.Controller = value;
            } 
        }
        public Configuration Config { get => config; set => config = value; }

        private Configuration config = new SetConfig().Config.Configuration;
        public virtual async Task<string> PostAsync(string url, object data, string token = "")
        { 
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = JsonConvert.SerializeObject(data);
            this.Logging.Description = $"URL post: {url}";
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                { 
                    logging.Description = "Status code: " +  response.StatusCode.ToString();
                }
                return "";
            }
           
        }

        public virtual async Task<string> PostAsyncFormUrlEncodedContent(string url, Dictionary<string, string> formData, string token = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var content = new FormUrlEncodedContent(formData);
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    logging.Description = "Status code: " + response.StatusCode.ToString();
                    return "";
                }
            }
        }


        public virtual async Task<string> GetAsyncWithNoParams(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this.Logging.Description = $"URL Get: {url}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    logging.Description = "Status code: " + response.StatusCode.ToString();
                }
                return "";
            }
        }
        public virtual async Task<string> GetAsyncWithParams(string url, Dictionary<string, string> parameters)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Tạo query string từ dictionary
            if (parameters != null && parameters.Count > 0)
            {
                var queryString = string.Join("&", parameters.Select(p =>
                    $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
                url += (url.Contains("?") ? "&" : "?") + queryString;
            }
            this.Logging.Description = $"URL: {url}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    logging.Description = "Status code: " + response.StatusCode.ToString();
                }
                return "";
            }
            /*
             Example
             var parameters = new Dictionary<string, string>
                {
                    { "ma_tinh", "HCM" },
                    { "loai", "2" },
                    { "active", "true" }
                };
                string response = await GetAsyncWithParams("https://api.example.com/data", parameters);
             */
        }


    }
}

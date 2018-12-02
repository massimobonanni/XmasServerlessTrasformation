using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SCP.Core.DTO;

namespace SCP.Client.APIService
{
    public class ApiClient : IApiClient
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }

        private const string RetrieveChildrenInfoFunctionUrl = "/api/Children";

        public async Task<ChildDto> GetChildByIdAsync(string childId)
        {
            var baseUrl = $"{this.ApiUrl}{RetrieveChildrenInfoFunctionUrl}";
            var uri = CreateFunctionUrl(baseUrl, this.ApiKey, $"idChild={childId}");
            var children = await GetObjectsAsync<ChildDto>(uri);
            if (children != null)
                return children.FirstOrDefault();
            throw new Exception("Si E' verificato un errore");
        }

        private string CreateFunctionUrl(string baseUrl, string key, string queryString = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (string.IsNullOrWhiteSpace(queryString))
                    return baseUrl;
                else
                    return $"{baseUrl}?{queryString}";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(queryString))
                    return $"{baseUrl}?code={key}";
                else
                    return $"{baseUrl}?code={key}&{queryString}";
            }
        }

        protected static async Task<IEnumerable<T>> GetObjectsAsync<T>(string apiUrl) where T : new()
        {
            IEnumerable<T> result = null;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
                }
            }
            return result;
        }
    }
}

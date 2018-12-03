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
        private const string EvaluateChildFunctionUrl = "/api/EvaluateChild";

        public async Task<(ApiClientResult, ChildDto)> GetChildByIdAsync(string childId)
        {
            var baseUrl = $"{this.ApiUrl}{RetrieveChildrenInfoFunctionUrl}";
            var uri = CreateFunctionUrl(baseUrl, this.ApiKey, $"idChild={childId}");
            var apiResult = await GetObjectsAsync<ChildDto>(uri);

            return (apiResult.Item1, apiResult.Item2?.FirstOrDefault());
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

        protected async Task<(ApiClientResult, IEnumerable<T>)> GetObjectsAsync<T>(string apiUrl) where T : new()
        {
            var result = ApiClientResult.OK;
            IEnumerable<T> items = null;
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        items = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result = ApiClientResult.NotFound;
                    }
                    else
                    {
                        result = ApiClientResult.GenericError;
                    }
                }
                catch (Exception ex)
                {
                    result = ApiClientResult.GenericError;
                }
            }
            return (result, items);
        }

        protected async Task<ApiClientResult> PostObjectAsync<T>(string apiUrl, T objectToPost)
        {
            ApiClientResult result = ApiClientResult.OK;
            using (var client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(objectToPost));
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        result = ApiClientResult.OK;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result = ApiClientResult.NotFound;
                    }
                    else
                    {
                        result = ApiClientResult.GenericError;
                    }
                }
                catch (Exception ex)
                {
                    result = ApiClientResult.GenericError;
                }
            }
            return result;
        }

        public Task<ApiClientResult> SubmitEvaluationAsync(string childId, string firstName, string lastName, int goodness)
        {
            var baseUrl = $"{this.ApiUrl}{EvaluateChildFunctionUrl}";
            var uri = CreateFunctionUrl(baseUrl, this.ApiKey);
            var evaluationDto = new ChildEvaluationDto()
            {
                ChildId = childId,
                Goodness = goodness,
                ChildFirstName = firstName,
                ChildLastName = lastName
            };
            return this.PostObjectAsync(uri, evaluationDto);
        }
    }
}

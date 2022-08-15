using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync(string url, int id, string Token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _httpClientFactory.CreateClient();
            if (Token != null && Token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }

            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string Token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            if (Token != null && Token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }

            return null;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate, string Token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate),
                    Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _httpClientFactory.CreateClient();

            if (Token != null && Token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token = "");
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, string Token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate),
                    Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }   
            var client = _httpClientFactory.CreateClient();
            if (Token != null && Token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int Id, string Token)
        {
            var newUrl = url + Id.ToString();
            var request = new HttpRequestMessage(HttpMethod.Delete, newUrl);
            var client = _httpClientFactory.CreateClient();
            if (Token != null && Token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }
    }
}

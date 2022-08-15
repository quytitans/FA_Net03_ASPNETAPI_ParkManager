using System.Net;
using System.Text;
using Newtonsoft.Json;
using Parky2Web.Models;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User> LoginAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate),
                    Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
               var jsonstring = await response.Content.ReadAsStringAsync();
               return JsonConvert.DeserializeObject<User>(jsonstring);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User objToCreate)
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
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

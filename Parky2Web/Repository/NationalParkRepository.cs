using Parky2Web.Models;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}

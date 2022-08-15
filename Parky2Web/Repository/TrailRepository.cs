using Parky2Web.Models;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}

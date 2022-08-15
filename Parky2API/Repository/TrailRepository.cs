using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Parky2API.Data;
using Parky2API.Model;
using Parky2API.Model.Dtos;
using Parky2API.Repository.IRepository;

namespace Parky2API.Repository
{

    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(s => s.NationalPark).OrderBy(s => s.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int id)
        {
            return _db.Trails.Include(s => s.NationalPark).Where(s => s.Id == id).ToList();
        }

        public Trail GetTrail(int id)
        {
            return _db.Trails.Include(s => s.NationalPark).FirstOrDefault(s => s.Id == id);
        }

        public bool TrailExists(string trailName)
        {
            bool value = _db.Trails.Any(s => s.Name.ToLower().Trim() == trailName.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(s => s.Id == id);
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

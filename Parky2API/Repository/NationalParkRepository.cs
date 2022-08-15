using System.Net.Mime;
using Parky2API.Data;
using Parky2API.Model;
using Parky2API.Repository.IRepository;

namespace Parky2API.Repository
{

    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext context)     
        {
            _db = context;
        }
        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalPark.OrderBy(s=>s.Name).ToList();
        }

        public NationalPark GetNationalPark(int id)
        {
          return  _db.NationalPark.FirstOrDefault(s => s.Id == id);
        }

        public bool NationalParkExists(string parkName)
        {
            bool value = _db.NationalPark.Any(s => s.Name.ToLower().Trim() == parkName.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalPark.Any(s => s.Id == id);
        }

        public bool CreateNationalPark(NationalPark park)
        {
           _db.NationalPark.Add(park);
           return Save();
        }

        public bool UpdateNationalPark(NationalPark park)
        {
            _db.NationalPark.Update(park);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark park)
        {
            _db.NationalPark.Remove(park);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >=  0 ? true : false;
        }
    }
}

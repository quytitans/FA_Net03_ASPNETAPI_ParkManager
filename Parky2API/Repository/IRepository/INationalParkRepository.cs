using Parky2API.Model;
using System.Diagnostics;

namespace Parky2API.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int id);
        bool NationalParkExists(string parkName);
        bool NationalParkExists(int id);
        bool CreateNationalPark(NationalPark park);
        bool UpdateNationalPark(NationalPark park);
        bool DeleteNationalPark(NationalPark park);
        bool Save();
    }
}

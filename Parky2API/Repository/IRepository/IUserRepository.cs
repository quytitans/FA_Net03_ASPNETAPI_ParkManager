using Parky2API.Model;

namespace Parky2API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Autheticate(string username, string password);
        User Register(string username, string password);
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parky2API.Data;
using Parky2API.Model;
using Parky2API.Repository.IRepository;

namespace Parky2API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }


        public User Autheticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            //GENERATE TOKEN START
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)

                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            //GENERATE TOKEN END

            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(s => s.UserName == username);
            if (user == null)
            {
                return true;
            }

            return false;
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                UserName = username,
                Password = password,
                Role = "Admin"
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";

            return userObj;
        }
    }
}

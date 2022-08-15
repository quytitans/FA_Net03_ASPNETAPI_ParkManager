using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky2API.Model;
using Parky2API.Repository.IRepository;

namespace Parky2API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AutheticationModel model)
        {
            var user = _userRepository.Autheticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AutheticationModel model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var user = _userRepository.Register(model.UserName, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }
    }


}

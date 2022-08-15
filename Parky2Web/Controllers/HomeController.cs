using Microsoft.AspNetCore.Mvc;
using Parky2Web.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Parky2Web.Models.ViewModel;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _parkRepository;
        private readonly IAccountRepository _accRepository;
        private readonly ITrailRepository _trailRepository;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository parkRepository, ITrailRepository trailRepository, IAccountRepository accRepository)
        {
            _parkRepository = parkRepository;
            _logger = logger;
            _trailRepository = trailRepository;
            _accRepository = accRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listOfParksAndTrails = new IndexVM()
            {
                NationalParkList = await _parkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWTToken")),
                TrailsList = await _trailRepository.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWTToken"))
            };
            return View(listOfParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            if (objUser == null)
            {
                return View();

            };

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWTToken", objUser.Token);
            TempData["alert"] = "Welcome " + objUser.UserName;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepository.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (!result)
            {
                return View();

            };
            TempData["alert"] = "Register Successfull";

            return RedirectToAction("Login");
        }

        public async Task<IActionResult>  Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWTToken", "");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
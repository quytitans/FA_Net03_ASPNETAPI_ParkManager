using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parky2Web.Models;
using Parky2Web.Models.ViewModel;
using Parky2Web.Repository.IRepository;

namespace Parky2Web.Controllers
{
    [Authorize]
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailController(INationalParkRepository iNationalParkRepository, ITrailRepository trailRepo)
        {
            _npRepo = iNationalParkRepository;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id) 
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWTToken"));
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Trail = new Trail(),
            };

            if (id == null)
            {
                //flow for insert
                return View(objVM);
            }

            //flow for update
            objVM.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWTToken"));
            if (objVM.Trail == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, obj.Trail, HttpContext.Session.GetString("JWTToken"));
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWTToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWTToken"));
                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWTToken")) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id, HttpContext.Session.GetString("JWTToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete success-full" });
            }
            return Json(new { success = false, message = "Delete not success-full" });
        }
    }
}

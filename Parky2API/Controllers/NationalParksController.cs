using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky2API.Model;
using Parky2API.Repository.IRepository;

namespace Parky2API.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "NationalPark")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository _nationalParkRepository, IMapper iMapper)
        {
            _npRepo = _nationalParkRepository;
            _mapper = iMapper;
        }


        /// <summary>
        /// get list of national park
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        //[Authorize]
        public IActionResult GetNationalParks()
        {
            var objlist = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach (var item in objlist)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(item));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// get indivisual national park
        /// </summary>
        /// <param name="id">Id of the national park</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        //[Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _npRepo.GetNationalPark(id);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists !!!");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark",new{version = HttpContext.GetRequestedApiVersion().ToString(), id = nationalParkObj.Id}, nationalParkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }

            var nationalParkObj = _npRepo.GetNationalPark(id);
            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

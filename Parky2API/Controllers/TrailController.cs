using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky2API.Model;
using Parky2API.Model.Dtos;
using Parky2API.Repository.IRepository;

namespace Parky2API.Controllers
{
    [Route("api/v{version:apiVersion}/trails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "Trails")]
    public class TrailController : ControllerBase
    {
        private ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepository, IMapper iMapper)
        {
            _mapper = iMapper;
            _trailRepository = trailRepository;
        }


        /// <summary>
        /// get list of trail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objlist = _trailRepository.GetTrails();
            var objDto = new List<TrailDto>();

            foreach (var item in objlist)
            {
                objDto.Add(_mapper.Map<TrailDto>(item));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// get indivisual trail
        /// </summary>
        /// <param name="id">Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int id)
        {
            var obj = _trailRepository.GetTrail(id);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        [HttpGet("[action]/{id:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInnationalPark(int id)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(id);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add( _mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists !!!");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id = trailObj.Id }, trailObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || id != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailRepository.TrailExists(id))
            {
                return NotFound();
            }

            var trailObj = _trailRepository.GetTrail(id);
            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Some thing went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

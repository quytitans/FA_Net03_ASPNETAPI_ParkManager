using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky2API.Model;
using Parky2API.Repository.IRepository;

namespace Parky2API.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "NationalPark")]
    public class NationalParksV2Controller : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository _nationalParkRepository, IMapper iMapper)
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
        public IActionResult GetNationalParks()
        {
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }
    }
}

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultipleProject.Controllers
{
    [Route("api/[controller]")]
    public class SourceInfosController : Controller
    {
        private readonly BusinessProvider _businessProvider;

        public SourceInfosController(BusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SourceInfoVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _businessProvider.GetSourceInfos());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SourceInfoVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            if (!await _businessProvider.ExistsSourceInfo(id))
            {
                return NotFound($"SourceInfoVm with Id {id} does not exist");
            }

            return Ok(await _businessProvider.GetSourceInfo(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SourceInfoVm value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (value.SourceInfoId > 0 && await _businessProvider.ExistsSourceInfo(value.SourceInfoId))
            {
                return BadRequest($"SourceInfo with Id {value.SourceInfoId} exists");
            }

            var result = _businessProvider.CreateSourceInfo(value);

            return Created("/api/SourceInfo", result);
        }
    }
}

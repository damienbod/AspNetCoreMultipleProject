using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultipleProject.Controllers
{
    [Route("api/[controller]")]
    public class SourceInfosController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public SourceInfosController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SourceInfoVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _dataAccessProvider.GetSourceInfos(false);

            var results = data.Select(si => new SourceInfoVm
            {
                Timestamp = si.Timestamp,
                Description = si.Description,
                Name = si.Name,
                SourceInfoId = si.SourceInfoId
            });

            return Ok(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SourceInfoVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            var si = await _dataAccessProvider.GetDataEventRecord(id);
            var result = new SourceInfoVm
            {
                Timestamp = si.Timestamp,
                Description = si.Description,
                Name = si.Name,
                SourceInfoId = si.SourceInfoId
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SourceInfoVm value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var sourceInfo = new SourceInfo
            {
                Timestamp = value.Timestamp,
                Description = value.Description,
                Name = value.Name,
                SourceInfoId = value.SourceInfoId
            };

            var si = await _dataAccessProvider.AddSourceInfo(sourceInfo);

            var result = new SourceInfoVm
            {
                Timestamp = si.Timestamp,
                Description = si.Description,
                Name = si.Name,
                SourceInfoId = si.SourceInfoId
            };

            return Created("/api/SourceInfo", result);
        }

        //[HttpGet("all/{withChildren}")]
        //[ProducesResponseType(typeof(IEnumerable<SourceInfoVm>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetSourceInfos(bool withChildren)
        //{
        //    var data = await _dataAccessProvider.GetSourceInfos(withChildren);

        //    var results = data.Select(si => new SourceInfoVm
        //    {
        //        Timestamp = si.Timestamp,
        //        Description = si.Description,
        //        Name = si.Name,
        //        SourceInfoId = si.SourceInfoId
        //    });

        //    return Ok(results);
        //}
    }
}

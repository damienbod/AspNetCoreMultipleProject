using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetCoreMultipleProject.Controllers
{
    [Route("api/[controller]")]
    public class DataEventRecordsController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public DataEventRecordsController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DataEventRecord>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dataAccessProvider.GetDataEventRecords());
        }

        [HttpGet]
        [Route("SourceInfos")]
        [ProducesResponseType(typeof(IEnumerable<SourceInfo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSourceInfos(bool withChildren)
        {
            return Ok(await _dataAccessProvider.GetSourceInfos(withChildren));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DataEventRecord), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            return Ok(await _dataAccessProvider.GetDataEventRecord(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DataEventRecord value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (value.SourceInfo == null && value.SourceInfoId == 0)
            {
                return BadRequest();
            }

            await _dataAccessProvider.AddDataEventRecord(value);
            return Created("/api/DataEventRecord", value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody]DataEventRecord value)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            await _dataAccessProvider.UpdateDataEventRecord(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            await _dataAccessProvider.DeleteDataEventRecord(id);

            return Ok();
            
        }
    }
}

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultipleProject.Controllers
{
    [Route("api/[controller]")]
    public class DataEventRecordsController : Controller
    {
        private readonly BusinessProvider _businessProvider;

        public DataEventRecordsController(BusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DataEventRecordVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _businessProvider.GetDataEventRecords());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DataEventRecordVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            if (!await _businessProvider.ExistsDataEventRecord(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            return Ok(await _businessProvider.GetDataEventRecordById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DataEventRecordVm value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (value.SourceInfo == null && value.SourceInfoId == 0)
            {
                return BadRequest();
            }

            var result = await _businessProvider.CreateDataEventRecord(value);

            return Created("/api/DataEventRecord", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody] DataEventRecordVm value)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (!await _businessProvider.ExistsDataEventRecord(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            await _businessProvider.UpdateDataEventRecord(id, value);
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

            if (!await _businessProvider.ExistsDataEventRecord(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            await _businessProvider.DeleteDataEventRecord(id);

            return Ok();
        }
    }
}

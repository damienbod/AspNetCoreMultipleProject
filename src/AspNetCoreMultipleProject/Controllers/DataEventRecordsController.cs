using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType(typeof(IEnumerable<DataEventRecordVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var data = await _dataAccessProvider.GetDataEventRecords();

            var results = data.Select(der => new DataEventRecordVm
            {
                Timestamp = der.Timestamp,
                Description = der.Description,
                Name = der.Name,
                SourceInfoId = der.SourceInfoId,
                DataEventRecordId = der.DataEventRecordId,
                SourceInfo = new SourceInfoVm
                {
                    Description = der.SourceInfo.Description,
                    Name = der.SourceInfo.Name,
                    SourceInfoId = der.SourceInfo.SourceInfoId,
                    Timestamp = der.SourceInfo.Timestamp
                },
            });

            return Ok(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DataEventRecordVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            var der = await _dataAccessProvider.GetDataEventRecord(id);

            var result = new DataEventRecordVm
            {
                Timestamp = der.Timestamp,
                Description = der.Description,
                Name = der.Name,
                SourceInfoId = der.SourceInfoId,
                DataEventRecordId = der.DataEventRecordId,
                SourceInfo = new SourceInfoVm
                {
                    Description = der.SourceInfo.Description,
                    Name = der.SourceInfo.Name,
                    SourceInfoId = der.SourceInfo.SourceInfoId,
                    Timestamp = der.SourceInfo.Timestamp
                },
            };

            return Ok(result);
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

            var dataEventRecord = new DataEventRecord
            {
                Timestamp = value.Timestamp,
                Description = value.Description,
                Name = value.Name,
                DataEventRecordId = value.DataEventRecordId,
                SourceInfoId = value.SourceInfoId
            };

            if (value.SourceInfo != null)
            {
                dataEventRecord.SourceInfo = new SourceInfo
                {
                    Description = value.SourceInfo.Description,
                    Name = value.SourceInfo.Name,
                    SourceInfoId = value.SourceInfo.SourceInfoId,
                    Timestamp = value.SourceInfo.Timestamp
                };
            }

            var der = await _dataAccessProvider.AddDataEventRecord(dataEventRecord);

            var result = new DataEventRecordVm
            {
                Timestamp = der.Timestamp,
                Description = der.Description,
                Name = der.Name,
                SourceInfoId = der.SourceInfoId,
                DataEventRecordId = der.DataEventRecordId,
                SourceInfo = new SourceInfoVm
                {
                    Description = der.SourceInfo.Description,
                    Name = der.SourceInfo.Name,
                    SourceInfoId = der.SourceInfo.SourceInfoId,
                    Timestamp = der.SourceInfo.Timestamp
                },
            };

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

            if (!await _dataAccessProvider.DataEventRecordExists(id))
            {
                return NotFound($"DataEventRecord with Id {id} does not exist");
            }

            var dataEventRecord = new DataEventRecord
            {
                Timestamp = value.Timestamp,
                Description = value.Description,
                Name = value.Name,
                DataEventRecordId = value.DataEventRecordId,
                SourceInfoId = value.SourceInfoId
            };

            if (value.SourceInfo != null)
            {
                dataEventRecord.SourceInfo = new SourceInfo
                {
                    Description = value.SourceInfo.Description,
                    Name = value.SourceInfo.Name,
                    SourceInfoId = value.SourceInfo.SourceInfoId,
                    Timestamp = value.SourceInfo.Timestamp
                };
            }

            await _dataAccessProvider.UpdateDataEventRecord(id, dataEventRecord);
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

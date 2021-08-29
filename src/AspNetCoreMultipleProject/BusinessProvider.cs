using DomainModel;
using DomainModel.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMultipleProject
{
    public class BusinessProvider
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public BusinessProvider(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        public async Task<IEnumerable<DataEventRecordVm>> GetDataEventRecords()
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

            return results;
        }

        public async Task<bool> ExistsDataEventRecord(long id)
        {
            return await _dataAccessProvider.DataEventRecordExists(id);
        }
        
        public async Task<DataEventRecordVm> GetDataEventRecordById(long id)
        {
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

            return result;
        }

        public async Task<DataEventRecordVm> CreateDataEventRecord(DataEventRecordVm value)
        {
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
                }
            };

            return result;
        }

        public async Task UpdateDataEventRecord(long id, DataEventRecordVm value)
        {
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
        }

        public async Task DeleteDataEventRecord(long id)
        {
            await _dataAccessProvider.DeleteDataEventRecord(id);
        }
    }
}
using DomainModel;
using DomainModel.Model;
using System;
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

        public async Task<IEnumerable<SourceInfoVm>> GetSourceInfos()
        {
            var data = await _dataAccessProvider.GetSourceInfos(false);

            var results = data.Select(si => new SourceInfoVm
            {
                Timestamp = si.Timestamp,
                Description = si.Description,
                Name = si.Name,
                SourceInfoId = si.SourceInfoId
            });

            return results;
        }

        public async Task<bool> ExistsSourceInfo(long id)
        {
            return await _dataAccessProvider.SourceInfoExists(id);
        }

        public async Task<SourceInfoVm> GetSourceInfo(long id)
        {
            var si = await _dataAccessProvider.GetDataEventRecord(id);
            var result = new SourceInfoVm
            {
                Timestamp = si.Timestamp,
                Description = si.Description,
                Name = si.Name,
                SourceInfoId = si.SourceInfoId
            };

            return result;
        }

        public async Task<SourceInfoVm> CreateSourceInfo(SourceInfoVm value)
        {
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

            return result;
        }
    }
}
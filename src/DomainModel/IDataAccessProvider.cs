using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModel.Model;

namespace DomainModel
{
    public interface IDataAccessProvider
    {
        void AddDataEventRecord(DataEventRecord dataEventRecord);
        void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord);
        Task DeleteDataEventRecord(long dataEventRecordId);
        Task<DataEventRecord> GetDataEventRecord(long dataEventRecordId);
        Task<List<DataEventRecord>> GetDataEventRecords();
        Task<List<SourceInfo>> GetSourceInfos(bool withChildren);
        Task<bool> DataEventRecordExists(long id);
    }
}

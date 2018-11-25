using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModel.Model;

namespace DomainModel
{
    public interface IDataAccessProvider
    {
        void AddDataEventRecord(DataEventRecord dataEventRecord);
        void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord);
        void DeleteDataEventRecord(long dataEventRecordId);
        DataEventRecord GetDataEventRecord(long dataEventRecordId);
        Task<List<DataEventRecord>> GetDataEventRecords();
        List<SourceInfo> GetSourceInfos(bool withChildren);       
    }
}

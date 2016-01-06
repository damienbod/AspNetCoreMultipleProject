namespace DomainModel
{
    using System.Collections.Generic;

    using DomainModel.Model;

    public interface IDataAccessProvider
    {
        void AddDataEventRecord(DataEventRecord dataEventRecord);

        void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord);

        void DeleteDataEventRecord(long dataEventRecordId);

        DataEventRecord GetDataEventRecord(long dataEventRecordId);

        List<DataEventRecord> GetDataEventRecords(bool withChildren);
    }
}

namespace DomainModel
{
    using System.Collections.Generic;

    using DomainModel.Model;

    public interface IDataAccessProvider
    {
        void AddDataEventRecord(DataEventRecord dataEventRecord);

        void UpdateDataEventRecord(DataEventRecord dataEventRecord);

        void DeleteDataEventRecord(DataEventRecord dataEventRecord);

        DataEventRecord GetDataEventRecord(int dataEventRecordId);

        List<DataEventRecord> GetDataEventRecords(bool withChildren);
    }
}

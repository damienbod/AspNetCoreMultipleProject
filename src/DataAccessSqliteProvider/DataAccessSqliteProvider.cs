using DomainModel;

namespace DataAccessSqliteProvider
{
    using System.Collections.Generic;

    using DomainModel.Model;
    using Microsoft.Extensions.Logging;
    public class DataAccessSqliteProvider : IDataAccessProvider
    {
        private readonly DomainModelSqliteContext _context;
        private readonly ILogger _logger;

        public DataAccessSqliteProvider(DomainModelSqliteContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("DataAccessSqliteProvider");
        }

        public void AddDataEventRecord(DataEventRecord dataEventRecord)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateDataEventRecord(DataEventRecord dataEventRecord)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteDataEventRecord(DataEventRecord dataEventRecord)
        {
            throw new System.NotImplementedException();
        }

        public DataEventRecord GetDataEventRecord(int dataEventRecordId)
        {
            throw new System.NotImplementedException();
        }

        public List<DataEventRecord> GetDataEventRecords(bool withChildren)
        {
            throw new System.NotImplementedException();
        }
    }
}

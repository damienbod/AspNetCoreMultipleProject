using DomainModel;

namespace DataAccessSqliteProvider
{
    using System.Collections.Generic;
    using System.Linq;

    using DomainModel.Model;

    using Microsoft.Data.Entity;
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

        public void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteDataEventRecord(long dataEventRecordId)
        {
            throw new System.NotImplementedException();
        }

        public DataEventRecord GetDataEventRecord(long dataEventRecordId)
        {
            throw new System.NotImplementedException();
        }

        public List<DataEventRecord> GetDataEventRecords(bool withChildren)
        {
            if (withChildren)
            {
                return _context.DataEventRecords.Include(s => s.SourceInfo).ToList();
            }
            
            return _context.DataEventRecords.ToList();
        }
    }
}

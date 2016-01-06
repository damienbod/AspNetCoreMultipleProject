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
            _context.DataEventRecords.Add(dataEventRecord);
            _context.SaveChanges();
        }

        public void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord)
        {
            _context.DataEventRecords.Update(dataEventRecord);
            _context.SaveChanges();
        }

        public void DeleteDataEventRecord(long dataEventRecordId)
        {
            var entity = _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
            _context.DataEventRecords.Remove(entity);
            _context.SaveChanges();
        }

        public DataEventRecord GetDataEventRecord(long dataEventRecordId)
        {
            return _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
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

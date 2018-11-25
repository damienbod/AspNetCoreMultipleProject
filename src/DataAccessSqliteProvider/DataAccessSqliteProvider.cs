using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DomainModel;
using DomainModel.Model;
using System;
using System.Threading.Tasks;

namespace DataAccessSqliteProvider
{
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
            if (dataEventRecord.SourceInfo != null && dataEventRecord.SourceInfoId == 0)
            {
                _context.SourceInfos.Add(dataEventRecord.SourceInfo);
            }
            else
            {
                var sourceInfo = _context.SourceInfos.Find(dataEventRecord.SourceInfo.SourceInfoId);
                sourceInfo.Description = dataEventRecord.Description;
                sourceInfo.Name = dataEventRecord.Name;
                dataEventRecord.SourceInfo = sourceInfo;
            }

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

        public async Task<List<DataEventRecord>> GetDataEventRecords()
        {
            // Using the shadow property EF.Property<DateTime>(dataEventRecord)
            return await _context.DataEventRecords.OrderByDescending(dataEventRecord => EF.Property<DateTime>(dataEventRecord, "UpdatedTimestamp")).ToListAsync();
        }

        public List<SourceInfo> GetSourceInfos(bool withChildren)
        {
            // Using the shadow property EF.Property<DateTime>(srcInfo)
            if (withChildren)
            {
                return _context.SourceInfos.Include(s => s.DataEventRecords).OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToList();
            }

            return _context.SourceInfos.OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToList();
        }
    }
}

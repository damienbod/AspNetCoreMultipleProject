using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessMySqlProvider
{
    public class DataAccessMySqlProvider : IDataAccessProvider
    {
        private readonly DomainModelMySqlContext _context;
        private readonly ILogger _logger;

        public DataAccessMySqlProvider(DomainModelMySqlContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("DataAccessMySqlProvider");
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

        public List<DataEventRecord> GetDataEventRecords()
        {
            // Using the shadow property EF.Property<DateTime>(dataEventRecord)
            return _context.DataEventRecords.OrderByDescending(dataEventRecord => EF.Property<DateTime>(dataEventRecord, "UpdatedTimestamp")).ToList();
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

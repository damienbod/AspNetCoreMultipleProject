using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessMsSqlServerProvider
{
    public class DataAccessMsSqlServerProvider : IDataAccessProvider
    {
        private readonly DomainModelMsSqlServerContext _context;
        private readonly ILogger _logger;

        public DataAccessMsSqlServerProvider(DomainModelMsSqlServerContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("DataAccessMsSqlServerProvider");

        }

        public void AddDataEventRecord(DataEventRecord dataEventRecord)
        {
            if (dataEventRecord.SourceInfo != null && dataEventRecord.SourceInfoId == 0)
            {
                _context.SourceInfos.Add(dataEventRecord.SourceInfo);
                _context.SaveChanges();
                dataEventRecord.SourceInfoId = dataEventRecord.SourceInfo.SourceInfoId;
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

        public async Task DeleteDataEventRecord(long dataEventRecordId)
        {
            var entity = _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
            _context.DataEventRecords.Remove(entity);
            await _context.SaveChangesAsync();
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

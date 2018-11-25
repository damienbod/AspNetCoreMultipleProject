using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessPostgreSqlProvider
{
    public class DataAccessPostgreSqlProvider : IDataAccessProvider
    {
        private readonly DomainModelPostgreSqlContext _context;
        private readonly ILogger _logger;

        public DataAccessPostgreSqlProvider(DomainModelPostgreSqlContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("DataAccessPostgreSqlProvider");
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

        public async Task<List<SourceInfo>> GetSourceInfos(bool withChildren)
        {
            // Using the shadow property EF.Property<DateTime>(srcInfo)
            if (withChildren)
            {
                return await _context.SourceInfos.Include(s => s.DataEventRecords).OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToListAsync();
            }

            return await _context.SourceInfos.OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToListAsync();
        }

        public async Task<bool> DataEventRecordExists(long id)
        {
            var filteredDataEventRecords = _context.DataEventRecords
                .Where(item => item.DataEventRecordId == id);

            return await filteredDataEventRecords.AnyAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<DataEventRecord> AddDataEventRecord(DataEventRecord dataEventRecord)
        {
            if (dataEventRecord.SourceInfo != null && dataEventRecord.SourceInfoId == 0)
            {
                _context.SourceInfos.Add(dataEventRecord.SourceInfo);
            }
            else
            {
                var sourceInfo = _context.SourceInfos.Find(dataEventRecord.SourceInfo.SourceInfoId);
                dataEventRecord.SourceInfo = sourceInfo;
            }

            _context.DataEventRecords.Add(dataEventRecord);
            await _context.SaveChangesAsync();
            return dataEventRecord;
        }
        public async Task UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord)
        {
            _context.DataEventRecords.Update(dataEventRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataEventRecord(long dataEventRecordId)
        {
            var entity = _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
            _context.DataEventRecords.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<DataEventRecord> GetDataEventRecord(long dataEventRecordId)
        {
            return await _context.DataEventRecords
                .Include(s => s.SourceInfo)
                .FirstAsync(t => t.DataEventRecordId == dataEventRecordId);
        }

        public async Task<List<DataEventRecord>> GetDataEventRecords()
        {
            // Using the shadow property EF.Property<DateTime>(dataEventRecord)
            return await _context.DataEventRecords
                .Include(s => s.SourceInfo)
                .OrderByDescending(dataEventRecord => EF.Property<DateTime>(dataEventRecord, "UpdatedTimestamp"))
                .ToListAsync();
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

        public async Task<SourceInfo> AddSourceInfo(SourceInfo sourceInfo)
        {
            _context.SourceInfos.Add(sourceInfo);
            await _context.SaveChangesAsync();
            return sourceInfo;
        }

        public async Task<bool> SourceInfoExists(long id)
        {
            var filteredSourceInfoRecords = _context.SourceInfos
                .Where(item => item.SourceInfoId == id);

            return await filteredSourceInfoRecords.AnyAsync();
        }
    }
}

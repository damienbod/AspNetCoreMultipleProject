using System;
using Newtonsoft.Json;

namespace DomainModel.Model
{
    public class DataEventRecord
    {
        public long DataEventRecordId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public SourceInfo SourceInfo { get; set; }
        public int SourceInfoId { get; set; }
    }
}

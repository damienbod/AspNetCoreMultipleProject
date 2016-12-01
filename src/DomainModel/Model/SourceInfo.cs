using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model
{
    public class SourceInfo
    {
        [Key]
        public long SourceInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public List<DataEventRecord> DataEventRecords { get; set; }
    }
}
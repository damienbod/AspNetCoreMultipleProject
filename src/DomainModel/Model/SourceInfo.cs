namespace DomainModel.Model
{
    using System;
    using System.Collections.Generic;

    public class SourceInfo
    {
        public long SourceInfoId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public List<DataEventRecord> DataEventRecords { get; set; }
    }
}
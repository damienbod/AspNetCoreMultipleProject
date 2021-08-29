using System;

namespace AspNetCoreMultipleProject
{
    public class DataEventRecordVm
    {
        public long DataEventRecordId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public SourceInfoVm SourceInfo { get; set; }
        public long SourceInfoId { get; set; }
    }
}

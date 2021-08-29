using System;

namespace AspNetCoreMultipleProject
{
    public class SourceInfoVm
    {
        public long SourceInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
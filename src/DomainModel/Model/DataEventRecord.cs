namespace DomainModel.Model
{
    using System;

    using Newtonsoft.Json;

    // >dnx . ef migration add testMigration

    public class DataEventRecord
    {
        public long DataEventRecordId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public SourceInfo SourceInfo { get; set; }

        [JsonIgnore]
        public int SourceInfoId { get; set; }
    }
}

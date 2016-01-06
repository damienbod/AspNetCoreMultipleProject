namespace DataAccessSqliteProvider.Model
{
    using System;

    public class SourceInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
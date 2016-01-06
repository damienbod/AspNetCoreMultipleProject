namespace DomainModel.Model
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Configuration;

    // >dnx . ef migration add testMigration
    public class DomainModelSqliteContext : DbContext
    {
        public DbSet<DataEventRecord> DataEventRecords { get; set; }

        public DbSet<SourceInfo> SourceInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        { 
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);


            base.OnModelCreating(builder); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
           .AddJsonFile("../config.json")
           .AddEnvironmentVariables();
            var configuration = builder.Build();

            var sqlConnectionString = configuration["DataAccessSqliteProvider:ConnectionString"];

            optionsBuilder.UseSqlite(sqlConnectionString);
        }
    }
}
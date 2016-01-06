namespace DomainModel.Model
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Configuration;

    // >dnx . ef migration add testMigration
    public class DomainModelSqliteContext : DbContext
    {
        public DbSet<DataEventRecord> DataEventRecords { get; set; }
      
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            builder.Entity<DataEventRecord>().HasKey(m => m.Id); 
            base.OnModelCreating(builder); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
           .AddJsonFile("config.json")
           .AddEnvironmentVariables();
            var configuration = builder.Build();

            var sqlConnectionString = configuration["DataAccessSqliteProvider:ConnectionString"];

            optionsBuilder.UseSqlite(sqlConnectionString);
        }
    }
}
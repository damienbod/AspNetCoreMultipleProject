This article shows how to use <a href="http://dev.mysql.com/downloads/mysql/">MySQL</a> with <a href="https://get.asp.net/">ASP.NET Core 1.0</a> using <a href="https://github.com/aspnet/EntityFramework">Entity Framework Core</a>. 

## Code: https://github.com/damienbod/AspNet5MultipleProject

Thanks to <a href="https://github.com/SapientGuardian">Noah Potash</a> for creating this example and adding his code to this code base.

The Entity Framework MySQL package can be downloaded using the NuGet package <a href="https://www.nuget.org/packages/SapientGuardian.EntityFrameworkCore.MySql/">SapientGuardian.EntityFrameworkCore.MySql</a>. At present no official provider from MySQL exists for Entity Framework Core which can be used in a ASP.NET Core application.

The SapientGuardian.EntityFrameworkCore.MySql package can be added to the project.json file.
```csharp
{
  "dependencies": {
    "Microsoft.NETCore.App": {
      "version": "1.0.0",
      "type": "platform"
    },
    "DomainModel": "*",
    "SapientGuardian.EntityFrameworkCore.MySql": "7.1.4"
  },

  "frameworks": {
    "netcoreapp1.0": {
      "imports": [
        "dotnet5.6",
        "dnxcore50",
        "portable-net45+win8"
      ]
    }
  }
}

```

A EfCore DbContext can be added then like any other context supported by Entity Framework Core.

```csharp
using System;
using System.Linq;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessMySqlProvider
{ 
    // >dotnet ef migration add testMigration
    public class DomainModelMySqlContext : DbContext
    {
        public DomainModelMySqlContext(DbContextOptions<DomainModelMySqlContext> options) :base(options)
        { }
        
        public DbSet<DataEventRecord> DataEventRecords { get; set; }

        public DbSet<SourceInfo> SourceInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
```

The DbContext can then be added to the startup class in your ASP.NET core web application.

```csharp
public Startup(IHostingEnvironment env)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("config.json", optional: true, reloadOnChange: true);

    Configuration = builder.Build();
}
        
public void ConfigureServices(IServiceCollection services)
{	
    var sqlConnectionString = Configuration["DataAccessMySqlProvider:ConnectionString"];

    services.AddDbContext<DomainModelMySqlContext>(options =>
        options.UseMySQL(
            sqlConnectionString,
            b => b.MigrationsAssembly("AspNet5MultipleProject")
        )
    );
}
```

The application uses the configuration from the config.json and this file is used to get the MySQL connection string. 

```csharp
  "DataAccessMySqlProvider": {
    "ConnectionString": "server=localhost;userid=damienbod;password=1234;database=damienbod;"
  }
```

MySQL workbench can be used to add the schema damienbod to the MySQL. A user damienbod is also required which must match the user in the connection string. If you configure the MySQL database differently, then you need to change the connection string in the config.json file.


<img src="https://damienbod.files.wordpress.com/2016/08/mysql_ercore_aspnetcore_01.png" alt="mySql_ercore_aspnetcore_01" width="352" height="532" class="alignnone size-full wp-image-7132" />

Now the database migrations can be created and the database can be updated.


```csharp
>
> dotnet ef migrations add testMySql
>
> dotnet ef database update
>
```

If successful, the tables are created.

<img src="https://damienbod.files.wordpress.com/2016/08/mysql_ercore_aspnetcore_02.png" alt="mySql_ercore_aspnetcore_02" width="193" height="194" class="alignnone size-full wp-image-7134" />


Now the MySQL provider can be used in a MVC 6 controller using construction injection.

```csharp
using System.Collections.Generic;
using DomainModel;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNet5MultipleProject.Controllers
{
    [Route("api/[controller]")]
    public class DataEventRecordsController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public DataEventRecordsController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpGet]
        public IEnumerable<DataEventRecord> Get()
        {
            return _dataAccessProvider.GetDataEventRecords();
        }

        [HttpGet]
        [Route("SourceInfos")]
        public IEnumerable<SourceInfo> GetSourceInfos(bool withChildren)
        {
            return _dataAccessProvider.GetSourceInfos(withChildren);
        }

        [HttpGet("{id}")]
        public DataEventRecord Get(long id)
        {
            return _dataAccessProvider.GetDataEventRecord(id);
        }

        [HttpPost]
        public void Post([FromBody]DataEventRecord value)
        {
            _dataAccessProvider.AddDataEventRecord(value);
        }

        [HttpPut("{id}")]
        public void Put(long id, [FromBody]DataEventRecord value)
        {
            _dataAccessProvider.UpdateDataEventRecord(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            _dataAccessProvider.DeleteDataEventRecord(id);
        }
    }
}

```

The controller api can be called using Fiddler:

```csharp
POST http://localhost:5000/api/dataeventrecords HTTP/1.1
User-Agent: Fiddler
Host: localhost:5000
Content-Length: 135
Content-Type: application/json;
 
{
  "DataEventRecordId":3,
  "Name":"Funny data",
  "Description":"yes",
  "Timestamp":"2015-12-27T08:31:35Z",
   "SourceInfo":
  { 
    "SourceInfoId":0,
    "Name":"Beauty",
    "Description":"second Source",
    "Timestamp":"2015-12-23T08:31:35+01:00",
    "DataEventRecords":[]
  },
 "SourceInfoId":0 
}
```

The data is added to the database as required.

<img src="https://damienbod.files.wordpress.com/2016/08/mysql_ercore_aspnetcore_03.png?w=600" alt="mySql_ercore_aspnetcore_03" width="600" height="198" class="alignnone size-medium wp-image-7139" />


## Links: 

https://github.com/SapientGuardian/SapientGuardian.EntityFrameworkCore.MySql

http://dev.mysql.com/downloads/mysql/

http://damienbod.com/2016/01/07/experiments-with-entity-framework-7-and-asp-net-5-mvc-6/

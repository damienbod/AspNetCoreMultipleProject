using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataAccessMsSqlServerProvider;
using DataAccessPostgreSqlProvider;
using DataAccessSqliteProvider;
using DataAccessMySqlProvider;
using MySQL.Data.Entity.Extensions;
using DomainModel;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace AspNet5MultipleProject
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use a SQLite database
            var sqlConnectionString = Configuration.GetConnectionString("DataAccessSqliteProvider");

            services.AddDbContext<DomainModelSqliteContext>(options =>
                options.UseSqlite(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("AspNet5MultipleProject")
                )
            );

            services.AddScoped<IDataAccessProvider, DataAccessSqliteProvider.DataAccessSqliteProvider>();

            // Use a MS SQL Server database
            // var sqlConnectionString = Configuration.GetConnectionString("DataAccessMsSqlServerProvider");

            //services.AddDbContext<DomainModelMsSqlServerContext>(options =>
            //    options.UseSqlServer(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNet5MultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessMsSqlServerProvider.DataAccessMsSqlServerProvider>();

            //Use a PostgreSQL database
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");

            //services.AddDbContext<DomainModelPostgreSqlContext>(options =>
            //    options.UseNpgsql(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNet5MultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessPostgreSqlProvider.DataAccessPostgreSqlProvider>();

            //Use a MySQL database
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessMySqlProvider");

            //services.AddDbContext<DomainModelMySqlContext>(options =>
            //    options.UseMySQL(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNet5MultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessMySqlProvider.DataAccessMySqlProvider>();

            //var serializerSettings = new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};

            //JsonOutputFormatter jsonOutputFormatter = new JsonOutputFormatter(serializerSettings, new System.Buffers.ArrayPool<object>());

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //}
            //options =>
            //        {
            //            options.OutputFormatters.Clear();
            //            options.OutputFormatters.Insert(0, jsonOutputFormatter);
            //        }
            //    );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseMvc();
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

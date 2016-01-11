using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspNet5MultipleProject
{
    using DataAccessMsSqlServerProvider;
    using DataAccessPostgreSqlProvider;
    using DataAccessSqliteProvider;
    using DomainModel;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddEntityFramework()
            //    .AddSqlite()
            //    .AddDbContext<DomainModelSqliteContext>();

            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<DomainModelMsSqlServerContext>();

            services.AddEntityFramework()
                .AddNpgsql()
                .AddDbContext<DomainModelPostgreSqlContext>();

            JsonOutputFormatter jsonOutputFormatter = new JsonOutputFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            };

            services.AddMvc(
                options =>
                {
                    options.OutputFormatters.Clear();
                    options.OutputFormatters.Insert(0, jsonOutputFormatter);
                }
            );

            // Use a PostgreSQL database
            services.AddScoped<IDataAccessProvider, DataAccessPostgreSqlProvider>();

            // Use a SQLite database
            //services.AddScoped<IDataAccessProvider, DataAccessSqliteProvider>();

            // Use a MS SQL Server database
            //services.AddScoped<IDataAccessProvider, DataAccessMsSqlServerProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

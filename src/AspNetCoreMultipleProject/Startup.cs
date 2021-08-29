using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessSqliteProvider;
using DomainModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using DataAccessMsSqlServerProvider;
using DataAccessPostgreSqlProvider;
using DataAccessMySqlProvider;

namespace AspNetCoreMultipleProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use a SQLite database
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessSqliteProvider");

            //services.AddDbContext<DomainModelSqliteContext>(options =>
            //    options.UseSqlite(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNetCoreMultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessSqliteProvider.DataAccessSqliteProvider>();

            //Use a MS SQL Server database
            var sqlConnectionString = Configuration.GetConnectionString("DataAccessMsSqlServerProvider");

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>
                options.UseSqlServer(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("AspNetCoreMultipleProject")
                )
            );

            services.AddScoped<IDataAccessProvider, DataAccessMsSqlServerProvider.DataAccessMsSqlServerProvider>();

            //Use a PostgreSQL database
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");

            //services.AddDbContext<DomainModelPostgreSqlContext>(options =>
            //    options.UseNpgsql(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNetCoreMultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessPostgreSqlProvider.DataAccessPostgreSqlProvider>();

            //Use a MySQL database
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessMySqlProvider");

            //services.AddDbContext<DomainModelMySqlContext>(options =>
            //    options.UseMySql(
            //        sqlConnectionString,
            //        b => b.MigrationsAssembly("AspNetCoreMultipleProject")
            //    )
            //);

            //services.AddScoped<IDataAccessProvider, DataAccessMySqlProvider.DataAccessMySqlProvider>();

            services.AddControllers()
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}

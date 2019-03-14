using CSharpWars.Common.DependencyInjection;
using CSharpWars.Web.Api.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using static System.Convert;
using static System.Environment;

namespace CSharpWars.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurationHelper(c =>
            {
                c.ConnectionString = GetEnvironmentVariable("CONNECTION_STRING");
                c.ArenaSize = ToInt32(GetEnvironmentVariable("ARENA_SIZE"));
            });

            services.ConfigureWebApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(routes => { routes.MapControllers(); });
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseMvc();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/docs/v1/swagger.json", "CSharpWars");
            //    c.RoutePrefix = "docs";
            //    c.DocExpansion(DocExpansion.List);
            //});
        }
    }
}
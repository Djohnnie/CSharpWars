using System.IO;
using System.Reflection;
using CSharpWars.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using CSharpWars.Logic.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CSharpWars.Web.Api.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWebApi(this IServiceCollection services)
        {
            services.ConfigureCommon();
            services.ConfigureLogic();
            services.AddMemoryCache();
            services.AddMvc().AddNewtonsoftJson();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CSharpWars", Version = "v1" });
                c.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
            });
        }
    }
}
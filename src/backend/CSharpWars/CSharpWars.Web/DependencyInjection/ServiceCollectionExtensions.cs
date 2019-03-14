using System;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Logic.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Web.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWeb(this IServiceCollection services)
        {
            services.ConfigureCommon();
            services.ConfigureLogic();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().AddNewtonsoftJson();
        }
    }
}
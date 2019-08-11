using System;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Logic.DependencyInjection;
using CSharpWars.Web.Helpers;
using CSharpWars.Web.Helpers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Web.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWeb(this IServiceCollection services)
        {
            services.AddTransient<IScriptValidationHelper, ScriptValidationHelper>();
            services.ConfigureCommon();
            services.ConfigureLogic();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().AddNewtonsoftJson();
        }
    }
}
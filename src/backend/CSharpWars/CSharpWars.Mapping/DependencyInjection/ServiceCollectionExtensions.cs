using CSharpWars.DtoModel;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Mapping.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.AddSingleton<IMapper<Player, PlayerDto>, PlayerMapper>();
            services.AddSingleton<IMapper<Bot, BotDto>, BotMapper>();
            services.AddSingleton<IMapper<Bot, BotToCreateDto>, BotToCreateMapper>();
            services.AddSingleton<IMapper<Message, MessageDto>, MessageMapper>();
        }
    }
}
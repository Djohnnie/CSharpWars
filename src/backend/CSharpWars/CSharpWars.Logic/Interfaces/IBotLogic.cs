using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface IBotLogic : ILogic
    {
        Task<IList<BotDto>> GetAllActiveBots();

        Task<IList<BotDto>> GetAllLiveBots();

        Task<String> GetBotScript(Guid botId);

        Task<BotDto> CreateBot(BotToCreateDto bot);

        Task UpdateBots(IList<BotDto> bots);
    }
}
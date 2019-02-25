using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface IBotLogic : ILogic
    {
        Task<IEnumerable<BotDto>> GetAllActiveBots();

        Task<BotDto> CreateBot(BotToCreateDto bot);
    }
}
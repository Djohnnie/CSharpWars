using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface IArenaLogic : ILogic
    {
        Task<ArenaDto> GetArena();
    }
}
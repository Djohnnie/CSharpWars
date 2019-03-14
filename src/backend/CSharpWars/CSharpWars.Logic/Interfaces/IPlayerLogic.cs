using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface IPlayerLogic : ILogic
    {
        Task<IList<PlayerDto>> GetAllPlayers();

        Task<PlayerDto> Login(LoginDto login);
    }
}
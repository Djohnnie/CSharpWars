using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface IPlayerLogic : ILogic
    {
        Task<IEnumerable<PlayerDto>> GetAllPlayers();
    }
}
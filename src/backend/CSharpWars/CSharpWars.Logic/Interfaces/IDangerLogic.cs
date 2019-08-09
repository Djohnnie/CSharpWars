using System.Threading.Tasks;

namespace CSharpWars.Logic.Interfaces
{
    public interface IDangerLogic : ILogic
    {
        Task CleanupMessages();

        Task CleanupBots();

        Task CleanupPlayers();
    }
}
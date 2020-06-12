using Adic;
using System.Threading.Tasks;

namespace Assets.Scripts.Controllers
{
    public class GameLoopController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        private IGameState _gameState;

        #endregion

        #region <| Start |>

        public async override Task Start()
        {
            await base.Start();

            InvokeRepeating(nameof(UpdateGameState), 2, 2);
        }

        #endregion

        #region <| Helper Methods |>

        private async Task UpdateGameState()
        {
            await _gameState.Update();
        }

        #endregion
    }
}
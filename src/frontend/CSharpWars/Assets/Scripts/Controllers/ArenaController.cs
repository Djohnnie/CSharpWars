using System.Threading.Tasks;
using Adic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ArenaController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        private IGameState _gameState;

        [Inject("floor")]
        private GameObject _floor;

        [Inject("floor-renderer")]
        private Renderer _floorRenderer;

        #endregion

        #region <| Start |>

        public async override Task Start()
        {
            await base.Start();

            // Refresh the arena on start!
            await _gameState.RefreshArena((x, y, arena, arenaThickness) =>
            {
                var vX = .5f + x - arena.Width / 2 + _floor.transform.position.x;
                var vY = arenaThickness / 2;
                var vZ = arena.Height - .5f - y - arena.Height / 2 + _floor.transform.position.z;
                return new Vector3(vX, vY, vZ);
            });

            _floor.transform.localScale = new Vector3(
                _gameState.Arena.Width, _gameState.ArenaThickness, _gameState.Arena.Height);

            _floorRenderer.material.mainTextureScale = new Vector2(
                _gameState.Arena.Width, _gameState.Arena.Height);
        }

        #endregion
    }
}
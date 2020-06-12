using System.Threading.Tasks;
using Adic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CameraController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        private IGameState _gameState;

        #endregion

        #region <| Start |>

        public override Task Start()
        {
            return base.Start();
        }

        #endregion

        #region <| LateUpdate |>

        public void LateUpdate()
        {
            // yAngle (in degrees) = 6 * x * deltaTime (seconds).
            // If RotationsPerMinute is equal to 1, yAngle = 360 degrees (6 * 1 * 60).
            var yAngle = 6.0f * _gameState.CameraRotationsPerMinute * Time.deltaTime;
            transform.Rotate(0f, yAngle, 0f);
        }

        #endregion
    }
}
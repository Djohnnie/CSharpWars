using System;
using System.Threading.Tasks;
using Adic;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public abstract class TagController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        protected IGameState _gameState;

        #endregion
        
        #region <| Private Members |>

        private readonly float _offset;

        private Guid _botId;

        #endregion

        #region <| Construction |>

        protected TagController(float offset)
        {
            _offset = offset;
        }

        #endregion

        #region <| Start |>

        public async override Task Start()
        {
            await base.Start();

            _gameState.BotShouldBeUpdated.AddListener(OnBotShouldBeUpdated);
            _gameState.BotHasDied.AddListener(OnBotHasDied);
        }

        #endregion

        #region <| Event Handlers |>

        private void OnBotShouldBeUpdated(Guid botId)
        {
            if( botId == _botId )
            {
                var bot = _gameState[_botId];
                UpdateTag(bot);
            }
        }

        private void OnBotHasDied(Guid botId)
        {
            if( botId == _botId )
            {
                Destroy(gameObject);
                Destroy(this);
            }
        }

        #endregion

        #region <| LateUpdate |>
        
        private void LateUpdate()
        {
            // Always let the name tags look directly at the camera.
            var mainCameraRotation = Camera.main.transform.rotation;
            transform.LookAt(transform.position + mainCameraRotation * Vector3.forward, mainCameraRotation * Vector3.up);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, _offset, transform.position.z);
        }        

        #endregion

        #region <| Public Methods |>

        public void SetBotId(Guid botId)
        {
            _botId = botId;
        }

        #endregion

        #region <| Private Methods |>

        protected abstract void UpdateTag(Bot bot);

        #endregion
    }
}
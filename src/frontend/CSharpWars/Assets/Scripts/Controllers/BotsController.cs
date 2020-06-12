using System;
using System.Threading.Tasks;
using Adic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotsController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        private IGameState _gameState;

        [Inject("prefab-bot")]
        private GameObject BotPrefab;

        [Inject("prefab-name-tag")]
        private GameObject NameTagPrefab;

        [Inject("prefab-health-tag")]
        private GameObject HealthTagPrefab;

        [Inject("prefab-stamina-tag")]
        private GameObject StaminaTagPrefab;

        #endregion

        #region <| Start |>

        public async override Task Start()
        {
            await base.Start();

            _gameState.BotShouldBeCreated.AddListener(OnBotShouldBeCreated);
        }

        #endregion

        #region <| Event Handlers |>

        private void OnBotShouldBeCreated(Guid botId)
        {
            var bot = _gameState[botId];
            var newBot = Instantiate(BotPrefab);
            newBot.transform.parent = transform;
            newBot.name = $"Bot-({bot.PlayerName})-{bot.Name}";
            var botController = newBot.GetComponent<BotController>(); 
            botController.SetBotId(botId);           
            InstantiateTag<NameTagController>(NameTagPrefab, botController, botId);
            InstantiateTag<HealthTagController>(HealthTagPrefab, botController, botId);
            InstantiateTag<StaminaTagController>(StaminaTagPrefab, botController, botId);
        }

        #endregion

        #region <| Helper Methods |>

        private void InstantiateTag<TTagController>(
            GameObject tagPrefab, BotController botController, Guid botId) where TTagController : TagController
        {
            var tagObject = Instantiate(tagPrefab);
            tagObject.transform.SetParent(botController.Head);
            var tagController = tagObject.GetComponent<TTagController>();
            tagController.SetBotId(botId);
        }

        #endregion
    }
}
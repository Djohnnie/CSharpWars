using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotsController : MonoBehaviour
    {
        private readonly Dictionary<Guid, BotController> _bots = new Dictionary<Guid, BotController>();
        private ArenaController _arenaController;

        [Header("The refresh rate in seconds")]
        public float RefreshRate = 2;

        [Space(10)]

        [Header("The PREFAB to use for bots")]
        public GameObject BotPrefab;
        [Header("The PREFAB to use for name tags")]
        public GameObject NameTagPrefab;
        [Header("The PREFAB to use for health bars")]
        public GameObject HealthTagPrefab;
        [Header("The PREFAB to use for stamina bars")]
        public GameObject StaminaTagPrefab;

        void Start()
        {
            _arenaController = GetComponent<ArenaController>();
            InvokeRepeating(nameof(RefreshBots), RefreshRate, RefreshRate);
        }

        private void RefreshBots()
        {
            var bots = ApiClient.GetBots();

            CleanKilledBots(bots);
            foreach (var bot in bots)
            {
                if (!_bots.ContainsKey(bot.Id))
                {
                    var newBot = Instantiate(BotPrefab);
                    newBot.transform.parent = transform;
                    newBot.name = $"Bot-{bot.PlayerName}-{bot.Name}";
                    var botController = newBot.GetComponent<BotController>();
                    botController.SetBot(bot);
                    botController.SetArenaController(_arenaController);
                    botController.InstantRefresh();
                    _bots.Add(bot.Id, botController);
                    
                    InstantiateTag<NameTagController>(NameTagPrefab, botController, bot);
                    InstantiateTag<HealthTagController>(HealthTagPrefab, botController, bot);
                    InstantiateTag<StaminaTagController>(StaminaTagPrefab, botController, bot);
                }
                else
                {
                    var botController = _bots[bot.Id];
                    botController.UpdateBot(bot);
                }
            }
        }

        private void InstantiateTag<TTagController>(
            GameObject tagPrefab, BotController botController, Bot bot) where TTagController : TagController
        {
            var tagObject = Instantiate(tagPrefab);
            tagObject.transform.SetParent(botController.Head);
            var tagController = tagObject.GetComponent<TTagController>();
            tagController.UpdateTag(bot);
            botController.SetTagController(tagController);
        }

        private void CleanKilledBots(List<Bot> bots)
        {
            var botIdsToClean = new List<Guid>();

            foreach (var botId in _bots.Keys)
            {
                if (bots.All(b => b.Id != botId))
                {
                    botIdsToClean.Add(botId);
                }
            }

            foreach (var botId in botIdsToClean)
            {
                var botController = _bots[botId];
                _bots.Remove(botId);
                botController.Destroy();
            }
        }
    }
}
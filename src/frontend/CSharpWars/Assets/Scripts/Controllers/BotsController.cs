using System;
using System.Collections.Generic;
using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotsController : MonoBehaviour
    {
        private readonly Dictionary<Guid, BotController> _bots = new Dictionary<Guid, BotController>();
        private GameObject _floor;
        private ArenaController _arenaController;

        public Single RefreshRate = 2;
        public GameObject BotPrefab;
        public GameObject NameTagPrefab;
        public GameObject HealthTagPrefab;
        public GameObject StaminaTagPrefab;

        void Start()
        {
            InvokeRepeating(nameof(RefreshBots), RefreshRate, RefreshRate);
            _floor = GameObject.Find("Floor");
            _arenaController = GetComponent<ArenaController>();
        }

        private void RefreshBots()
        {
            var bots = ApiClient.GetBots();
            foreach (var bot in bots)
            {
                if (!_bots.ContainsKey(bot.Id))
                {
                    var newBot = Instantiate(BotPrefab);
                    newBot.transform.parent = transform;
                    //newBot.transform.position = _arenaController.ArenaToWorldPosition(bot.X, bot.Y);
                    //newBot.transform.eulerAngles = OrientationVector.CreateFrom(bot.Orientation);
                    newBot.name = $"Bot-{bot.Id}";
                    var botController = newBot.GetComponent<BotController>();
                    botController.SetBot(bot);
                    botController.SetArenaController(_arenaController);
                    botController.InstantRefresh();
                    _bots.Add(bot.Id, botController);

                    var nameTag = Instantiate(NameTagPrefab);
                    nameTag.transform.SetParent(botController.Head);
                    var nameTagController = nameTag.GetComponent<NameTagController>();
                    nameTagController.BotGameObject = newBot;
                    nameTagController.UpdateBot(bot);
                    //currentBotController.NameTagController = nameTagController;

                    var healthTag = Instantiate(HealthTagPrefab);
                    healthTag.transform.SetParent(botController.Head);
                    var healthTagController = healthTag.GetComponent<HealthTagController>();
                    healthTagController.BotGameObject = newBot;
                    healthTagController.UpdateBot(bot);
                    //currentBotController.HealthTagController = healthTagController;

                    var staminaTag = Instantiate(StaminaTagPrefab);
                    staminaTag.transform.SetParent(botController.Head);
                    var staminaTagController = staminaTag.GetComponent<StaminaTagController>();
                    staminaTagController.BotGameObject = newBot;
                    staminaTagController.UpdateBot(bot);
                    //currentBotController.StaminaTagController = staminaTagController;
                }
                else
                {
                    var botController = _bots[bot.Id];
                    botController.SetBot(bot);
                }
            }
        }
    }
}
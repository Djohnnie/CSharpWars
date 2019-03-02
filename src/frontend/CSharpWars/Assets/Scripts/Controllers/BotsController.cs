using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Model;
using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotsController : MonoBehaviour
    {
        private readonly Dictionary<Guid, Bot> _bots = new Dictionary<Guid, Bot>();
        private GameObject _floor;

        public Single RefreshRate = 2;
        public GameObject BotPrefab;

        void Start()
        {
            InvokeRepeating(nameof(RefreshBots), RefreshRate, RefreshRate);
            _floor = GameObject.Find("Floor");
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
                    var arenaController = GetComponent<ArenaController>();
                    newBot.transform.position = arenaController.ArenaToWorldPosition(bot.LocationX, bot.LocationY);
                    newBot.transform.eulerAngles = OrientationVector.CreateFrom(bot.Orientation);
                    newBot.name = $"Bot-{bot.Id}";
                    _bots.Add(bot.Id, bot);
                }
                else
                {

                }

            }
        }
    }
}
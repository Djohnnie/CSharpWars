using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Events;
using Assets.Scripts.Model;
using Assets.Scripts.Networking;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public interface IGameState
    {
        Bot this[Guid botId] { get; }

        float CameraRotationsPerMinute { get; }
        Arena Arena { get; }

        List<Bot> Bots { get; }

        float ArenaThickness { get; }

        UnityEvent Updated { get; }
        BotEvent BotShouldBeCreated { get; }
        BotEvent BotShouldBeUpdated { get; }
        BotEvent BotShouldBeRemoved { get; }
        BotEvent BotHasDied { get; }

        Task Update();

        Task RefreshArena(Func<int, int, Arena, float, Vector3> arenaToWorldPositionFunc);

        Vector3 ArenaToWorldPosition(int x, int y);
    }

    public class GameState : IGameState
    {
        #region <| Dependencies |>

        private readonly IApiClient _apiClient;

        #endregion

        private Func<int, int, Arena, float, Vector3> _arenaToWorldPositionFunc;
        private readonly Dictionary<Guid, Bot> _cachedBots = new Dictionary<Guid, Bot>();

        #region <| Properties - Indexer |>

        public Bot this[Guid botId]
        { 
            get
            {
                return _bots.Single( x => x.Id == botId );
            }
        }

        #endregion

        #region <| Properties - CameraRotationsPerMinute |>

        public float CameraRotationsPerMinute
        {
            get => 2.0f;
        }

        #endregion

        #region <| Properties - Arena |>

        private Arena _arena;

        public Arena Arena
        {
            get => _arena ?? new Arena { Width = 1.0f, Height = 1.0f };
        }

        #endregion

        #region <| Properties - Bots |>

        private List<Bot> _bots;

        public List<Bot> Bots
        {
            get => _bots ?? new List<Bot>();
        }

        #endregion

        #region <| Properties - ArenaThickness |>

        public float ArenaThickness
        {
            get => .2f;
        }

        #endregion

        #region <| Events |>

        public UnityEvent Updated { get; }
        public BotEvent BotShouldBeCreated { get; }
        public BotEvent BotShouldBeUpdated { get; }
        public BotEvent BotShouldBeRemoved { get; }
        public BotEvent BotHasDied { get; }

        #endregion
        
        #region <| Constructor |>

        public GameState(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Updated = new UnityEvent();
            BotShouldBeCreated = new BotEvent();
            BotShouldBeUpdated = new BotEvent();
            BotShouldBeRemoved = new BotEvent();
            BotHasDied = new BotEvent();
        }

        #endregion

        #region <| Operations |>

        public async Task Update()
        {
            _bots = await _apiClient.GetBots();
            
            RefreshBots();

            Updated.Invoke();
        }

        public async Task RefreshArena(Func<int, int, Arena, float, Vector3> arenaToWorldPositionFunc)
        {
            _arenaToWorldPositionFunc = arenaToWorldPositionFunc;
            _arena = await _apiClient.GetArena();
        }

        private void RefreshBots()
        {
            CleanKilledBots();

            foreach (var bot in _bots)
            {
                if (!_cachedBots.ContainsKey(bot.Id))
                {
                    _cachedBots.Add(bot.Id, bot);
                    BotShouldBeCreated.Invoke(bot.Id);                  
                }

                BotShouldBeUpdated.Invoke(bot.Id);

                switch( bot.Move )
                {
                    case PossibleMoves.Died:
                    case PossibleMoves.SelfDestruct:
                        BotHasDied.Invoke(bot.Id);
                        break;
                }
            }
        }

        private void CleanKilledBots()
        {
            var botIdsToClean = new List<Guid>();

            foreach (var botId in _cachedBots.Keys)
            {
                if (_bots.All(b => b.Id != botId))
                {
                    botIdsToClean.Add(botId);
                }
            }

            foreach (var botId in botIdsToClean)
            {
                var botController = _cachedBots[botId];
                _cachedBots.Remove(botId);
                BotShouldBeRemoved.Invoke(botId);
            }
        }

        public Vector3 ArenaToWorldPosition(int x, int y)
        {
            if (_arena != null)
            {
                return _arenaToWorldPositionFunc(x, y, _arena, ArenaThickness);
            }

            return Vector3.zero;
        }

        #endregion
    }
}
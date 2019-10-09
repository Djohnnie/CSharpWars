using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic.Exceptions;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class BotLogic : IBotLogic
    {
        private readonly IRandomHelper _randomHelper;
        private readonly IRepository<Bot> _botRepository;
        private readonly IRepository<BotScript> _scriptRepository;
        private readonly IRepository<Bot, BotScript> _botScriptRepository;
        private readonly IRepository<Player> _playerRepository;
        private readonly IMapper<Bot, BotDto> _botMapper;
        private readonly IMapper<Bot, BotToCreateDto> _botToCreateMapper;
        private readonly IArenaLogic _arenaLogic;
        private readonly IConfigurationHelper _configurationHelper;

        public BotLogic(
            IRandomHelper randomHelper,
            IRepository<Bot> botRepository,
            IRepository<BotScript> scriptRepository,
            IRepository<Bot, BotScript> botScriptRepository,
            IRepository<Player> playerRepository,
            IMapper<Bot, BotDto> botMapper,
            IMapper<Bot, BotToCreateDto> botToCreateMapper,
            IArenaLogic arenaLogic,
            IConfigurationHelper configurationHelper)
        {
            _randomHelper = randomHelper;
            _botRepository = botRepository;
            _scriptRepository = scriptRepository;
            _botScriptRepository = botScriptRepository;
            _playerRepository = playerRepository;
            _botMapper = botMapper;
            _botToCreateMapper = botToCreateMapper;
            _arenaLogic = arenaLogic;
            _configurationHelper = configurationHelper;
        }

        public async Task<IList<BotDto>> GetAllActiveBots()
        {
            var dateTimeToCompare = DateTime.UtcNow.AddSeconds(-10);

            var activeBots = await _botRepository.Find(x => x.CurrentHealth > 0 || x.TimeOfDeath > dateTimeToCompare, i => i.Player);
            return _botMapper.Map(activeBots);
        }

        public async Task<IList<BotDto>> GetAllLiveBots()
        {
            var activeBots = await _botRepository.Find(x => x.CurrentHealth > 0, i => i.Player);
            return _botMapper.Map(activeBots);
        }

        public async Task<String> GetBotScript(Guid botId)
        {
            var botScript = await _scriptRepository.Single(x => x.Id == botId);
            return botScript?.Script;
        }

        public async Task<BotDto> CreateBot(BotToCreateDto botToCreate)
        {
            var bot = _botToCreateMapper.Map(botToCreate);
            var arena = await _arenaLogic.GetArena();
            var player = await _playerRepository.Single(x => x.Id == botToCreate.PlayerId);

            if (player.LastDeployment >= DateTime.UtcNow.AddMinutes(-_configurationHelper.BotDeploymentLimit))
            {
                throw new LogicException("You are not allowed to create multiple robots in rapid succession!");
            }

            player.LastDeployment = DateTime.UtcNow;

            bot.Player = player;
            bot.Orientation = _randomHelper.Get<PossibleOrientations>();
            var bots = await GetAllActiveBots();
            var freeLocations = BuildFreeLocation(arena, bots);
            var randomFreeLocation = freeLocations[_randomHelper.Get(freeLocations.Count)];
            bot.X = randomFreeLocation.X;
            bot.Y = randomFreeLocation.Y;
            bot.FromX = bot.X;
            bot.FromY = bot.Y;
            bot.CurrentHealth = bot.MaximumHealth;
            bot.CurrentStamina = bot.MaximumStamina;
            bot.Memory = new Dictionary<String, String>().Serialize();
            bot.TimeOfDeath = DateTime.MaxValue;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bot = await _botScriptRepository.Create(bot);
                var botScript = await _scriptRepository.Single(x => x.Id == bot.Id);
                botScript.Script = botToCreate.Script;
                await _scriptRepository.Update(botScript);
                await _playerRepository.Update(player);

                transaction.Complete();
            }

            var createdBot = _botMapper.Map(bot);
            return createdBot;
        }

        private IList<(Int32 X, Int32 Y)> BuildFreeLocation(ArenaDto arena, IList<BotDto> bots)
        {
            var freeLocations = new List<(Int32 X, Int32 Y)>();

            for (Int32 x = 0; x < arena.Width; x++)
            {
                for (Int32 y = 0; y < arena.Height; y++)
                {
                    if (!bots.Any(b => b.X == x && b.Y == y))
                    {
                        freeLocations.Add((x, y));
                    }
                }
            }

            return freeLocations;
        }

        public async Task UpdateBots(IList<BotDto> bots)
        {
            var botEntities = _botMapper.Map(bots);
            await _botRepository.Update(botEntities);
        }
    }
}
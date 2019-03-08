using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class BotLogic : IBotLogic
    {
        private readonly IRandomHelper _randomHelper;
        private readonly IRepository<Bot> _botRepository;
        private readonly IMapper<Bot, BotDto> _botMapper;
        private readonly IMapper<Bot, BotToCreateDto> _botToCreateMapper;
        private readonly IArenaLogic _arenaLogic;

        public BotLogic(
            IRandomHelper randomHelper,
            IRepository<Bot> botRepository,
            IMapper<Bot, BotDto> botMapper,
            IMapper<Bot, BotToCreateDto> botToCreateMapper,
            IArenaLogic arenaLogic)
        {
            _randomHelper = randomHelper;
            _botRepository = botRepository;
            _botMapper = botMapper;
            _botToCreateMapper = botToCreateMapper;
            _arenaLogic = arenaLogic;
        }

        public async Task<IList<BotDto>> GetAllActiveBots()
        {
            var activeBots = await _botRepository.Find(x => x.CurrentHealth > 0);
            return _botMapper.Map(activeBots);
        }

        public async Task<BotDto> CreateBot(BotToCreateDto botToCreate)
        {
            var bot = _botToCreateMapper.Map(botToCreate);
            var arena = await _arenaLogic.GetArena();
            bot.Orientation = _randomHelper.Get<PossibleOrientations>();
            bot.X = _randomHelper.Get(arena.Width);
            bot.Y = _randomHelper.Get(arena.Height);
            bot.CurrentHealth = bot.MaximumHealth;
            bot.CurrentStamina = bot.MaximumStamina;
            bot.Memory = new Dictionary<String, String>().Serialize();
            bot = await _botRepository.Create(bot);
            var createdBot = _botMapper.Map(bot);
            return createdBot;
        }

        public async Task UpdateBots(IList<BotDto> bots)
        {
            var botEntities = _botMapper.Map(bots);
            await _botRepository.Update(botEntities);
        }
    }
}
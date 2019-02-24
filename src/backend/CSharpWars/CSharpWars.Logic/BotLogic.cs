using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class BotLogic : IBotLogic
    {
        private readonly IRepository<Bot> _botRepository;
        private readonly IMapper<Bot, BotDto> _botMapper;

        public BotLogic(IRepository<Bot> botRepository, IMapper<Bot, BotDto> botMapper)
        {
            _botRepository = botRepository;
            _botMapper = botMapper;
        }

        public async Task<IEnumerable<BotDto>> GetAllActiveBots()
        {
            var activeBots = await _botRepository.Find(x => x.CurrentHealth > 0);
            return _botMapper.Map(activeBots);
        }
    }
}
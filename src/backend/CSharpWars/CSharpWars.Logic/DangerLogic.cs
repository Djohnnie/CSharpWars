using System;
using System.Threading.Tasks;
using System.Transactions;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class DangerLogic : IDangerLogic
    {
        private readonly IRepository<Bot, BotScript> _botScriptRepository;
        private readonly IRepository<Player> _playerRepository;

        public DangerLogic(
            IRepository<Bot, BotScript> botScriptRepository,
            IRepository<Player> playerRepository)
        {
            _botScriptRepository = botScriptRepository;
            _playerRepository = playerRepository;
        }

        public Task CleanupMessages()
        {
            throw new NotImplementedException();
        }

        public async Task CleanupBots()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var bots = await _botScriptRepository.GetAll();
                await _botScriptRepository.Delete(bots);

                transaction.Complete();
            }
        }

        public async Task CleanupPlayers()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await CleanupBots();
                var players = await _playerRepository.GetAll();
                await _playerRepository.Delete(players);

                transaction.Complete();
            }
        }
    }
}
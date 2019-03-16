using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.DtoModel;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Middleware
{
    public class ProcessingContext
    {
        private readonly ConcurrentDictionary<Guid, BotProperties> _botProperties = new ConcurrentDictionary<Guid, BotProperties>();

        public ArenaDto Arena { get; private set; }
        public IList<BotDto> Bots { get; private set; }

        private ProcessingContext() { }

        public static ProcessingContext Build(ArenaDto arena, IList<BotDto> bots)
        {
            return new ProcessingContext
            {
                Arena = arena,
                Bots = bots
            };
        }

        public void AddBotProperties(Guid botId, BotProperties botProperties)
        {
            _botProperties.TryAdd(botId, botProperties);
        }

        public BotProperties GetBotProperties(Guid botId)
        {
            return _botProperties[botId];
        }

        public IEnumerable<BotProperties> GetOrderedBotProperties()
        {
            return _botProperties.Values.OrderBy(x => x.CurrentMove, new MoveComparer());
        }

        public void UpdateBotProperties(BotDto bot)
        {
            foreach (var botProperties in _botProperties.Values)
            {
                var botToUpdate = botProperties.Bots.Single(x => x.Id == bot.Id);
                botToUpdate.Update(bot);
            }

            var botPropertiesToUpdate = _botProperties.Values.Single(x => x.BotId == bot.Id);
            botPropertiesToUpdate.Update(bot);
        }
    }
}
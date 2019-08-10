using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Bot> _botRepository;
        private readonly IMapper<Message, MessageDto> _messageMapper;

        public MessageLogic(
            IRepository<Message> messageRepository,
            IRepository<Bot> botRepository,
            IMapper<Message, MessageDto> messageMapper)
        {
            _messageRepository = messageRepository;
            _botRepository = botRepository;
            _messageMapper = messageMapper;
        }

        public async Task<IList<MessageDto>> GetLastMessages()
        {
            var messages = await _messageRepository.FindDescending(x => x.DateTime, 10);
            return _messageMapper.Map(messages);
        }

        public async Task CreateMessages(IList<MessageToCreateDto> messagesToCreate)
        {
            var messages = new List<Message>();
            foreach (MessageToCreateDto messageToCreate in messagesToCreate)
            {
                var message = new Message
                {
                    BotName = messageToCreate.BotName,
                    DateTime = messageToCreate.DateTime,
                    Content = messageToCreate.Content
                };

                messages.Add(message);
            }

            await _messageRepository.Create(messages);
        }
    }
}
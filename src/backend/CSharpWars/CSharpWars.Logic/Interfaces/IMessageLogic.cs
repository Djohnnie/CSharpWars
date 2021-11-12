using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces;

public interface IMessageLogic : ILogic
{
    Task<IList<MessageDto>> GetLastMessages();

    Task CreateMessages(IList<MessageToCreateDto> messagesToCreate);
}
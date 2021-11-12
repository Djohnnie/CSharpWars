using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping;

public class MessageMapper : Mapper<Message, MessageDto>
{
    public MessageMapper() : base(config =>
    {
        config.CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.BotName, opt => opt.MapFrom(src => src.BotName))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content));
    })
    { }
}
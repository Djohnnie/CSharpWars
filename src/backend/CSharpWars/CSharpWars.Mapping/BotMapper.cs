using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping;

public class BotMapper : Mapper<Bot, BotDto>
{
    public BotMapper() : base(config =>
    {
        config.CreateMap<Bot, BotDto>();
        config.CreateMap<BotDto, Bot>();
    })
    { }
}
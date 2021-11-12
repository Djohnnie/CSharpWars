using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping;

public class BotToCreateMapper : Mapper<Bot, BotToCreateDto>
{
    public BotToCreateMapper() : base(config =>
    {
        config.CreateMap<Bot, BotToCreateDto>();
        config.CreateMap<BotToCreateDto, Bot>();
    })
    { }
}
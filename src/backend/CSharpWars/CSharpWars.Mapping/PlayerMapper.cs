using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping;

public class PlayerMapper : Mapper<Player, PlayerDto>
{
    public PlayerMapper() : base(config =>
    {
        config.CreateMap<Player, PlayerDto>();
        config.CreateMap<PlayerDto, Player>();
    })
    { }
}
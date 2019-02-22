using System;
using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class PlayerMapper : Mapper<Player, PlayerDto>
    {
        private readonly IMapper _mapper;

        public PlayerMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Player, PlayerDto>();
                cfg.CreateMap<PlayerDto, Player>();
            });
            _mapper = config.CreateMapper();
        }

        public override Player Map(PlayerDto dto)
        {
            throw new NotImplementedException();
        }

        public override PlayerDto Map(Player model)
        {
            throw new NotImplementedException();
        }
    }
}
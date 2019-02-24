using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class BotMapper : Mapper<Bot, BotDto>
    {
        private readonly IMapper _mapper;

        public BotMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bot, BotDto>();
                cfg.CreateMap<BotDto, Bot>();
            });
            _mapper = config.CreateMapper();
        }

        public override Bot Map(BotDto dto)
        {
            return _mapper.Map<Bot>(dto);
        }

        public override BotDto Map(Bot model)
        {
            return _mapper.Map<BotDto>(model);
        }
    }
}
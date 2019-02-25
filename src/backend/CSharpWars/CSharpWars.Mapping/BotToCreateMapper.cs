using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class BotToCreateMapper : Mapper<Bot, BotToCreateDto>
    {
        private readonly IMapper _mapper;

        public BotToCreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bot, BotToCreateDto>();
                cfg.CreateMap<BotToCreateDto, Bot>();
            });
            _mapper = config.CreateMapper();
        }

        public override Bot Map(BotToCreateDto dto)
        {
            return _mapper.Map<Bot>(dto);
        }

        public override BotToCreateDto Map(Bot model)
        {
            return _mapper.Map<BotToCreateDto>(model);
        }
    }
}
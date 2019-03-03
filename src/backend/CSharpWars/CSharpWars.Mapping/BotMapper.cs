using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class BotMapper : Mapper<Bot, BotDto>
    {
        public BotMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bot, BotDto>();
                cfg.CreateMap<BotDto, Bot>();
            });
            _mapper = config.CreateMapper();
        }
    }
}
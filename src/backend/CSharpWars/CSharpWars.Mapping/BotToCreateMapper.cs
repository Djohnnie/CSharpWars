using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class BotToCreateMapper : Mapper<Bot, BotToCreateDto>
    {
        public BotToCreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bot, BotToCreateDto>();
                cfg.CreateMap<BotToCreateDto, Bot>();
            });
            _mapper = config.CreateMapper();
        }
    }
}
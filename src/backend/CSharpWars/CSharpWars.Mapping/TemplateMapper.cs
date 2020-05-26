using AutoMapper;
using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping
{
    public class TemplateMapper : Mapper<Template, TemplateDto>
    {
        public TemplateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Template, TemplateDto>();
                cfg.CreateMap<TemplateDto, Template>();
            });
            _mapper = config.CreateMapper();
        }
    }
}
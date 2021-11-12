using CSharpWars.DtoModel;
using CSharpWars.Model;

namespace CSharpWars.Mapping;

public class TemplateMapper : Mapper<Template, TemplateDto>
{
    public TemplateMapper() : base(config =>
    {
        config.CreateMap<Template, TemplateDto>();
        config.CreateMap<TemplateDto, Template>();
    })
    { }
}
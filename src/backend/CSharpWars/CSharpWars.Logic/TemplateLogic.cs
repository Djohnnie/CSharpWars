using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Constants;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class TemplateLogic : ITemplateLogic
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly IMapper<Template, TemplateDto> _templateMapper;

        public TemplateLogic(
            IRepository<Template> templateRepository,
            IMapper<Template, TemplateDto> templateMapper)
        {
            _templateRepository = templateRepository;
            _templateMapper = templateMapper;
        }

        public async Task<IList<TemplateDto>> GetAllTemplates()
        {
            var templates = await _templateRepository.GetAll();
            if (templates.Count == 0)
            {
                templates = BotScripts.All;
            }

            return _templateMapper.Map(templates);
        }
    }
}
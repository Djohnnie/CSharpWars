using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic.Interfaces
{
    public interface ITemplateLogic
    {
        Task<IList<TemplateDto>> GetAllTemplates();
    }
}
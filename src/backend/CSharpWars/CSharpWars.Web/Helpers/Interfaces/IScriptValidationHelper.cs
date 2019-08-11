using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Web.Helpers.Interfaces
{
    public interface IScriptValidationHelper
    {
        Task<ValidatedScriptDto> Validate(ScriptToValidateDto script);
    }
}
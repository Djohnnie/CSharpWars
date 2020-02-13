using System.Threading.Tasks;

namespace CSharpWars.Validator.Helpers.Interfaces
{
    public interface IScriptValidationHelper
    {
        Task<ScriptValidationResponse> Validate(ScriptValidationRequest script);
    }
}
using System.Threading.Tasks;
using CSharpWars.Validator.Helpers.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CSharpWars.Validator.Services
{
    public class ScriptValidatorService : ScriptValidator.ScriptValidatorBase
    {
        private readonly IScriptValidationHelper _scriptValidationHelper;
        private readonly ILogger<ScriptValidatorService> _logger;

        public ScriptValidatorService(
            IScriptValidationHelper scriptValidationHelper,
            ILogger<ScriptValidatorService> logger)
        {
            _scriptValidationHelper = scriptValidationHelper;
            _logger = logger;
        }

        public override Task<ScriptValidationResponse> Validate(ScriptValidationRequest request, ServerCallContext context)
        {
            return _scriptValidationHelper.Validate(request);
        }
    }
}
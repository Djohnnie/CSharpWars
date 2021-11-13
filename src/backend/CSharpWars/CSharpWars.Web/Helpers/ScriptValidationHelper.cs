using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Validator;
using CSharpWars.Web.Helpers.Interfaces;
using Grpc.Net.Client;
using ScriptValidationMessage = CSharpWars.DtoModel.ScriptValidationMessage;

namespace CSharpWars.Web.Helpers;

public class ScriptValidationHelper : IScriptValidationHelper
{
    private readonly IConfigurationHelper _configurationHelper;
    private readonly ILogger<ScriptValidationHelper> _logger;

    public ScriptValidationHelper(
        IConfigurationHelper configurationHelper,
        ILogger<ScriptValidationHelper> logger)
    {
        _configurationHelper = configurationHelper;
        _logger = logger;
    }

    public async Task<ValidatedScriptDto> Validate(ScriptToValidateDto script)
    {
        try
        {
            var request = new ScriptValidationRequest { Script = script.Script };

            var validationHost = _configurationHelper.ValidationHost;
            var channel = GrpcChannel.ForAddress(validationHost);
            var client = new ScriptValidator.ScriptValidatorClient(channel);
            var response = await client.ValidateAsync(request);

            return new ValidatedScriptDto(script.Script, response.CompilationTimeInMilliseconds, response.RunTimeInMilliseconds,
                new List<ScriptValidationMessage>(response.ValidationMessages.Select(x => new ScriptValidationMessage(x.Message, x.LocationStart, x.LocationEnd)))
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ValidatedScriptDto(script.Script, 0, 0, new() { new(ex.Message, 0, 0) });
        }
    }
}
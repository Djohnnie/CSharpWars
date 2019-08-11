using System.Threading.Tasks;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Web.Helpers.Interfaces;
using RestSharp;

namespace CSharpWars.Web.Helpers
{
    public class ScriptValidationHelper : IScriptValidationHelper
    {
        private readonly IConfigurationHelper _configurationHelper;

        public ScriptValidationHelper(
            IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public async Task<ValidatedScriptDto> Validate(ScriptToValidateDto script)
        {
            try
            {
                var validationClient = new RestClient(_configurationHelper.ValidationHost);
                var validationRequest = new RestRequest("api/validation", Method.POST);
                validationRequest.AddJsonBody(script);
                var validationResponse = await validationClient.ExecuteTaskAsync<ValidatedScriptDto>(validationRequest);
                if (validationResponse.IsSuccessful)
                {
                    return validationResponse.Data;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
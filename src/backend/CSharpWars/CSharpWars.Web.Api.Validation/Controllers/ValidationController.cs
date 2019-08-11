using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Web.Api.Validation.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Validation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly ScriptValidationHelper _scriptValidationHelper;

        public ValidationController(ScriptValidationHelper scriptValidationHelper)
        {
            _scriptValidationHelper = scriptValidationHelper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBot([FromBody] ScriptToValidateDto script)
        {
            var validationResult = await _scriptValidationHelper.Validate(script);
            return Ok(validationResult);
        }
    }
}
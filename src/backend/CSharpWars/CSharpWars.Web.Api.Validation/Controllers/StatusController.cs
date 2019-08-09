using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Validation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetStatus()
        {
            return Task.FromResult((IActionResult)Ok());
        }
    }
}
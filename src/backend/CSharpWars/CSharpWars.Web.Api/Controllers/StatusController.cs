using System.Threading.Tasks;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ApiController
    {
        [HttpGet]
        public Task<IActionResult> GetStatus()
        {
            return Success();
        }
    }
}
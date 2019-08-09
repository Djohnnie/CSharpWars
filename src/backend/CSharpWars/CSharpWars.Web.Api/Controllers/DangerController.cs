using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangerController : ApiController<IDangerLogic>
    {
        public DangerController(IDangerLogic dangerLogic) : base(dangerLogic) { }

        [HttpDelete]
        [Route("messages")]
        public Task<IActionResult> CleanupMessages()
        {
            return Success(l => l.CleanupMessages());
        }

        [HttpDelete]
        [Route("bots")]
        public Task<IActionResult> CleanupBots()
        {
            return Success(l => l.CleanupBots());
        }

        [HttpDelete]
        [Route("players")]
        public Task<IActionResult> CleanupPlayers()
        {
            return Success(l => l.CleanupPlayers());
        }
    }
}
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ApiController<IBotLogic>
    {
        public BotController(IBotLogic botLogic) : base(botLogic) { }

        [HttpGet]
        public Task<IActionResult> GetAllActiveBots()
        {
            return Ok(l => l.GetAllActiveBots());
        }

        [HttpPost]
        public Task<IActionResult> CreateBot([FromBody] BotToCreateDto bot)
        {
            return Created(l => l.CreateBot(bot));
        }
    }
}
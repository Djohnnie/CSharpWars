using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotsController : ApiController<IBotLogic>
    {
        public BotsController(IBotLogic botLogic, IMemoryCache memoryCache) : base(botLogic, memoryCache) { }

        [HttpGet]
        public Task<IActionResult> GetAllActiveBots()
        {
            return Success(l => l.GetAllActiveBots());
        }

        [HttpPost]
        public Task<IActionResult> CreateBot([FromBody] BotToCreateDto bot)
        {
            return Created(l => l.CreateBot(bot));
        }
    }
}
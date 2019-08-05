using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ApiController<IPlayerLogic>
    {
        public PlayersController(IPlayerLogic playerLogic) : base(playerLogic) { }

        [HttpGet]
        public Task<IActionResult> GetAllPlayers()
        {
            return Success(l => l.GetAllPlayers());
        }
    }
}
using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArenaController : ApiController<IArenaLogic>
    {
        public ArenaController(IArenaLogic arenaLogic) : base(arenaLogic) { }

        // GET api/values
        [HttpGet]
        public Task<IActionResult> GetArena()
        {
            return Success(l => l.GetArena());
        }
    }
}
using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ApiController<IMessageLogic>
    {
        public MessagesController(IMessageLogic messageLogic, IMemoryCache memoryCache) : base(messageLogic, memoryCache) { }

        [HttpGet]
        public Task<IActionResult> GetLastMessages()
        {
            return Success(l => l.GetLastMessages());
        }
    }
}
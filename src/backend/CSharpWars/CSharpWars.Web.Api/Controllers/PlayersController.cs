using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
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

        // GET api/values
        [HttpGet]
        public Task<IActionResult> GetAllPlayers()
        {
            return Ok(l => l.GetAllPlayers());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
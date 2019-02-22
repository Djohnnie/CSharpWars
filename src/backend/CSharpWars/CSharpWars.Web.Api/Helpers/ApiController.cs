using System;
using System.Threading.Tasks;
using CSharpWars.Logic.Exceptions;
using CSharpWars.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Api.Helpers
{
    public class ApiController : ControllerBase { }

    public class ApiController<TLogic> : ApiController where TLogic : ILogic
    {
        private readonly TLogic _logic;

        protected ApiController(TLogic logic)
        {
            _logic = logic;
        }

        protected async Task<IActionResult> Ok<TResult>(Func<TLogic, Task<TResult>> logicCall)
        {
            return await Try(async () =>
            {
                var result = await logicCall(_logic);
                return result != null ? Ok(result) : (ActionResult)NotFound();
            });
        }

        protected async Task<IActionResult> Ok(Func<TLogic, Task> logicCall)
        {
            return await Try(async () =>
            {
                await logicCall(_logic);
                return Ok();
            });
        }

        private async Task<IActionResult> Try(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (LogicException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
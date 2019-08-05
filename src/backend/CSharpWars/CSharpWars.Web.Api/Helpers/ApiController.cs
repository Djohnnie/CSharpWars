using System;
using System.Threading.Tasks;
using CSharpWars.Logic.Exceptions;
using CSharpWars.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CSharpWars.Web.Api.Helpers
{
    public class ApiController : ControllerBase
    {
        protected Task<IActionResult> Success()
        {
            return Task.FromResult((IActionResult)Ok());
        }
    }

    public class ApiController<TLogic> : ApiController where TLogic : ILogic
    {
        private readonly TLogic _logic;
        private readonly IMemoryCache _memoryCache;

        protected ApiController(TLogic logic)
        {
            _logic = logic;
        }

        protected ApiController(TLogic logic, IMemoryCache memoryCache)
        {
            _logic = logic;
            _memoryCache = memoryCache;
        }

        protected async Task<IActionResult> Success<TResult>(Func<TLogic, Task<TResult>> logicCall)
        {
            return await Try(async () =>
            {
                if (_memoryCache != null)
                {
                    if (!_memoryCache.TryGetValue(logicCall.Method.Name, out TResult result))
                    {
                        result = await logicCall(_logic);
                        _memoryCache.Set(logicCall.Method.Name, result, TimeSpan.FromSeconds(1));
                    }
                    return result != null ? Ok(result) : (ActionResult)NotFound();
                }
                else
                {
                    TResult result = await logicCall(_logic);
                    return result != null ? Ok(result) : (ActionResult)NotFound();
                }
            });
        }

        protected async Task<IActionResult> Success(Func<TLogic, Task> logicCall)
        {
            return await Try(async () =>
            {
                await logicCall(_logic);
                return Ok();
            });
        }

        protected async Task<IActionResult> Created<TResult>(Func<TLogic, Task<TResult>> logicCall)
        {
            return await Try(async () =>
            {
                var result = await logicCall(_logic);
                return result != null ? Created("", result) : (ActionResult)NotFound();
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
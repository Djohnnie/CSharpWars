using System;
using System.Linq;
using CSharpWars.DtoModel;
using CSharpWars.Web.Constants;
using CSharpWars.Web.Extensions;
using CSharpWars.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                return RedirectToAction(nameof(Play));
            }

            var vm = new PlayerViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult Index(PlayerViewModel vm)
        {
            var player = new PlayerDto
            {
                Name = vm.Name,
                Secret = vm.Secret
            };
            HttpContext.Session.SetObject("PLAYER", player);
            return RedirectToAction(nameof(Play));
        }

        public IActionResult Play()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");
                var vm = new GameViewModel
                {
                    PlayerName = player.Name,
                    BotHealth = 100,
                    BotStamina = 100,
                    BotScript = BotScripts.WalkAround
                };
                return View(vm);
            }

            return RedirectToAction(nameof(Play));
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Constants;
using CSharpWars.Web.Extensions;
using CSharpWars.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Controllers
{
    public class PlayController : Controller
    {
        private readonly IBotLogic _botLogic;

        public PlayController(IBotLogic botLogic)
        {
            _botLogic = botLogic;
        }

        public IActionResult Template()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");
                var vm = new PlayViewModel
                {
                    PlayerName = player.Name,
                    BotName = "<name your bot>",
                    BotHealth = 100,
                    BotStamina = 100,
                    Scripts = BotScripts.All
                };
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Template(PlayViewModel vm)
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var valid = IsValid(vm);
                if (valid)
                {
                    var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");

                    var botToCreate = new BotToCreateDto
                    {
                        PlayerId = player.Id,
                        Name = vm.BotName,
                        MaximumHealth = vm.BotHealth,
                        MaximumStamina = vm.BotStamina,
                        Script = BotScripts.All.Single(x => x.Id == vm.SelectedScript).Script.Base64Encode()
                    };

                    await _botLogic.CreateBot(botToCreate);
                }

                vm = new PlayViewModel
                {
                    PlayerName = vm.PlayerName,
                    BotName = vm.BotName,
                    BotHealth = vm.BotHealth,
                    BotStamina = vm.BotStamina,
                    SelectedScript = vm.SelectedScript,
                    Scripts = BotScripts.All
                };

                if (valid)
                {
                    vm.HappyMessage = $"{vm.BotName} for player {vm.PlayerName} has been created successfully!";
                }
                else
                {
                    vm.SadMessage = "You have made some errors!";
                }

                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Custom()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");
                var vm = new PlayViewModel
                {
                    PlayerName = player.Name,
                    BotName = "<name your bot>",
                    BotHealth = 100,
                    BotStamina = 100,
                    Script = BotScripts.WalkBackAndForth
                };
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Custom(PlayViewModel vm)
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var valid = IsValid(vm);
                if (valid)
                {
                    var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");

                    var botToCreate = new BotToCreateDto
                    {
                        PlayerId = player.Id,
                        Name = vm.BotName,
                        MaximumHealth = vm.BotHealth,
                        MaximumStamina = vm.BotStamina,
                        Script = vm.Script.Base64Encode()
                    };

                    await _botLogic.CreateBot(botToCreate);
                }

                vm = new PlayViewModel
                {
                    PlayerName = vm.PlayerName,
                    BotName = vm.BotName,
                    BotHealth = vm.BotHealth,
                    BotStamina = vm.BotStamina,
                    Script = vm.Script
                };

                if (valid)
                {
                    vm.HappyMessage = $"{vm.BotName} for player {vm.PlayerName} has been created successfully!";
                }
                else
                {
                    vm.SadMessage = "You have made some errors!";
                }

                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        private Boolean IsValid(PlayViewModel vm)
        {
            var validBotName = !String.IsNullOrEmpty(vm.BotName);
            var validHealthAndStamina = vm.BotHealth > 0 && vm.BotStamina > 0;
            var validScript = vm.SelectedScript != Guid.Empty || !string.IsNullOrEmpty(vm.Script);
            return validBotName && validHealthAndStamina && validScript;
        }
    }
}
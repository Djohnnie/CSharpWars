using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Constants;
using CSharpWars.Logic.Exceptions;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Extensions;
using CSharpWars.Web.Helpers.Interfaces;
using CSharpWars.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CSharpWars.Web.Controllers
{
    public class PlayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IBotLogic _botLogic;
        private readonly ITemplateLogic _templateLogic;
        private readonly IScriptValidationHelper _scriptValidationHelper;
        private readonly IConfigurationHelper _configurationHelper;

        public PlayController(
            IConfiguration configuration,
            IBotLogic botLogic,
            ITemplateLogic templateLogic,
            IScriptValidationHelper scriptValidationHelper,
            IConfigurationHelper configurationHelper)
        {
            _configuration = configuration;
            _botLogic = botLogic;
            _templateLogic = templateLogic;
            _scriptValidationHelper = scriptValidationHelper;
            _configurationHelper = configurationHelper;
        }

        public async Task<IActionResult> Template()
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
                    Scripts = await _templateLogic.GetAllTemplates()
                };
                ViewData["ArenaUrl"] = _configuration.GetValue<string>("ARENA_URL");
                ViewData["ScriptTemplateUrl"] = _configuration.GetValue<string>("SCRIPT_TEMPLATE_URL");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Template(PlayViewModel vm)
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");

                var templates = await _templateLogic.GetAllTemplates();

                var valid = IsValid(vm);
                string sadMessage = "You have made some errors!";

                if (valid)
                {
                    var script = templates.Single(x => x.Id == vm.SelectedScript).Script.Base64Encode();

                    var scriptValidationResult = await _scriptValidationHelper.Validate(
                        new ScriptToValidateDto { Script = script });

                    if (scriptValidationResult != null && scriptValidationResult.Messages.Count == 0)
                    {
                        var botToCreate = new BotToCreateDto
                        {
                            PlayerId = player.Id,
                            Name = vm.BotName,
                            MaximumHealth = vm.BotHealth,
                            MaximumStamina = vm.BotStamina,
                            Script = script
                        };

                        try
                        {
                            await _botLogic.CreateBot(botToCreate);
                        }
                        catch (LogicException ex)
                        {
                            valid = false;
                            sadMessage = ex.Message;
                        }
                    }
                    else
                    {
                        valid = false;
                        if (scriptValidationResult == null)
                        {
                            sadMessage = "Your script could not be validated for an unknown reason.";
                        }
                        else
                        {
                            var scriptErrors = string.Join(", ", scriptValidationResult.Messages.Select(x => x.Message));
                            sadMessage = $"Your script contains some compile errors: {scriptErrors}";
                        }
                    }
                }

                vm = new PlayViewModel
                {
                    PlayerName = player.Name,
                    BotName = vm.BotName,
                    BotHealth = vm.BotHealth,
                    BotStamina = vm.BotStamina,
                    SelectedScript = vm.SelectedScript,
                    Scripts = templates
                };

                if (valid)
                {
                    vm.HappyMessage = $"{vm.BotName} for player {vm.PlayerName} has been created successfully!";
                }
                else
                {
                    vm.SadMessage = sadMessage;
                }

                ViewData["ArenaUrl"] = _configuration.GetValue<string>("ARENA_URL");
                ViewData["ScriptTemplateUrl"] = _configuration.GetValue<string>("SCRIPT_TEMPLATE_URL");
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
                    Script = BotScripts.WalkAround
                };
                ViewData["ArenaUrl"] = _configuration.GetValue<string>("ARENA_URL");
                ViewData["ScriptTemplateUrl"] = _configuration.GetValue<string>("SCRIPT_TEMPLATE_URL");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Custom(PlayViewModel vm)
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");

                var valid = IsValid(vm);
                string sadMessage = "You have made some errors!";

                if (valid)
                {
                    var script = vm.Script.Base64Encode();

                    var scriptValidationResult = await _scriptValidationHelper.Validate(
                        new ScriptToValidateDto { Script = script });

                    if (scriptValidationResult != null && scriptValidationResult.Messages.Count == 0)
                    {
                        var botToCreate = new BotToCreateDto
                        {
                            PlayerId = player.Id,
                            Name = vm.BotName,
                            MaximumHealth = vm.BotHealth,
                            MaximumStamina = vm.BotStamina,
                            Script = script
                        };

                        try
                        {
                            await _botLogic.CreateBot(botToCreate);
                        }
                        catch (LogicException ex)
                        {
                            valid = false;
                            sadMessage = ex.Message;
                        }
                    }
                    else
                    {
                        valid = false;
                        if (scriptValidationResult == null)
                        {
                            sadMessage = "Your script could not be validated for an unknown reason.";
                        }
                        else
                        {
                            var scriptErrors = string.Join(", ", scriptValidationResult.Messages.Select(x => x.Message));
                            sadMessage = $"Your script contains some compile errors: {scriptErrors}";
                        }
                    }
                }

                vm = new PlayViewModel
                {
                    PlayerName = player.Name,
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
                    vm.SadMessage = sadMessage;
                }

                ViewData["ArenaUrl"] = _configuration.GetValue<string>("ARENA_URL");
                ViewData["ScriptTemplateUrl"] = _configuration.GetValue<string>("SCRIPT_TEMPLATE_URL");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(PlayViewModel vm)
        {
            var validBotName = !string.IsNullOrEmpty(vm.BotName);
            var validHealthAndStamina = vm.BotHealth > 0 && vm.BotStamina > 0 && vm.BotHealth + vm.BotStamina <= _configurationHelper.PointsLimit;
            var validScript = vm.SelectedScript != Guid.Empty || !string.IsNullOrEmpty(vm.Script);
            return validBotName && validHealthAndStamina && validScript;
        }
    }
}
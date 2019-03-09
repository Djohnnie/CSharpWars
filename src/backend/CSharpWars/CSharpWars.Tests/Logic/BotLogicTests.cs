using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping;
using CSharpWars.Model;
using CSharpWars.Tests.Framework.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Logic
{
    public class BotLogicTests
    {
        [Fact]
        public async Task BotLogic_GetAllActiveBots_Should_Only_Return_Active_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            IBotLogic botLogic = new BotLogic(randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object);

            var bots = new List<Bot>
            {
                new Bot
                {
                    CurrentHealth = 1
                }
            };

            // Mock
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>()))
                .ReturnsAsync((Expression<Func<Bot, Boolean>> predicate) =>
                    (IList<Bot>)bots.Where(predicate.Compile()).ToList());

            // Act
            var result = await botLogic.GetAllActiveBots();

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task BotLogic_GetBotScript_Should_Return_Correct_Script()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            IBotLogic botLogic = new BotLogic(randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object);

            var botId = Guid.NewGuid();
            var script = "BotScript!";
            var botScripts = new List<BotScript>
            {
                new BotScript
                {
                    Id = botId,
                    Script = script
                }
            };

            // Mock
            scriptRepository.Setup(x => x.Single(Any.Predicate<BotScript>()))
                .ReturnsAsync((Expression<Func<BotScript, Boolean>> predicate) => botScripts.SingleOrDefault(predicate.Compile()));

            // Act
            var result = await botLogic.GetBotScript(botId);

            // Assert
            result.Should().Be(script);
        }

        [Fact]
        public async Task BotLogic_CreateBot_Should_Create_A_Bot()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            IBotLogic botLogic = new BotLogic(randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object);

            var arenaDto = new ArenaDto { Width = 4, Height = 3 };
            var botToCreateDto = new BotToCreateDto
            {
                Name = "BotName",
                MaximumHealth = 100,
                MaximumStamina = 200,
                Script = "BotScript"
            };
            var botScripts = new List<BotScript>
            {
                new BotScript
                {
                    Id = Guid.Empty,
                    Script = botToCreateDto.Script
                }
            };

            // Mock
            randomHelper.Setup(x => x.Get<PossibleOrientations>()).Returns(PossibleOrientations.South);
            randomHelper.Setup(x => x.Get(It.IsAny<Int32>())).Returns(2);
            arenaLogic.Setup(x => x.GetArena()).ReturnsAsync(arenaDto);
            botScriptRepository.Setup(x => x.Create(It.IsAny<Bot>())).Returns<Bot>(Task.FromResult);
            scriptRepository.Setup(x => x.Single(Any.Predicate<BotScript>()))
                .ReturnsAsync((Expression<Func<BotScript, Boolean>> predicate) => botScripts.SingleOrDefault(predicate.Compile()));
            scriptRepository.Setup(x => x.Update(It.IsAny<BotScript>())).Returns<BotScript>(Task.FromResult);

            // Act
            var result = await botLogic.CreateBot(botToCreateDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(botToCreateDto.Name);
            result.Orientation.Should().Be(PossibleOrientations.South);
            result.X.Should().Be(2);
            result.Y.Should().Be(2);
            result.MaximumHealth.Should().Be(botToCreateDto.MaximumHealth);
            result.CurrentHealth.Should().Be(botToCreateDto.MaximumHealth);
            result.MaximumStamina.Should().Be(botToCreateDto.MaximumStamina);
            result.CurrentStamina.Should().Be(botToCreateDto.MaximumStamina);
            result.Memory.Should().NotBeNull();
        }

        [Fact]
        public async Task BotLogic_UpdateBots_Should_Update_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            IBotLogic botLogic = new BotLogic(randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object);

            var botDto = new BotDto();

            // Mock
            botRepository.Setup(x => x.Update(It.IsAny<IList<Bot>>())).Returns<IList<Bot>>(Task.FromResult);

            // Act
            await botLogic.UpdateBots(new[] { botDto }.ToList());

            // Assert
            botRepository.Verify(x => x.Update(It.IsAny<IList<Bot>>()), Times.Once);
        }
    }
}
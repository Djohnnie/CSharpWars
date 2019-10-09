using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic;
using CSharpWars.Logic.Exceptions;
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
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var bots = new List<Bot>
            {
                new Bot
                {
                    CurrentHealth = 1,
                    Player = new Player { Name="PlayerName" }
                }
            };

            // Mock
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>(), Any.Include<Bot, Player>()))
                .ReturnsAsync((Expression<Func<Bot, Boolean>> predicate, Expression<Func<Bot, Player>> include) =>
                    (IList<Bot>)bots.Where(predicate.Compile()).ToList());

            // Act
            var result = await botLogic.GetAllActiveBots();

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(x => x.PlayerName == "PlayerName");
        }

        [Fact]
        public async Task BotLogic_GetAllActiveBots_Should_Only_Return_Recently_Died_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var bots = new List<Bot>
            {
                new Bot
                {
                    Name = "BOT1",
                    CurrentHealth = 0,
                    TimeOfDeath = DateTime.UtcNow.AddSeconds(-11)
                },
                new Bot
                {
                    Name = "BOT2",
                    Id = Guid.NewGuid(),
                    CurrentHealth = 0,
                    TimeOfDeath = DateTime.UtcNow.AddSeconds(-1)
                }
            };

            // Mock
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>(), Any.Include<Bot, Player>()))
                .ReturnsAsync((Expression<Func<Bot, Boolean>> predicate, Expression<Func<Bot, Player>> include) =>
                    (IList<Bot>)bots.Where(predicate.Compile()).ToList());

            // Act
            var result = await botLogic.GetAllActiveBots();

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(x => x.Name == "BOT2");
        }

        [Fact]
        public async Task BotLogic_GetAllLiveBots_Should_Only_Return_Live_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var bots = new List<Bot>
            {
                new Bot
                {
                    Name = "BOT1",
                    CurrentHealth = 0
                },
                new Bot
                {
                    Name = "BOT2",
                    CurrentHealth = 1
                },
                new Bot
                {
                    Name = "BOT3",
                    CurrentHealth = 0
                }
            };

            // Mock
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>(), Any.Include<Bot, Player>()))
                .ReturnsAsync((Expression<Func<Bot, Boolean>> predicate, Expression<Func<Bot, Player>> include) =>
                    (IList<Bot>)bots.Where(predicate.Compile()).ToList());

            // Act
            var result = await botLogic.GetAllLiveBots();

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(x => x.Name == "BOT2");
        }

        [Fact]
        public async Task BotLogic_GetBotScript_Should_Return_Correct_Script()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

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
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var arenaDto = new ArenaDto { Width = 4, Height = 3 };
            var player = new Player { Id = Guid.NewGuid(), LastDeployment = DateTime.MinValue };
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
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>(), i => i.Player)).ReturnsAsync(new List<Bot>());
            randomHelper.Setup(x => x.Get(It.IsAny<Int32>())).Returns(5);
            arenaLogic.Setup(x => x.GetArena()).ReturnsAsync(arenaDto);
            botScriptRepository.Setup(x => x.Create(It.IsAny<Bot>())).Returns<Bot>(Task.FromResult);
            playerRepository.Setup(x => x.Single(Any.Predicate<Player>())).ReturnsAsync(player);
            scriptRepository.Setup(x => x.Single(Any.Predicate<BotScript>()))
                .ReturnsAsync((Expression<Func<BotScript, Boolean>> predicate) => botScripts.SingleOrDefault(predicate.Compile()));
            scriptRepository.Setup(x => x.Update(It.IsAny<BotScript>())).Returns<BotScript>(Task.FromResult);

            // Act
            var result = await botLogic.CreateBot(botToCreateDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(botToCreateDto.Name);
            result.Orientation.Should().Be(PossibleOrientations.South);
            result.X.Should().Be(1);
            result.Y.Should().Be(2);
            result.FromX.Should().Be(1);
            result.FromY.Should().Be(2);
            result.MaximumHealth.Should().Be(botToCreateDto.MaximumHealth);
            result.CurrentHealth.Should().Be(botToCreateDto.MaximumHealth);
            result.MaximumStamina.Should().Be(botToCreateDto.MaximumStamina);
            result.CurrentStamina.Should().Be(botToCreateDto.MaximumStamina);
            result.Memory.Should().NotBeNull();
            result.TimeOfDeath.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public async Task BotLogic_CreateBot_Should_Create_A_Bot_And_Not_Overlap_With_Existing_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var arenaDto = new ArenaDto { Width = 3, Height = 3 };
            var player = new Player { Id = Guid.NewGuid(), LastDeployment = DateTime.MinValue };
            var botToCreateDto = new BotToCreateDto
            {
                Name = "BotName",
                MaximumHealth = 100,
                MaximumStamina = 200,
                Script = "BotScript"
            };
            var otherBots = new List<Bot>
            {
                new Bot { X = 0, Y = 0 },
                new Bot { X = 1, Y = 0 },
                new Bot { X = 2, Y = 0 },
                new Bot { X = 0, Y = 1 },
                new Bot { X = 2, Y = 1 },
                new Bot { X = 0, Y = 2 },
                new Bot { X = 1, Y = 2 },
                new Bot { X = 2, Y = 2 },
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
            botRepository.Setup(x => x.Find(Any.Predicate<Bot>(), i => i.Player)).ReturnsAsync(otherBots);
            randomHelper.Setup(x => x.Get(It.IsAny<Int32>())).Returns(0);
            arenaLogic.Setup(x => x.GetArena()).ReturnsAsync(arenaDto);
            botScriptRepository.Setup(x => x.Create(It.IsAny<Bot>())).Returns<Bot>(Task.FromResult);
            playerRepository.Setup(x => x.Single(Any.Predicate<Player>())).ReturnsAsync(player);
            scriptRepository.Setup(x => x.Single(Any.Predicate<BotScript>()))
                .ReturnsAsync((Expression<Func<BotScript, Boolean>> predicate) => botScripts.SingleOrDefault(predicate.Compile()));
            scriptRepository.Setup(x => x.Update(It.IsAny<BotScript>())).Returns<BotScript>(Task.FromResult);

            // Act
            var result = await botLogic.CreateBot(botToCreateDto);

            // Assert
            result.Should().NotBeNull();
            result.X.Should().Be(1);
            result.Y.Should().Be(1);
        }

        [Fact]
        public async Task BotLogic_CreateBot_Should_Not_Create_A_Bot_In_Rapid_Succession()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

            var arenaDto = new ArenaDto { Width = 3, Height = 3 };
            var player = new Player { Id = Guid.NewGuid(), LastDeployment = DateTime.UtcNow };
            var botToCreateDto = new BotToCreateDto
            {
                Name = "BotName",
                MaximumHealth = 100,
                MaximumStamina = 200,
                Script = "BotScript"
            };

            // Mock
            arenaLogic.Setup(x => x.GetArena()).ReturnsAsync(arenaDto);
            playerRepository.Setup(x => x.Single(Any.Predicate<Player>())).ReturnsAsync(player);
            configurationHelper.Setup(x => x.BotDeploymentLimit).Returns(1);

            // Act
            Func<Task> act = async () => await botLogic.CreateBot(botToCreateDto);

            // Assert
            act.Should().Throw<LogicException>().WithMessage("You are not allowed to create multiple robots in rapid succession!");
        }

        [Fact]
        public async Task BotLogic_UpdateBots_Should_Update_Bots()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var botRepository = new Mock<IRepository<Bot>>();
            var scriptRepository = new Mock<IRepository<BotScript>>();
            var botScriptRepository = new Mock<IRepository<Bot, BotScript>>();
            var playerRepository = new Mock<IRepository<Player>>();
            var botMapper = new BotMapper();
            var botToCreateMapper = new BotToCreateMapper();
            var arenaLogic = new Mock<IArenaLogic>();
            var configurationHelper = new Mock<IConfigurationHelper>();
            IBotLogic botLogic = new BotLogic(
                randomHelper.Object, botRepository.Object, scriptRepository.Object, botScriptRepository.Object,
                playerRepository.Object, botMapper, botToCreateMapper, arenaLogic.Object, configurationHelper.Object);

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
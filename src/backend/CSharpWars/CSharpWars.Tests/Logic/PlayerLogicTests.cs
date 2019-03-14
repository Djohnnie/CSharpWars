using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
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
    public class PlayerLogicTests
    {
        [Fact]
        public async Task PlayerLogic_GetAllPlayers_Should_Return_All_Players()
        {
            // Arrange
            var playerRepository = new Mock<IRepository<Player>>();
            IPlayerLogic playerLogic = new PlayerLogic(playerRepository.Object, new PlayerMapper());
            Player player1 = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Player1Name",
                Hashed = "Player1Secret"
            };
            Player player2 = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Player2Name",
                Hashed = "Player2Secret"
            };

            // Mock
            playerRepository.Setup(x => x.GetAll()).ReturnsAsync(new[] { player1, player2 });

            // Act
            var result = await playerLogic.GetAllPlayers();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(player1, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name));
            result.Should().ContainEquivalentOf(player2, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name));
        }

        [Fact]
        public async Task PlayerLogic_Login_Should_Create_Unknown_Player()
        {
            // Arrange
            var playerRepository = new Mock<IRepository<Player>>();
            IPlayerLogic playerLogic = new PlayerLogic(playerRepository.Object, new PlayerMapper());
            var login = new LoginDto
            {
                Name = "Player",
                Secret = "Pa$$w0rd"
            };

            // Mock

            playerRepository.Setup(x => x.Single(Any.Predicate<Player>())).ReturnsAsync((Player)null);
            playerRepository.Setup(x => x.Create(It.IsAny<Player>())).ReturnsAsync((Player player) => player);

            // Act
            var result = await playerLogic.Login(login);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(login.Name);
            playerRepository.Verify(x => x.Create(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async Task PlayerLogic_Login_Should_Return_Null_For_Known_Player_With_Wrong_Password()
        {
            // Arrange
            var playerRepository = new Mock<IRepository<Player>>();
            IPlayerLogic playerLogic = new PlayerLogic(playerRepository.Object, new PlayerMapper());
            var player = new Player
            {
                Name = "Player"
            };
            var login = new LoginDto
            {
                Name = "Player",
                Secret = "Pa$$w0rd"
            };

            // Mock
            playerRepository.Setup(x => x.Single(Any.Predicate<Player>()))
                .ReturnsAsync((Expression<Func<Player, Boolean>> predicate) =>
                    new[] { player }.SingleOrDefault(predicate.Compile()));

            // Act
            var result = await playerLogic.Login(login);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task PlayerLogic_Login_Should_Return_Known_Player()
        {
            // Arrange
            var playerRepository = new Mock<IRepository<Player>>();
            IPlayerLogic playerLogic = new PlayerLogic(playerRepository.Object, new PlayerMapper());
            var player = new Player
            {
                Name = "Player",
                Salt = "08IhqtPCEwbCSXj6LzduXA==",
                Hashed = "2aYxVxvAwYw4uT0ehZGC1CjHnwY3Wohyg8wu21zI5CM="
            };
            var login = new LoginDto
            {
                Name = "Player",
                Secret = "Pa$$w0rd"
            };

            // Mock
            playerRepository.Setup(x => x.Single(Any.Predicate<Player>()))
                .ReturnsAsync((Expression<Func<Player, Boolean>> predicate) =>
                    new[] { player }.SingleOrDefault(predicate.Compile()));

            // Act
            var result = await playerLogic.Login(login);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(player.Name);
        }
    }
}
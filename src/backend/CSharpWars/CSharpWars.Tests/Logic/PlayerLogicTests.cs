using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Logic;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping;
using CSharpWars.Model;
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
            Player player1 = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Player1Name",
                Secret = "Player1Secret"
            };
            Player player2 = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Player2Name",
                Secret = "Player2Secret"
            };

            var playerHelper = new Mock<IRepository<Player>>();
            playerHelper.Setup(x => x.GetAll()).Returns(Task.FromResult((IList<Player>)new[] { player1, player2 }));
            IPlayerLogic playerLogic = new PlayerLogic(playerHelper.Object, new PlayerMapper());

            // Act
            var result = await playerLogic.GetAllPlayers();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(player1, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name)
                .Including(p => p.Secret));
            result.Should().ContainEquivalentOf(player2, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name)
                .Including(p => p.Secret));
        }
    }
}
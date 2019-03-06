using System.Threading.Tasks;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Logic;
using CSharpWars.Logic.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Logic
{
    public class ArenaLogicTests
    {
        [Fact]
        public async Task ArenaLogic_GetArena_Should_Return_An_Arena_Of_Size_10x10()
        {
            // Arrange
            var configurationHelper = new Mock<IConfigurationHelper>();
            configurationHelper.Setup(x => x.ArenaSize).Returns(10);
            IArenaLogic arenaLogic = new ArenaLogic(configurationHelper.Object);

            // Act
            var result = await arenaLogic.GetArena();

            // Assert
            result.Width.Should().Be(10);
            result.Height.Should().Be(10);
        }
    }
}
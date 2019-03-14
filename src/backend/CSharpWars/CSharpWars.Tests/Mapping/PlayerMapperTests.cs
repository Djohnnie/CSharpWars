using System;
using CSharpWars.DtoModel;
using CSharpWars.Mapping;
using CSharpWars.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Mapping
{
    public class PlayerMapperTests
    {
        [Fact]
        public void PlayerMapper_Can_Map_PlayerModel_To_PlayerDto()
        {
            // Arrange
            var mapper = new PlayerMapper();
            var playerModel = new Player
            {
                Id = Guid.NewGuid(),
                Name = "PlayerName",
                Hashed = "PlayerSecret"
            };

            // Act
            var playerDto = mapper.Map(playerModel);

            // Assert
            playerDto.Should().BeEquivalentTo(playerModel,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name));
        }

        [Fact]
        public void PlayerMapper_Can_Map_PlayerDto_To_PlayerModel()
        {
            // Arrange
            var mapper = new PlayerMapper();
            var playerDto = new PlayerDto
            {
                Id = Guid.NewGuid(),
                Name = "PlayerName"
            };

            // Act
            var playerModel = mapper.Map(playerDto);

            // Assert
            playerModel.Should().BeEquivalentTo(playerDto,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name));
        }
    }
}
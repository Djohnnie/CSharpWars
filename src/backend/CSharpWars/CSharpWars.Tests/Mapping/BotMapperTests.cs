using System;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Mapping;
using CSharpWars.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Mapping
{
    public class BotMapperTests
    {
        [Fact]
        public void BotMapper_Can_Map_BotModel_To_BotDto()
        {
            // Arrange
            var mapper = new BotMapper();
            var botModel = new Bot
            {
                Id = Guid.NewGuid(),
                Name = "BotName",
                X = 5,
                Y = 6,
                FromX = 7,
                FromY = 8,
                Orientation = PossibleOrientations.South,
                MaximumHealth = 100,
                CurrentHealth = 98,
                MaximumStamina = 200,
                CurrentStamina = 123,
                Move = PossibleMoves.MeleeAttack,
                Player = new Player { Name = "PlayerName" },
                LastAttackX = 1,
                LastAttackY = 2,
                TimeOfDeath = new DateTime(2000, 10, 1)
            };

            // Act
            var botDto = mapper.Map(botModel);

            // Assert
            botDto.Should().BeEquivalentTo(botModel,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name)
                    .Including(x => x.X)
                    .Including(x => x.Y)
                    .Including(x => x.FromX)
                    .Including(x => x.FromY)
                    .Including(x => x.Orientation)
                    .Including(x => x.MaximumHealth)
                    .Including(x => x.CurrentHealth)
                    .Including(x => x.MaximumStamina)
                    .Including(x => x.CurrentStamina)
                    .Including(x => x.Move)
                    .Including(x => x.LastAttackX)
                    .Including(x => x.LastAttackY)
                    .Including(x => x.TimeOfDeath));
            botDto.PlayerName.Should().Be(botModel.Player.Name);
        }

        [Fact]
        public void BotMapper_Can_Map_BotDto_To_BotModel()
        {
            // Arrange
            var mapper = new BotMapper();
            var botDto = new BotDto
            {
                Id = Guid.NewGuid(),
                Name = "BotName",
                X = 5,
                Y = 6,
                FromX = 7,
                FromY = 8,
                Orientation = PossibleOrientations.South,
                MaximumHealth = 100,
                CurrentHealth = 98,
                MaximumStamina = 200,
                CurrentStamina = 123,
                Move = PossibleMoves.MeleeAttack,
                LastAttackX = 1,
                LastAttackY = 2,
                TimeOfDeath = new DateTime(2000, 10, 1)
            };

            // Act
            var botModel = mapper.Map(botDto);

            // Assert
            botModel.Should().BeEquivalentTo(botDto,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name)
                    .Including(x => x.X)
                    .Including(x => x.Y)
                    .Including(x => x.FromX)
                    .Including(x => x.FromY)
                    .Including(x => x.Orientation)
                    .Including(x => x.MaximumHealth)
                    .Including(x => x.CurrentHealth)
                    .Including(x => x.MaximumStamina)
                    .Including(x => x.CurrentStamina)
                    .Including(x => x.Move)
                    .Including(x => x.LastAttackX)
                    .Including(x => x.LastAttackY)
                    .Including(x => x.TimeOfDeath));
        }
    }
}
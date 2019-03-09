using CSharpWars.DtoModel;
using CSharpWars.Mapping;
using CSharpWars.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Mapping
{
    public class BotToCreateMapperTests
    {
        [Fact]
        public void BotToCreateMapper_Can_Map_BotModel_To_BotToCreateDto()
        {
            // Arrange
            var mapper = new BotToCreateMapper();
            var botModel = new Bot
            {
                Name = "BotName",
                MaximumHealth = 100,
                MaximumStamina = 200
            };

            // Act
            var botToCreateDto = mapper.Map(botModel);

            // Assert
            botToCreateDto.Should().BeEquivalentTo(botModel,
                properties => properties
                    .Including(x => x.Name)
                    .Including(x => x.MaximumHealth)
                    .Including(x => x.MaximumStamina));
        }

        [Fact]
        public void BotToCreateMapper_Can_Map_BotToCreateDto_To_BotModel()
        {
            // Arrange
            var mapper = new BotToCreateMapper();
            var botToCreateDto = new BotToCreateDto
            {
                Name = "BotName",
                MaximumHealth = 100,
                MaximumStamina = 200
            };

            // Act
            var botModel = mapper.Map(botToCreateDto);

            // Assert
            botModel.Should().BeEquivalentTo(botToCreateDto,
                properties => properties
                    .Including(x => x.Name)
                    .Including(x => x.MaximumHealth)
                    .Including(x => x.MaximumStamina));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Middleware;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class PostprocessorTests
    {
        [Fact]
        public async Task Postprocessor_Go_Should_Work()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 1, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid(), X = 0, Y = 2, Orientation = PossibleOrientations.North, CurrentStamina = 10, CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 0,
                Y = 1,
                FromX = 0,
                FromY = 2,
                Orientation = PossibleOrientations.North,
                CurrentStamina = 9,
                Move = PossibleMoves.WalkForward
            }, c => c
                .Including(p => p.X)
                .Including(p => p.Y)
                .Including(p => p.FromX)
                .Including(p => p.FromY)
                .Including(p => p.Orientation)
                .Including(p => p.CurrentStamina)
                .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Kill_Bots_That_Have_Zero_Health_Left()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var bot = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 0 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.Idling;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                Move = PossibleMoves.Died
            }, c => c.Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Kill_Bots_That_Have_Negative_Health_Left()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var bot = new BotDto { Id = Guid.NewGuid(), CurrentHealth = -1 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.Idling;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                CurrentHealth = 0,
                Move = PossibleMoves.Died
            }, c => c.Including(p => p.CurrentHealth)
                           .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Kill_Bots_That_Have_Self_Destructed()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var bot = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                Move = PossibleMoves.SelfDestruct,
                CurrentHealth = 0
            }, c => c
                .Including(p => p.Move)
                .Including(p => p.CurrentHealth));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Ignore_Bots_That_Have_Zero_Stamina_Left()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid(), X = 1, Y = 1, CurrentHealth = 1, CurrentStamina = 0 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 1,
                Y = 1,
                CurrentStamina = 0,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Ignore_Bots_That_Have_Negative_Stamina_Left()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid(), X = 1, Y = 1, CurrentHealth = 1, CurrentStamina = -1 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 1,
                Y = 1,
                CurrentStamina = 0,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                .Including(p => p.Y)
                .Including(p => p.CurrentStamina)
                .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Not_Move_Two_Bots_That_Are_Facing_Each_Other()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 4, Height = 1 };
            var bot1 = new BotDto { Id = Guid.NewGuid(), X = 1, Y = 0, Orientation = PossibleOrientations.East, CurrentStamina = 3, CurrentHealth = 1 };
            var bot2 = new BotDto { Id = Guid.NewGuid(), X = 2, Y = 0, Orientation = PossibleOrientations.West, CurrentStamina = 4, CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot1, bot2 });
            var context = ProcessingContext.Build(arena, bots);
            var bot1Properties = BotProperties.Build(bot1, arena, bots);
            var bot2Properties = BotProperties.Build(bot2, arena, bots);
            bot1Properties.CurrentMove = PossibleMoves.WalkForward;
            bot2Properties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot1.Id, bot1Properties);
            context.AddBotProperties(bot2.Id, bot2Properties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(2);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 1,
                Y = 0,
                Orientation = PossibleOrientations.East,
                CurrentStamina = 3,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 2,
                Y = 0,
                Orientation = PossibleOrientations.West,
                CurrentStamina = 4,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Allow_A_Bot_To_Teleport_Onto_Another_Idling_Bot()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 4, Height = 1 };
            var bot1 = new BotDto { Id = Guid.NewGuid(), X = 0, Y = 0, Orientation = PossibleOrientations.East, CurrentStamina = 15, CurrentHealth = 1 };
            var bot2 = new BotDto { Id = Guid.NewGuid(), X = 3, Y = 0, Orientation = PossibleOrientations.West, CurrentStamina = 1, CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot1, bot2 });
            var context = ProcessingContext.Build(arena, bots);
            var bot1Properties = BotProperties.Build(bot1, arena, bots);
            var bot2Properties = BotProperties.Build(bot2, arena, bots);
            bot1Properties.CurrentMove = PossibleMoves.Teleport;
            bot1Properties.MoveDestinationX = 3;
            bot1Properties.MoveDestinationY = 0;
            bot2Properties.CurrentMove = PossibleMoves.Idling;
            context.AddBotProperties(bot1.Id, bot1Properties);
            context.AddBotProperties(bot2.Id, bot2Properties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(2);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 3,
                Y = 0,
                FromX = 0,
                FromY = 0,
                Orientation = PossibleOrientations.East,
                CurrentStamina = 15 - Constants.STAMINA_ON_TELEPORT,
                Move = PossibleMoves.Teleport
            }, c => c
                .Including(p => p.X)
                .Including(p => p.Y)
                .Including(p => p.FromX)
                .Including(p => p.FromY)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 0,
                Y = 0,
                Orientation = PossibleOrientations.West,
                CurrentStamina = 1,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Execute_A_Teleport_Before_A_Regular_Move()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 5, Height = 1 };
            var bot1 = new BotDto { Id = Guid.NewGuid(), X = 1, Y = 0, Orientation = PossibleOrientations.East, CurrentStamina = 15, CurrentHealth = 1 };
            var bot2 = new BotDto { Id = Guid.NewGuid(), X = 3, Y = 0, Orientation = PossibleOrientations.West, CurrentStamina = 1, CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot1, bot2 });
            var context = ProcessingContext.Build(arena, bots);
            var bot1Properties = BotProperties.Build(bot1, arena, bots);
            var bot2Properties = BotProperties.Build(bot2, arena, bots);
            bot1Properties.CurrentMove = PossibleMoves.Teleport;
            bot1Properties.MoveDestinationX = 3;
            bot1Properties.MoveDestinationY = 0;
            bot2Properties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot1.Id, bot1Properties);
            context.AddBotProperties(bot2.Id, bot2Properties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(2);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 3,
                Y = 0,
                Orientation = PossibleOrientations.East,
                CurrentStamina = 15 - Constants.STAMINA_ON_TELEPORT,
                Move = PossibleMoves.Teleport
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 0,
                Y = 0,
                Orientation = PossibleOrientations.West,
                CurrentStamina = 0,
                Move = PossibleMoves.WalkForward
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
        }

        [Fact]
        public async Task Postprocessor_Go_Should_Execute_A_Teleport_Before_A_Regular_Move_And_Thus_Block_Other_Bots_In_Their_Path()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 5, Height = 1 };
            var bot1 = new BotDto { Id = Guid.NewGuid(), X = 0, Y = 0, Orientation = PossibleOrientations.East, CurrentStamina = 15, CurrentHealth = 1 };
            var bot2 = new BotDto { Id = Guid.NewGuid(), X = 4, Y = 0, Orientation = PossibleOrientations.West, CurrentStamina = 1, CurrentHealth = 1 };
            var bots = new List<BotDto>(new[] { bot1, bot2 });
            var context = ProcessingContext.Build(arena, bots);
            var bot1Properties = BotProperties.Build(bot1, arena, bots);
            var bot2Properties = BotProperties.Build(bot2, arena, bots);
            bot1Properties.CurrentMove = PossibleMoves.Teleport;
            bot1Properties.MoveDestinationX = 3;
            bot1Properties.MoveDestinationY = 0;
            bot2Properties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot1.Id, bot1Properties);
            context.AddBotProperties(bot2.Id, bot2Properties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(2);
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 3,
                Y = 0,
                Orientation = PossibleOrientations.East,
                CurrentStamina = 15 - Constants.STAMINA_ON_TELEPORT,
                Move = PossibleMoves.Teleport
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
            context.Bots.Should().ContainEquivalentOf(new BotDto
            {
                X = 4,
                Y = 0,
                Orientation = PossibleOrientations.West,
                CurrentStamina = 1,
                Move = PossibleMoves.Idling
            }, c => c.Including(p => p.X)
                           .Including(p => p.Y)
                           .Including(p => p.Orientation)
                           .Including(p => p.CurrentStamina)
                           .Including(p => p.Move));
        }
    }
}
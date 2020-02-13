using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Common.Configuration;
using CSharpWars.DataAccess;
using CSharpWars.DataAccess.Repositories;
using CSharpWars.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CSharpWars.Tests.DataAccess.Repositories
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Repository_GetAll_Should_Return_All_Records()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1" };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2" };
            await dbContext.Players.AddRangeAsync(player1, player2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await playerRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(player1);
            result.Should().ContainEquivalentOf(player2);
        }

        [Fact]
        public async Task Repository_Find_Should_Return_Searched_Records()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1" };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2" };
            var player3 = new Player { Name = "Player3", Hashed = "Secret3" };
            await dbContext.Players.AddRangeAsync(player1, player2, player3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await playerRepository.Find(x => x.Name.Contains("2"));

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(player2);
        }

        [Fact]
        public async Task Repository_Find_And_Include_Should_Return_Searched_Records()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1", Bots = new List<Bot>() };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2", Bots = new List<Bot>() };
            var player3 = new Player { Name = "Player3", Hashed = "Secret3", Bots = new List<Bot>() };
            await dbContext.Players.AddRangeAsync(player1, player2, player3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await playerRepository.Find(x => x.Name.Contains("2"), i => i.Bots);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(player2);
        }

        [Fact]
        public async Task Repository_Single_Should_Return_Searched_Record()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1" };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2" };
            var player3 = new Player { Name = "Player3", Hashed = "Secret3" };
            await dbContext.Players.AddRangeAsync(player1, player2, player3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await playerRepository.Single(x => x.Name.Contains("2"));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(player2);
        }

        [Fact]
        public async Task Repository_Single_Should_Return_Null_If_Searched_Record_Not_Found()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1" };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2" };
            var player3 = new Player { Name = "Player3", Hashed = "Secret3" };
            await dbContext.Players.AddRangeAsync(player1, player2, player3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await playerRepository.Single(x => x.Name.Contains("5"));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Repository_Create_Should_Create_A_Record()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player = new Player { Name = "Player", Hashed = "Secret" };

            // Act
            var result = await playerRepository.Create(player);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(player);
        }

        [Fact]
        public async Task Repository_Update_Should_Update_A_Record()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player = new Player { Name = "Player", Hashed = "Secret" };
            await dbContext.Players.AddRangeAsync(player);
            await dbContext.SaveChangesAsync();

            // Act
            var playerToUpdate = new Player
            {
                Id = player.Id,
                Name = "NewPlayer",
                Hashed = "NewSecret"
            };
            await playerRepository.Update(playerToUpdate);

            // Assert
            var result = await dbContext.Players.SingleOrDefaultAsync();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(playerToUpdate);
        }

        [Fact]
        public async Task Repository_Update_Should_Update_Multiple_Records()
        {
            // Arrange
            var configurationHelper = new ConfigurationHelper();
            var dbContext = new CSharpWarsDbContext(configurationHelper);
            var playerRepository = new Repository<Player>(dbContext, dbContext.Players);
            var player1 = new Player { Name = "Player1", Hashed = "Secret1" };
            var player2 = new Player { Name = "Player2", Hashed = "Secret2" };
            var player3 = new Player { Name = "Player3", Hashed = "Secret3" };
            await dbContext.Players.AddRangeAsync(player1, player2, player3);
            await dbContext.SaveChangesAsync();

            // Act
            var playerToUpdate1 = new Player
            {
                Id = player1.Id,
                Name = "NewPlayer1",
                Hashed = "NewSecret1"
            };
            var playerToUpdate2 = new Player
            {
                Id = player2.Id,
                Name = "NewPlayer2",
                Hashed = "NewSecret2"
            };
            var playerToUpdate3 = new Player
            {
                Id = player3.Id,
                Name = "NewPlayer3",
                Hashed = "NewSecret3"
            };
            await playerRepository.Update(new List<Player>(new[] { playerToUpdate1, playerToUpdate2, playerToUpdate3 }));

            // Assert
            var result = await dbContext.Players.ToListAsync();
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().ContainEquivalentOf(playerToUpdate1);
            result.Should().ContainEquivalentOf(playerToUpdate2);
            result.Should().ContainEquivalentOf(playerToUpdate3);
        }
    }
}
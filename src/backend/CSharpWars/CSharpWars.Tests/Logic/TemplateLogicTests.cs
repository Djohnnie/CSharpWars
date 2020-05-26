using System;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Logic;
using CSharpWars.Logic.Constants;
using CSharpWars.Mapping;
using CSharpWars.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Logic
{
    public class TemplateLogicTests
    {
        [Fact]
        public async Task TemplateLogic_GetAllTemplates_Should_Return_Available_Templates()
        {
            // Arrange
            var templateRepository = new Mock<IRepository<Template>>();
            var templateLogic = new TemplateLogic(templateRepository.Object, new TemplateMapper());
            Template template1 = new Template
            {
                Id = Guid.NewGuid(),
                Name = "Template1",
                Script = "Script1"
            };
            Template template2 = new Template
            {
                Id = Guid.NewGuid(),
                Name = "Template2",
                Script = "Script2"
            };

            // Mock
            templateRepository.Setup(x => x.GetAll()).ReturnsAsync(new[] { template1, template2 });

            // Act
            var result = await templateLogic.GetAllTemplates();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(template1, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name)
                .Including(p => p.Script));
            result.Should().ContainEquivalentOf(template2, properties => properties
                .Including(p => p.Id)
                .Including(p => p.Name)
                .Including(p => p.Script));
        }

        [Fact]
        public async Task TemplateLogic_GetAllTemplates_Should_Return_A_Number_Of_Fixed_Scripts_If_None_Are_Available()
        {
            // Arrange
            var templateRepository = new Mock<IRepository<Template>>();
            var templateLogic = new TemplateLogic(templateRepository.Object, new TemplateMapper());

            // Mock
            templateRepository.Setup(x => x.GetAll()).ReturnsAsync(new Template[] { });

            // Act
            var result = await templateLogic.GetAllTemplates();

            // Assert
            result.Should().HaveCount(BotScripts.All.Count);
            foreach (var script in BotScripts.All)
            {
                result.Should().ContainEquivalentOf(script, properties => properties
                    .Including(p => p.Id)
                    .Including(p => p.Name)
                    .Including(p => p.Script));
            }
        }
    }
}
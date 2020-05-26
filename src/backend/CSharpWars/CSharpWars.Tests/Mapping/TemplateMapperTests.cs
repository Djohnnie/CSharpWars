using System;
using CSharpWars.DtoModel;
using CSharpWars.Mapping;
using CSharpWars.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Mapping
{
    public class TemplateMapperTests
    {
        [Fact]
        public void TemplateMapper_Can_Map_TemplateModel_To_TemplateDto()
        {
            // Arrange
            var mapper = new TemplateMapper();
            var templateModel = new Template
            {
                Id = Guid.NewGuid(),
                Name = "TemplateName",
                Script = "Script"
            };

            // Act
            var templateDto = mapper.Map(templateModel);

            // Assert
            templateDto.Should().BeEquivalentTo(templateModel,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name)
                    .Including(x => x.Script));
        }

        [Fact]
        public void TemplateMapper_Can_Map_TemplateDto_To_TemplateModel()
        {
            // Arrange
            var mapper = new TemplateMapper();
            var templateDto = new TemplateDto
            {
                Id = Guid.NewGuid(),
                Name = "TemplateName",
                Script = "Script"
            };

            // Act
            var templateModel = mapper.Map(templateDto);

            // Assert
            templateModel.Should().BeEquivalentTo(templateDto,
                properties => properties
                    .Including(x => x.Id)
                    .Including(x => x.Name)
                    .Including(x => x.Script));
        }
    }
}
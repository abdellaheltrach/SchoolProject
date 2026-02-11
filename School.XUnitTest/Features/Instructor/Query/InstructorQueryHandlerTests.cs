using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Base.ApiResponse;
using School.Core.Features.Instructors.Queries.Handlers;
using School.Core.Features.Instructors.Queries.Models;
using School.Core.Mapping.InstructorMapping;
using School.Core.Resources;
using School.Service.Services.Interfaces;
using System.Net;

namespace School.Tests.Features.Instructor.Query
{
    public class InstructorQueryHandlerTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly Mock<IInstructorService> _instructorServiceMock;
        private readonly InstructorProfile _instructorProfile;

        public InstructorQueryHandlerTests()
        {
            _localizerMock = new();
            _instructorProfile = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_instructorProfile));
            _mapper = new Mapper(configuration);
            _instructorServiceMock = new();
        }

        #region Get Summation Salary Query
        [Fact]
        public async Task Handle_GetSummationSalary_Should_Return_StatusCode200()
        {
            // Arrange
            var handler = new InstructorQueryHandler(_localizerMock.Object, _mapper, _instructorServiceMock.Object);
            var query = new GetSummationSalaryOfInstructorQuery();
            _instructorServiceMock.Setup(x => x.GetSalarySummationOfInstructor()).ReturnsAsync(5000);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            // Result Data is object { TotalSalary = result }
            // We can check if Data contains TotalSalary = 5000
            result.Data.Should().NotBeNull();
            var type = result.Data.GetType();
            var property = type.GetProperty("TotalSalary");
            property.Should().NotBeNull();
            property!.GetValue(result.Data).Should().Be(5000);
            
            _instructorServiceMock.Verify(x => x.GetSalarySummationOfInstructor(), Times.Once);
        }
        #endregion
    }
}

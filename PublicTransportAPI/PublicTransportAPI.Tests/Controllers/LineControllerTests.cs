using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PublicTransportAPI.Controllers;
using PublicTransportAPI.Data;

namespace PublicTransportAPI.Tests.Controllers;

[TestFixture]
public class LineControllerTests
{
    private ApplicationDbContext? _dbContext;
    private LineController? _lineController;

    [SetUp]
    public void SetUp()
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("database");
        _dbContext = new ApplicationDbContext(dbOptionsBuilder.Options);
        _lineController = new LineController(_dbContext);
    }

    [Test]
    public async Task AddMethod_NameIsStringEmpty_ValidationFail()
    {
        //Arrange
        var name = string.Empty;

        //Act
        var result = await _lineController!.Add(name);

        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddMethod_NameIsNull_ValidationFail()
    {
        //Arrange
        string? name = null;
        
        //Act
        var result = await _lineController!.Add(name!);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddMethod_NameIsProvided_Success()
    {
        //Arrange
        const string name = "test";
        
        //Act
        var result = await _lineController!.Add(name);
        
        //Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task AddMethod_DbContextIsNull_AssertionFail()
    {
        //Arrange
        const string name = "test";
        _dbContext = null;
        _lineController = new LineController(_dbContext!);

        //Act
        var result = await _lineController.Add(name);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
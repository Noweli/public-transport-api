using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PublicTransportAPI.Controllers;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.Models;

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

        PrepareDatabaseRecord();
    }

    private void PrepareDatabaseRecord()
    {
        if (_dbContext!.Lines!.Any())
        {
            return;
        }
        
        _dbContext.Lines!.Add(new Line {LineIdentifier = "test"});
        _dbContext.Lines!.Add(new Line {LineIdentifier = "test2"});
        _dbContext.SaveChanges();
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

    [Test]
    public async Task DeleteMethod_IdIsNegative_AssertionFail()
    {
        //Arrange
        const int id = -1;

        //Act
        var result = await _lineController!.Delete(id);

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task DeleteMethod_IdProvidedButInvalid_AssertionFail()
    {
        //Arrange
        const int id = int.MaxValue;
        
        //Act
        var result = await _lineController!.Delete(id);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task DeleteMethod_IdIsCorrect_Success()
    {
        //Arrange
        const int id = 2;
        
        //Act
        var result = await _lineController!.Delete(id);
        
        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task DeleteMethod_DbContextIsNull_AssertionFail()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _lineController = new LineController(_dbContext!);
        
        //Act
        var result = await _lineController!.Delete(id);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GetMethod_IdIsNegative_AssertionFail()
    {
        //Arrange
        const int id = -1;
        
        //Act
        var result = await _lineController!.GetLine(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GetMethod_IdProvidedButIncorrect_AssertionFail()
    {
        //Arrange
        const int id = int.MaxValue;
        
        //Act
        var result = await _lineController!.GetLine(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GetMethod_IdIsCorrect_Success()
    {
        //Arrange
        const int id = 1;
        
        //Act
        var result = await _lineController!.GetLine(id);
        
        //Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task GetMethod_DbContextIsNull_AssertionFail()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _lineController = new LineController(_dbContext!);
        
        //Act
        var result = await _lineController!.GetLine(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
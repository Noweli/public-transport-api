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
public class StopPointControllerTests
{
    private ApplicationDbContext? _dbContext;
    private StopPointController? _stopPointController;

    [SetUp]
    public void SetUp()
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("database");
        _dbContext = new ApplicationDbContext(dbOptionsBuilder.Options);
        _stopPointController = new StopPointController(_dbContext);

        PrepareDatabaseRecord();
    }

    private void PrepareDatabaseRecord()
    {
        if (_dbContext!.StopPoints!.Any())
        {
            return;
        }

        _dbContext.StopPoints!.Add(new StopPoint {Name = "test"});
        _dbContext.StopPoints!.Add(new StopPoint {Name = "test2"});
        _dbContext.SaveChanges();
    }

    [Test]
    public async Task AddMethod_NameIsEmpty_AssertionFail()
    {
        //Arrange
        var name = string.Empty;
        
        //Act
        var result = await _stopPointController!.Add(name);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_NameIsNull_AssertionFail()
    {
        //Arrange
        string? name = null;
        
        //Act
        var result = await _stopPointController!.Add(name!);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        const string name = "test";
        _dbContext = null;
        _stopPointController = new StopPointController(_dbContext!);
        
        //Act
        var result = await _stopPointController!.Add(name);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_NameProvidedCorrectly_Success()
    {
        //Arrange
        const string name = "test";

        //Act
        var result = await _stopPointController!.Add(name);
        
        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task GetMethod_IdIsNegative_AssertionFail()
    {
        //Arrange
        const int id = -1;
        
        //Act
        var result = await _stopPointController!.GetPoint(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetMethod_IdProvidedButIncorrect_AssertionFail()
    {
        //Arrange
        const int id = int.MaxValue;
        
        //Act
        var result = await _stopPointController!.GetPoint(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetMethod_IdProvidedAndCorrect_Success()
    {
        //Arrange
        const int id = 1;
        
        //Act
        var result = await _stopPointController!.GetPoint(id);
        
        //Assert
        result.Value.Should().BeOfType<StopPoint>();
    }
    
    [Test]
    public async Task GetMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _stopPointController = new StopPointController(_dbContext!);
        
        //Act
        var result = await _stopPointController!.GetPoint(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task DeleteMethod_IdIsNegative_AssertionFail()
    {
        //Arrange
        const int id = -1;

        //Act
        var result = await _stopPointController!.DeletePoint(id);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task DeleteMethod_IdProvidedButIncorrect_AssertionFail()
    {
        //Arrange
        const int id = int.MaxValue;

        //Act
        var result = await _stopPointController!.DeletePoint(id);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task DeleteMethod_IdProvidedAndCorrect_Success()
    {
        //Arrange
        const int id = 2;

        //Act
        var result = await _stopPointController!.DeletePoint(id);
        
        //Assert
        result.Should().BeOfType<OkResult>();
    }
    
    [Test]
    public async Task DeleteMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _stopPointController = new StopPointController(_dbContext!);

        //Act
        var result = await _stopPointController!.DeletePoint(id);
        
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
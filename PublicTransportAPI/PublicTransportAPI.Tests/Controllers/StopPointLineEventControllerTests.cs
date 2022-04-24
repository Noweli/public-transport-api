using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PublicTransportAPI.Controllers;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.DTOs;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Tests.Controllers;

[TestFixture]
public class StopPointLineEventControllerTests
{
    private ApplicationDbContext? _dbContext;
    private StopPointLineEventController? _stopPointLineEventController;

    [SetUp]
    public void SetUp()
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("database");
        _dbContext = new ApplicationDbContext(dbOptionsBuilder.Options);
        _stopPointLineEventController = new StopPointLineEventController(_dbContext);

        PrepareDatabaseRecord();
    }

    private void PrepareDatabaseRecord()
    {
        if (_dbContext!.StopPointLineEvents!.Any())
        {
            return;
        }

        _dbContext.StopPointLineEvents!.Add(new StopPointLineEvent
        {
            Arrival = new DateTime(2022, 01, 01, 12, 00, 30),
            Departure = new DateTime(2022, 01, 01, 12, 01, 00),
            Line = new Line {LineIdentifier = "testLine"},
            StopPoint = new StopPoint {Name = "testStopPoint"}
        });
        
        _dbContext.StopPointLineEvents!.Add(new StopPointLineEvent
        {
            Arrival = new DateTime(2022, 01, 01, 13, 00, 30),
            Departure = new DateTime(2022, 01, 01, 13, 01, 00),
            Line = new Line {LineIdentifier = "testLine2"},
            StopPoint = new StopPoint {Name = "testStopPoint2"}
        });
        
        _dbContext.SaveChanges();
    }

    [Test]
    public async Task AddMethod_ArrivalIsNull_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = null,
            Departure = "12:00:00",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddMethod_DepartureIsNull_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = null,
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_ArrivalIsEmpty_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = string.Empty,
            Departure = "12:00:00",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_DepartureIsEmpty_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = string.Empty,
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_DepartureIsBeforeArrival_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = "11:59:59",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [TestCase("2022-01-01 12:00:00")]
    [TestCase("25:12:00")]
    [TestCase("test")]
    public async Task AddMethod_ArrivalProvidedButIncorrectFormat_ReturnBadRequest(string arrival)
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = arrival,
            Departure = "11:59:59",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [TestCase("2022-01-01 12:00:00")]
    [TestCase("25:12:00")]
    [TestCase("test")]
    public async Task AddMethod_DepartureProvidedButIncorrectFormat_ReturnBadRequest(string departure)
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = departure,
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddMethod_LineIdIsIncorrect_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = "12:00:30",
            LineId = int.MaxValue,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_StopIdIsIncorrect_ReturnBadRequest()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = "12:00:30",
            LineId = 1,
            StopPointId = int.MaxValue
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task AddMethod_CorrectData_Success()
    {
        //Arrange
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = "12:00:30",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Value.Should().BeOfType<StopPointLineEvent>();
    }
    
    [Test]
    public async Task AddMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        _dbContext = null;
        _stopPointLineEventController = new StopPointLineEventController(_dbContext!);
        var input = new StopPointLineEventDTO
        {
            Arrival = "12:00:00",
            Departure = "12:00:30",
            LineId = 1,
            StopPointId = 1
        };
        
        //Act
        var result = await _stopPointLineEventController!.Add(input);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task DeleteMethod_IdIsNegative_ReturnBadRequest()
    {
        //Arrange
        const int id = -1;
        
        //Act
        var result = await _stopPointLineEventController!.Delete(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task DeleteMethod_IdIsIncorrect_ReturnBadRequest()
    {
        //Arrange
        const int id = int.MaxValue;
        
        //Act
        var result = await _stopPointLineEventController!.Delete(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task DeleteMethod_IdIsCorrect_Success()
    {
        //Arrange
        const int id = 2;
        
        //Act
        var result = await _stopPointLineEventController!.Delete(id);
        
        //Assert
        result.Value.Should().BeOfType<StopPointLineEvent>();
    }
    
    [Test]
    public async Task DeleteMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _stopPointLineEventController = new StopPointLineEventController(_dbContext!);
        
        //Act
        var result = await _stopPointLineEventController!.Delete(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetMethod_IdIsNegative_ReturnBadRequest()
    {
        //Arrange
        const int id = -1;
        
        //Act
        var result = await _stopPointLineEventController!.Get(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetMethod_IdIsIncorrect_ReturnBadRequest()
    {
        //Arrange
        const int id = int.MaxValue;
        
        //Act
        var result = await _stopPointLineEventController!.Get(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetMethod_IdIsCorrect_Success()
    {
        //Arrange
        const int id = 1;
        
        //Act
        var result = await _stopPointLineEventController!.Get(id);
        
        //Assert
        result.Value.Should().BeOfType<StopPointLineEvent>();
    }
    
    [Test]
    public async Task GetMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        const int id = 1;
        _dbContext = null;
        _stopPointLineEventController = new StopPointLineEventController(_dbContext!);
        
        //Act
        var result = await _stopPointLineEventController!.Get(id);
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetAllMethod_DbContextIsNull_ReturnBadRequest()
    {
        //Arrange
        _dbContext = null;
        _stopPointLineEventController = new StopPointLineEventController(_dbContext!);
        
        //Act
        var result = await _stopPointLineEventController!.Get();
        
        //Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Test]
    public async Task GetAllMethod_CorrectFlow_Success()
    {
        //Arrange
        //Act
        var result = await _stopPointLineEventController!.Get();
        
        //Assert
        result.Value.Should().BeOfType<List<StopPointLineEvent>>();
    }
}
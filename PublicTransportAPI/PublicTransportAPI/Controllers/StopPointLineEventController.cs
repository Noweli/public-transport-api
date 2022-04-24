using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.DTOs;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StopPointLineEventController
{
    private const string ARRIVAL_DEPARTURE_VALID_FORMAT = "HH:mm:ss";
    private readonly ApplicationDbContext _dbContext;

    public StopPointLineEventController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut]
    public async Task<ActionResult<StopPointLineEvent>> Add([FromBody] StopPointLineEventDTO stopPointLineEventDTO)
    {
        if (stopPointLineEventDTO.Arrival is null || stopPointLineEventDTO.Departure is null)
        {
            return new BadRequestObjectResult(
                $"Arrival and departure has to be provided. Allowed format: {ARRIVAL_DEPARTURE_VALID_FORMAT}");
        }

        if (!DateTime.TryParseExact(stopPointLineEventDTO.Arrival, ARRIVAL_DEPARTURE_VALID_FORMAT,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var arrival))
        {
            return new BadRequestObjectResult(
                $"Arrival format is invalid. Allowed format: {ARRIVAL_DEPARTURE_VALID_FORMAT}");
        }

        if (!DateTime.TryParseExact(stopPointLineEventDTO.Departure, ARRIVAL_DEPARTURE_VALID_FORMAT,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var departure))
        {
            return new BadRequestObjectResult(
                $"Departure format is invalid. Allowed format: {ARRIVAL_DEPARTURE_VALID_FORMAT}");
        }

        if (departure < arrival)
        {
            return new BadRequestObjectResult("Departure occurs before arrival.");
        }

        try
        {
            var searchResultLine = await _dbContext.Lines!.FindAsync(stopPointLineEventDTO.LineId);

            if (searchResultLine is null)
            {
                return new BadRequestObjectResult(
                    $"Could not find line with provided id - {stopPointLineEventDTO.LineId}");
            }

            var searchResultStopPoint = await _dbContext.StopPoints!.FindAsync(stopPointLineEventDTO.StopPointId);

            if (searchResultStopPoint is null)
            {
                return new BadRequestObjectResult(
                    $"Could not find stop point with provided id - {stopPointLineEventDTO.StopPointId}");
            }

            var addResult = await _dbContext.StopPointLineEvents!.AddAsync(new StopPointLineEvent
            {
                Arrival = arrival,
                Departure = departure,
                Line = searchResultLine,
                StopPoint = searchResultStopPoint
            });
            _ = await _dbContext.SaveChangesAsync();

            return addResult.Entity;
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(
                $"Exception occured during stop point line event addition. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not add stop point line event. Check logs.");
    }

    [HttpDelete]
    public async Task<ActionResult<StopPointLineEvent>> Delete(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be negative.");
        }

        try
        {
            var searchResult = await _dbContext.StopPointLineEvents!.Include(model => model.Line)
                .Include(model => model.StopPoint).FirstOrDefaultAsync(stopPointEvent => stopPointEvent.Id.Equals(id));

            if (searchResult is null)
            {
                return new BadRequestObjectResult($"Could not find stop point line event with id - {id}");
            }

            _ = _dbContext.StopPointLineEvents!.Remove(searchResult);
            _ = await _dbContext.SaveChangesAsync();

            return searchResult;
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(
                $"Exception occured during stop point line event removal. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not delete stop point line event. Check logs.");
    }

    [HttpGet]
    public async Task<ActionResult<StopPointLineEvent>> Get(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be negative.");
        }

        try
        {
            var searchResult = await _dbContext.StopPointLineEvents!.Include(model => model.Line)
                .Include(model => model.StopPoint).FirstOrDefaultAsync(stopPointEvent => stopPointEvent.Id.Equals(id));

            if (searchResult is null)
            {
                return new BadRequestObjectResult($"Could not find stop point line event with id - {id}");
            }

            return searchResult;
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(
                $"Exception occured during look up for stop point line event. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not find stop point line event. Check logs.");
    }
    
    [HttpGet("findAll")]
    public async Task<ActionResult<List<StopPointLineEvent>>> Get()
    {
        try
        {
            var searchResult = await _dbContext.StopPointLineEvents!.Include(model => model.Line)
                .Include(model => model.StopPoint).ToListAsync();

            if (!searchResult.Any())
            {
                return new BadRequestObjectResult($"There are no stop point line events found in database.");
            }

            return searchResult;
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(
                $"Exception occured during look up for stop point line events. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not find stop point line events. Check logs.");
    }
}
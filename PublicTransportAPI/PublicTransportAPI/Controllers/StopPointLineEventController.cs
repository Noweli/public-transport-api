using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.DTOs;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StopPointLineEventController
{
    private readonly ApplicationDbContext _dbContext;
    private const string ARRIVAL_DEPARTURE_VALID_FORMAT = "HH:mm:ss";

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

            return new OkObjectResult(addResult.Entity);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Exception occured during stop point line event addition. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not add stop point line event. Check logs.");
    }
}
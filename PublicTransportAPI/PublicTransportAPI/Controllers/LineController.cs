using Microsoft.AspNetCore.Mvc;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.DTOs;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LineController
{
    private readonly ApplicationDbContext _dbContext;
    
    public LineController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut]
    public async Task<ActionResult<Line>> Add([FromBody] LineDTO lineDTO)
    {
        if (string.IsNullOrWhiteSpace(lineDTO.LineIdentifier))
        {
            return new BadRequestObjectResult("Line identifier not provided.");
        }

        Line addedLineResult;

        try
        {
            var stopPointIds = Array.Empty<StopPoint>();

            if (lineDTO.StopPointIds is not null && lineDTO.StopPointIds.Any())
            {
                var result = _dbContext.StopPoints!.Where(point => lineDTO.StopPointIds.Contains(point.Id));
                
                if (!result.Any())
                {
                    return new BadRequestObjectResult("Could not find line stops with selected ids.");
                }

                stopPointIds = result.ToArray();
            }

            var resultAdded = await _dbContext.Lines!.AddAsync(new Line
            {
                LineIdentifier = lineDTO.LineIdentifier,
                StopPoints = stopPointIds
            });
            _ = await _dbContext.SaveChangesAsync();

            addedLineResult = resultAdded.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new OkObjectResult(addedLineResult);
    } 
    
}
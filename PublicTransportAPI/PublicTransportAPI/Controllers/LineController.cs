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
    public async Task<ActionResult<Line>> Add([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new BadRequestObjectResult("Line identifier has to be provided.");
        }

        try
        {
            var resultAdded = await _dbContext.Lines!.AddAsync(new Line
            {
                LineIdentifier = name,
            });
            _ = await _dbContext.SaveChangesAsync();

            return new OkObjectResult(resultAdded.Entity);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Error occured during line addition. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not add line. Checks logs.");
    } 
    
}
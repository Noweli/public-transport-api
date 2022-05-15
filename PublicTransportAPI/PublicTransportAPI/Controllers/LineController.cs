using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Controllers;

[Authorize]
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
    [Authorize(Roles = "Admin")]
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

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be negative.");
        }

        try
        {
            var searchResult = await _dbContext.Lines!.FindAsync(id);

            if (searchResult is null)
            {
                return new BadRequestObjectResult($"Could not find line with id {id}.");
            }

            _ = _dbContext.Lines.Remove(searchResult);
            _ = await _dbContext.SaveChangesAsync();

            return new OkObjectResult(searchResult);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Exception occured when removing line. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not remove line. Check logs.");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<Line>> GetLine(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be negative.");
        }

        try
        {
            var searchResult = await _dbContext.Lines!.FindAsync(id);

            if (searchResult is null)
            {
                return new BadRequestObjectResult($"Could not find line with provided id - {id}.");
            }

            return new OkObjectResult(searchResult);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Exception occured during search for line. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not find line. Check logs.");
    }
}
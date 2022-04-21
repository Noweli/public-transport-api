using Microsoft.AspNetCore.Mvc;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StopPointController
{
    private readonly ApplicationDbContext _dbContext;

    public StopPointController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut]
    public async Task<ActionResult> Add([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new BadRequestObjectResult("Failed to add new stop point. Name has to be provided.");
        }

        try
        {
            var result = await _dbContext.StopPoints!.AddAsync(new StopPoint {Name = name});
            _ = await _dbContext.SaveChangesAsync();

            return new OkObjectResult(result.Entity);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Failed to add new stop point. Exception message: {e.Message}");
        }

        return new BadRequestObjectResult("Failed to add new stop point. Check logs.");
    }

    [HttpGet]
    public async Task<ActionResult<StopPoint>> GetPoint(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be lower than zero.");
        }

        try
        {
            var result = await _dbContext.StopPoints!.FindAsync(id);

            if (result is null)
            {
                return new BadRequestObjectResult("Could not find point with specified id.");
            }

            return result;
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Exception occured while gathering information about point with id {id}. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Could not find stop point. Check logs.");
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePoint(int id)
    {
        if (id < 0)
        {
            return new BadRequestObjectResult("Id index cannot be negative.");
        }

        try
        {
            var result = await _dbContext.StopPoints!.FindAsync(id);

            if (result is null)
            {
                return new BadRequestObjectResult($"Could not find stop point with id {id}.");
            }

            _ = _dbContext.StopPoints!.Remove(result);
            _ = await _dbContext.SaveChangesAsync();

            return new OkResult();
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Could not delete stop point with id {id}. Error message: {e.Message}");
        }

        return new BadRequestObjectResult("Failed to delete stop point. Check logs.");
    }
}
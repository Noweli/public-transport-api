using Microsoft.AspNetCore.Mvc;
using PublicTransportAPI.Data;

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
    
}
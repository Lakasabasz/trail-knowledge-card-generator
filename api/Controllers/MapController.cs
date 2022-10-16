using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController, Route("[controller]")]
public class MapController : Controller
{
    private readonly ILogger<MapController> _logger;
    private RailroadMapDbContext _dbCtx;

    public MapController(ILogger<MapController> logger)
    {
        _logger = logger;
        _dbCtx = new RailroadMapDbContext();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Category>> GetCategories()
    {
        _logger.Log(LogLevel.Information, "Request from {HostHost}", Request.Host.Host);
        return await _dbCtx.Categories.ToArrayAsync();
    }
}
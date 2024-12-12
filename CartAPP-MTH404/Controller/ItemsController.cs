using CartAPP_MTH404.Data;
using CartAPP_MTH404.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(MongoDbService mongoDbService, ILogger<ItemsController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsAsync()
    {
        try
        {
            var items = await _mongoDbService.Database.GetCollection<Item>("Items")
                .Find(Builders<Item>.Filter.Empty)
                .ToListAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching items: {ex.Message}");
            return StatusCode(500, new { Message = $"Error fetching items: {ex.Message}" });
        }
    }
}

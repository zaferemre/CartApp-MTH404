using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CartAPP_MTH404.Data; // Adjust namespace as necessary
using MongoDB.Driver;

using CartAPP_MTH404.Entities;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly MongoDbService _mongoDbService; // Service for MongoDB interactions
    private readonly ILogger<CustomerController> _logger; // Add logging for better debugging

    public CustomerController(MongoDbService mongoDbService, ILogger<CustomerController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    // Test MongoDB Connection
    [HttpGet("test-connection")]
    public IActionResult TestConnection()
    {
        try
        {
            var collections = _mongoDbService.Database
                .WithWriteConcern(WriteConcern.WMajority)
                .ListCollectionNames()
                .ToList();

            return Ok(new { Status = "Success", Collections = collections });
        }
        catch (TimeoutException ex)
        {
            _logger.LogError($"MongoDB timeout: {ex.Message}");
            return StatusCode(500, new { Status = "Failed", Message = "MongoDB connection timed out.", Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"MongoDB connection error: {ex.Message}");
            return StatusCode(500, new { Status = "Failed", Message = ex.Message });
        }
    }

    // Get All Customers
    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        try
        {
            var customers = await _mongoDbService.Database.GetCollection<Customer>("Customers")
                .Find(Builders<Customer>.Filter.Empty)
                .ToListAsync();

            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching customers: {ex.Message}");
            return StatusCode(500, new { Message = $"Error fetching customers: {ex.Message}" });
        }
    }

    // Get Customer by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerByIdAsync(string id)
    {
        try
        {
            var customer = await _mongoDbService.Database.GetCollection<Customer>("Customers")
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound(new { Message = $"Customer with ID {id} not found." });

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching customer with ID {id}: {ex.Message}");
            return StatusCode(500, new { Message = $"Error fetching customer with ID {id}: {ex.Message}" });
        }
    }

    // Create Customer
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
    {
        try
        {
            // The Id is automatically set as an ObjectId
            await _mongoDbService.Database.GetCollection<Customer>("Customers").InsertOneAsync(customer);
            return Created("", customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating customer: {ex.Message}");
            return StatusCode(500, new { Message = $"Error creating customer: {ex.Message}" });
        }
    }

    // Update Customer
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerAsync(string id, [FromBody] Customer customer)
    {
        try
        {
            var updateResult = await _mongoDbService.Database.GetCollection<Customer>("Customers")
                .ReplaceOneAsync(c => c.Id == id, customer);

            if (updateResult.MatchedCount == 0)
                return NotFound(new { Message = $"Customer with ID {id} not found." });

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating customer with ID {id}: {ex.Message}");
            return StatusCode(500, new { Message = $"Error updating customer with ID {id}: {ex.Message}" });
        }
    }

    // Delete Customer
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerAsync(string id)
    {
        try
        {
            var deleteResult = await _mongoDbService.Database.GetCollection<Customer>("Customers")
                .DeleteOneAsync(c => c.Id == id);

            if (deleteResult.DeletedCount == 0)
                return NotFound(new { Message = $"Customer with ID {id} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting customer with ID {id}: {ex.Message}");
            return StatusCode(500, new { Message = $"Error deleting customer with ID {id}: {ex.Message}" });
        }
    }
}


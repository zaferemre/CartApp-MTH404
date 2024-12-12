using CartAPP_MTH404.Data;
using CartAPP_MTH404.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;
    private readonly ILogger<CartController> _logger;

    public CartController(MongoDbService mongoDbService, ILogger<CartController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCartAsync(string customerId)
    {
        try
        {
            var cartCollection = _mongoDbService.Database.GetCollection<Cart>("Carts");
            var itemsCollection = _mongoDbService.Database.GetCollection<Item>("Items"); // Assuming an Items collection exists

            Console.WriteLine($"Looking for customer cart with ID: {customerId}");

            // Find the cart
            var filter = Builders<Cart>.Filter.Eq(c => c.CustomerId, customerId);
            var cart = await cartCollection.Find(filter).FirstOrDefaultAsync();

            if (cart == null)
            {
                Console.WriteLine("Cart not found.");
                return NotFound(new { Message = "Cart not found" });
            }

            // Enrich cart items with price and other details from Items collection
            foreach (var item in cart.Items)
            {
                var itemDetails = await itemsCollection.Find(Builders<Item>.Filter.Eq(i => i.Id, item.ItemId)).FirstOrDefaultAsync();
                if (itemDetails != null)
                {
                    item.Price = itemDetails.Price; // Assign the price from the Items collection
                }
                else
                {
                    Console.WriteLine($"Item with ID {item.ItemId} not found in Items collection.");
                }
            }

            return Ok(cart);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart: {ex.Message}");
            return StatusCode(500, new { Message = $"Error fetching cart: {ex.Message}" });
        }
    }



    [HttpPost("{customerId}/add")]
    public async Task<IActionResult> AddItemToCartAsync(string customerId, [FromBody] CartItem cartItem)
    {
        try
        {
            var cart = await _mongoDbService.Database.GetCollection<Cart>("Carts")
                .Find(c => c.CustomerId == customerId)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart { CustomerId = customerId, Items = new List<CartItem> { cartItem } };
                await _mongoDbService.Database.GetCollection<Cart>("Carts").InsertOneAsync(cart);
            }
            else
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == cartItem.ItemId);
                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity;
                }
                else
                {
                    cart.Items.Add(cartItem);
                }
                await _mongoDbService.Database.GetCollection<Cart>("Carts")
                    .ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }

            return Ok(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding item to cart: {ex.Message}");
            return StatusCode(500, new { Message = $"Error adding item to cart: {ex.Message}" });
        }
    }

    [HttpPost("{customerId}/remove")]
    public async Task<IActionResult> RemoveItemFromCartAsync(string customerId, [FromBody] string itemId)
    {
        try
        {
            var cart = await _mongoDbService.Database.GetCollection<Cart>("Carts")
                .Find(c => c.CustomerId == customerId)
                .FirstOrDefaultAsync();

            if (cart == null)
                return NotFound(new { Message = "Cart not found" });

            // Remove the item by matching the itemId
            cart.Items.RemoveAll(i => i.ItemId == itemId);

            // Save the updated cart
            await _mongoDbService.Database.GetCollection<Cart>("Carts")
                .ReplaceOneAsync(c => c.Id == cart.Id, cart);

            return Ok(cart);  // Return updated cart
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing item from cart: {ex.Message}");
            return StatusCode(500, new { Message = $"Error removing item from cart: {ex.Message}" });
        }
    }
    [HttpDelete("cart/{customerId}/remove/{itemId}")]
    public async Task<IActionResult> RemoveFromCart(string customerId, string itemId)
    {
        try
        {
            var cartCollection = _mongoDbService.Database.GetCollection<Cart>("Carts");
            var filter = Builders<Cart>.Filter.Eq(c => c.CustomerId, customerId);
            var update = Builders<Cart>.Update.PullFilter(c => c.Items, Builders<CartItem>.Filter.Eq(i => i.ItemId, itemId));
            await cartCollection.UpdateOneAsync(filter, update);
            return Ok(new { Message = "Item removed from cart." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error removing item: {ex.Message}" });
        }
    }

}

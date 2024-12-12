using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CartAPP_MTH404.Entities
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("items")]
        public List<CartItem> Items { get; set; } = new();

    }

    public class CartItem
    {
        [BsonElement("itemId")]
        public string ItemId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("imageSrc")]
        public string ImageSrc { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }

}

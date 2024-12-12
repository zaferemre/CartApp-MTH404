using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartAPP_MTH404.Entities
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("imageSrc")]
        public string? ImageSrc { get; set; } // Optional field for the image URL

    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartAPP_MTH404.Entities
{
    public class Customer
    {
        [BsonId] // Marks this property as the primary key
        [BsonRepresentation(BsonType.ObjectId)] // Ensures MongoDB serializes and deserializes as an ObjectId
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("customerName")] // Optional: Maps to a field named 'customerName' in MongoDB
        public string CustomerName { get; set; } = string.Empty;

        [BsonElement("email")] // Optional: Maps to a field named 'email' in MongoDB
        public string Email { get; set; } = string.Empty;
    }
}

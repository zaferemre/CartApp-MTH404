using MongoDB.Driver;

namespace CartAPP_MTH404.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb"); // Correct key from appsettings.json
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("MongoDB connection string is not configured.");
            }

            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);

            if (string.IsNullOrEmpty(mongoUrl.DatabaseName))
            {
                throw new ArgumentException("Database name is missing in the MongoDB connection string.");
            }

            _mongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase Database => _mongoDatabase; // Expose the database instance
    }
}

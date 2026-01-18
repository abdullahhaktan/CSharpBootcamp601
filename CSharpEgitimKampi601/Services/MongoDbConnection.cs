using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEgitimKampi601.Services
{
    public class MongoDbConnection
    {
        private IMongoDatabase _database;
        
        // Constructor establishes connection to MongoDB
        public MongoDbConnection()
        {
            // Connect to MongoDB running on localhost default port
            var client = new MongoClient("mongodb://localhost:27017");
            
            // Get reference to specific database
            _database = client.GetDatabase("Db601Customer");
        }

        // Get collection for customer documents
        public IMongoCollection<BsonDocument> GetCustomersCollection()
        {
            return _database.GetCollection<BsonDocument>("Customers");
        }
    }
}

using CSharpEgitimKampi601.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEgitimKampi601.Services
{
    public class CustomerOperations
    {
        // Add a new customer to MongoDB
        public void AddCustomer(Customer customer)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();

            // Create BSON document from Customer object
            var document = new BsonDocument
            {
                {"CustomerName", customer.CustomerName },
                {"CustomerSurname", customer.CustomerSurname },
                {"CustomerCity", customer.CustomerCity },
                {"CustomerBalance", customer.CustomerBalance },
                {"CustomerShoppingCount", customer.CustomerShoppingCount }
            };

            // Insert document into MongoDB collection
            customerCollection.InsertOne(document);
        }
        
        // Retrieve all customers from MongoDB
        public List<Customer> GetAllCustomer()
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            
            // Find all documents in the collection
            var customers = customerCollection.Find(new BsonDocument()).ToList();
            
            // Convert BSON documents to Customer objects
            List<Customer> customerList = new List<Customer>();
            foreach (var c in customers)
            {
                customerList.Add(new Customer
                {
                    CustomerId = c["_id"].ToString(),
                    CustomerBalance = decimal.Parse(c["CustomerBalance"].ToString()),
                    CustomerCity = c["CustomerCity"].ToString(),
                    CustomerName = c["CustomerName"].ToString(),
                    CustomerShoppingCount = int.Parse(c["CustomerShoppingCount"].ToString()),
                    CustomerSurname = c["CustomerSurname"].ToString()
                });
            }
            return customerList;
        }
        
        // Delete customer by ID from MongoDB
        public void DeleteCustomer(string id)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            
            // Create filter to find document by ID
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            
            // Delete the document
            customerCollection.DeleteOne(filter);
        }
        
        // Update existing customer in MongoDB
        public void UpdateCustomer(Customer customer)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            
            // Create filter to find document by ID
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(customer.CustomerId));
            
            // Create update definition with all fields
            var updatedValue = Builders<BsonDocument>.Update
                .Set("CustomerName", customer.CustomerName)
                .Set("CustomerSurname", customer.CustomerSurname)
                .Set("CustomerCity", customer.CustomerCity)
                .Set("CustomerBalance", customer.CustomerBalance)
                .Set("CustomerShoppingCount", customer.CustomerShoppingCount);
            
            // Update the document
            customerCollection.UpdateOne(filter, updatedValue);
        }
        
        // Get single customer by ID from MongoDB
        public Customer GetCustomerById(string id)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            
            // Create filter to find document by ID
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            
            // Find the document
            var result = customerCollection.Find(filter).FirstOrDefault();
            
            // Convert BSON document to Customer object
            return new Customer
            {
                CustomerBalance = decimal.Parse(result["CustomerBalance"].ToString()),
                CustomerCity = result["CustomerCity"].ToString(),
                CustomerId = id,
                CustomerName = result["CustomerName"].ToString(),
                CustomerShoppingCount = int.Parse(result["CustomerShoppingCount"].ToString()),
                CustomerSurname = result["CustomerSurname"].ToString()
            };
        }
    }
}

using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {

        public CatalogContext(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);

            Products = db.GetCollection<Product>(collectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Product>> Get() =>
            _context.Products.FindAsync(p => true)
                .ContinueWith(findTask => (IEnumerable<Product>) findTask.Result.ToList());

        public Task<Product> Get(string id) =>
            _context.Products.Find(product => product.Id == id).FirstOrDefaultAsync();

        public Task<IEnumerable<Product>> GetByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return _context.Products.FindAsync(filter)
                .ContinueWith(findTask => (IEnumerable<Product>) findTask.Result.ToList());
        }

        public Task<IEnumerable<Product>> GetByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return _context.Products.FindAsync(filter)
                .ContinueWith(findTask => (IEnumerable<Product>)findTask.Result.ToList());
        }

        public Task Add(Product product) => _context.Products.InsertOneAsync(product);

        public async Task Update(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            if (updateResult.IsAcknowledged == false || updateResult.ModifiedCount < 1)
            {
                throw new InvalidOperationException("repository update failed");
            }
        }

        public Task Delete(string id) => _context.Products.DeleteOneAsync(product => product.Id == id);
    }
}

using MarketApi.DBContext;
using MarketApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketApi.Services
{
    public class ProductService : IProductService
    {
        private ApiContext _db;

        public ProductService(ApiContext db)
        {
            _db = db;
        }

        //Async suffix
        public async Task<Product> CreateAsync(Product product)
        {
            await _db.AddAsync(product);
            await _db.SaveChangesAsync();

            return product;
        }

        //Async suffix 
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Proucts.ToListAsync();
        }

        //Async suffix
        public async Task<Product> GetAsync(int id)
        {
            return await _db.Proucts.FirstOrDefaultAsync(p => p.Id == id);
        }

        //Async suffix
        public async Task<Product> UpdateAsync(Product product)
        {
            _db.Proucts.Update(product);
            await _db.SaveChangesAsync();

            return product;
        }

        //Async suffix
        public async Task<Product> DeleteAsync(int id)
        {
            var product = await _db.Proucts.FirstOrDefaultAsync(p => p.Id == id);

            _db.Remove(product);
            await _db.SaveChangesAsync();

            return product;
        }

        //Async suffix
        public async Task<bool> IsExistAsync(int id)
        {
            return await _db.Proucts.AnyAsync(p => p.Id == id);
        }
    }
}

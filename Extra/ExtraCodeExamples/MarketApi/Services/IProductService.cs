using MarketApi.Models;

namespace MarketApi.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> UpdateAsync(Product product);
        Task<Product> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);

    }
}

using MarketApi.Models;

namespace MarketApi.Services
{
    public interface IProductService
    {
        //Async suffix      
        Task<Product> CreateAsync(Product product);

        //Async suffix
        Task<IEnumerable<Product>> GetAllAsync();

        //Async suffix
        Task<Product> GetAsync(int id);

        //Async suffix
        Task<Product> UpdateAsync(Product product);

        //Async suffix
        Task<Product> DeleteAsync(int id);

        //Async suffix
        Task<bool> IsExistAsync(int id);
    }
}

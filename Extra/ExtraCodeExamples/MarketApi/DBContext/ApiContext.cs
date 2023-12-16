using MarketApi.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketApi.DBContext
{
    public class ApiContext : DbContext
    {
        public DbSet<Product> Proucts { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) 
            : base(options)
        {
               
        }
    }
}

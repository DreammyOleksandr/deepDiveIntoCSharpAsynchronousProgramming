## Recommendations for using Async suffix in asynchronous methods

To understand when it is worth using the async suffix, and when not, let's consider the naming of asynchronous functions using the example of
the Web Api, which in this case is the interface for interacting with the products of a certain market. In this project, the most important for us
will be the `ProductController.cs` and `ProductService.cs` modules together with the interface `IProductService.cs`.

## [IProductService.cs](../ExtraCodeExamples/MarketApi/Services/IProductService.cs) and [ProductService.cs](../ExtraCodeExamples/MarketApi/Services/ProductService.cs)

*IProductService.cs* is an interface that describes the general set of operations required to interact with data, regardless of where they come from. (Repository Pattern)

*ProductService.cs* is an implementation of the IProductService interface that clearly describes how data from a specific source will be processed.

Let's take a look at them:

IProductService.cs
```csharp
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
```

ProductService.cs
```csharp
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
```

Here we are most interested in the types and names of methods. We see that they are all asynchronous, as evidenced by the `async` keyword in
the interface implementation, and as a result, the `Task` type is returned. Here we also see the use of the Async suffix. It`s used to indicate that
this method is asynchronous, thereby helping the developer make decisions regarding its usage depending on the context.

## Additional reasons for using the Async suffix

Sometimes there are situations when the service provides both synchronous and asynchronous methods for solving one problem. In such
cases, the async suffix serves as a separator for synchronous code from asnychronous.

```csharp
public class ProductServiceExt : IProductServiceExt
{
    private ApiContext _db;

    public ProductService(ApiContext db)
    {
        _db = db;
    }

    //Synchronous method
    public Product Create(Product product)              
    {
        _db.Add(product);
        _db.SaveChanges();

        return product;
    }

    //Asynchronous  method
    public async Task<Product> CreateAsync(Product product)              
    {
        await _db.AddAsync(product);
        await _db.SaveChangesAsync();

        return product;
    }
```

It is also worth mentioning that some environments and analyzers, seeing the Async suffix in the code, can automatically recognize that this
method is asynchronous, and thus it can improve debugging and code quality.

## [ProductController.cs](../ExtraCodeExamples/MarketApi/Controllers/ProductController.cs)

```csharp
[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    //No Async suffix
    public async Task<IActionResult> Create(Product product)
    {
        if (await _productService.IsExistAsync(product.Id))
        {
            return BadRequest("Product with this ID already exist");
        }

        var createdProduct = await _productService.CreateAsync(product);

        return CreatedAtAction("Create", createdProduct);
    }

    [HttpGet]
    //No Async suffix
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet]
    //No Async suffix
    public async Task<IActionResult> Get(int id)
    {
        if (!await _productService.IsExistAsync(id))
        {
            return BadRequest("Product not found");
        }

        var products = await _productService.GetAsync(id);

        return Ok(products);
    }

    [HttpPut]
    //No Async suffix
    public async Task<IActionResult> Update(Product product)
    {
        if (!await _productService.IsExistAsync(product.Id))
        {
            return base.BadRequest("Product not found");
        }

        var updatedProduct = await _productService.UpdateAsync(product);

        return base.Ok(updatedProduct);
    }

    [HttpDelete]
    //No Async suffix
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _productService.IsExistAsync(id))
        {
            return BadRequest("Product with this ID does not exist");
        }

        var product = await _productService.DeleteAsync(id);

        return Ok(product);
    }
}
```

Here we immediately notice that the controller method names lack the Async suffix, despite all of them being asynchronous. Why aren't we
using our suffix here? The reason is that controllers are directly used by the ASP.NET Core framework, **NOT THE DEVELOPER**, and therefore there is
no need for us to indicate that this method is asynchronous.

## Conclusion

So let's summarize and determine when it's appropriate to use the async suffix and when it's not.

We use the Async suffix in:
- asynchronous methods of services, or class libraries, when it is assumed that these methods will be used by other developers;
- libraries or services, when there are two implementations - synchronous and asynchronous;


We don`t use the Async suffix when:
- asynchronous methods are called automatically, without their use by the developer;
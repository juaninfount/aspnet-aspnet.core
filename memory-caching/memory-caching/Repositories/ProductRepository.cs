using Models;

namespace Repositories;

public class ProductRepository : IProductRepository
{
    public ProductRepository()
    {        
        using var context = new ApplicationDbContext();
        List<Product> products = new();

        for(var i=0;i<Math.Pow(2,7);i++)
        {
            products.Add(new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = "Product-" + i,
                Price = (decimal)Random.Shared.NextDouble() * 100
            }); 
            
        };
        context.Products?.AddRange(products);
        context.SaveChanges();
    }

    public List<Product> GetProducts()
    {
         using var context = new ApplicationDbContext();
         return context.Products?.ToList()?? new List<Product>();
    }
}
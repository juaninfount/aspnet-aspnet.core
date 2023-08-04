using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Models;
using Repositories;
using System.Diagnostics;

namespace memory_caching.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMemoryCache _cache;
    //private readonly ApplicationDbContext _context;
    private readonly IProductRepository _productRepository;

    private readonly string cacheKey = "productsCacheKey";

    public HomeController(ILogger<HomeController> logger,
                        //ApplicationDbContext context,
                        IMemoryCache cache,
                        IProductRepository productRepository)
    {
        _logger = logger;
        _cache = cache;
        //_context = context;
        _productRepository = productRepository;
    }

    [HttpGet("Index")]
    public IActionResult Index()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Product> products))
        {
            _logger.Log(LogLevel.Information, "Products found in cache");
        }
        else
        {
            _logger.Log(LogLevel.Information, "Products NOT Found in cache. Loading");
            products = _productRepository.GetProducts();  //_context.Products.ToList();
            var options = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                    .SetPriority(CacheItemPriority.Normal);

            _cache.Set(cacheKey, products, options);
        }

        stopwatch.Stop();
        _logger.Log(LogLevel.Information, "Passed time " + stopwatch.Elapsed.TotalMilliseconds);

        products ??= new List<Product>();
        return Ok(products);
    }

    [HttpGet("ClearCache")]
    public IActionResult ClearCache()
    {
        _cache.Remove(cacheKey);
        _logger.Log(LogLevel.Information, "Cache Cleared");
        return Ok();
       // return RedirectToAction("Index");
    }

}
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Catalog.API.Data
{
    public class CatalogContext: ICatalogContext
    {
        public CatalogContext(IConfiguration configuration) 
        {
            var client = new MongoClient(configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));
            var database = client.GetDatabase(configuration.GetSection("DatabaseSettings").GetValue<string>("DatabaseName"));
            Products = database.GetCollection<Product>(configuration.GetSection("DatabaseSettings").GetValue<string>("CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; set; }
    }
}

using System;

using ApiNextjs.Domain;

namespace ApiNextjs.Services;

public class ProductsService
{
   private static readonly List<Product> ProductsRepository = [];

    public void Create(Product product)
    {
        // store the product in the database
        ProductsRepository.Add(product);
    }

    public Product? Get(Guid productId)
    {
        // pull the product from the database
        return ProductsRepository.Find(x => x.Id == productId);
    }

}

using System.ComponentModel;

using ApiNextjs.Domain;
using ApiNextjs.Services;

using Microsoft.AspNetCore.Mvc;

namespace ApiNextjs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Products : Controller
    {
       
        private readonly ProductsService _productsService;

        [HttpPost]
        public IActionResult Create(CreateProductRequest request)
        {
            
            // mapping to internal reprsentation --> To Domain
            var product = request.ToDomain();

            // create the product --> Invoke use case
            _productsService.Create(product);

            // return 201 created response
            return CreatedAtAction(
               actionName: nameof(Get),
               routeValues: new {ProductId = product.Id},
               value:  ProductResponse.FromDomain(product));

        }

        [HttpGet("{productId:guid}")]
        public IActionResult Get(Guid productId)
        {
           // invoking the use case
            var product = _productsService.Get(productId);

            // mapping to external representation
            return product is null
                ? Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Product not found (product id: {productId})")
                : Ok(ProductResponse.FromDomain(product));
        }
    }

    public record CreateProductRequest(
            string Name,
            string Category,
            string SubCategory)
    {
        public Product ToDomain()
        {
            return new Product { 
                Name = Name, 
                Category = Category, 
                SubCategory = SubCategory, 
            };
        }
    }

    public record ProductResponse(
        Guid Id, 
        string Name, 
        string Category, 
        string SubCategory
    )
    {
       public static ProductResponse FromDomain(Product product)
       {
            return new ProductResponse(
                product.Id, 
                product.Name, 
                product.Category, 
                product.SubCategory
            );
        }
    }
}

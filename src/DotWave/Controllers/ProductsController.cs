using DotWave.Domain;
using DotWave.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotWave.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(ProductService productService) : ControllerBase
    {
        [HttpGet("{productId:guid}")]
        public IActionResult Get(Guid productId)
        {
            var product = productService.Get(productId);
            return product is null
                ? Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Product not found (product id: {productId})"
                )
                : Ok(ProductResponse.FromDomain(product));
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductRequest request)
        {
            var product = request.ToDomain();

            productService.Create(product);

            return CreatedAtAction(
                actionName: nameof(Get),
                routeValues: new { ProductId = product.Id },
                value: ProductResponse.FromDomain(product)
            );
        }
    }

    public record CreateProductRequest(string Name, string Category, string SubCategory)
    {
        public Product ToDomain()
        {
            return new Product
            {
                Name = Name,
                Category = Category,
                SubCategory = SubCategory
            };
        }
    }

    public record ProductResponse(Guid Id, string Name, string Category, string SubCategory)
    {
        public static ProductResponse FromDomain(Product product)
        {
            return new ProductResponse(
                Id: product.Id,
                Name: product.Name,
                Category: product.Category,
                SubCategory: product.SubCategory
            );
        }
    }
}

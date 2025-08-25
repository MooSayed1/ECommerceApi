using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared;
namespace Presintation;


public class ProductsController(IServiceManager serviceManager) :  ApiController 
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery]ProductParametersSpecifications productParametersSpecifications)
    {
        var products = await serviceManager.ProductService.GetAllProductsAsync(productParametersSpecifications);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
    {
        var product = await serviceManager.ProductService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("Brands")]
    public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
    {
        var brands = await serviceManager.ProductService.GetAllProductBrandsAsync();
        return Ok(brands);
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
    {
        var types = await serviceManager.ProductService.GetAllProductTypeAsync();
        return Ok(types);
    }
}
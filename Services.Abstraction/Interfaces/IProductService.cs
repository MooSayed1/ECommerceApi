using Shared;

namespace Services.Abstraction.Interfaces;

public interface IProductService
{
    Task <PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductParametersSpecifications productParametersSpecifications ,bool asNoTracking = false);
    Task<ProductResultDto?> GetProductByIdAsync(int id);
    Task<IEnumerable<BrandResultDto>> GetAllProductBrandsAsync();
    Task<IEnumerable<TypeResultDto>> GetAllProductTypeAsync();
}
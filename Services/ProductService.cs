using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction.Interfaces;
using Services.Specifications;
using Shared;

namespace Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductParametersSpecifications productParametersSpecifications ,bool asNoTracking = false)
    {
        var products = await unitOfWork.GetRepo<Product, int>().GetAllAsync(new ProductWithBrandAndTypeSpecifications(productParametersSpecifications));
        var mappedProducts = mapper.Map<IEnumerable<ProductResultDto>>(products);
        
        var totalCount = await unitOfWork.GetRepo<Product, int>().CountAsync(new ProductWithCountSpecifications(productParametersSpecifications));
        
        var paginatedResult = new PaginatedResult<ProductResultDto>(
            productParametersSpecifications.PageIndex,
            productParametersSpecifications.PageSize,
            totalCount,
           mappedProducts 
        );
        
        return paginatedResult;
        // return mappedProducts;
    }

    public async Task<ProductResultDto?> GetProductByIdAsync(int id)
    {
        var product = await unitOfWork.GetRepo<Product, int>()
            .GetByIdAsync(new ProductWithBrandAndTypeSpecifications(id));
        
        return product is null? throw new ProductNotFoundException(id) : mapper.Map<ProductResultDto?>(product);
    }

    public async Task<IEnumerable<BrandResultDto>> GetAllProductBrandsAsync()
    {
        var brands = await unitOfWork.GetRepo<ProductBrand, int>().GetAllAsync();
        var mappedBrands = mapper.Map<IEnumerable<BrandResultDto>>(brands);
        return mappedBrands;
    }

    public async Task<IEnumerable<TypeResultDto>> GetAllProductTypeAsync()
    {
        var types = await unitOfWork.GetRepo<ProductType, int>().GetAllAsync();
        var mappedTypes = mapper.Map<IEnumerable<TypeResultDto>>(types);
        return mappedTypes;
    }
}
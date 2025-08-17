using Domain.Entities;
using Shared;

namespace Services.Specifications;

public class ProductWithCountSpecifications : Specifications<Product>
{
    public ProductWithCountSpecifications(ProductParametersSpecifications parameters) : base(product =>
        (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) && (
            !parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) &&
        (string.IsNullOrEmpty(parameters.Search) ||
         product.Name.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()))
    )
    {
    }
}
using Domain.Entities;
using Shared;

namespace Services.Specifications;

public class ProductWithBrandAndTypeSpecifications : Specifications<Product>
{
    public ProductWithBrandAndTypeSpecifications(int id) : base(product => product.Id == id)
    {
        IncludeExpressions.Add(product => product.ProductBrand);
        IncludeExpressions.Add(product => product.ProductType);
    }

    public ProductWithBrandAndTypeSpecifications() : base(null)
    {
        IncludeExpressions.Add(product => product.ProductBrand);
        IncludeExpressions.Add(product => product.ProductType);
    }

    public ProductWithBrandAndTypeSpecifications(ProductParametersSpecifications parameters) : base(product =>
        (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
        (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) &&
        (string.IsNullOrEmpty(parameters.Search) ||
         product.Name.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()))
    )
    {
        IncludeExpressions.Add(product => product.ProductBrand);
        IncludeExpressions.Add(product => product.ProductType);

        if (parameters.Sort is not null)
        {
            switch (parameters.Sort)
            {
                case ProductSortOptions.PriceDesc:
                    SetOrderByDescending(product => product.Price);
                    break;
                case ProductSortOptions.PriceAsc:
                    SetOrderByAscending(product => product.Price);
                    break;
                case ProductSortOptions.NameAsc:
                    SetOrderByAscending(product => product.Name);
                    break;
                default:
                    SetOrderByDescending(product => product.Name);
                    break;
            }
        }

        ApplyPagination(parameters.PageIndex, parameters.PageSize);
    }
}
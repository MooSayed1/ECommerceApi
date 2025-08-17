namespace Shared;

public class ProductParametersSpecifications
{
    public ProductSortOptions? Sort { get; set; }
    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public string? Search { get; set; }
    public int PageIndex { get; set; } = 1;
    public const int DefaultPageSize = 5;
    public const int MaxPageSize = 10;
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? DefaultPageSize : value;
    }
}

public enum ProductSortOptions
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc
}
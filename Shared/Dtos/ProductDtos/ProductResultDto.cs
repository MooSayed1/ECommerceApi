namespace Shared;

public record ProductResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public string TypeName { get; set; }
    public string BrandName { get; set; }
    public int TotalCount { get; set; }
}
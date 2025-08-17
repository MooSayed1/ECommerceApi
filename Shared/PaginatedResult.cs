namespace Shared;

public record PaginatedResult<TEntity>(
    int PageIndex,
    int PageSize,
    int TotalCount,
    IEnumerable<TEntity> Data)
{
    public int TotalPages => (PageSize == 0) ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}
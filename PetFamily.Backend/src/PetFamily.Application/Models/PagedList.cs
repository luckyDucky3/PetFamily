namespace PetFamily.Application.Models;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public long TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public bool HasNextPage => TotalCount > Page * PageSize;
    public bool HasPreviousPage => Page > 1;
}
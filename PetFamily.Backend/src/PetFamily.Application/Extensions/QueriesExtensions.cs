using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;

namespace PetFamily.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> ToPageList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>
        { 
            Items = items, 
            TotalCount = totalCount, 
            Page = page, 
            PageSize = pageSize
        };
    }
}
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Helpers;

public static class PaginationHelper
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 50 ? 50 : pageSize;

        var totalItems = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}

using System.Collections;

namespace Curriculum.EF.Models;

public class PagedList<T>: IPagedList<T>
{
    private readonly List<T> _items = new();

    internal PagedList()
    {
    }

    /// <summary>
    ///     Return the paged query result
    /// </summary>
    /// <param name="index">Index to fetch item from paged query result</param>
    /// <returns>/returns item from paged query result</returns>
    public T this[int index] => _items[index];

    /// <summary>
    ///     Return the number of records in the paged query result
    /// </summary>
    public long Count => _items.Count();

    /// <summary>
    ///     Generic Enumerator
    /// </summary>
    /// <returns>Generic Enumerator of paged query result</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    /// <summary>
    ///     Enumerator
    /// </summary>
    /// <returns>Enumerator of paged query result</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    /// <summary>
    ///     Gets current page number
    /// </summary>
    public long PageNumber { get; protected set; }

    /// <summary>
    ///     Gets page size
    /// </summary>
    public long PageSize { get; protected set; }

    /// <summary>
    ///     Gets number of pages
    /// </summary>
    public long PageCount { get; protected set; }

    /// <summary>
    ///     Gets the total number records
    /// </summary>
    public long TotalItemCount { get; protected set; }

    /// <summary>
    ///     Gets a value indicating whether there is a previous page
    /// </summary>
    public bool HasPreviousPage { get; protected set; }

    /// <summary>
    ///     Gets a value indicating whether there is next page
    /// </summary>
    public bool HasNextPage { get; protected set; }

    /// <summary>
    ///     Gets a value indicating whether the current page is first page
    /// </summary>
    public bool IsFirstPage { get; protected set; }

    /// <summary>
    ///     Gets a value indicating whether the current page is last page
    /// </summary>
    public bool IsLastPage { get; protected set; }

    /// <summary>
    ///     Gets one-based index of first item in current page
    /// </summary>
    public long FirstItemOnPage { get; protected set; }

    /// <summary>
    ///     Gets one-based index of last item in current page
    /// </summary>
    public long LastItemOnPage { get; protected set; }

    internal IPagedList<T> ProcessResults(int pageSize, IReadOnlyList<T> items, long totalResults)
    {
        _items.AddRange(items);

        // fetch the total record count
        TotalItemCount = totalResults > 0 ? totalResults : items.Count;

        // compute the number of pages based on page size and total records
        PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)pageSize) : 0;

        // compute if there is a previous page
        HasPreviousPage = PageNumber > 1;

        // compute if there is next page
        HasNextPage = PageNumber < PageCount;

        // compute if the current page is first page
        IsFirstPage = PageCount > 0 && PageNumber == 1;

        // compute if the current page is last page
        IsLastPage = PageCount > 0 && PageNumber >= PageCount;

        // compute one-based index of first item on a specific page
        FirstItemOnPage = PageCount > 0 ? ((PageNumber - 1) * PageSize) + 1 : 0;

        // compute one-based index of last item on a specific page
        var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
        LastItemOnPage = numberOfLastItemOnPage > TotalItemCount ? TotalItemCount : numberOfLastItemOnPage;
        return this;
    }
}

public static class PagedList
{
    public static IPagedList<T> ToPagedListEx<T>(this IQueryable<T> items, int pageSize, long totalCount = 0) {
        return new PagedList<T>().ProcessResults(pageSize, items.ToList(), totalCount);
    }
}
using System.Collections;

namespace Curriculum.EF.Models;

//
// Summary:
//     Interface for paged list
//
// Type parameters:
//   T:
public interface IPagedList<out T> : IEnumerable<T>, IEnumerable
{
    //
    // Summary:
    //     Return the paged query result
    //
    // Parameters:
    //   index:
    //     Index to fetch item from paged query result
    //
    // Returns:
    //     /returns item from paged query result
    T this[int index] { get; }

    //
    // Summary:
    //     Return the number of records in the paged query result
    long Count { get; }

    //
    // Summary:
    //     Gets current page number
    long PageNumber { get; }

    //
    // Summary:
    //     Gets page size
    long PageSize { get; }

    //
    // Summary:
    //     Gets number of pages
    long PageCount { get; }

    //
    // Summary:
    //     Gets the total number records
    long TotalItemCount { get; }

    //
    // Summary:
    //     Gets a value indicating whether there is a previous page
    bool HasPreviousPage { get; }

    //
    // Summary:
    //     Gets a value indicating whether there is next page
    bool HasNextPage { get; }

    //
    // Summary:
    //     Gets a value indicating whether the current page is first page
    bool IsFirstPage { get; }

    //
    // Summary:
    //     Gets a value indicating whether the current page is last page
    bool IsLastPage { get; }

    //
    // Summary:
    //     Gets one-based index of first item in current page
    long FirstItemOnPage { get; }

    //
    // Summary:
    //     Gets one-based index of last item in current page
    long LastItemOnPage { get; }
}

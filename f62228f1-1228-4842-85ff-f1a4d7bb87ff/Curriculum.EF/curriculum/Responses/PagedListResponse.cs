
using Newtonsoft.Json;
using Ser = System.Text.Json.Serialization;

namespace Curriculum.EF.Models;

public class PagedListResponse<T>
{
    [Ser.JsonPropertyName("items")]
    [JsonProperty("items")]
    public IReadOnlyList<T> Items { get; set;}

    [Ser.JsonPropertyName("totalItemCount")]
    [JsonProperty("totalItemCount")]
    public long TotalItemCount { get; set;}

    [Ser.JsonPropertyName("hasNextPage")]
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set;}

    public PagedListResponse() {        
    }

    // [JsonConstructor]
    public PagedListResponse(IEnumerable<T> items, long totalItemCount, bool hasNextPage)
    {
        Items = items.ToList();
        TotalItemCount = totalItemCount;
        HasNextPage = hasNextPage;
    }
}

public static class PagedListResponse
{
    public static PagedListResponse<T> From<T>(IPagedList<T> pagedList)
    {
        return new PagedListResponse<T>(pagedList, pagedList.TotalItemCount, pagedList.HasNextPage);
    }
}
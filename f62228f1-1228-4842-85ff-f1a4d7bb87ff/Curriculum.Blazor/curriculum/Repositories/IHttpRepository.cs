using Curriculum.EF.Models;

namespace Curriculum.Blazor
{
    public interface IHttpRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetItems(ItemParameters itemParameters);
        Task<PagedListResponse<T>> GetPagedItems(ItemParameters itemParameters);
        Task<T> CreateItem(T item);
        Task<string> UploadImage(MultipartFormDataContent content);
        Task<T> GetItem(string id);
        Task<T> UpdateItem(Guid id, T item);
        Task DeleteItem(Guid id);
        Task<int> GetCount(string searchTerm = "");

        Task<IReadOnlyList<SummaryResponse>> PostSummary(SummaryRequest request);
    }
}

using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Curriculum.EF.Models;

namespace Curriculum.Blazor
{
    public class HttpRepository<T> : IHttpRepository<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly IApplicationState _state;
        private readonly JsonSerializerOptions _options;
        private readonly string _endPoint;

        public HttpRepository(HttpClient client, IApplicationState state)
        {
            _client = client;
            _state = state;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true,
                Converters = {
                    new JsonStringEnumConverter(),
                }
            };
            _endPoint = $"data/{typeof(T).Name.Plural()}.json";
        }

        public async Task<IReadOnlyList<T>> GetItems(ItemParameters itemParameters)
        {
            _state.ToggleLoading();
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = itemParameters.PageNumber.ToString(),
                ["pageSize"] = itemParameters.PageSize.ToString(),
                ["searchTerm"] = itemParameters.SearchTerm ?? string.Empty,
                ["searchCase"] = itemParameters.SearchCase.ToString(),
                ["sortBy"] = itemParameters.SortBy ?? string.Empty,
                ["sortDirection"] = itemParameters.SortDirection.ToString(),
                ["parentId"] = itemParameters.ParentId.ToString(),
            };
            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_endPoint, queryStringParam));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(content);
            }

            _state.ToggleLoading();

            return JsonSerializer.Deserialize<List<T>>(content, _options);
        }
        
        public async Task<PagedListResponse<T>> GetPagedItems(ItemParameters itemParameters)
        {
            _state.ToggleLoading();
            var url = _endPoint;
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = itemParameters.PageNumber.ToString(),
                ["pageSize"] = itemParameters.PageSize.ToString(),
                ["searchTerm"] = itemParameters.SearchTerm ?? string.Empty,
                ["searchCase"] = itemParameters.SearchCase.ToString(),
                ["sortBy"] = itemParameters.SortBy ?? string.Empty,
                ["sortDirection"] = itemParameters.SortDirection.ToString(),
                ["parentId"] = itemParameters.ParentId.ToString(),
            };
            HttpResponseMessage response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(content);
            }

            _state.ToggleLoading();
            return JsonSerializer.Deserialize<PagedListResponse<T>>(content, _options);
        }

        public async Task<T> CreateItem(T item)
        {
            _state.ToggleLoading();
            var content = JsonSerializer.Serialize(item);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var postResult = await _client.PostAsync(_endPoint, bodyContent);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(postContent);
            }

            _state.ToggleLoading();

            return JsonSerializer.Deserialize<T>(postContent, _options);
        }

        public async Task<string> UploadImage(MultipartFormDataContent content)
        {
            _state.ToggleLoading();
            var postResult = await _client.PostAsync("upload", content);
            var postContent = await postResult.Content.ReadAsStringAsync();
            if (!postResult.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(postContent);
            }
            else
            {
                _state.ToggleLoading();
                var imgUrl = Path.Combine("https://localhost:5011/", postContent);
                return imgUrl;
            }
        }

        public async Task<T> GetItem(string id)
        {
            _state.ToggleLoading();
            var url = Path.Combine(_endPoint, id);

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(content);
            }

            _state.ToggleLoading();
            var item = JsonSerializer.Deserialize<T>(content, _options);
            return item;
        }

        public async Task<int> GetCount(string searchTerm = "")
        {
            _state.ToggleLoading();
            var url = Path.Combine(_endPoint, "count");
            var response = await _client.GetAsync($"{url}?searchTerm={searchTerm}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(content);
            }

            _state.ToggleLoading();
            var count = JsonSerializer.Deserialize<int>(content, _options);
            return count;
        }

        public async Task<T> UpdateItem(Guid id, T item)
        {
            _state.ToggleLoading();
            var content = JsonSerializer.Serialize(item);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var url = Path.Combine(_endPoint, id.ToString());

            var postResult = await _client.PutAsync(url, bodyContent);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(postContent);
            }

            _state.ToggleLoading();
            return JsonSerializer.Deserialize<T>(postContent, _options);
        }

        public async Task DeleteItem(Guid id)
        {
            _state.ToggleLoading();
            var url = Path.Combine(_endPoint, id.ToString());

            var deleteResult = await _client.DeleteAsync(url);
            var deleteContent = await deleteResult.Content.ReadAsStringAsync();

            if (!deleteResult.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(deleteContent);
            }

            _state.ToggleLoading();
        }

        public async Task<IReadOnlyList<SummaryResponse>> PostSummary(SummaryRequest request) {
            _state.ToggleLoading();
            var url = Path.Combine(_endPoint, "summary");
            var reqContent = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(reqContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, bodyContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _state.ToggleLoading();
                throw new ApplicationException(content);
            }

            _state.ToggleLoading();
            return JsonSerializer.Deserialize<IReadOnlyList<SummaryResponse>>(content, _options);            
        }
    }
}

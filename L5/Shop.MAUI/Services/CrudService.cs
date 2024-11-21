using Shared.Services;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Shop.MAUI.Services
{
    public class CrudService<T, TKey> : ICrudService<T, TKey>
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public CrudService(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<ServiceReponse<IEnumerable<T>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_endpoint}");
            return await DeserializeResponse<IEnumerable<T>>(response);
        }

        public async Task<ServiceReponse<T>> GetByIdAsync(TKey id)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
            return await DeserializeResponse<T>(response);
        }

        public async Task<ServiceReponse<T>> CreateAsync(T entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_endpoint}", entity);
            return await DeserializeResponse<T>(response);
        }

        public async Task<ServiceReponse<T>> UpdateAsync(TKey id, T entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", entity);
            return await DeserializeResponse<T>(response);
        }

        public async Task<ServiceReponse<bool>> DeleteAsync(TKey id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
            return await DeserializeResponse<bool>(response);
        }

        public async Task<ServiceReponse<bool>> DeleteAllAsync()
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}");
            return await DeserializeResponse<bool>(response);
        }

        private async Task<ServiceReponse<TData>> DeserializeResponse<TData>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceReponse<TData>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

}

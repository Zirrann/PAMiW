using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Shop.BLZR.Services
{
    public class CrudServiceDto<T, TKey> : ICrudService<T, TKey>
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _endpoint;

        public CrudServiceDto(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<ServiceReponse<IEnumerable<T>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_endpoint}");
            return await DeserializeResponse<IEnumerable<T>>(response);
        }

        public async Task<ServiceReponse<IEnumerable<T>>> GetFilteredProducts(
            ObservableCollection<ProductDto> products,
            ObservableCollection<CategoryDto> categories,
            ProductDto filterData,
            ProductDto sortBy,
            bool sortRising,
            int pageNumber,
            int pageSize)
        {
            // Połączenie produktów z kategoriami i magazynami
            var combinedProducts = products
                            .Join(categories,
                                p => p.CategoryId,
                                c => c.CategoryId,
                                (p, c) => new { Product = p, Category = c })
                            .Select(pc => new { pc.Product, pc.Category })
                            .AsQueryable();

            // Filtracja
            if (!string.IsNullOrEmpty(filterData.Name))
            {
                combinedProducts = combinedProducts.Where(x => x.Product.Name.Contains(filterData.Name));
            }

            if (filterData.Price.HasValue)
            {
                combinedProducts = combinedProducts.Where(x => x.Product.Price == filterData.Price.Value);
            }

            if (filterData.CategoryId.HasValue)
            {
                combinedProducts = combinedProducts.Where(x => x.Product.CategoryId == filterData.CategoryId.Value);
            }

            if (filterData.Quantity.HasValue)
            {
                combinedProducts = combinedProducts.Where(x => x.Product.Quantity == filterData.Quantity.Value);
            }

            // Sortowanie
            if (!string.IsNullOrEmpty(sortBy.Name))
            {
                combinedProducts = sortRising
                    ? combinedProducts.OrderBy(x => x.Product.Name)
                    : combinedProducts.OrderByDescending(x => x.Product.Name);
            }
            else if (sortBy.Price.HasValue)
            {
                combinedProducts = sortRising
                    ? combinedProducts.OrderBy(x => x.Product.Price)
                    : combinedProducts.OrderByDescending(x => x.Product.Price);
            }
            else if (!string.IsNullOrEmpty(sortBy.CategoryId?.ToString()))
            {
                combinedProducts = sortRising
                    ? combinedProducts.OrderBy(x => x.Category.Name)
                    : combinedProducts.OrderByDescending(x => x.Category.Name);
            }
            else if (!string.IsNullOrEmpty(sortBy.Quantity?.ToString()))
            {
                combinedProducts = sortRising
                    ? combinedProducts.OrderBy(x => x.Product.Quantity)
                    : combinedProducts.OrderByDescending(x => x.Product.Quantity);
            }


            // Stronicowanie
            var pagedData = combinedProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Product) // Pobieramy tylko produkty
                .Cast<T>()
                .ToList();

            // Zwracamy odpowiedź z przefiltrowanymi danymi
            return new ServiceReponse<IEnumerable<T>>
            {
                Data = pagedData,
                Success = true
            };
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

        public async Task<ServiceReponse<T>> UpdateAsync(TKey? id, T entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", entity);
            return await DeserializeResponse<T>(response);
        }

        public virtual async Task<ServiceReponse<bool>> DeleteAsync(TKey? id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ServiceReponse<bool>() { Success = true, Data = true };
            }

            return new ServiceReponse<bool>() { Success = false, Message = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}" };
        }

        public virtual async Task<ServiceReponse<bool>> DeleteAllAsync()
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}");

            if (response.IsSuccessStatusCode)
            {
                return new ServiceReponse<bool>() { Success = true, Data = true };
            }

            return new ServiceReponse<bool>() {Success = false, Message = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"};
        }

        private async Task<ServiceReponse<TData>> DeserializeResponse<TData>(HttpResponseMessage response)
        {
            var serviceResponse = new ServiceReponse<TData>();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<TData>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    serviceResponse.Data = data;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Operation succeeded"; 
                }
                catch (JsonException ex)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Deserialization failed: {ex.Message}";
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {response.StatusCode} - {errorContent}";
            }

            return serviceResponse;
        }
    }

}

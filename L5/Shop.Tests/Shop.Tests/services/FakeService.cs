using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models.Dto;
using Shared.Services;


namespace Shop.Tests.services
{
    public class FakeService<T, TKey> : ICrudService<T, TKey>
    {
        protected readonly List<T> _dataStore = new List<T>();

        public virtual Task<ServiceReponse<IEnumerable<T>>> GetAllAsync()
        {
            return Task.FromResult(new ServiceReponse<IEnumerable<T>>
            {
                Success = true,
                Data = _dataStore
            });
        }

        public Task<ServiceReponse<IEnumerable<T>>> GetFilteredProducts(
            ObservableCollection<ProductDto> products,
            ObservableCollection<CategoryDto> categories,
            ProductDto filterData,
            ProductDto sortBy,
            bool sortRising,
            int pageNumber,
            int pageSize)
        {
            // Prosta implementacja, która zwraca wszystkie produkty.
            var filteredData = _dataStore.OfType<ProductDto>().Where(p => p.Name.Contains(filterData.Name)).ToList();
            return Task.FromResult(new ServiceReponse<IEnumerable<T>>
            {
                Success = true,
                Data = filteredData.Cast<T>()
            });
        }

        public Task<ServiceReponse<T>> GetByIdAsync(TKey id)
        {
            var entity = _dataStore.FirstOrDefault(e => e.Equals(id));
            return Task.FromResult(new ServiceReponse<T>
            {
                Success = entity != null,
                Data = entity
            });
        }

        public Task<ServiceReponse<T>> CreateAsync(T entity)
        {
            _dataStore.Add(entity);
            return Task.FromResult(new ServiceReponse<T>
            {
                Success = true,
                Data = entity
            });
        }

        public Task<ServiceReponse<T>> UpdateAsync(TKey? id, T entity)
        {
            var index = _dataStore.FindIndex(e => e.Equals(id));
            if (index != -1)
            {
                _dataStore[index] = entity;
                return Task.FromResult(new ServiceReponse<T>
                {
                    Success = true,
                    Data = entity
                });
            }
            return Task.FromResult(new ServiceReponse<T>
            {
                Success = false
            });
        }

        public Task<ServiceReponse<bool>> DeleteAsync(TKey? id)
        {
            var entity = _dataStore.FirstOrDefault(e => e.Equals(id));
            if (entity != null)
            {
                _dataStore.Remove(entity);
                return Task.FromResult(new ServiceReponse<bool> { Success = true, Data = true });
            }
            return Task.FromResult(new ServiceReponse<bool> { Success = false, Data = false });
        }

        public Task<ServiceReponse<bool>> DeleteAllAsync()
        {
            _dataStore.Clear();
            return Task.FromResult(new ServiceReponse<bool> { Success = true, Data = true });
        }
    }
}

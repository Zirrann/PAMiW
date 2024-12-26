using Shared.Models;
using Shared.Models.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ICrudService<T, TKey>
    {
        Task<ServiceReponse<IEnumerable<T>>> GetAllAsync();
        Task<ServiceReponse<IEnumerable<T>>> GetFilteredProducts(ObservableCollection<ProductDto> products, ObservableCollection<CategoryDto> categories,
            ProductDto filterData, ProductDto sortBy, bool sortRising, int pageNumber, int pageSize);
        Task<ServiceReponse<T>> GetByIdAsync(TKey id);
        Task<ServiceReponse<T>> CreateAsync(T entity);
        Task<ServiceReponse<T>> UpdateAsync(TKey? id, T entity);
        Task<ServiceReponse<bool>> DeleteAsync(TKey? id);
        Task<ServiceReponse<bool>> DeleteAllAsync();
    }
}

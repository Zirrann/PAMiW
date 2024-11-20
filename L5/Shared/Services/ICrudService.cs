using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ICrudService<T, TKey>
    {
        Task<ServiceReponse<IEnumerable<T>>> GetAllAsync();
        Task<ServiceReponse<T>> GetByIdAsync(TKey id);
        Task<ServiceReponse<T>> CreateAsync(T entity);
        Task<ServiceReponse<T>> UpdateAsync(TKey id, T entity);
        Task<ServiceReponse<bool>> DeleteAsync(TKey id);
        Task<ServiceReponse<bool>> DeleteAllAsync();
    }
}

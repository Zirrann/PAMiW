using Microsoft.EntityFrameworkCore;
using Shared.Services;

namespace Shop.DB.Services
{
    public class CrudService<T, TKey> : ICrudService<T, TKey> where T : class
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public CrudService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        protected ServiceReponse<T> HandleException<T>(Exception ex)
        {
            return new ServiceReponse<T>
            {
                Success = false,
                Message = ex.Message,
                Data = default
            };
        }

        public virtual async Task<ServiceReponse<IEnumerable<T>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<T>>();
            try
            {
                response.Data = await _dbSet.ToListAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceReponse<T>> GetByIdAsync(TKey id)
        {
            var response = new ServiceReponse<T>();
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "Entity not found.";
                }
                else
                {
                    response.Data = entity;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual async Task<ServiceReponse<T>> CreateAsync(T entity)
        {
            var response = new ServiceReponse<T>();
            try
            {
                _dbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
                response.Data = entity;
                response.Success = true;
                response.Message = "Entity created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceReponse<T>> UpdateAsync(TKey id, T entity)
        {
            var response = new ServiceReponse<T>();
            try
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity == null)
                {
                    response.Success = false;
                    response.Message = "Entity not found.";
                }
                else
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await _dbContext.SaveChangesAsync();
                    response.Data = entity;
                    response.Success = true;
                    response.Message = "Entity updated successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceReponse<bool>> DeleteAsync(TKey id)
        {
            var response = new ServiceReponse<bool>();
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "Entity not found.";
                    response.Data = false;
                }
                else
                {
                    _dbSet.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    response.Data = true;
                    response.Success = true;
                    response.Message = "Entity deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = false;
            }
            return response;
        }
    }
}
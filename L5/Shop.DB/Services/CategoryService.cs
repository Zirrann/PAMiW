using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class CategoryService : CrudService<Category, int>, ICategoryService
    {
        public CategoryService(AppDbContext dbContext) : base(dbContext) { }


    }
}

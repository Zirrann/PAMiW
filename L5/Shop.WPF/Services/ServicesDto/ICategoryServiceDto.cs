using Shared.Models.Dto;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.WPF.Services.ServicesDto
{
    public interface ICategoryServiceDto : ICrudService<CategoryDto, int>
    {
    }
}

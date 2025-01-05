using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;
using Shop.BLZR.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests.services
{
    public class FakeOrderProductServiceDto : FakeService<OrderProductDto, OrderProductKey>, IOrderProductServiceDto
    {
    }
}

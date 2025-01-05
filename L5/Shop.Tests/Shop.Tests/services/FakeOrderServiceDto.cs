using Shared.Models.Dto;
using Shop.BLZR.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests.services
{
    public class FakeOrderServiceDto : FakeService<OrderDto, int>, IOrderServiceDto
    {
    }
}

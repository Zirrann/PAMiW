using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4.Services
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; } = default;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = null;
    }
}

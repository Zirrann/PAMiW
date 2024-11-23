using Shared.Models;
using Shared.Services;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;

namespace Shop.MAUI.Services
{
    public class OrderProductServiceDto : CrudServiceDto<OrderProductDto, OrderProductKey>, IOrderProductServiceDto
    {
        public OrderProductServiceDto(HttpClient httpClient)
            : base(httpClient, "api/OrderProduct")
        {
        }


        public override async Task<ServiceReponse<bool>> DeleteAsync(OrderProductKey id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_endpoint}/{id.OrderId}/{id.ProductId}");
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceReponse<bool>
                    {
                        Success = true,
                        Data = true
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ServiceReponse<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = $"Failed to delete. Server responded with: {errorMessage}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceReponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = $"Exception occurred: {ex.Message}"
                };
            }
        }


    }
}

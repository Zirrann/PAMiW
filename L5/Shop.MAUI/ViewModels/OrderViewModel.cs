using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L4.Services;
using Newtonsoft.Json;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using Shop.MAUI.Views;
using System.Collections.ObjectModel;

namespace Shop.MAUI.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IOrderServiceDto _orderServiceDto;

        [ObservableProperty]
        private ObservableCollection<OrderDto> orders;


        public OrderViewModel(
            IMessageDialogService messageDialogService,
            IOrderServiceDto orderService)
        {
            _messageDialogService = messageDialogService;
            _orderServiceDto = orderService;

            LoadOrders();
        }

        private async void LoadOrders() 
        {
            var response = await _orderServiceDto.GetAllAsync();
            if (response.Success)
            {
                Orders = new ObservableCollection<OrderDto>(response.Data);
            }
            else
            {
                _messageDialogService.ShowMessage("Failed to load orders");
            }
        }


        [RelayCommand]
        private async Task DeleteOrderAsync(OrderDto order)
        {
            if (order == null)
                return;

            var response = await _orderServiceDto.DeleteAsync(order.OrderId);
            if (response.Success)
            {
                Orders.Remove(order);
            }
            else
            {
                _messageDialogService.ShowMessage(response.Message);
            }
        }


        [RelayCommand]
        private async Task NavigateToProductsAsync()
        {
            await Shell.Current.GoToAsync(nameof(ProductsPage));
        }

        [RelayCommand]
        private async Task ViewOrderDetailsAsync(OrderDto order)
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "OrderProducts", order.OrderProducts },
            };

            await Shell.Current.GoToAsync(nameof(OrderDetailsPage), false, navigationParameters);
        }



    }
}

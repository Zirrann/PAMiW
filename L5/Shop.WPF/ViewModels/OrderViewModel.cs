using System.Collections.ObjectModel;
using Shared.Models.Dto;
using Shop.WPF.Services.ServicesDto;

using L4.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;


namespace Shop.WPF.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IOrderServiceDto _orderServiceDto;
        private readonly IProductServiceDto _productServiceDto;
        private readonly IOrderProductServiceDto _orderProductServiceDto;

        [ObservableProperty]
        private ObservableCollection<OrderDto> orders;

        public OrderViewModel(
            IMessageDialogService messageDialogService,
            IOrderServiceDto orderService,
            IProductServiceDto productServiceDto,
            IOrderProductServiceDto orderProductService)
        {
            _messageDialogService = messageDialogService;
            _orderServiceDto = orderService;
            _productServiceDto = productServiceDto;
            _orderProductServiceDto = orderProductService;

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
        public async Task NavigateToProducts()
        {
            var productsWindow = App.ServiceProvider.GetRequiredService<ProductsWindow>();

            productsWindow.Show();
        }

        [RelayCommand]
        private async Task ViewOrderDetailsAsync(OrderDto order)
        {
            if (order == null) return;

            var orderDetailsViewModel = new OrderDetailsViewModel(order, _orderProductServiceDto, _messageDialogService, _productServiceDto);


            var orderDetailsWindow = new OrderDetailsWindow(orderDetailsViewModel)
            {
                DataContext = orderDetailsViewModel
            };

            // Show the window
            orderDetailsWindow.ShowDialog();
        }
    }
}

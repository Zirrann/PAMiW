using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using Shop.WPF.Services.ServicesDto;
using Shared.Models;
using Shared.Models.Dto;
using L4.Services;

namespace Shop.WPF.ViewModels
{
    public partial class OrderDetailsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IOrderProductServiceDto _orderProductServiceDto;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProductServiceDto _productService;

        [ObservableProperty]
        private ObservableCollection<OrderProductDto> orderProducts;

        [ObservableProperty]
        private ProductDto selectedProduct;

        [ObservableProperty]
        private ObservableCollection<ProductDto> selectedProducts;

        [ObservableProperty]
        private ObservableCollection<ProductDto> products = new ObservableCollection<ProductDto>();

        public OrderDetailsViewModel(
            OrderDto orderDto,
            IOrderProductServiceDto orderProductServiceDto,
            IMessageDialogService messageDialogService,
            IProductServiceDto productServiceDto)
        {
            _orderProductServiceDto = orderProductServiceDto;
            _messageDialogService = messageDialogService;
            _productService = productServiceDto;

            orderProducts = new ObservableCollection<OrderProductDto>(orderDto.OrderProducts);

            LoadProductsToAddList();
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (SelectedProduct == null || SelectedProduct.Id <= 0)
                return;

            var pOrder = new OrderProductDto
            {
                ProductId = SelectedProduct.Id,
                OrderId = OrderProducts.FirstOrDefault()?.OrderId ?? 0
            };

            var response = await _orderProductServiceDto.CreateAsync(pOrder);
            if (response.Success)
            {
                OrderProducts.Add(response.Data);
                SelectedProduct = null;
                AddProduct(response.Data.ProductId);

                Products.Remove(Products.FirstOrDefault(p => p.Id == response.Data.ProductId));
            }
            else
            {
                _messageDialogService.ShowMessage(response.Message);
            }
        }

        [RelayCommand]
        private async Task DeleteProductAsync(ProductDto product)
        {
            if (product == null)
                return;

            var orderProductDto = OrderProducts.FirstOrDefault(o => o.ProductId == product.Id);

            if (orderProductDto != null)
            {
                var response = await _orderProductServiceDto.DeleteAsync(new OrderProductKey(orderProductDto.ProductId, orderProductDto.OrderId));
                if (response.Success)
                {
                    OrderProducts.Remove(orderProductDto);
                    Products.Add(product);
                    SelectedProducts.Remove(product);
                }
                else
                {
                    _messageDialogService.ShowMessage(response.Message);
                }
            }
        }

        private async void LoadProductsToAddList()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
            {
                Products = new ObservableCollection<ProductDto>(response.Data);

                var orderProductIds = new HashSet<int>(OrderProducts.Select(orderP => orderP.ProductId));
                var productsToRemove = Products.Where(p => orderProductIds.Contains(p.Id)).ToList();
                foreach (var product in productsToRemove)
                {
                    Products.Remove(product);
                }
            }
            else
            {
                _messageDialogService.ShowMessage("Failed to load products");
            }
            LoadProductsToDisplay();
        }

        private async void LoadProductsToDisplay()
        {
            SelectedProducts = new ObservableCollection<ProductDto>();

            foreach (var orderProduct in OrderProducts)
            {
                var response = await _productService.GetByIdAsync(orderProduct.ProductId);
                if (response.Success && response.Data != null)
                {
                    SelectedProducts.Add(response.Data);
                }
            }
        }

        private async void AddProduct(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product.Success && product.Data != null)
            {
                if (!Products.Any(p => p.Id == product.Data.Id))
                {
                    SelectedProducts.Add(product.Data);
                }
                else
                {
                    _messageDialogService.ShowMessage("Product already exists in the collection.");
                }
            }
            else
            {
                _messageDialogService.ShowMessage($"Failed to fetch product with ID: {productId}");
            }
        }
    }
}

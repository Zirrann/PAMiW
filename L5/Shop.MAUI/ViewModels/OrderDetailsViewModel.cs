using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L4.Services;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.MAUI.ViewModels
{
    public partial class OrderDetailsViewModel : ObservableObject, INotifyPropertyChanged, IQueryAttributable
    {
        private readonly IOrderProductServiceDto _orderProductServiceDto;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProductServiceDto _productService;

        [ObservableProperty]
        private ObservableCollection<OrderProductDto> orderProducts;

        [ObservableProperty]
        private ProductDto selectedProduct;

        [ObservableProperty]
        private ObservableCollection<ProductDto> slectedProducts;

        [ObservableProperty]
        private ObservableCollection<ProductDto> products = new ObservableCollection<ProductDto>();


        public OrderDetailsViewModel(
            IOrderProductServiceDto orderProductServiceDto,
            IMessageDialogService messageDialogService,
            IProductServiceDto productServiceDto)
        {
            _orderProductServiceDto = orderProductServiceDto;
            _messageDialogService = messageDialogService;
            _productService = productServiceDto;


            LoadProductsToAddList();
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (SelectedProduct is null || SelectedProduct.Id <= 0)
                return;
            var pOrder = new OrderProductDto
            {
                ProductId = SelectedProduct.Id,
                OrderId = OrderProducts.First().OrderId
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
            
            OrderProductDto orderProductDto = orderProducts.FirstOrDefault(o => o.ProductId == product.Id);

            var response = await _orderProductServiceDto.DeleteAsync(new OrderProductKey(orderProductDto.ProductId, orderProductDto.OrderId));
            if (response.Success)
            {
                OrderProducts.Remove(orderProductDto);
                Products.Add(product);
                SlectedProducts.Remove(product);
            }
            else
            {
                _messageDialogService.ShowMessage(response.Message);
            }
        }

        private async void LoadProductsToAddList()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
            {
                Products = new ObservableCollection<ProductDto>(response.Data);

                var orderProductIds = new HashSet<int>(orderProducts.Select(orderP => orderP.ProductId));
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
            SlectedProducts = new ObservableCollection<ProductDto>();

            foreach (var orderProduct in OrderProducts) 
            {
                var response = await _productService.GetByIdAsync(orderProduct.ProductId);
                if (response.Success && response.Data != null)
                {
                    SlectedProducts.Add(response.Data);
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
                    SlectedProducts.Add(product.Data);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var orderProductsList = query["OrderProducts"] as List<OrderProductDto>;
            OnPropertyChanged("OrderProducts");
            if (orderProductsList != null)
            {
                OrderProducts = new ObservableCollection<OrderProductDto>(orderProductsList);
            }
        }
    }
}

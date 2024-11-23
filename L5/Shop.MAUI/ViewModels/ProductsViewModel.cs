using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L4.Services;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using Shop.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.MAUI.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductServiceDto _productService;
        private readonly ICategoryServiceDto _categoryService;
        private readonly IStockServiceDto _stockService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private ObservableCollection<ProductDto> products;

        [ObservableProperty]
        private string newProductName;

        [ObservableProperty]
        private decimal newProductPrice;

        [ObservableProperty]
        private CategoryDto selectedCategory;

        [ObservableProperty]
        private int selectedStockQuantity;

        [ObservableProperty]
        private ObservableCollection<CategoryDto> categories;

        public ProductsViewModel(
            IProductServiceDto productService,
            ICategoryServiceDto categoryService,
            IStockServiceDto stockService,
            IMessageDialogService messageDialogService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _stockService = stockService;
            _messageDialogService = messageDialogService;


            LoadProducts();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var response = await _categoryService.GetAllAsync();
            if (response.Success)
            {
                Categories = new ObservableCollection<CategoryDto>(response.Data);
            }
            else
            {
                _messageDialogService.ShowMessage("Nie udało się załadować kategorii.");
            }
        }


        private async void LoadProducts()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
            {
                Products = new ObservableCollection<ProductDto>(response.Data);
            }
            else
            {
                _messageDialogService.ShowMessage("Failed to load products");
            }
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (string.IsNullOrWhiteSpace(NewProductName) || NewProductPrice <= 0 || SelectedCategory == null || SelectedStockQuantity <= 0)
                return;

            var stock = new StockDto
            {
                StockId = 0,
                Quantity = SelectedStockQuantity
            };


            var stockResponse = await _stockService.CreateAsync(stock);
            if (!stockResponse.Success)
            {
                _messageDialogService.ShowMessage(stockResponse.Message);
                return;
            }


            var newProduct = new ProductDto
            {
                Name = NewProductName,
                Price = NewProductPrice,
                CategoryId = SelectedCategory.CategoryId,
                StockId = stockResponse.Data.StockId
            };

            var response = await _productService.CreateAsync(newProduct);
            if (response.Success)
            {
                Products.Add(response.Data);
                NewProductName = string.Empty;
                NewProductPrice = 0;
                SelectedCategory = null;
                SelectedStockQuantity = 0;
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

            var response = await _productService.DeleteAsync(product.Id);
            if (response.Success)
            {
                Products.Remove(product);
            }
            else
            {
                _messageDialogService.ShowMessage(response.Message);
            }
        }


        [RelayCommand]
        private async Task NavigateToProductDetailsAsync(ProductDto product)
        {
            if (product == null) return;

            var navigation = Application.Current.MainPage.Navigation;
            var viewModel = new ProductDetailsViewModel(
                                    product,
                                    _categoryService,
                                    _stockService,
                                    navigation,
                                    _messageDialogService,
                                    _productService);

            var detailsPage = new ProductDetailsPage(viewModel);

            await navigation.PushAsync(detailsPage);
        }


    }

}

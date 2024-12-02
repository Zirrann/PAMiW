using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Shared.Models.Dto;
using Shop.WPF.Services.ServicesDto;
using L4.Services;

namespace Shop.WPF.ViewModels
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
                MessageBox.Show("Nie udało się załadować kategorii.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Nie udało się załadować produktów.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (string.IsNullOrWhiteSpace(NewProductName) || NewProductPrice <= 0 || SelectedCategory == null || SelectedStockQuantity <= 0)
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var stock = new StockDto
            {
                StockId = 0,
                Quantity = SelectedStockQuantity
            };

            var stockResponse = await _stockService.CreateAsync(stock);
            if (!stockResponse.Success)
            {
                MessageBox.Show(stockResponse.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(response.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteProductAsync(ProductDto product)
        {
            if (product == null) return;

            var response = await _productService.DeleteAsync(product.Id);
            if (response.Success)
            {
                Products.Remove(product);
            }
            else
            {
                MessageBox.Show(response.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void NavigateToProductDetails(ProductDto product)
        {
            if (product == null) return;

            // Create the ViewModel for ProductDetails
            var productDetailsViewModel = new ProductDetailsViewModel(
                product,
                _categoryService,
                _stockService,
                _messageDialogService,
                _productService);

            var detailsWindow = new ProductDetailsWindow(productDetailsViewModel);

            detailsWindow.ShowDialog();
        }
    }
}

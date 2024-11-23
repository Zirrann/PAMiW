﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L4.Services;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using System.Collections.ObjectModel;

namespace Shop.MAUI.ViewModels
{
    public partial class ProductDetailsViewModel : ObservableObject
    {
        private readonly ICategoryServiceDto _categoryService;
        private readonly IStockServiceDto _stockService;
        private readonly INavigation _navigation;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProductServiceDto _productService;

        [ObservableProperty]
        private ProductDto _product;

        [ObservableProperty]
        private ObservableCollection<CategoryDto> categories;

        [ObservableProperty]
        private CategoryDto selectedCategory;

        [ObservableProperty]
        private int selectedStockQuantity;

        private StockDto selectedStock;


        public ProductDetailsViewModel(
            ProductDto product,
            ICategoryServiceDto categoryService,
            IStockServiceDto stockService,
            INavigation navigation,
            IMessageDialogService messageDialogService,
            IProductServiceDto productService)
        {
            _product = product;
            _categoryService = categoryService;
            _stockService = stockService;
            _navigation = navigation;
            _messageDialogService = messageDialogService;
            _productService = productService;   
            
            LoadProductDetails();
            LoadCategories();
        }


        [RelayCommand]
        private async Task EditProductAsync()
        {
            if (Product == null || SelectedCategory == null || SelectedStockQuantity <=0 )
            {
                _messageDialogService.ShowMessage("Proszę wypełnić wszystkie pola.");
                return;
            }

            selectedStock.Quantity = SelectedStockQuantity;

            var stockResponse = await _stockService.UpdateAsync(selectedStock.StockId, selectedStock);
            if (!stockResponse.Success) 
            {
                _messageDialogService.ShowMessage(stockResponse.Message);
                return;
            } 

            Product.CategoryId = SelectedCategory.CategoryId;
            Product.StockId = selectedStock.StockId;

            var response = await _productService.UpdateAsync(Product.Id, Product);
            if (response.Success)
            {
                _messageDialogService.ShowMessage("Produkt został pomyślnie zaktualizowany.");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                _messageDialogService.ShowMessage(response.Message);
            }
        }

        private async void LoadProductDetails()
        {
            var categoryResponse = await _categoryService.GetByIdAsync(Product.CategoryId);
            if (categoryResponse.Success)
            {
                SelectedCategory = categoryResponse.Data;
            }

            var stockResponse = await _stockService.GetByIdAsync(Product.StockId);
            if (stockResponse.Success)
            {
                selectedStock = stockResponse.Data;
                SelectedStockQuantity = selectedStock.Quantity;
            }
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

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await _navigation.PopAsync();
        }
    }
}

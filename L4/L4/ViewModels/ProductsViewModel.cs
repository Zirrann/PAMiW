using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L4.Models;
using L4.Services;
using L4.Services.ProductService;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace L4.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IConnectivity _connectivity;

        // Obserwowalne właściwości
        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private Product selectedProduct;

        [ObservableProperty]
        private string searchText;

        // Konstruktor
        public ProductsViewModel(IProductService productService, IMessageDialogService messageDialogService, IConnectivity connectivity)
        {
            _productService = productService;
            _messageDialogService = messageDialogService;
            _connectivity = connectivity;

            // Pobieranie produktów przy inicjalizacji ViewModelu
            GetProductsAsync();
        }

        // Pobierz wszystkie produkty
        [RelayCommand]
        public async Task GetProductsAsync()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            var result = await _productService.GetProductsAsync();
            if (result.Success)
            {
                Products = new ObservableCollection<Product>(result.Data);
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        // Polecenie wyszukiwania
        [RelayCommand]
        public async Task SearchAsync()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            // Wywołanie usługi do wyszukiwania produktów
            var result = await _productService.SearchProductsAsync(SearchText, 0, 20); // Można dostosować paginację
            if (result.Success)
            {
                Products = new ObservableCollection<Product>(result.Data);
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        // Wyświetlenie szczegółów wybranego produktu
        [RelayCommand]
        public async Task ShowDetailsAsync(Product product)
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            SelectedProduct = product;

            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                { "Product", product },
                { nameof(ProductsViewModel), this }
            });
        }

        // Utworzenie nowego produktu
        [RelayCommand]
        public async Task NewAsync()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            SelectedProduct = new Product();

            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                { "Product", SelectedProduct },
                { nameof(ProductsViewModel), this }
            });
        }
    }
}

using L4.Services;
using Microsoft.Extensions.DependencyInjection;
using Shop.WPF.Services;
using Shop.WPF.Services.ServicesDto;
using Shop.WPF.ViewModels;
using System.Net.Http;
using System.Windows;

namespace Shop.WPF;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Konfiguracja DI
        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        var viewModel = ServiceProvider.GetRequiredService<OrderViewModel>();
        mainWindow.DataContext = viewModel;
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Rejestracja serwisów, ViewModeli i widoków
        ConfigureAppServices(services);
        ConfigureViewModels(services);
        ConfigureViews(services);
    }

    private void ConfigureAppServices(IServiceCollection services)
    {
        // Rejestracja serwisów
        services.AddSingleton<ICategoryServiceDto, CategoryServiceDto>();
        services.AddSingleton<IOrderProductServiceDto, OrderProductServiceDto>();
        services.AddSingleton<IOrderServiceDto, OrderServiceDto>();
        services.AddSingleton<IProductServiceDto, ProductServiceDto>();
        services.AddSingleton<IStockServiceDto, StockServiceDto>();
        services.AddSingleton<IMessageDialogService, WpfMessageDialogService>();

        services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5104/")
        });
    }

    private void ConfigureViewModels(IServiceCollection services)
    {
        // Rejestracja ViewModeli
        services.AddSingleton<OrderViewModel>();
        services.AddTransient<ProductsViewModel>();
        services.AddTransient<ProductDetailsViewModel>();
        services.AddTransient<OrderDetailsViewModel>();
    }

    private void ConfigureViews(IServiceCollection services)
    {
        // Rejestracja widoków
        services.AddSingleton<MainWindow>();
        services.AddTransient<ProductsWindow>();
          //  services.AddTransient<ProductDetailsPage>();
         //   services.AddTransient<OrderDetailsPage>();
    }
}

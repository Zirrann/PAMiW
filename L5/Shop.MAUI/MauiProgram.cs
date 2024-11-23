using L4.Services;
using Microsoft.Extensions.Logging;
using P12MAUI.Client.MessageBox;
using Shared.Models.Dto;
using Shop.MAUI.Services;
using Shop.MAUI.Services.ServicesDto;
using Shop.MAUI.ViewModels;
using Shop.MAUI.Views;

namespace Shop.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        ConfigureServices(builder.Services);

        return builder.Build();
    }


    private static void ConfigureServices(IServiceCollection services)
    {
        ConfigureAppServices(services);
        ConfigureViewModels(services);
        ConfigureViews(services);
    }

    private static void ConfigureAppServices(IServiceCollection services)
    {
        // Rejestracja serwisów
        services.AddSingleton<IConnectivity>(Connectivity.Current);
        services.AddSingleton<IGeolocation>(Geolocation.Default);
        services.AddSingleton<IMap>(Map.Default);

        services.AddSingleton<ICategoryServiceDto, CategoryServiceDto>();
        services.AddSingleton<IOrderProductServiceDto, OrderProductServiceDto>();
        services.AddSingleton<IOrderServiceDto, OrderServiceDto>();
        services.AddSingleton<IProductServiceDto, ProductServiceDto>();
        services.AddSingleton<IStockServiceDto, StockServiceDto>();

        services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();

        services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5104/")
        });
    }

    private static void ConfigureViewModels(IServiceCollection services)
    {
        // Rejestracja ViewModeli
        services.AddSingleton<OrderViewModel>();
        services.AddTransient<ProductsViewModel>();
        services.AddTransient<ProductDetailsViewModel>();
        services.AddTransient<OrderDetailsViewModel>();
    }

    private static void ConfigureViews(IServiceCollection services)
    {
        // Rejestracja widoków
        services.AddSingleton<MainPage>();
        services.AddTransient<ProductsPage>();
        services.AddTransient<ProductDetailsPage>();
        services.AddTransient<OrderDetailsPage>();
    }
}

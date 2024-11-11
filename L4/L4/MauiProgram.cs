using L4.Models;
using L4.Services;
using L4.Services.ProductService;
using L4.ViewModels;
using Microsoft.Extensions.Logging;
using P12MAUI.Client.MessageBox;

namespace L4
{
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
            services.AddSingleton<IConnectivity>(Connectivity.Current);
            services.AddSingleton<IGeolocation>(Geolocation.Default);
            services.AddSingleton<IMap>(Map.Default);

            services.AddSingleton<ProductService>();

            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();

        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProductDetailsViewModel>();

        }

        private static void ConfigureViews(IServiceCollection services)
        {
            services.AddSingleton<MainPage>();
            services.AddTransient<ProductDetailsView>();
        }

    }
}

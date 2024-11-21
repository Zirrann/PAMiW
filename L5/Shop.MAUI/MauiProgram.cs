using Microsoft.Extensions.Logging;

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
        ConfigureAppServices(builder.Services);

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

        services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7013/api/")
        });



    }

    private static void ConfigureViewModels(IServiceCollection services)
    {
        // services.AddSingleton<ProductsViewModel>();
        // services.AddSingleton<ProductDetailsViewModel>();

    }

    private static void ConfigureViews(IServiceCollection services)
    {
        services.AddSingleton<MainPage>();
    }
}

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shop.BLZR.Services.ServicesDto;
using Shop.BLZR.Services;
using SHOP.BLZR;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register your services here before Build
builder.Services.AddSingleton<ICategoryServiceDto, CategoryServiceDto>();
builder.Services.AddSingleton<IOrderProductServiceDto, OrderProductServiceDto>();
builder.Services.AddSingleton<IOrderServiceDto, OrderServiceDto>();
builder.Services.AddSingleton<IProductServiceDto, ProductServiceDto>();

// Configure HttpClient for API calls
builder.Services.AddSingleton(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5104/") // API base address
});



await builder.Build().RunAsync();
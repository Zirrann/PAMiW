using L4.Services;
using Shop.MAUI.Services;
using Shop.MAUI.Services.ServicesDto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Rejestracja HttpClient
builder.Services.AddSingleton(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5104/")
});

// Rejestracja serwisów
builder.Services.AddSingleton<ICategoryServiceDto, CategoryServiceDto>();
builder.Services.AddSingleton<IOrderProductServiceDto, OrderProductServiceDto>();
builder.Services.AddSingleton<IOrderServiceDto, OrderServiceDto>();
builder.Services.AddSingleton<IProductServiceDto, ProductServiceDto>();
builder.Services.AddSingleton<IStockServiceDto, StockServiceDto>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

app.Run();

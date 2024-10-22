var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(2); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "calculator",
    pattern: "Calculator/{action=Index}/{id?}",
    defaults: new { controller = "Calculator", action = "Index" });

app.MapControllerRoute(
    name: "quotes",
    pattern: "Quotes/{action=Index}/{id?}",
    defaults: new { controller = "Quotes" });

app.Run();

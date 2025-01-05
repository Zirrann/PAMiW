using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using TechTalk.SpecFlow;
using Shared.Models.Dto;
using NUnit.Framework;
using Shop.BLZR.Services.ServicesDto;

[Binding]
public class OrderManagementStepDefinitions
{
    private readonly TestDbContext _context;
    private readonly IProductServiceDto _productService;
    private readonly IOrderServiceDto _orderService;
    private readonly IOrderProductServiceDto _orderProductService;
    private int _orderId;
    private ProductDto _addedProduct;

    public OrderManagementStepDefinitions(TestDbContext context,
                                          IProductServiceDto productService,
                                          IOrderServiceDto orderService,
                                          IOrderProductServiceDto orderProductService)
    {
        _context = context;
        _productService = productService;
        _orderService = orderService;
        _orderProductService = orderProductService;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        // Wyczyœæ istniej¹ce dane i dodaj testowe
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.Products.Add(new ProductDto { Name = "Laptop", Price = 1000 });
        _context.Products.Add(new ProductDto { Name = "Phone", Price = 500 });
        await _context.SaveChangesAsync();
    }

    [Given(@"I am an authenticated user")]
    public void GivenIAmAnAuthenticatedUser()
    {
        _orderId = 1;
    }

    [When(@"I add ""([^""]*)"" to my order")]
    public async Task WhenIAddToMyOrder(string productName)
    {
        var products = await _productService.GetAllAsync();
        _addedProduct = products.Data.FirstOrDefault(p => p.Name == productName);

        if (_addedProduct == null)
        {
            throw new InvalidOperationException($"Produkt {productName} nie istnieje.");
        }

        var orderProduct = new OrderProductDto
        {
            ProductId = (int)_addedProduct.Id,
            OrderId = _orderId
        };

        var response = await _orderProductService.CreateAsync(orderProduct);
        response.Success.Should().BeTrue("Produkt powinien zostaæ dodany do zamówienia");
    }

    [Then(@"my order should contain ""([^""]*)""")]
    public async Task ThenMyOrderShouldContain(string productName)
    {
        var order = await _orderService.GetByIdAsync(_orderId);
        var productExists = order.Data.OrderProducts.Any(op => op.ProductId == _addedProduct.Id);
        productExists.Should().BeTrue($"Zamówienie powinno zawieraæ produkt {productName}");
    }
}

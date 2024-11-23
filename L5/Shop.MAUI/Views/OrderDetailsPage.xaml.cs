using Newtonsoft.Json;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.ViewModels;

namespace Shop.MAUI.Views;

public partial class OrderDetailsPage : ContentPage
{
    public List<OrderProductDto> OrderProducts { get; set; }

    public OrderDetailsPage(OrderDetailsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

}
using Shop.MAUI.ViewModels;

namespace Shop.MAUI.Views;

public partial class MainPage : ContentPage
{
    public MainPage(OrderViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

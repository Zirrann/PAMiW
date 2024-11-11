using L4.ViewModels;

namespace L4;

public partial class ProductDetailsView : ContentPage
{
    public ProductDetailsView(ProductDetailsViewModel productDetailsViewModel)
    {
        BindingContext = productDetailsViewModel;
        InitializeComponent();
    }
}
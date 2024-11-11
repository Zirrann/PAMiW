using L4.ViewModels;

namespace L4
{
    public partial class MainPage : ContentPage
    {

        public MainPage(ProductsViewModel productsViewModel)
        {
            BindingContext = productsViewModel;
            InitializeComponent();
        }

    }

}

using ShoppingListKI.ViewModels;

namespace ShoppingListKI.Views;

public partial class ShopListPage : ContentPage
{
    public ShopListPage()
    {
        InitializeComponent();
        BindingContext = App.Current!.Windows[0].Page.BindingContext;
    }
}

using System.Collections.ObjectModel;

namespace ShoppingListKI.Models;

public class Category
{
    public string Nazwa { get; set; }
    public ObservableCollection<Product> Produkty { get; set; }

    public Category(string nazwa)
    {
        Nazwa = nazwa;
        Produkty = new ObservableCollection<Product>();
    }

    public Category()
    {
        Nazwa = "";
        Produkty = new ObservableCollection<Product>();
    }
}

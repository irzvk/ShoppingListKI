using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingListKI.Models;

public class Product : INotifyPropertyChanged
{
    private int _ilosc;
    private bool _kupione;

    public string Nazwa { get; set; } = "";
    public string Sklep { get; set; } = "";
    public string Jednostka { get; set; } = "szt.";

    public int Ilosc
    {
        get => _ilosc;
        set { _ilosc = value; OnPropertyChanged(); }
    }

    public bool Kupione
    {
        get => _kupione;
        set { _kupione = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}

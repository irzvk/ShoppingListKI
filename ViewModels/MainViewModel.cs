using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ShoppingListKI.Models;
using ShoppingListKI.Services;

namespace ShoppingListKI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Category> Kategorie { get; } = new();

    public ObservableCollection<string> Sklepy { get; } = new()
    {
        "Wszystkie",
        "Biedronka",
        "Lidl",
        "Selgros"
    };

    public ObservableCollection<Product> ProduktyDoSklepu { get; } = new();

    private Category? _wybranaKategoria;
    private string _wybranySklepFiltr = "Wszystkie";

    private string _nazwaProduktu = "";
    private int _iloscProduktu = 1;
    private string _wybranySklepProduktu = "Biedronka";

    public Category? WybranaKategoria
    {
        get => _wybranaKategoria;
        set { _wybranaKategoria = value; OnPropertyChanged(); }
    }

    public string WybranySklepFiltr
    {
        get => _wybranySklepFiltr;
        set
        {
            _wybranySklepFiltr = value;
            OnPropertyChanged();
            OdswiezListe();
        }
    }

    public string NazwaProduktu
    {
        get => _nazwaProduktu;
        set { _nazwaProduktu = value; OnPropertyChanged(); }
    }

    public int IloscProduktu
    {
        get => _iloscProduktu;
        set { _iloscProduktu = value; OnPropertyChanged(); }
    }

    public string WybranySklepProduktu
    {
        get => _wybranySklepProduktu;
        set { _wybranySklepProduktu = value; OnPropertyChanged(); }
    }

    public ICommand DodajProduktCommand { get; }
    public ICommand ZwiekszIloscFormularzaCommand { get; }
    public ICommand ZmniejszIloscFormularzaCommand { get; }

    public ICommand ZwiekszIloscProduktuCommand { get; }
    public ICommand ZmniejszIloscProduktuCommand { get; }

    public ICommand UsunProduktCommand { get; }

    public MainViewModel()
    {
        DodajProduktCommand = new Command(DodajProdukt);

        ZwiekszIloscFormularzaCommand = new Command(() => IloscProduktu++);
        ZmniejszIloscFormularzaCommand = new Command(() =>
        {
            if (IloscProduktu > 1)
                IloscProduktu--;
        });

        ZwiekszIloscProduktuCommand = new Command<Product>(p =>
        {
            p.Ilosc++;
            ZapiszDoPliku();
        });

        ZmniejszIloscProduktuCommand = new Command<Product>(p =>
        {
            if (p.Ilosc > 1)
            {
                p.Ilosc--;
                ZapiszDoPliku();
            }
        });

        UsunProduktCommand = new Command<Product>(UsunProdukt);

        _ = WczytajZPlikuAsync();
    }

    private async Task WczytajZPlikuAsync()
    {
        var dane = await FileService.WczytajAsync();

        if (dane.Count == 0)
        {
            Kategorie.Add(new Category("Nabiał"));
            Kategorie.Add(new Category("Warzywa"));
            Kategorie.Add(new Category("Elektronika"));
        }
        else
        {
            foreach (var k in dane)
                Kategorie.Add(k);
        }

        WybranaKategoria = Kategorie.First();
        OdswiezListe();
    }

    private void DodajProdukt()
    {
        if (WybranaKategoria == null || string.IsNullOrWhiteSpace(NazwaProduktu))
            return;

        WybranaKategoria.Produkty.Add(new Product
        {
            Nazwa = NazwaProduktu,
            Ilosc = IloscProduktu,
            Sklep = WybranySklepProduktu
        });

        NazwaProduktu = "";
        IloscProduktu = 1;

        ZapiszDoPliku();
        OdswiezListe();
    }

    private void UsunProdukt(Product produkt)
    {
        foreach (var k in Kategorie)
        {
            if (k.Produkty.Remove(produkt))
                break;
        }

        ZapiszDoPliku();
        OdswiezListe();
    }

    private async void ZapiszDoPliku()
    {
        await FileService.ZapiszAsync(Kategorie);
    }

    private void OdswiezListe()
    {
        ProduktyDoSklepu.Clear();

        foreach (var k in Kategorie)
        {
            foreach (var p in k.Produkty)
            {
                if (p.Kupione)
                    continue;

                if (WybranySklepFiltr != "Wszystkie" &&
                    p.Sklep != WybranySklepFiltr)
                    continue;

                ProduktyDoSklepu.Add(p);
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}

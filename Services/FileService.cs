using System.Text.Json;
using ShoppingListKI.Models;

namespace ShoppingListKI.Services;

public static class FileService
{
    private static string SciezkaPliku =>
        Path.Combine(FileSystem.AppDataDirectory, "lista_zakupow.json");

    public static async Task ZapiszAsync(IEnumerable<Category> kategorie)
    {
        var json = JsonSerializer.Serialize(kategorie, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(SciezkaPliku, json);
    }

    public static async Task<List<Category>> WczytajAsync()
    {
        if (!File.Exists(SciezkaPliku))
            return new List<Category>();

        var json = await File.ReadAllTextAsync(SciezkaPliku);

        return JsonSerializer.Deserialize<List<Category>>(json)
               ?? new List<Category>();
    }
}

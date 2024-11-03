using ELibrary.Models;
using Newtonsoft.Json;

public class JsonFileHandler
{
    private readonly string _filePath;

    public JsonFileHandler(string filePath)
    {
        _filePath = filePath;
    }

    public List<Book> LoadBooks()
    {
        if (!File.Exists(_filePath))
            return new List<Book>();

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<Book>>(json);
    }

    public void SaveBooks(List<Book> books)
    {
        var json = JsonConvert.SerializeObject(books, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}

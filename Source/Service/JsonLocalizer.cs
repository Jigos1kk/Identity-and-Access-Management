using System;
using System.Globalization;
using System.Text.Json;

namespace Source.Service;

public interface IJsonLocalizer
{
    string? this[string key] { get; }
    string? this[string key, params object[] arguments] { get; }
    CultureInfo? CurrentCulture { get; set; }
}

public class JsonLocalizer : IJsonLocalizer
{
    private readonly IWebHostEnvironment _environment;
    private Dictionary<string, string>? _resources;
    private readonly string _resourcesPath;
    
    public CultureInfo? CurrentCulture { get; set; }

    public JsonLocalizer(IWebHostEnvironment environment)
    {
        _environment = environment;
        _resourcesPath = Path.Combine(_environment.ContentRootPath, "Resources");
        CurrentCulture = CultureInfo.CurrentCulture;
        LoadResources();
    }

    public string? this[string key] => GetString(key);

    public string? this[string key, params object[] arguments]
    {
        get
        {
            var format = GetString(key);
            return format != null ? string.Format(format, arguments) : null;
        }
    }

    private void LoadResources()
    {
        var culture = CurrentCulture?.Name ?? "en-EN";
        var fileName = $"Cultures.{culture}.json";
        var filePath = Path.Combine(_resourcesPath, fileName);

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _resources = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            _resources = new Dictionary<string, string>();
        }
    }

    private string? GetString(string key)
    {
        if (_resources != null && _resources.TryGetValue(key, out var value))
        {
            return value;
        }
        return null;
    }
}

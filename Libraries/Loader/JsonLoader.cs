using Libraries.StructureOfObjects;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Libraries.Loader;

public static class JsonLoader
{
    public static async Task UploadAsync(
        List<FanData>? fanCollection,
        string? pathJsonFile
    )
    {
        if (fanCollection == null)
        {
            throw new ArgumentNullException(nameof(fanCollection));
        }

        if (pathJsonFile == null)
        {
            throw new ArgumentNullException(nameof(pathJsonFile));
        }

        if (File.Exists(pathJsonFile))
        {
            Console.WriteLine(
                "Файл уже существует. Хотите перезаписать его? (Y/N)"
            );
            var response = Console.ReadLine()?.ToUpper();
            if (response != "Y")
            {
                return;
            }
        }

        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(
                UnicodeRanges.BasicLatin,
                UnicodeRanges.Cyrillic
            )
        };
        var json = JsonSerializer.Serialize(fanCollection, options);
        var file = File.CreateText(pathJsonFile);
        await file.WriteLineAsync(json);
        Console.WriteLine(json);
        file.Close();
    }

    public static List<T>? Download<T>(string pathJsonFile)
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(
                UnicodeRanges.BasicLatin,
                UnicodeRanges.Cyrillic
            )
        };

        List<T>? restoredFanData = null;
        if (File.Exists(pathJsonFile))
        {
            try
            {
                using var streamJson = File.OpenRead(pathJsonFile);
                {
                    restoredFanData = JsonSerializer.Deserialize<List<T>>(
                        streamJson,
                        options
                    );
                    //Console.WriteLine(restoredFanData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Файл *.json не найден");
        }

        return restoredFanData;
    }
}

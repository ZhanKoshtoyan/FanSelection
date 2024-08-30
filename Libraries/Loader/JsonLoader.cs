using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Libraries.Loader;

public static class JsonLoader
{
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

    public static async Task<List<T>?> DownloadAsync<T>(string pathJsonFile)
    {
        // Проверяем, существует ли файл
        if (!File.Exists(pathJsonFile))
        {
            Console.WriteLine("Файл не найден.");
            return null;
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

        try
        {
            var jsonContent = await File.ReadAllTextAsync(pathJsonFile);
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                {
                    Console.WriteLine("Корневой элемент не является массивом.");
                    return null;
                }
            }

            // Используем FileStream для асинхронного чтения файла
            await using var fileStream = new FileStream(
                pathJsonFile,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            // Десериализуем данные из файла в список объектов
            var data = await JsonSerializer.DeserializeAsync<List<T>?>(
                fileStream,
                options
            );

            return data;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Ошибка десериализации: {jsonEx.Message}");
            return null;
        }
        catch (InvalidCastException castEx)
        {
            Console.WriteLine($"Ошибка приведения типов: {castEx.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            return null;
        }
    }
}

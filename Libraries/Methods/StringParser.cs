using System.Globalization;

namespace Libraries.Methods;

/// <summary>
/// Интерфейс для парсинга строк в типизированные значения.
/// Поддерживает разные форматы и языки.
/// </summary>
public interface IStringParser
{
    /// <summary>
    /// Преобразует строку в double.
    /// </summary>
    /// <param name="value">Строковое значение для парсинга</param>
    /// <param name="parameterName">Имя параметра (для сообщений об ошибках)</param>
    /// <returns>Преобразованное число double</returns>
    /// <exception cref="FormatException">Если парсинг не удался</exception>
    double ParseDouble(string? value, string parameterName);

    /// <summary>
    /// Преобразует строку в int.
    /// </summary>
    /// <param name="value">Строковое значение для парсинга</param>
    /// <param name="parameterName">Имя параметра (для сообщений об ошибках)</param>
    /// <returns>Преобразованное целое число int</returns>
    /// <exception cref="FormatException">Если парсинг не удался</exception>
    int ParseInt(string? value, string parameterName);
}

/// <summary>
/// Реализация парсера с поддержкой локальных форматов.
/// Автоматически обрабатывает запятые как разделители дробной части.
/// </summary>
public class LocalizedStringParser : IStringParser
{
    private readonly CultureInfo _cultureInfo;

    /// <summary>
    /// Инициализирует парсер с указанной культурой.
    /// </summary>
    /// <param name="cultureInfo">Культура для парсинга. По умолчанию InvariantCulture.</param>
    public LocalizedStringParser(CultureInfo? cultureInfo = null)
    {
        _cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
    }

    /// <summary>
    /// Преобразует строку в double, обрабатывая запятые как точки.
    /// Например: "1,5" -> 1.5
    /// </summary>
    public double ParseDouble(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0.0;

        // Заменяем запятую на точку для корректного парсинга
        var normalizedValue = value.Replace(',', '.');

        if (double.TryParse(
            normalizedValue,
            NumberStyles.Any,
            _cultureInfo,
            out var result))
        {
            return result;
        }

        throw new FormatException(
            $"Параметр '{parameterName}' имеет значение '{value}', которое не может быть " +
            $"преобразовано в число. Используйте формат: '123.45' или '123,45'");
    }

    /// <summary>
    /// Преобразует строку в int.
    /// </summary>
    public int ParseInt(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        if (int.TryParse(value, NumberStyles.Any, _cultureInfo, out var result))
        {
            return result;
        }

        throw new FormatException(
            $"Параметр '{parameterName}' имеет значение '{value}', которое не может быть " +
            $"преобразовано в целое число.");
    }
}
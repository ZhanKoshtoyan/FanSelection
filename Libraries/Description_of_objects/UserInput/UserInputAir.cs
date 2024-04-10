namespace Libraries.Description_of_objects.UserInput;

public class UserInputAir
{
    /// <summary>
    ///     Минимальная температура эксплуатации, которое ввел пользователь; [°C]
    /// </summary>
    public required double FanOperatingMinTemperature { get; init; }

    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации (по умолчанию = 0), которое ввел пользователь; [%]
    /// </summary>
    public double? RelativeHumidity { get; init; } = 0;

    /// <summary>
    ///     Высота над уровнем моря (по умолчанию = 20), которое ввел пользователь; [м]
    /// </summary>
    public double? Altitude { get; init; } = 20;

    /// <summary>
    ///     Максимальная температура эксплуатации, которое ввел пользователь; [°C]
    /// </summary>
    public double? FanOperatingMaxTemperature { get; init; }
}

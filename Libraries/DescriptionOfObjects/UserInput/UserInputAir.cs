namespace Libraries.DescriptionOfObjects.UserInput;

public class UserInputAir
{
    /// <summary>
    /// Значение по умолчанию для температуры эксплуатации; [°C]
    /// </summary>
    public const double FanOperatingCurrentTemperatureByDefault = 20;

    /// <summary>
    ///     Температура эксплуатации, которую ввел пользователь; [°C]
    /// </summary>
    public double? FanOperatingCurrentTemperature { get; set; } =
        FanOperatingCurrentTemperatureByDefault;

    /// <summary>
    /// Значение по умолчанию для Относительная влажность температуры ежедневной эксплуатации; [%]
    /// </summary>
    public const double RelativeHumidityByDefault = 0;

    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации (по умолчанию = 0), которое ввел пользователь; [%]
    /// </summary>
    public double? RelativeHumidity { get; set; } = RelativeHumidityByDefault;

    /// <summary>
    ///     Высота над уровнем моря, которое ввел пользователь; [м]
    /// </summary>
    public double? Altitude { get; set; }

    /// <summary>
    ///     Максимальная температура эксплуатации, которое ввел пользователь, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    public double FanOperatingMaxTemperature { get; init; }
}

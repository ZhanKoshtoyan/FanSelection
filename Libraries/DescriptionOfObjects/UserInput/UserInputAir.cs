namespace Libraries.DescriptionOfObjects.UserInput;

public class UserInputAir
{
    /// <summary>
    ///     Температура эксплуатации, которую ввел пользователь; [°C]
    /// </summary>
    public double? FanOperatingCurrentTemperature { get; set; } = 20;

    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации (по умолчанию = 0), которое ввел пользователь; [%]
    /// </summary>
    public double? RelativeHumidity { get; set; } = 0;

    /// <summary>
    ///     Высота над уровнем моря (по умолчанию = 20), которое ввел пользователь; [м]
    /// </summary>
    public double? Altitude { get; set; } //= 20;

    /// <summary>
    ///     Максимальная температура эксплуатации, которое ввел пользователь, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    public double FanOperatingMaxTemperature { get; init; }
}

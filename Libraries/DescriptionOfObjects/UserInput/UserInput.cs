using Libraries.Methods;
using Libraries.StructureOfObjects;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.DescriptionOfObjects.UserInput;

/// <summary>
///     Введенные пользователем данные
/// </summary>
public record UserInput
{
    /// <summary>
    /// Путь к файлу исходных данных [.json]
    /// </summary>
    public static readonly string PathJsonFileFanData = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Fans.json"
    );

    /// <summary>
    /// Характеристики рабочей точки вентилятора
    /// </summary>
    public required UserInputWorkPoint UserInputWorkPoint { get; init; }

    /// <summary>
    /// Характеристики воздуха рабочей точки вентилятора
    /// </summary>
    public required UserInputAir UserInputAir { get; init; }

    /// <summary>
    /// Характеристики вентилятора
    /// </summary>
    public required UserInputFan UserInputFan { get; init; }

    /// <summary>
    ///     Расчетная плотность воздуха, температура которого введена пользователем, [кг/м3]
    /// </summary>
    public IHumidAir DataAir =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(
                UserInputAir.Altitude.GetValueOrDefault().Meters()
            ),
            InputHumidAir.Temperature(
                UserInputAir.FanOperatingMinTemperature
                    .GetValueOrDefault()
                    .DegreesCelsius()
            ),
            InputHumidAir.RelativeHumidity(
                UserInputAir.RelativeHumidity
                    .GetValueOrDefault()
                    .Percent()
            )
        );
}

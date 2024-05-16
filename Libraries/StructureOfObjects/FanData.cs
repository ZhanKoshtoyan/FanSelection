using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.StructureOfObjects;

/// <summary>
///     Информация о вентиляторе
/// </summary>
public record FanData
{
    /// <summary>
    ///     Исполнение вентилятора
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    ///     Типоразмер
    /// </summary>
    public required string Size { get; init; }

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря, [кг/м3]
    /// </summary>
    public static readonly IHumidAir AirInTests = new HumidAir().WithState(
        InputHumidAir.Altitude(20.Meters()),
        InputHumidAir.Temperature(20.DegreesCelsius()),
        InputHumidAir.RelativeHumidity(50.Percent())
    );

    /*/// <summary>
    /// Длина корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    public string? FanBodyLength { get; init; }*/

    /*/// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    public string? FanOperatingMaxTemperature { get; init; }*/

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    public required string ImpellerRotationDirection { get; init; }

    /// <summary>
    ///     Номинальная мощность двигателя, [кВт]
    /// </summary>
    public required double NominalPower { get; init; }

    /// <summary>
    ///     Номинальная скорость вращения крыльчатки без учета скольжения двигателя, [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    public required double NominalImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public required double ImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Максимальная скорость вращения крыльчатки с учетом загрузки двигателя, [об/мин]
    /// </summary>
    public required double MaxImpellerRotationSpeed { get; init; }

    /*/// <summary>
    /// Материал корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    public string? CaseExecutionMaterial { get; init; }*/

    /// <summary>
    ///     Минимальный объем воздуха, [м3/ч]
    /// </summary>
    public required double MinVolumeFlow { get; init; }

    /// <summary>
    ///     Максимальный объем воздуха, [м3/ч]
    /// </summary>
    public required double MaxVolumeFlow { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Pv(Q) - полного давления от объемного воздуха
    /// </summary>

    public required PolynomialType TotalPressureQvCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени N(Q) - мощности вентилятора в рабочей точке от объемного воздуха
    /// </summary>
    public required PolynomialType PowerQvCoefficients { get; init; }

    /// <summary>
    ///     Площадь сечения на выходе, [м2]
    /// </summary>
    public required double InletCrossSection { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseQvCoefficients8000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Efficiency(Phi) - полного КПД от коэффициента производительности
    /// </summary>
    public required PolynomialType EfficiencyPhiCoefficients { get; init; }

    /// <summary>
    /// Максимальное значение КПД аэродинамической схемы вентилятора, [%]
    /// </summary>
    public required double EfficiencyMax { get; init; }

    /// <summary>
    /// Минимальное левое значение КПД аэродинамической схемы вентилятора, [%]
    /// </summary>
    public required double EfficiencyMinLeft { get; init; }

    /// <summary>
    /// Максимальное левое значение КПД аэродинамической схемы вентилятора, [%]
    /// </summary>
    public required double EfficiencyMinRight { get; init; }

    /// <summary>
    /// Значение коэффициента производительности при максимальном значении КПД аэродинамической схемы вентилятора, [б/р]
    /// </summary>
    public required double PhiEfficiencyMax { get; init; }

    /// <summary>
    /// Значение коэффициента производительности при минимальном левом значении КПД аэродинамической схемы вентилятора, [б/р]
    /// </summary>
    public required double PhiMin { get; init; }

    /// <summary>
    /// Значение коэффициента производительности при минимальном правом значении КПД аэродинамической схемы вентилятора, [б/р]
    /// </summary>
    public required double PhiMax { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Psi(Phi) - коэффициента полного давления от коэффициента производительности
    /// </summary>
    public required PolynomialType PsiPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lambda(Phi) - коэффициента потребляемой мощности от коэффициента производительности
    /// </summary>
    public required PolynomialType LambdaPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени SpecificSpeed(Phi) - коэффициента быстроходности от коэффициента производительности
    /// </summary>
    public required PolynomialType SpecificSpeedPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени SpecificSize(Phi) - коэффициента габаритности от коэффициента производительности
    /// </summary>
    public required PolynomialType SpecificSizePhiCoefficients { get; init; }
}

using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.StructureOfObjects;

/// <summary>
///     Информация о вентиляторе
/// </summary>
public abstract record FanData
{

    public required string ExcelWorkSheetName { get; init; }
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
    public IHumidAir AirInTests =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(Altitude.Meters()),
            InputHumidAir.Temperature(Temperature.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(RelativeHumidity.Percent())
        );

    public required double AirDensity { get; set; }

    public required double Altitude { get; init; }

    public required double Temperature { get; init; }

    public required double RelativeHumidity { get; init; }

    public required double Weight { get; init; }

    /// <summary>
    /// Длина корпуса. Допустимые значения: ("1" - полногабаритный; "2" - короткий)
    /// </summary>
    public required List<double>? FanBodyLength { get; init; }

    /// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения: 300 или 400°C.
    /// </summary>
    public required List<double>? FanOperatingMaxTemperature { get; init; }

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения: "RRO" - поток на мотор, "LRO" - поток на колесо или
    ///     "REV" - реверс.
    /// </summary>
    public required List<string>? ImpellerRotationDirection { get; init; }

    /// <summary>
    ///     Номинальная мощность, [кВт]
    /// </summary>
    public required double NominalPower { get; init; }

    /// <summary>
    ///     Номинальная скорость вращения крыльчатки, [об/мин]
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

    /// <summary>
    /// Материал корпуса. Допустимые значения: "ZN" - оцинкованная сталь, "NR" - нержавеющая сталь или "KR" - кислотостойкая нержавеющая сталь.
    /// </summary>
    public List<string>? CaseExecutionMaterial { get; init; }

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
    public required PolynomialType OctaveNoiseLw5QvCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw5QvCoefficients8000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseLw6QvCoefficients8000 { get; init; }

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

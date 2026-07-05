using Libraries.Methods;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.StructureOfObjects;

/// <summary>
///     Информация о вентиляторе
/// </summary>
public record FanData : ShortDescriptionOfTheFanData
{
    public string? ExcelWorkSheetName { get; init; }

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря, [кг/м3]
    /// </summary>
    public IHumidAir AirInTests =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(Altitude.Meters()),
            InputHumidAir.Temperature(
                FanOperatingCurrentTemperature.DegreesCelsius()
            ),
            InputHumidAir.RelativeHumidity(RelativeHumidity.Percent())
        );

    /// <summary>
    /// Плотность воздуха, [кг/м3]
    /// </summary>
    public required double AirDensity { get; init; }

    /// <summary>
    /// Высота над уровнем моря, [м]
    /// </summary>
    public required double Altitude { get; init; }

    /// <summary>
    /// Температура эксплуатации, [°C]
    /// </summary>
    public required double FanOperatingCurrentTemperature { get; init; }

    /// <summary>
    /// Относительная влажность температуры ежедневной эксплуатации, [%]
    /// </summary>
    public required double RelativeHumidity { get; init; }

    /// <summary>
    /// Минимальная температура перемещаемой среды, [°C].
    /// </summary>
    public required List<double>? FanOperatingMinTemperature { get; init; }

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

    public required PolynomialType? TotalPressureQvCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени N(Q) - мощности вентилятора в рабочей точке от объемного воздуха
    /// </summary>
    public required PolynomialType? PowerQvCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw5(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw5QvCoefficients8000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lw6(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType? OctaveNoiseLw6QvCoefficients8000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Efficiency(Phi) - полного КПД от коэффициента производительности
    /// </summary>
    public required PolynomialType? EfficiencyPhiCoefficients { get; init; }

    /// <summary>
    /// Максимальное значение КПД текущего вентилятора, [%]
    /// </summary>
    public double EfficiencyMax { get; set; }

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
    public required PolynomialType? PsiPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени Lambda(Phi) - коэффициента потребляемой мощности от коэффициента производительности
    /// </summary>
    public required PolynomialType? LambdaPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени SpecificSpeed(Phi) - коэффициента быстроходности от коэффициента производительности
    /// </summary>
    public required PolynomialType? SpecificSpeedPhiCoefficients { get; init; }

    /// <summary>
    ///     Коэффициенты полинома n-й степени SpecificSize(Phi) - коэффициента габаритности от коэффициента производительности
    /// </summary>
    public required PolynomialType? SpecificSizePhiCoefficients { get; init; }

    /// <summary>
    /// Максимальное значение КПД базового вентилятора, [%]
    /// </summary>
    public required double OriginalFanDataEfficiencyMax { get; init; }

    /// <summary>
    /// Электрическая мощность при максимальном КПД базового вентилятора, [кВт]
    /// </summary>
    public required double OriginalFanDataPowerByEfficiencyMax { get; init; }

    /// <summary>
    /// Условный типоразмер
    /// </summary>
    public required double OriginalFanDataConditionalStandardSize { get; init; }

    /// <summary>
    /// ImpellerRotationSpeedWithSlidingEngineForWorkPoint для OriginalFanData
    /// </summary>
    public double OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint { get; init; } =
        1;

    /// <summary>
    /// ConditionalStandardSize для OriginalFanData
    /// </summary>
    public double OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades { get; init; } =
        1;

    /// <summary>
    /// AirDensity для OriginalFanData
    /// </summary>
    public double OriginalFanDataAirDensity { get; init; } = 1;

    /// <summary>
    /// Коэффициент отображающий эффект масштабности согласно коэффициенту эффективности вентиляторов FEG (ГОСТ 31961-2012, ГОСТ 33660-2015)
    /// </summary>
    //public double FanEfficiencyGradeCoefficient { get; set; } = 1;

    /// <summary>
    /// Коэффициент отображающий эффект масштабности согласно коэффициенту эффективности вентиляторов FMEG (ГОСТ 33660-2015)
    /// </summary>
    public double FanMotorEfficiencyGradeCoefficient { get; set; } = 1;

    public FanEfficiencyGradeCollection FanEfficiencyGradeList { get; set; } = new();

    /// <summary>
    /// Площадь диска колеса по концам лопаток [м2]
    /// </summary>
    public double SquareOfWheelDisc =>
        DimensionlessData.SquareOfWheelDisc(
            DiameterOfTheImpellerAtTheEndsOfTheBlades / 1000
        );

    /// <summary>
    /// Окружная скорость по концам лопаток [м/с]
    /// </summary>
    public double CircumferentialSpeed =>
        DimensionlessData.CircumferentialSpeed(
            DiameterOfTheImpellerAtTheEndsOfTheBlades / 1000,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint
        );
}
namespace Libraries.Description_of_objects;

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

    /*/// <summary>
    /// Длина корпуса. Допустимые значения: ("1" - полногабаритный; "2" - короткий)
    /// </summary>
    public string? FanBodyLength { get; init; }*/

    /*/// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения: 300 или 400°C.
    /// </summary>
    public string? FanOperatingMaxTemperature { get; init; }*/

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения: "RRO" - поток на мотор, "LRO" - поток на колесо или
    ///     "REV" - реверс.
    /// </summary>
    public required string ImpellerRotationDirection { get; init; }

    /// <summary>
    ///     Номинальная мощность, [кВт]
    /// </summary>
    public required double NominalPower { get; init; }

    /// <summary>
    ///     Номинальная скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public required int NominalImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public required int ImpellerRotationSpeed { get; init; }

    /*/// <summary>
    /// Материал корпуса. Допустимые значения: "ZN" - оцинкованная сталь, "NR" - нержавеющая сталь или "KR" - кислотостойкая нержавеющая сталь.
    /// </summary>
    public string? CaseExecutionMaterial { get; init; }*/

    /// <summary>
    ///     Минимальный объем воздуха, [м3/ч]
    /// </summary>
    public required int MinVolumeFlow { get; init; }

    /// <summary>
    ///     Максимальный объем воздуха, [м3/ч]
    /// </summary>
    public required int MaxVolumeFlow { get; init; }

    /// <summary>
    ///     Коэффиуиенты полинома 6-й степени Pv(Q) - полного давления от объемного воздуха
    /// </summary>

    public required PolynomialType TotalPressureCoefficients { get; init; }

    /// <summary>
    ///     Коэффиуиенты полинома 6-й степени Pv(N) - мощности вентилятора в рабочей точке от объемного воздуха
    /// </summary>
    public required PolynomialType PowerCoefficients { get; init; }

    /// <summary>
    ///     Площадь сечения на входе, [м2]
    /// </summary>
    public required double InletCrossSection { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required PolynomialType OctaveNoiseCoefficients8000 { get; init; }
}

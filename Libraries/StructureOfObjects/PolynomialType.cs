namespace Libraries.StructureOfObjects;

public record PolynomialType
{
    /// <summary>
    ///     Шестой коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double SixthCoefficient { get; init; }

    /// <summary>
    ///     Пятый коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double FifthCoefficient { get; init; }

    /// <summary>
    ///     Четвертый коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double FourthCoefficient { get; init; }

    /// <summary>
    ///     Третий коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double ThirdCoefficient { get; init; }

    /// <summary>
    ///     Второй коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double SecondCoefficient { get; init; }

    /// <summary>
    ///     Первый коэффициент уравнения полинома 6-й степени
    /// </summary>
    public required double FirstCoefficient { get; init; }

    /// <summary>
    ///     Коэффициент "С" уравнения полинома 6-й степени
    /// </summary>
    public required double ZeroCoefficient { get; init; }
}

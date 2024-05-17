namespace Libraries.StructureOfObjects;

public record PolynomialType
{
    /// <summary>
    ///     Коэффициенты уравнения полинома от 6-го до 0-го.
    /// </summary>
    public required List<double> Coefficients { get; init; }
}

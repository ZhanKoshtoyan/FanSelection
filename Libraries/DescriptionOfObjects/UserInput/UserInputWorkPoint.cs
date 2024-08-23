namespace Libraries.DescriptionOfObjects.UserInput;

public record UserInputWorkPoint
{
    /// <summary>
    ///     Объемный расход воздуха, который ввел пользователь;  [м3/ч]
    /// </summary>
    public required double VolumeFlow { get; init; }

    /// <summary>
    ///     Полное давление воздуха, которое ввел пользователь; [Па]
    /// </summary>
    public required double TotalPressure { get; init; }

    /// <summary>
    /// Значение по умолчанию для погрешности подбора по полному давлению воздуха; [%]
    /// </summary>
    public const double TotalPressureDeviationByDefault = 30;

    /// <summary>
    ///     Допустимая погрешность подбора, которое ввел пользователь; [%]
    /// </summary>
    public double? VolumeFlowAndTotalPressureDeviation { get; set; } =
        TotalPressureDeviationByDefault;

    /// <summary>
    /// Значение по умолчанию для погрешности быстроходности/ габаритности слева; [%]
    /// </summary>
    public const double SpecificDeviationLeftByDefault = 20;

    /// <summary>
    ///     Погрешность быстроходности/ габаритности слева, которое ввел пользователь; [%]
    /// </summary>
    public double? SpecificDeviationLeft { get; set; } =
        SpecificDeviationLeftByDefault;

    /// <summary>
    /// Значение по умолчанию для погрешности быстроходности/ габаритности справа; [%]
    /// </summary>
    public const double SpecificDeviationRightByDefault = 25;

    /// <summary>
    ///     Погрешность быстроходности/ габаритности справа, которое ввел пользователь; [%]
    /// </summary>
    public double? SpecificDeviationRight { get; set; } =
        SpecificDeviationRightByDefault;
}

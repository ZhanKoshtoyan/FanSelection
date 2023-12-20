namespace Libraries.Description_of_objects.UserInput;

public record UserInputFan
{
    /// <summary>
    ///     Исполнение вентилятора
    /// </summary>
    public required int? FanVersion { get; init; }

    /// <summary>
    ///     Типоразмер вентилятора, которое ввел пользователь
    /// </summary>
    public int? Size { get; init; }

    /// <summary>
    ///     Длина корпуса, которое ввел пользователь
    /// </summary>
    public int? FanBodyLength { get; init; }

    /// <summary>
    ///     Направление движения крыльчатки, которое ввел пользователь
    /// </summary>
    public string? ImpellerRotationDirection { get; init; } = "RRO";

    /// <summary>
    ///     Номинальная мощность, которое ввел пользователь; [кВт]
    /// </summary>
    public double? NominalPower { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, которое ввел пользователь; [об/мин]
    /// </summary>
    public int? ImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Материал исполнения корпуса, которое ввел пользователь
    /// </summary>
    public string? CaseExecutionMaterial { get; init; }
}

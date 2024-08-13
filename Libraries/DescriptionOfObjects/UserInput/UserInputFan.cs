using Libraries.DescriptionOfObjects.Parameters;
using System.Globalization;

namespace Libraries.DescriptionOfObjects.UserInput;

public record UserInputFan
{
    /// <summary>
    /// Исполнение вентилятора
    /// </summary>
    public required int FanVersion { get; init; }

    /// <summary>
    /// Логика подбора вентилятора
    /// </summary>
    public required int FanLogic { get; init; } = 1;

    /// <summary>
    /// Количество вентиляторов, обеспечивающих рабочую точку
    /// </summary>
    public int NumberOfFans { get; set; }

    /// <summary>
    ///     Типоразмер вентилятора, которое ввел пользователь
    /// </summary>
    public double ConditionalStandardSize { get; init; }

    /// <summary>
    ///     Длина корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    public int FanBodyLength { get; init; }

    /// <summary>
    ///     Направление движения крыльчатки, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    public string? ImpellerRotationDirection { get; set; } =
        NominalImpellerRotationSpeeds.Values
            .First()
            .ToString(CultureInfo.InvariantCulture);

    /// <summary>
    ///     Номинальная мощность двигателя, которое ввел пользователь; [кВт]
    /// </summary>
    public double NominalPower { get; init; }

    /// <summary>
    ///     Номинальная скорость вращения крыльчатки без учета скольжения двигателя,, которое ввел пользователь; [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    public double NominalImpellerRotationSpeedWithoutSlidingEngine { get; init; }

    /// <summary>
    ///     Материал исполнения корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    public string? FanBodyExecutionMaterial { get; init; }
}

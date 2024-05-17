using Libraries.DescriptionOfObjects.Parameters;
using System.Globalization;

namespace Libraries.DescriptionOfObjects.UserInput;

public record UserInputFan
{
    private readonly int _temporaryFanVersion;
    /// <summary>
    ///     Исполнение вентилятора
    /// </summary>
    public required int FanVersion { get => _temporaryFanVersion; init => _temporaryFanVersion = value - 1; }

    private readonly int _temporaryFanLogic;
    /// <summary>
    ///     Логика подбора вентилятора
    /// </summary>
    public required int FanLogic { get => _temporaryFanLogic; init => _temporaryFanLogic = value - 1; }

    /// <summary>
    /// Количество вентиляторов, обеспечивающих рабочую точку
    /// </summary>
    public double NumberOfFans { get; set; } = 1;

    /// <summary>
    ///     Типоразмер вентилятора, которое ввел пользователь
    /// </summary>
    public double Size { get; init; }

    /// <summary>
    ///     Длина корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    public int FanBodyLength { get; init; }

    /// <summary>
    ///     Направление движения крыльчатки, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    public string? ImpellerRotationDirection { get; set; } = NominalImpellerRotationSpeeds.Values.First().ToString(CultureInfo.InvariantCulture);

    /// <summary>
    ///     Номинальная мощность двигателя, которое ввел пользователь; [кВт]
    /// </summary>
    public double NominalPower { get; init; }

    /// <summary>
    ///     Номинальная скорость вращения крыльчатки без учета скольжения двигателя,, которое ввел пользователь; [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    public double NominalImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Материал исполнения корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    public string? CaseExecutionMaterial { get; init; }

    /// <summary>
    /// Размер крыльчатки, которое требуется подобрать, [мм]
    /// </summary>
    public double RequiredSize { get; set; }
}

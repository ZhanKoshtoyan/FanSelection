namespace Libraries.StructureOfObjects;

public record ShortDescriptionOfTheFanData
{
    /// <summary>
    ///     Исполнение вентилятора
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// Аэродинамическая схема вентилятора. Нумерация в пределах одной Version
    /// </summary>
    public required int AerodynamicDesign { get; init; }

    /// <summary>
    ///     Типоразмер
    /// </summary>
    public required double ConditionalStandardSize { get; init; }

    /// <summary>
    ///     Площадь отверстия выпускной трубы, [м2]
    /// </summary>
    public required double SquareOfOutletPipeOpening { get; init; }

    /// <summary>
    /// Длина корпуса. Допустимые значения: ("1" - полногабаритный; "2" - короткий)
    /// </summary>
    public required List<double>? FanBodyLength { get; init; }

    /// <summary>
    /// Максимальная температура перемещаемой среды, [°C]. Допустимые значения: 300 или 400°C.
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
    /// Номинальная скорость вращения крыльчатки без учета скольжения двигателя, [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    public required double NominalImpellerRotationSpeedWithoutSlidingEngine { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки со скольжением двигателя, [об/мин]
    /// </summary>
    public required double ImpellerRotationSpeedWithSlidingEngineForWorkPoint { get; set; }

    /// <summary>
    ///     Максимальная скорость вращения крыльчатки с учетом загрузки двигателя, [об/мин]
    /// </summary>
    public required double MaxImpellerRotationSpeedWithSlidingEngine { get; init; }

    /// <summary>
    /// Материал корпуса. Допустимые значения: "ZN" - оцинкованная сталь, "NR" - нержавеющая сталь или "KR" - кислотостойкая нержавеющая сталь.
    /// </summary>
    public required List<string>? FanBodyExecutionMaterial { get; init; }

    /// <summary>
    /// Вес вентилятора, [кг]
    /// </summary>
    public required double Weight { get; init; }
}

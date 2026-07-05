namespace Libraries.DescriptionOfObjects.UserInput;

/// <summary>
/// Data Transfer Object для данных с UI формы.
/// Содержит строковые значения как они приходят из пользовательского интерфейса.
/// </summary>
public class UserInputFormData
{
    // ===================== ОБЯЗАТЕЛЬНЫЕ ПАРАМЕТРЫ =====================
    
    /// <summary>
    /// Объемный расход воздуха, [м3/ч]
    /// </summary>
    public string? VolumeFlow { get; set; }

    /// <summary>
    /// Полное давление воздуха, [Па]
    /// </summary>
    public string? TotalPressure { get; set; }

    /// <summary>
    /// Номер исполнения вентилятора (0, 1, 2, ...)
    /// </summary>
    public string? FanVersion { get; set; }

    /// <summary>
    /// Номер логики подбора вентилятора
    /// </summary>
    public string? FanLogic { get; set; }

    /// <summary>
    /// Максимальная температура перемещаемой среды
    /// </summary>
    public string? FanOperatingMaxTemperature { get; set; }

    // ===================== ОПЦИОНАЛЬНЫЕ ПАРАМЕТРЫ =====================

    /// <summary>
    /// Условный типоразмер крыльчатки
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Длина корпуса функциональной сборки
    /// </summary>
    public string? FanBodyLength { get; set; }

    /// <summary>
    /// Направление вращения крыльчатки
    /// </summary>
    public string? ImpellerRotationDirection { get; set; }

    /// <summary>
    /// Номинальная мощность двигателя, [кВт]
    /// </summary>
    public string? NominalPower { get; set; }

    /// <summary>
    /// Условное число оборотов двигателя, [об/мин]
    /// </summary>
    public string? NominalImpellerRotationSpeed { get; set; }

    /// <summary>
    /// Материал корпуса функциональной сборки
    /// </summary>
    public string? FanBodyExecutionMaterial { get; set; }

    // ===================== ОТКЛОНЕНИЯ И УСЛОВИЯ =====================

    /// <summary>
    /// Допустимая погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public string? TotalPressureDeviation { get; set; }

    /// <summary>
    /// Погрешность быстроходности/габаритности слева, [%]
    /// </summary>
    public string? SpecificDeviationLeft { get; set; }

    /// <summary>
    /// Погрешность быстроходности/габаритности справа, [%]
    /// </summary>
    public string? SpecificDeviationRight { get; set; }

    /// <summary>
    /// Относительная влажность, [%]
    /// </summary>
    public string? RelativeHumidity { get; set; }

    /// <summary>
    /// Высота над уровнем моря, [м]
    /// </summary>
    public string? Altitude { get; set; }

    /// <summary>
    /// Температура ежедневной эксплуатации, [°C]
    /// </summary>
    public string? FanOperatingCurrentTemperature { get; set; }

    /// <summary>
    /// Количество вентиляторов
    /// </summary>
    public string? NumberOfFans { get; set; }
}
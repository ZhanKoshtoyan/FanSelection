using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public interface IFan : IFanNoise, IFanCurves, IFanDimensionlessData
{
    /// <summary>
    /// Данные экземпляра вентилятора из исходных данных. К ним происходит обращение через UserInput.PathJsonFileFanData
    /// </summary>
    public FanData Data { get; }

    /// <summary>
    /// Данные, которые введ пользователь
    /// </summary>
    public UserInput UserInput { get; }

    /// <summary>
    ///     Проектное наименование вентилятора
    /// </summary>
    public string? ProjectId { get; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public double ImpellerRotationSpeed { get; }

    /// <summary>
    /// Минимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MinImpellerRotationFrequency { get; }

    /// <summary>
    /// Максимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MaxImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeed,
            NominalImpellerRotationSpeed
        );

    /// <summary>
    /// Типоразмер, [мм]
    /// </summary>
    public double Size =>
        Convert.ToDouble(
            UserInput.UserInputFan.Size == 0
                ? Data.Size
                : UserInput.UserInputFan.Size
        ) / 1000;

    /// <summary>
    /// Номинальная скорость вращения крыльчатки без учета скольжения двигателя, [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    public double NominalImpellerRotationSpeed =>
        UserInput.UserInputFan.NominalImpellerRotationSpeed == 0
            ? Data.NominalImpellerRotationSpeed
            : Math.Round(
                UserInput.UserInputFan.NominalImpellerRotationSpeed,
                0
            );

    /// <summary>
    /// Номинальная мощность двигателя, [кВт]
    /// </summary>
    public double NominalPower =>
        UserInput.UserInputFan.NominalPower == 0
            ? Data.NominalPower
            : UserInput.UserInputFan.NominalPower;

    /// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    public double FanOperatingMaxTemperature =>
        UserInput.UserInputAir.FanOperatingMaxTemperature == 0 && Data.FanOperatingMaxTemperature != null
            ? Data.FanOperatingMaxTemperature.First()
            : UserInput.UserInputAir.FanOperatingMaxTemperature;

    /// <summary>
    /// Длина корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    public double FanBodyLength =>
        UserInput.UserInputFan.FanBodyLength == 0 && Data.FanBodyLength != null
    ? Data.FanBodyLength.First()
    : UserInput.UserInputFan.FanBodyLength;

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    public string ImpellerRotationDirection =>
        string.IsNullOrEmpty(UserInput.UserInputFan.ImpellerRotationDirection) && Data.ImpellerRotationDirection != null
         ? Data.ImpellerRotationDirection.First()
         : UserInput.UserInputFan.ImpellerRotationDirection!;

    /// <summary>
    /// Материал корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    public string CaseExecutionMaterial =>
        string.IsNullOrEmpty(UserInput.UserInputFan.CaseExecutionMaterial) && Data.CaseExecutionMaterial != null
            ? Data.CaseExecutionMaterial.First()
            : UserInput.UserInputFan.CaseExecutionMaterial!;

    /// <summary>
    /// Частота вращения крыльчатки, [Гц]
    /// </summary>
    public double ImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            ImpellerRotationSpeed,
            NominalImpellerRotationSpeed
        );

    /// <summary>
    /// Полное давление воздуха, которое ввел пользователь, приведенное к нормальной плотности воздуха
    /// </summary>
    public double InputTotalNormalPressure =>
        SimilarityCalculator.SimilarPressure(
            UserInput.UserInputWorkPoint.TotalPressure,
            1,
            1,
            UserInput.DataAir.Density.KilogramsPerCubicMeter,
            1,
            1,
            Data.AirDensity
        );

    /// <summary>
    /// Расход объемного воздуха на исходной кривой вентилятора, [м3/ч]
    /// </summary>
    public double VolumeFlowOnPolynomial => Calculate.MethodOfHalfDivisionVolumeFlow(
        Data.MinVolumeFlow,
        Data.MaxVolumeFlow,
        Data.TotalPressureQvCoefficients,
        UserInput.UserInputWorkPoint.VolumeFlow / UserInput.UserInputFan.NumberOfFans,
        InputTotalNormalPressure
    );

    /// <summary>
    /// Полное давление воздуха на исходной кривой вентилятора, [Па]
    /// </summary>
    public double TotalPressureOnPolynomial =>
        Calculate.Polynomial(
            Data.TotalPressureQvCoefficients,
            VolumeFlowOnPolynomial
        );


    /// <summary>
    ///     Расход объемного воздуха на кривой вентилятора, эквивалентный зависимости Pv=Q^2 - характеристика сети воздуховода, [м3/ч]
    /// </summary>
    public double VolumeFlow =>
        SimilarityCalculator.SimilarVolumeFlow(
            VolumeFlowOnPolynomial,
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        SimilarityCalculator.SimilarPressure(
            TotalPressureOnPolynomial,
            Data.ImpellerRotationSpeed,
            Size,
            Data.AirDensity,
            ImpellerRotationSpeed,
            Size,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Calculate.Deviation(
            UserInput.UserInputWorkPoint.VolumeFlow / UserInput.UserInputFan.NumberOfFans,
            VolumeFlow
        );

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Calculate.Deviation(
            UserInput.UserInputWorkPoint.TotalPressure,
            TotalPressure
        );

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        SimilarityCalculator.SimilarPower(
            Calculate.Polynomial(
                Data.PowerQvCoefficients,
                VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            Size,
            Data.AirDensity,
            ImpellerRotationSpeed,
            Size,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Calculate.StaticPressure(TotalPressure, DynamicPressure);

    /// <summary>
    ///     Расчетное динамическое давление воздуха, [Па]
    /// </summary>
    public double DynamicPressure =>
        Calculate.DynamicPressure(UserInput.DataAir, AirVelocity);

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double TotalEfficiency =>
        Calculate.Efficiency(VolumeFlow, TotalPressure, Power);

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Calculate.AirVelocity(VolumeFlow, Data.InletCrossSection);
}

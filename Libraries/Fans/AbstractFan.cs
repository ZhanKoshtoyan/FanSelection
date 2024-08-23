using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Libraries.Fans;

public abstract class AbstractFan : INotifyPropertyChanged
{
    // Конструктор для инициализации свойств
    protected AbstractFan(FanData data, UserInput userInput, int numberOfFans)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data)); // Проверка на null
        UserInput =
            userInput ?? throw new ArgumentNullException(nameof(userInput)); // Проверка на null
        NumberOfFans = numberOfFans;
    }

    /// <summary>
    /// Данные экземпляра вентилятора из исходных данных. К ним происходит обращение через UserInput.PathDataOfFansJsonFile
    /// </summary>
    public FanData Data { get; }

    /// <summary>
    /// Данные, которые ввел пользователь
    /// </summary>
    private UserInput UserInput { get; }

    /// <summary>
    ///     Проектное наименование вентилятора
    /// </summary>
    public string? ProjectId { get; protected init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public double ImpellerRotationSpeedWithSlidingEngineForWorkPoint
    {
        get;
        protected init;
    }

    /// <summary>
    /// Минимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MinImpellerRotationFrequency { get; protected init; }

    private double? _maxImpellerRotationFrequency;

    /// <summary>
    /// Максимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MaxImpellerRotationFrequency =>
        _maxImpellerRotationFrequency ??= Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            NominalImpellerRotationSpeedWithoutSlidingEngine
        );

    private double? _similarVolumeFlowCoefficient;

    /// <summary>
    /// Коэффициент подобия VolumeFlow с оригинальной FanData
    /// </summary>
    private double SimilarVolumeFlowCoefficient =>
        _similarVolumeFlowCoefficient ??= Similarity.SimilarVolumeFlow(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataConditionalStandardSize,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize
        );

    private double? _similarTotalPressureCoefficient;

    /// <summary>
    /// Коэффициент подобия TotalPressure с оригинальной FanData
    /// </summary>
    private double SimilarTotalPressureCoefficient =>
        _similarTotalPressureCoefficient ??= Similarity.SimilarPressure(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataConditionalStandardSize,
            Data.OriginalFanDataAirDensity,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

    private double? _similarPowerCoefficient;

    /// <summary>
    /// Коэффициент подобия Power с оригинальной FanData
    /// </summary>
    private double SimilarPowerCoefficient =>
        _similarPowerCoefficient ??=
            Similarity.SimilarPower(
                1.0,
                Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.OriginalFanDataConditionalStandardSize,
                Data.OriginalFanDataAirDensity,
                ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                ConditionalStandardSize,
                UserInput.DataAir.Density.KilogramsPerCubicMeter
            ) / Data.FanEfficiencyGradeCoefficient;

    private double? _similarNoiseCoefficient;

    /// <summary>
    /// Коэффициент подобия Noise с оригинальной FanData
    /// </summary>
    private double SimilarNoiseCoefficient =>
        _similarNoiseCoefficient ??= Similarity.SimilarNoise(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataConditionalStandardSize,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize
        );

    private double? _conditionalStandardSize;

    /// <summary>
    /// Типоразмер, [мм]
    /// </summary>
    public double ConditionalStandardSize =>
        _conditionalStandardSize ??=
            (
                UserInput.UserInputFan.ConditionalStandardSize == 0
                    ? Data.ConditionalStandardSize
                    : UserInput.UserInputFan.ConditionalStandardSize
            ) / 1000;

    private double? _nominalImpellerRotationSpeedWithoutSlidingEngine;

    /// <summary>
    /// Номинальная скорость вращения крыльчатки без учета скольжения двигателя, [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    protected double NominalImpellerRotationSpeedWithoutSlidingEngine =>
        _nominalImpellerRotationSpeedWithoutSlidingEngine ??=
            UserInput
                .UserInputFan
                .NominalImpellerRotationSpeedWithoutSlidingEngine == 0
                ? Data.NominalImpellerRotationSpeedWithoutSlidingEngine
                : Math.Round(
                    UserInput
                        .UserInputFan
                        .NominalImpellerRotationSpeedWithoutSlidingEngine,
                    0
                );

    private double? _nominalPower;

    /// <summary>
    /// Номинальная мощность двигателя, [кВт]
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public double NominalPower =>
        _nominalPower ??=
            UserInput.UserInputFan.NominalPower == 0
                ? Data.NominalPower
                : UserInput.UserInputFan.NominalPower;

    private double? _fanOperatingMaxTemperature;

    /// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    protected double FanOperatingMaxTemperature =>
        _fanOperatingMaxTemperature ??=
            UserInput.UserInputAir.FanOperatingMaxTemperature == 0
            && Data.FanOperatingMaxTemperature != null
                ? Data.FanOperatingMaxTemperature.First()
                : UserInput.UserInputAir.FanOperatingMaxTemperature;

    private double? _fanBodyLength;

    /// <summary>
    /// Длина корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    protected double FanBodyLength =>
        _fanBodyLength ??=
            UserInput.UserInputFan.FanBodyLength == 0
            && Data.FanBodyLength != null
                ? Data.FanBodyLength.First()
                : UserInput.UserInputFan.FanBodyLength;

    private string? _impellerRotationDirection;

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    protected string ImpellerRotationDirection =>
        _impellerRotationDirection ??=
            string.IsNullOrEmpty(
                UserInput.UserInputFan.ImpellerRotationDirection
            )
            && Data.ImpellerRotationDirection != null
                ? Data.ImpellerRotationDirection.First()
                : UserInput.UserInputFan.ImpellerRotationDirection!;

    private string? _fanBodyExecutionMaterial;

    /// <summary>
    /// Материал корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    protected string FanBodyExecutionMaterial =>
        _fanBodyExecutionMaterial ??=
            string.IsNullOrEmpty(
                UserInput.UserInputFan.FanBodyExecutionMaterial
            )
            && Data.FanBodyExecutionMaterial != null
                ? Data.FanBodyExecutionMaterial.First()
                : UserInput.UserInputFan.FanBodyExecutionMaterial!;

    private double? _impellerRotationFrequency;

    /// <summary>
    /// Частота вращения крыльчатки, [Гц]
    /// </summary>
    public double ImpellerRotationFrequency =>
        _impellerRotationFrequency ??= Calculate.ImpellerRotationFrequency(
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            NominalImpellerRotationSpeedWithoutSlidingEngine
        );

    private double? _inputTotalNormalPressure;

    /// <summary>
    /// Полное давление воздуха, которое ввел пользователь, приведенное к нормальной плотности воздуха
    /// </summary>
    protected double InputTotalNormalPressure =>
        _inputTotalNormalPressure ??= Similarity.SimilarPressure(
            UserInput.UserInputWorkPoint.TotalPressure,
            1,
            1,
            UserInput.DataAir.Density.KilogramsPerCubicMeter,
            1,
            1,
            Data.AirDensity
        );

    private double? _volumeFlowOnPolynomial;

    /// <summary>
    /// Расход объемного воздуха на исходной кривой вентилятора, [м3/ч]
    /// </summary>
    public double VolumeFlowOnPolynomial =>
        _volumeFlowOnPolynomial ??= Calculate.MethodOfHalfDivisionVolumeFlow(
            Data,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            InputTotalNormalPressure,
            ConditionalStandardSize
        );

    private double? _totalPressureOnPolynomial;

    /// <summary>
    /// Полное давление воздуха на исходной кривой вентилятора, [Па]
    /// </summary>
    public double TotalPressureOnPolynomial =>
        _totalPressureOnPolynomial ??= Calculate.Polynomial(
            Data.TotalPressureQvCoefficients,
            VolumeFlowOnPolynomial
        );

    private double? _volumeFlow;

    /// <summary>
    ///     Расход объемного воздуха на кривой вентилятора, эквивалентный зависимости Pv=Q^2 - характеристика сети воздуховода, [м3/ч]
    /// </summary>
    public double VolumeFlow =>
        _volumeFlow ??= VolumeFlowOnPolynomial * SimilarVolumeFlowCoefficient;

    private double? _totalPressure;

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        _totalPressure ??=
            TotalPressureOnPolynomial * SimilarTotalPressureCoefficient;

    private double? _volumeFlowDeviation;

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        _volumeFlowDeviation ??= Calculate.Deviation(
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            VolumeFlow
        );

    private double? _totalPressureDeviation;

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        _totalPressureDeviation ??= Calculate.Deviation(
            UserInput.UserInputWorkPoint.TotalPressure,
            TotalPressure
        );

    private double? _power;

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        _power ??=
            Calculate.Polynomial(
                Data.PowerQvCoefficients,
                VolumeFlowOnPolynomial
            ) * SimilarPowerCoefficient;

    private double? _staticPressure;

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        _staticPressure ??= Calculate.StaticPressure(
            TotalPressure,
            DynamicPressure
        );

    private double? _dynamicPressure;

    /// <summary>
    ///     Расчетное динамическое давление воздуха, [Па]
    /// </summary>
    public double DynamicPressure =>
        _dynamicPressure ??= Calculate.DynamicPressure(
            UserInput.DataAir,
            AirVelocityOfOutletPipeOpening
        );

    private double? _totalEfficiency;

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double TotalEfficiency =>
        _totalEfficiency ??= Calculate.Efficiency(
            VolumeFlow,
            TotalPressure,
            Power
        );

    private double? _airVelocityOfOutletPipeOpening;

    /// <summary>
    ///     Скорость воздуха в выпускной трубе, [м/с]
    /// </summary>
    public double AirVelocityOfOutletPipeOpening =>
        _airVelocityOfOutletPipeOpening ??=
            Calculate.AirVelocityOfOutletPipeOpening(
                VolumeFlow,
                Data.SquareOfOutletPipeOpening
            );

    public int NumberOfFans { get; set; }

    //____________________________________________________________________________________________________________________________
    //Уровень шума

    private IEnumerable<(int Frequency, double Value)>? _octaveNoiseLw5;

    /// <summary>
    /// Список уровней звуковой мощности Lw5 на входе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw5 =>
        _octaveNoiseLw5 ??= Noise.CalcOctaveNoiseLw(
            "Lw5",
            Data,
            VolumeFlowOnPolynomial,
            SimilarNoiseCoefficient,
            NumberOfFans
        );

    private IEnumerable<(int Frequency, double Value)>? _octaveNoiseLw6;

    /// <summary>
    /// Список уровней звуковой мощности Lw6 на выходе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw6 =>
        _octaveNoiseLw6 ??= Noise.CalcOctaveNoiseLw(
            "Lw6",
            Data,
            VolumeFlowOnPolynomial,
            SimilarNoiseCoefficient,
            NumberOfFans
        );

    //____________________________________________________________________________________________________________________________
    //Уровень шума по А

    private IEnumerable<(int Frequency, double Value)>? _octaveNoiseLwA5;

    /// <summary>
    /// Список уровней звуковой мощности Lw5 на входе вентилятора по октавам с поправкой по спектру А
    /// </summary>
    private IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA5 =>
        _octaveNoiseLwA5 ??= Noise.CalcOctaveNoiseLwA(OctaveNoiseLw5);

    private IEnumerable<(int Frequency, double Value)>? _octaveNoiseLwA6;

    /// <summary>
    /// Список уровней звуковой мощности Lw6 на входе вентилятора по октавам с поправкой по спектру А
    /// </summary>
    private IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA6 =>
        _octaveNoiseLwA6 ??= Noise.CalcOctaveNoiseLwA(OctaveNoiseLw6);

    private double? _sumNoiseLwA5;

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA5 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA5 =>
        _sumNoiseLwA5 ??= Calculate.SumNoise(OctaveNoiseLwA5);

    private double? _sumNoiseLwA6;

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA6 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA6 =>
        _sumNoiseLwA6 ??= Calculate.SumNoise(OctaveNoiseLwA6);

    //____________________________________________________________________________________________________________________________
    //Расчет безразмерных характеристик

    private double? _performanceCoefficientUserInput;

    /// <summary>
    /// Коэффициент производительности для расхода и давления, введенных пользователем
    /// </summary>
    private double PerformanceCoefficientUserInput =>
        _performanceCoefficientUserInput ??= DimensionlessData.PhiCoefficient(
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            Data.SquareOfWheelDisc,
            Data.CircumferentialSpeed
        );

    private double? _totalPressureCoefficientUserInput;

    /// <summary>
    /// Коэффициент полного давления для расхода и давления, введенных пользователем
    /// </summary>
    public double TotalPressureCoefficientUserInput =>
        _totalPressureCoefficientUserInput ??= Calculate.Polynomial(
            Data.PsiPhiCoefficients,
            PerformanceCoefficientUserInput
        );

    private double? _staticPressureCoefficientUserInput;

    /// <summary>
    /// Коэффициент статического давления для расхода и давления, введенных пользователем
    /// </summary>
    public double StaticPressureCoefficientUserInput =>
        _staticPressureCoefficientUserInput ??=
            DimensionlessData.PsiCoefficient(
                StaticPressure,
                Data.AirDensity,
                Data.CircumferentialSpeed
            );

    private double? _powerCoefficientUserInput;

    /// <summary>
    /// Коэффициент потребляемой мощности для расхода и давления, введенных пользователем
    /// </summary>
    public double PowerCoefficientUserInput =>
        _powerCoefficientUserInput ??= DimensionlessData.PowerCoefficient(
            Power,
            Data.AirDensity,
            Data.CircumferentialSpeed,
            Data.SquareOfWheelDisc
        );

    private double? _specificSpeedEfficiencyMax;

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSpeedEfficiencyMax =>
        _specificSpeedEfficiencyMax ??= Calculate.Polynomial(
            Data.SpecificSpeedPhiCoefficients,
            Data.PhiEfficiencyMax
        );

    private double? _specificSpeedPhiMin;

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMin =>
        _specificSpeedPhiMin ??= Calculate.Polynomial(
            Data.SpecificSpeedPhiCoefficients,
            Data.PhiMin
        );

    private double? _specificSpeedPhiMax;

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMax =>
        _specificSpeedPhiMax ??= Calculate.Polynomial(
            Data.SpecificSpeedPhiCoefficients,
            Data.PhiMax
        );

    private double? _specificSpeedCoefficientWithImpellerRotationSpeed;

    /// <summary>
    /// Коэффициент быстроходности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSpeedCoefficientWithImpellerRotationSpeed =>
        _specificSpeedCoefficientWithImpellerRotationSpeed ??=
            DimensionlessData.SpeedCoefficient(
                ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
                InputTotalNormalPressure
            );

    private double? _specificSizeEfficiencyMax;

    /// <summary>
    /// Коэффициент габаритности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSizeEfficiencyMax =>
        _specificSizeEfficiencyMax ??= Calculate.Polynomial(
            Data.SpecificSizePhiCoefficients,
            Data.PhiEfficiencyMax
        );

    private double? _specificSizePhiMin;

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMin =>
        _specificSizePhiMin ??= Calculate.Polynomial(
            Data.SpecificSizePhiCoefficients,
            Data.PhiMin
        );

    private double? _specificSizePhiMax;

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMax =>
        _specificSizePhiMax ??= Calculate.Polynomial(
            Data.SpecificSizePhiCoefficients,
            Data.PhiMax
        );

    private double? _specificSizeCoefficientWithRequiredSize;

    /// <summary>
    /// Коэффициент габаритности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSizeCoefficientWithRequiredSize =>
        _specificSizeCoefficientWithRequiredSize ??=
            DimensionlessData.SizeCoefficient(
                UserInput.UserInputFan.ConditionalStandardSize / 1000,
                UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
                InputTotalNormalPressure
            );

    /// <summary>
    /// Количество точек на новой кривой
    /// </summary>
    private const int NumberOfDataCurves = 8;

    private IEnumerable<DataCurve>? _originalCurve;

    /// <summary>
    /// Расчет рабочих точек для оригинальной кривой
    /// </summary>
    private IEnumerable<DataCurve> OriginalCurve =>
        _originalCurve ??= Calculate
            .CreateDataCurves(
                Data.MinVolumeFlow,
                Data.MaxVolumeFlow,
                NumberOfDataCurves,
                Data.TotalPressureQvCoefficients,
                ConditionalStandardSize,
                ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.AirDensity,
                Data.PowerQvCoefficients
            )
            .Select(
                item =>
                    item with
                    {
                        DcEfficiency = Calculate.Efficiency(
                            item.DcVolumeFlow,
                            item.DcTotalPressure,
                            item.DcPower
                        )
                    }
            );

    private List<DataCurve>? _newCurve;

    /// <summary>
    /// Расчет рабочих точек для новой кривой
    /// </summary>
    public List<DataCurve> NewCurve =>
        _newCurve ??= OriginalCurve
            .Select(
                (workPoint, index) =>
                    new DataCurve
                    {
                        DcIndex = index,
                        DcVolumeFlow =
                            workPoint.DcVolumeFlow
                            * SimilarVolumeFlowCoefficient,
                        DcTotalPressure =
                            workPoint.DcTotalPressure
                            * SimilarTotalPressureCoefficient,
                        DcConditionalStandardSize =
                            workPoint.DcConditionalStandardSize,
                        DcImpellerRotationSpeed =
                            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                        DcAir = UserInput
                            .DataAir
                            .Density
                            .KilogramsPerCubicMeter,
                        DcPower = workPoint.DcPower * SimilarPowerCoefficient
                    }
            )
            .Select(
                item =>
                    item with
                    {
                        DcEfficiency = Calculate.Efficiency(
                            item.DcVolumeFlow,
                            item.DcTotalPressure,
                            item.DcPower
                        )
                    }
            )
            .ToList();

    //____________________________________________________________________________________________________________________________
    //Методы INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null
    )
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );
    }

    protected bool SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null
    )
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

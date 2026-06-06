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
    protected AbstractFan(
        FanData data,
        UserInput userInput,
        BladeType bladeType,
        BladeOrientation bladeOrientation,
        int numberOfFans)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data)); // Проверка на null
        UserInput =
            userInput ?? throw new ArgumentNullException(nameof(userInput)); // Проверка на null
        BladeType = bladeType;
        BladeOrientation = bladeOrientation;
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
    public virtual string? ProjectId { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public virtual double ImpellerRotationSpeedWithSlidingEngineForWorkPoint { get; init; }

    /// <summary>
    /// Минимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public virtual double MinImpellerRotationFrequency { get; protected init; }

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public virtual double TotalEfficiency { get; init; }

    private double EfficiencyGradeCoefficient => Calculate.PowerScaleEffectByFanMotorEfficiencyGrade(
        Data.OriginalFanDataPowerByEfficiencyMax,
        Data.OriginalFanDataEfficiencyMax,
        Data.OriginalFanDataPowerByEfficiencyMax,
        BladeType,
        BladeOrientation);

    private BladeType BladeType { get; }

    private BladeOrientation BladeOrientation { get; }

    /// <summary>
    /// Максимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MaxImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            NominalImpellerRotationSpeedWithoutSlidingEngine
        );

    /// <summary>
    /// Коэффициент подобия VolumeFlow с оригинальной FanData
    /// </summary>
    private double SimilarVolumeFlowCoefficient =>
        Similarity.SimilarVolumeFlow(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.DiameterOfTheImpellerAtTheEndsOfTheBlades
        );

    /// <summary>
    /// Коэффициент подобия TotalPressure с оригинальной FanData
    /// </summary>
    private double SimilarTotalPressureCoefficient =>
        Similarity.SimilarPressure(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
            Data.OriginalFanDataAirDensity,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

    /// <summary>
    /// Коэффициент подобия Power с оригинальной FanData
    /// </summary>
    private double SimilarPowerCoefficient =>
        Similarity.SimilarPower(
            1.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
            Data.OriginalFanDataAirDensity,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        ) / EfficiencyGradeCoefficient;

    /// <summary>
    /// Коэффициент подобия Noise с оригинальной FanData
    /// </summary>
    private double SimilarNoiseCoefficient =>
        Similarity.SimilarNoise(
            0.0,
            Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            Data.DiameterOfTheImpellerAtTheEndsOfTheBlades
        );

    public double EfficiencyMaxWithEfficiencyGradeCoefficient =>

    Data.EfficiencyMax * EfficiencyGradeCoefficient;

    /// <summary>
    /// Типоразмер, [мм]
    /// </summary>
    public double ConditionalStandardSize =>
        (
            UserInput.UserInputFan.ConditionalStandardSize == 0
                ? Data.ConditionalStandardSize
                : UserInput.UserInputFan.ConditionalStandardSize
        ) / 1000;

    /// <summary>
    /// Номинальная скорость вращения крыльчатки без учета скольжения двигателя, [об/мин]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.NominalImpellerRotationSpeeds
    /// </summary>
    protected double NominalImpellerRotationSpeedWithoutSlidingEngine =>
        UserInput.UserInputFan.NominalImpellerRotationSpeedWithoutSlidingEngine
        == 0
            ? Data.NominalImpellerRotationSpeedWithoutSlidingEngine
            : Math.Round(
                UserInput
                    .UserInputFan
                    .NominalImpellerRotationSpeedWithoutSlidingEngine,
                0
            );

    /// <summary>
    /// Номинальная мощность двигателя, [кВт]
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public double NominalPower =>
        UserInput.UserInputFan.NominalPower == 0
            ? Data.NominalPower
            : UserInput.UserInputFan.NominalPower;

    /// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    protected double FanOperatingMaxTemperature =>
        UserInput.UserInputAir.FanOperatingMaxTemperature == 0
        && Data.FanOperatingMaxTemperature != null
            ? Data.FanOperatingMaxTemperature.First()
            : UserInput.UserInputAir.FanOperatingMaxTemperature;

    /// <summary>
    /// Длина корпуса, которое ввел пользователь. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanBodyLengths
    /// </summary>
    protected double FanBodyLength =>
        UserInput.UserInputFan.FanBodyLength == 0 && Data.FanBodyLength != null
            ? Data.FanBodyLength.First()
            : UserInput.UserInputFan.FanBodyLength;

    /// <summary>
    ///     Направление вращения рабочего колеса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.ImpellerRotationDirections
    /// </summary>
    protected string ImpellerRotationDirection =>
        string.IsNullOrEmpty(UserInput.UserInputFan.ImpellerRotationDirection)
        && Data.ImpellerRotationDirection != null
            ? Data.ImpellerRotationDirection.First()
            : UserInput.UserInputFan.ImpellerRotationDirection!;

    /// <summary>
    /// Материал корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    protected string FanBodyExecutionMaterial =>
        string.IsNullOrEmpty(UserInput.UserInputFan.FanBodyExecutionMaterial)
        && Data.FanBodyExecutionMaterial != null
            ? Data.FanBodyExecutionMaterial.First()
            : UserInput.UserInputFan.FanBodyExecutionMaterial!;

    /// <summary>
    /// Частота вращения крыльчатки, [Гц]
    /// </summary>
    public double ImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            NominalImpellerRotationSpeedWithoutSlidingEngine
        );

    /// <summary>
    /// Полное давление воздуха, которое ввел пользователь, приведенное к нормальной плотности воздуха
    /// </summary>
    protected double InputTotalNormalPressure =>
        Similarity.SimilarPressure(
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
    public double VolumeFlowOnPolynomial =>
        Calculate.MethodOfHalfDivisionVolumeFlow(
            Data,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            InputTotalNormalPressure
        //Data.DiameterOfTheImpellerAtTheEndsOfTheBlades
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
        VolumeFlowOnPolynomial * SimilarVolumeFlowCoefficient;

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        TotalPressureOnPolynomial * SimilarTotalPressureCoefficient;

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Calculate.Deviation(
            VolumeFlow,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans
        );

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Calculate.Deviation(
            TotalPressure,
            UserInput.UserInputWorkPoint.TotalPressure
        );

    /// <summary>
    /// Отклонение текущей быстроходности от быстроходности при максимальном КПД. С увеличением VolumeFlow быстроходность возрастает.
    /// </summary>
    public double SpecificSpeedDeviation =>
        Calculate.Deviation(
            SpecificSpeedCoefficientWithImpellerRotationSpeed,
            SpecificSpeedEfficiencyMax
        );

    /// <summary>
    /// Отклонение текущей габаритности от габаритности при максимальном КПД. С увеличением VolumeFlow габаритность убывает.
    /// </summary>
    public double SpecificSizeDeviation =>
        Calculate.Deviation(
            SpecificSizeCoefficientWithRequiredSize,
            SpecificSizeEfficiencyMax
        );

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        Calculate.Polynomial(Data.PowerQvCoefficients, VolumeFlowOnPolynomial)
        * SimilarPowerCoefficient;

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Calculate.StaticPressure(TotalPressure, DynamicPressure);

    /// <summary>
    ///     Расчетное динамическое давление воздуха, [Па]
    /// </summary>
    public double DynamicPressure =>
        Calculate.DynamicPressure(
            UserInput.DataAir,
            AirVelocityOfOutletPipeOpening
        );

    /// <summary>
    ///     Скорость воздуха в выпускной трубе, [м/с]
    /// </summary>
    public double AirVelocityOfOutletPipeOpening =>
        Calculate.AirVelocityOfOutletPipeOpening(
            VolumeFlow,
            Data.SquareOfOutletPipeOpening
        );

    public int NumberOfFans { get; }

    //____________________________________________________________________________________________________________________________
    //Уровень шума

    /// <summary>
    /// Список уровней звуковой мощности Lw5 на входе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw5 =>
        Noise.CalcOctaveNoiseLw(
            "Lw5",
            Data,
            VolumeFlowOnPolynomial,
            SimilarNoiseCoefficient,
            NumberOfFans
        );

    /// <summary>
    /// Список уровней звуковой мощности Lw6 на выходе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw6 =>
        Noise.CalcOctaveNoiseLw(
            "Lw6",
            Data,
            VolumeFlowOnPolynomial,
            SimilarNoiseCoefficient,
            NumberOfFans
        );

    //____________________________________________________________________________________________________________________________
    //Уровень шума по А

    /// <summary>
    /// Список уровней звуковой мощности Lw5 на входе вентилятора по октавам с поправкой по спектру А
    /// </summary>
    private IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA5 =>
        Noise.CalcOctaveNoiseLwA(OctaveNoiseLw5);

    /// <summary>
    /// Список уровней звуковой мощности Lw6 на входе вентилятора по октавам с поправкой по спектру А
    /// </summary>
    private IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA6 =>
        Noise.CalcOctaveNoiseLwA(OctaveNoiseLw6);

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA5 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA5 => Calculate.SumNoise(OctaveNoiseLwA5);

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA6 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA6 => Calculate.SumNoise(OctaveNoiseLwA6);

    //____________________________________________________________________________________________________________________________
    //Расчет безразмерных характеристик

    /// <summary>
    /// Коэффициент производительности для расхода и давления, введенных пользователем
    /// </summary>
    private double PerformanceCoefficientUserInput =>
        DimensionlessData.PhiCoefficient(
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            Data.SquareOfWheelDisc,
            Data.CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент полного давления для расхода и давления, введенных пользователем
    /// </summary>
    public double TotalPressureCoefficientUserInput =>
        Calculate.Polynomial(
            Data.PsiPhiCoefficients,
            PerformanceCoefficientUserInput
        );

    /// <summary>
    /// Коэффициент статического давления для расхода и давления, введенных пользователем
    /// </summary>
    public double StaticPressureCoefficientUserInput =>
        DimensionlessData.PsiCoefficient(
            StaticPressure,
            Data.AirDensity,
            Data.CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент потребляемой мощности для расхода и давления, введенных пользователем
    /// </summary>
    public double PowerCoefficientUserInput =>
        DimensionlessData.PowerCoefficient(
            Power,
            Data.AirDensity,
            Data.CircumferentialSpeed,
            Data.SquareOfWheelDisc
        );

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSpeedEfficiencyMax =>
        Calculate.Polynomial(
            Data.SpecificSpeedPhiCoefficients,
            Data.PhiEfficiencyMax
        );

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMin =>
        Calculate.Polynomial(Data.SpecificSpeedPhiCoefficients, Data.PhiMin);

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMax =>
        Calculate.Polynomial(Data.SpecificSpeedPhiCoefficients, Data.PhiMax);

    /// <summary>
    /// Коэффициент быстроходности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSpeedCoefficientWithImpellerRotationSpeed =>
        DimensionlessData.SpeedCoefficient(
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            InputTotalNormalPressure
        );

    /// <summary>
    /// Коэффициент габаритности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSizeEfficiencyMax =>
        Calculate.Polynomial(
            Data.SpecificSizePhiCoefficients,
            Data.PhiEfficiencyMax
        );

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMin =>
        Calculate.Polynomial(Data.SpecificSizePhiCoefficients, Data.PhiMin);

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMax =>
        Calculate.Polynomial(Data.SpecificSizePhiCoefficients, Data.PhiMax);

    /// <summary>
    /// Коэффициент габаритности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSizeCoefficientWithRequiredSize =>
        DimensionlessData.SizeCoefficient(
            (
                UserInput.UserInputFan.ConditionalStandardSize == 0
                    ? 0
                    : Data.DiameterOfTheImpellerAtTheEndsOfTheBlades
            ) / 1000,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            InputTotalNormalPressure
        );

    /// <summary>
    /// Количество точек на новой кривой
    /// </summary>
    private const int NumberOfDataCurves = 8;

    /// <summary>
    /// Расчет рабочих точек для оригинальной кривой
    /// </summary>
    private IEnumerable<DataCurve> OriginalCurve =>
        Calculate
            .CreateDataCurves(
                Data.MinVolumeFlow,
                Data.MaxVolumeFlow,
                NumberOfDataCurves,
                Data.TotalPressureQvCoefficients,
                Data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.AirDensity,
                Data.PowerQvCoefficients
            )
            .Select(
                item =>
                    item with
                    {
                        DcTotalEfficiency = Calculate.Efficiency(
                            item.DcVolumeFlow,
                            item.DcTotalPressure,
                            item.DcPower
                        )
                    }
            );

    /// <summary>
    /// Расчет рабочих точек для новой кривой
    /// </summary>
    public List<DataCurve> NewCurve =>
        OriginalCurve
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
                        DcDiameterOfTheImpellerAtTheEndsOfTheBlades =
                            workPoint.DcDiameterOfTheImpellerAtTheEndsOfTheBlades,
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
                        DcTotalEfficiency = Calculate.Efficiency(
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

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
    protected AbstractFan(FanData data, UserInput userInput)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data)); // Проверка на null
        UserInput =
            userInput ?? throw new ArgumentNullException(nameof(userInput)); // Проверка на null
    }

    /// <summary>
    /// Данные экземпляра вентилятора из исходных данных. К ним происходит обращение через UserInput.PathDataOfFansJsonFile
    /// </summary>
    public FanData Data { get; }

    /// <summary>
    /// Данные, которые ввел пользователь
    /// </summary>
    protected UserInput UserInput { get; }

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

    /// <summary>
    /// Максимальная частота вращения крыльчатки, [Гц]
    /// </summary>
    public double MaxImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            NominalImpellerRotationSpeedWithoutSlidingEngine
        );

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
    public double NominalImpellerRotationSpeedWithoutSlidingEngine =>
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
    public double NominalPower =>
        UserInput.UserInputFan.NominalPower == 0
            ? Data.NominalPower
            : UserInput.UserInputFan.NominalPower;

    /// <summary>
    /// Температура перемещаемой среды, [°C]. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.FanOperatingMaxTemperatures
    /// </summary>
    public double FanOperatingMaxTemperature =>
        UserInput.UserInputAir.FanOperatingMaxTemperature == 0
        && Data.FanOperatingMaxTemperature != null
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
        string.IsNullOrEmpty(UserInput.UserInputFan.ImpellerRotationDirection)
        && Data.ImpellerRotationDirection != null
            ? Data.ImpellerRotationDirection.First()
            : UserInput.UserInputFan.ImpellerRotationDirection!;

    /// <summary>
    /// Материал корпуса. Допустимые значения указаны в Libraries.DescriptionOfObjects.Parameters.CaseExecutionMaterials
    /// </summary>
    public string FanBodyExecutionMaterial =>
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
    public double InputTotalNormalPressure =>
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
            Data.MinVolumeFlow,
            Data.MaxVolumeFlow,
            Data.TotalPressureQvCoefficients,
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            InputTotalNormalPressure,
            Data.SimilarVolumeFlowCoefficient,
            Data.SimilarTotalPressureCoefficient
        );

    /// <summary>
    /// Полное давление воздуха на исходной кривой вентилятора, [Па]
    /// </summary>
    public double TotalPressureOnPolynomial =>
        Calculate.Polynomial(
            Data.TotalPressureQvCoefficients,
            VolumeFlowOnPolynomial / Data.SimilarVolumeFlowCoefficient
        ) * Data.SimilarTotalPressureCoefficient;

    /// <summary>
    ///     Расход объемного воздуха на кривой вентилятора, эквивалентный зависимости Pv=Q^2 - характеристика сети воздуховода, [м3/ч]
    /// </summary>
    public double VolumeFlow =>
        Similarity.SimilarVolumeFlow(
            VolumeFlowOnPolynomial,
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize
        );

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        Similarity.SimilarPressure(
            TotalPressureOnPolynomial,
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
            Data.AirDensity,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Calculate.Deviation(
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
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
        Similarity.SimilarPower(
            Calculate.Polynomial(
                Data.PowerQvCoefficients,
                VolumeFlowOnPolynomial / Data.SimilarVolumeFlowCoefficient
            ) * Data.SimilarPowerCoefficient,
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
            Data.AirDensity,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ConditionalStandardSize,
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

    //TODO Динамическое давление слишком велико.
    //Скорее всего оно не верно считается. Хорошо, что оно больше не используется в дальнейших расчетах.
    //Из ГОСТ 10616-2015: Динамическое давление потока при выходе из вентилятора, рассчитанное по величине объемной производительности, средней плотности газа на выходе и площади нагнетательного отверстия вентилятора.
    // Из ГОСТ 10921-2017: Условное давление на выходе из вентилятора, рассчитанное по среднерасходной скорости v.

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double TotalEfficiency =>
        Calculate.Efficiency(VolumeFlow, TotalPressure, Power);

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Calculate.AirVelocity(VolumeFlow, Data.AreaOfInletPipeOpening);

    public int NumberOfFans { get; set; } = 1;

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
            ConditionalStandardSize,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
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
            ConditionalStandardSize,
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
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
    public double PerformanceCoefficientUserInput =>
        DimensionlessData.PhiCoefficient(
            UserInput.UserInputWorkPoint.VolumeFlow / NumberOfFans,
            Data.AreaOfWheelDisc,
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
            Data.AreaOfWheelDisc
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
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
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
            ConditionalStandardSize,
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
                ConditionalStandardSize,
                ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.AirDensity,
                Data.PowerQvCoefficients,
                Data.SimilarVolumeFlowCoefficient,
                Data.SimilarTotalPressureCoefficient,
                Data.SimilarPowerCoefficient
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
                        DcVolumeFlow = Similarity.SimilarVolumeFlow(
                            workPoint.DcVolumeFlow,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ConditionalStandardSize
                        ),
                        DcTotalPressure = Similarity.SimilarPressure(
                            workPoint.DcTotalPressure,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            workPoint.DcAir,
                            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ConditionalStandardSize,
                            UserInput.DataAir.Density.KilogramsPerCubicMeter
                        ),
                        DcConditionalStandardSize =
                            workPoint.DcConditionalStandardSize,
                        DcImpellerRotationSpeed =
                            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                        DcAir = UserInput
                            .DataAir
                            .Density
                            .KilogramsPerCubicMeter,
                        DcPower = Similarity.SimilarPower(
                            workPoint.DcPower,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            workPoint.DcAir,
                            ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ConditionalStandardSize,
                            UserInput.DataAir.Density.KilogramsPerCubicMeter
                        )
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

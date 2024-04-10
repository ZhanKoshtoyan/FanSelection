using Libraries.Description_of_objects;
using Libraries.Description_of_objects.UserInput;
using Libraries.Methods;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.Fans;

public interface IFan
{
    /// <summary>
    ///     Расчетная плотность воздуха, температура которого введена пользователем, [кг/м3]
    /// </summary>
    public IHumidAir UserInputAir =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(
                UserInput.UserInputAir.Altitude.GetValueOrDefault().Meters()
            ),
            InputHumidAir.Temperature(
                UserInput.UserInputAir.FanOperatingMinTemperature.DegreesCelsius()
            ),
            InputHumidAir.RelativeHumidity(
                UserInput.UserInputAir.RelativeHumidity
                    .GetValueOrDefault()
                    .Percent()
            )
        );

    public FanData Data { get; }
    public UserInput UserInput { get; }

    public double Size =>
        Convert.ToDouble(
            UserInput.UserInputFan.Size == 0
                ? Data.Size
                : UserInput.UserInputFan.Size
        ) / 1000;

    public double RoundedImpellerRotationSpeed =>
        UserInput.UserInputFan.ImpellerRotationSpeed == 0
            ? Data.NominalImpellerRotationSpeed
            : Math.Round(
                UserInput.UserInputFan.ImpellerRotationSpeed.GetValueOrDefault(),
                0
            );

    public double NominalPower =>
        UserInput.UserInputFan.NominalPower == 0
            ? Math.Round(Data.NominalPower * 100, 1)
            : Math.Round(
                UserInput.UserInputFan.NominalPower.GetValueOrDefault() * 100,
                1
            );

    /// <summary>
    ///     Проектное наименование вентилятора
    /// </summary>
    public string? ProjectId => null;

    public double ImpellerRotationSpeed { get; }

    public double VolumeFlowOnPolynomial =>
        Calculate.MethodOfHalfDivision(
            Data.MinVolumeFlow,
            Data.MaxVolumeFlow,
            Data.TotalPressureCoefficients,
            UserInput.UserInputWorkPoint.VolumeFlow,
            UserInput.UserInputWorkPoint.TotalPressure
        );

    /// <summary>
    ///     Расход Объемного воздуха на кривой вентилятора, эквивалентный зависимости Pv=Q^2 - характеристика сети воздуховода
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
            Calculate.Polynomial(
                Data.TotalPressureCoefficients,
                VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            Size,
            FanData.AirInTests,
            ImpellerRotationSpeed,
            Size,
            UserInputAir
        );

    /// <summary>
    ///     Расчетное динамическое давление воздуха, [Па]
    /// </summary>
    public double DynamicPressure =>
        Calculate.DynamicPressure(UserInputAir, AirVelocity);

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Calculate.StaticPressure(TotalPressure, DynamicPressure);

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency =>
        Calculate.Efficiency(VolumeFlow, TotalPressure, Power);

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Calculate.AirVelocity(VolumeFlow, Data.InletCrossSection);

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        SimilarityCalculator.SimilarPower(
            Calculate.Polynomial(
                Data.PowerCoefficients,
                VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            Size,
            FanData.AirInTests,
            ImpellerRotationSpeed,
            Size,
            UserInputAir
        );

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Calculate.VolumeFlowDeviation(
            UserInput.UserInputWorkPoint.VolumeFlow,
            VolumeFlow
        );

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Calculate.TotalPressureDeviation(
            UserInput.UserInputWorkPoint.TotalPressure,
            TotalPressure
        );
}

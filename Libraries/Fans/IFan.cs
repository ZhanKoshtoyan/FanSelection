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
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA63 = -26.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA125 = -16.1;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA250 = -8.6;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA500 = -3.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA1000 = 0;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA2000 = 1.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA4000 = 1.0;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA8000 = -1.1;

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

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря
    /// </summary>
    public IHumidAir AirInTests =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(20.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );

    public int Size =>
        Convert.ToInt32(
            UserInput.UserInputFan.Size == 0
                ? Data.Size
                : UserInput.UserInputFan.Size
        );

    public int RoundedImpellerRotationSpeed =>
        (int) (
            UserInput.UserInputFan.ImpellerRotationSpeed == 0
                ? Data.NominalImpellerRotationSpeed
                : Math.Round(
                    (double)
                    UserInput.UserInputFan.ImpellerRotationSpeed.GetValueOrDefault(),
                    0
                )
        );

    public int NominalPower =>
        (int) (
            UserInput.UserInputFan.NominalPower == 0
                ? Math.Round(Data.NominalPower * 100, 1)
                : Math.Round(
                    UserInput.UserInputFan.NominalPower.GetValueOrDefault()
                    * 100,
                    1
                )
        );

    /// <summary>
    ///     Проектное наименование вентилятора
    /// </summary>
    public string? ProjectId => null;

    public int ImpellerRotationSpeed { get; }

    public double VolumeFlowOnPolynomial =>
        MethodOfHalfDivision.Calculate(
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
            PolynomialCalculator.Calculate(Data.TotalPressureCoefficients, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            UserInputAir,
            ImpellerRotationSpeed,
            Size,
            AirInTests
        );

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Math.Round(TotalPressure - 0.5 * AirInTests.Density.KilogramsPerCubicMeter
            * Math.Pow(AirVelocity, 2),
            0
        );

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency =>
        Math.Round(VolumeFlow / 3600 * TotalPressure
            / (Power * 1000) * 100,
            1
        );

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Math.Round(VolumeFlow / 3600 / Data.InletCrossSection, 1);

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        SimilarityCalculator.SimilarPower(
            PolynomialCalculator.Calculate(Data.PowerCoefficients, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            UserInputAir,
            ImpellerRotationSpeed,
            Size,
            AirInTests
        );

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Math.Round((1 - UserInput.UserInputWorkPoint.VolumeFlow / VolumeFlow) * 100, 2);

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Math.Round((1 - UserInput.UserInputWorkPoint.TotalPressure / TotalPressure) * 100, 2);

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности на частоте 63Гц
    /// </summary>
    public double OctaveNoise63 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients63, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 125Гц
    /// </summary>
    public double OctaveNoise125 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients125, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 250Гц
    /// </summary>
    public double OctaveNoise250 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients250, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 500Гц
    /// </summary>
    public double OctaveNoise500 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients500, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 1000Гц
    /// </summary>
    public double OctaveNoise1000 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients1000, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 2000Гц
    /// </summary>
    public double OctaveNoise2000 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients2000, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 4000Гц
    /// </summary>
    public double OctaveNoise4000 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients4000, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 8000Гц
    /// </summary>
    public double OctaveNoise8000 =>
        SimilarityCalculator.SimilarNoise(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients8000, VolumeFlowOnPolynomial),
            Data.ImpellerRotationSpeed,
            Size,
            ImpellerRotationSpeed,
            Size
        );

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности частоты 63Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA63 =>
        Math.Round(OctaveNoise63 + NoiseCorrectionA63, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 125Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA125 =>
        Math.Round(OctaveNoise125 + NoiseCorrectionA125, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 250Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA250 =>
        Math.Round(OctaveNoise250 + NoiseCorrectionA250, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 500Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA500 =>
        Math.Round(OctaveNoise500 + NoiseCorrectionA500, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 1000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA1000 =>
        Math.Round(OctaveNoise1000 + NoiseCorrectionA1000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 2000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA2000 =>
        Math.Round(OctaveNoise2000 + NoiseCorrectionA2000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 4000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA4000 =>
        Math.Round(OctaveNoise4000 + NoiseCorrectionA4000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 8000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA8000 => Math.Round(OctaveNoise8000 + NoiseCorrectionA8000, 1);

    /// <summary>
    ///     Суммарный уровень звуковой мощности частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseA
    {
        get
        {
            var arrayOctaveNoiseA = new List<double>
            {
                OctaveNoiseA63,
                OctaveNoiseA125,
                OctaveNoiseA250,
                OctaveNoiseA500,
                OctaveNoiseA1000,
                OctaveNoiseA2000,
                OctaveNoiseA4000,
                OctaveNoiseA8000
            };
            var sum = arrayOctaveNoiseA.Sum(
                singleOctave => Math.Pow(10, singleOctave / 10)
            );
            return Math.Round(10 * Math.Log10(sum), 1);
        }
    }
}

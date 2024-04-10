using Libraries.Description_of_objects;
using Libraries.Methods;
using System.Drawing;

namespace Libraries.Fans;

public interface IFanNoise
{
    public FanData Data { get; }
    public double ImpellerRotationSpeed  { get; }

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
    ///     Уровень звуковой мощности на частоте 63Гц
    /// </summary>
    public double OctaveNoise63 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients63,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 125Гц
    /// </summary>
    public double OctaveNoise125 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients125,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 250Гц
    /// </summary>
    public double OctaveNoise250 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients250,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 500Гц
    /// </summary>
    public double OctaveNoise500 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients500,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 1000Гц
    /// </summary>
    public double OctaveNoise1000 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients1000,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 2000Гц
    /// </summary>
    public double OctaveNoise2000 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients2000,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 4000Гц
    /// </summary>
    public double OctaveNoise4000 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients4000,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 8000Гц
    /// </summary>
    public double OctaveNoise8000 =>
        SimilarityCalculator.SimilarNoise(
            Calculate.Polynomial(
                Data.OctaveNoiseCoefficients8000,
                ((IFan) this).VolumeFlowOnPolynomial
            ),
            Data.ImpellerRotationSpeed,
            ((IFan) this).Size,
            ImpellerRotationSpeed,
            ((IFan) this).Size
        );

    //____________________________________________________________________________________________________________________________
    //Уровень шума по А

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
    public double OctaveNoiseA8000 =>
        Math.Round(OctaveNoise8000 + NoiseCorrectionA8000, 1);

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

            return Calculate.SumNoise(arrayOctaveNoiseA);
        }
    }
    //TODO хочу оптимизировать OctaveNoiseA и список arrayOctaveNoiseA

}

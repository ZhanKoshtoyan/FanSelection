using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Methods;

namespace Libraries.Fans;

public interface IFanCurves
{
    public FanData Data { get; }
    public double ImpellerRotationSpeed  { get; }

    /// <summary>
    /// Количество точек на новой кривой
    /// </summary>
    private const int CountArray = 8;

    /// <summary>
    /// Расчет рабочих точек для оригинальной кривой
    /// </summary>
    public IEnumerable<DataCurve> OriginalCurve => Enumerable.Range(0, CountArray).Select(i => i switch
        {
            0 => DataCurveCalculate(Data.MinVolumeFlow),
            CountArray-1 => DataCurveCalculate(Data.MaxVolumeFlow),
            _ => DataCurveCalculate((Data
                .MaxVolumeFlow - Data.MinVolumeFlow) / (CountArray-1) * i + Data.MinVolumeFlow)
        }
    ).ToArray();

    /// <summary>
    /// Расчет рабочих точек для новой кривой
    /// </summary>
    public DataCurve[] NewCurve => OriginalCurve.Select((workPoint, index) => new DataCurve
        {
            DcIndex = index,
            DcVolumeFlow = SimilarityCalculator.SimilarVolumeFlow(
                workPoint.DcVolumeFlow,
                workPoint.DcImpellerRotationSpeed,
                workPoint.DcSize,
                ImpellerRotationSpeed,
                ((IFan) this).Size),
            DcTotalPressure = SimilarityCalculator.SimilarPressure(
                workPoint.DcTotalPressure,
                workPoint.DcImpellerRotationSpeed,
                workPoint.DcSize,
                workPoint.DcAir,
                ImpellerRotationSpeed,
                ((IFan) this).Size,
                ((IFan) this).UserInputAir),
            DcSize = workPoint.DcSize,
            DcImpellerRotationSpeed = ImpellerRotationSpeed,
            DcAir = ((IFan) this).UserInputAir,
            DcPower = SimilarityCalculator.SimilarPower(
                workPoint.DcPower,
                workPoint.DcImpellerRotationSpeed,
                workPoint.DcSize,
                workPoint.DcAir,
                ImpellerRotationSpeed,
                ((IFan) this).Size,
                ((IFan) this).UserInputAir)
        }
    ).ToArray();

    /// <summary>
    /// Расчет основных параметров для расхода воздуха на OriginalCurve
    /// </summary>
    /// <param name="volumeFlow"></param>
    /// <returns></returns>
    private DataCurve DataCurveCalculate(double volumeFlow) =>
        new()
        {
            DcVolumeFlow = volumeFlow,
            DcTotalPressure = Calculate.Polynomial(
                Data.TotalPressureCoefficients,
                volumeFlow
            ),
            DcSize = ((IFan) this).Size,
            DcImpellerRotationSpeed = Data.ImpellerRotationSpeed,
            DcAir = FanData.AirInTests,
            DcPower = Calculate.Polynomial(
                Data.PowerCoefficients,
                volumeFlow
            )
        };
}

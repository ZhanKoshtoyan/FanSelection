using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Methods;

namespace Libraries.Fans;

public interface IFanCurves
{
    /// <summary>
    /// Количество точек на новой кривой
    /// </summary>
    private const int NumberOfDataCurves = 8;

    /// <summary>
    /// Расчет рабочих точек для оригинальной кривой
    /// </summary>
    public IEnumerable<DataCurve> OriginalCurve =>
        Calculate.CreateDataCurves(
            ((IFan)this).Data.MinVolumeFlow,
            ((IFan)this).Data.MaxVolumeFlow,
            NumberOfDataCurves,
            ((IFan)this).Data.TotalPressureQvCoefficients,
            ((IFan)this).ConditionalStandardSize,
            ((IFan)this).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ((IFan)this).Data.AirDensity,
            ((IFan)this).Data.PowerQvCoefficients,
            ((IFan)this).Data.SimilarVolumeFlowCoefficient,
            ((IFan)this).Data.SimilarTotalPressureCoefficient,
            ((IFan)this).Data.SimilarPowerCoefficient
        );

    /// <summary>
    /// Расчет рабочих точек для новой кривой
    /// </summary>
    public DataCurve[] NewCurve =>
        OriginalCurve
            .Select(
                (workPoint, index) =>
                    new DataCurve
                    {
                        DcIndex = index,
                        DcVolumeFlow = SimilarityCalculator.SimilarVolumeFlow(
                            workPoint.DcVolumeFlow,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            (
                                (IFan)this
                            ).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ((IFan)this).ConditionalStandardSize
                        ),
                        DcTotalPressure = SimilarityCalculator.SimilarPressure(
                            workPoint.DcTotalPressure,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            workPoint.DcAir,
                            (
                                (IFan)this
                            ).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ((IFan)this).ConditionalStandardSize,
                            ((IFan)this)
                                .UserInput
                                .DataAir
                                .Density
                                .KilogramsPerCubicMeter
                        ),
                        DcConditionalStandardSize =
                            workPoint.DcConditionalStandardSize,
                        DcImpellerRotationSpeed = (
                            (IFan)this
                        ).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                        DcAir = ((IFan)this)
                            .UserInput
                            .DataAir
                            .Density
                            .KilogramsPerCubicMeter,
                        DcPower = SimilarityCalculator.SimilarPower(
                            workPoint.DcPower,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcConditionalStandardSize,
                            workPoint.DcAir,
                            (
                                (IFan)this
                            ).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            ((IFan)this).ConditionalStandardSize,
                            ((IFan)this)
                                .UserInput
                                .DataAir
                                .Density
                                .KilogramsPerCubicMeter
                        )
                    }
            )
            .Select(
                item =>
                    item with
                    {
                        Efficiency = Calculate.Efficiency(
                            item.DcVolumeFlow,
                            item.DcTotalPressure,
                            item.DcPower
                        )
                    }
            )
            .ToArray();

    public DataCurve DataOriginalCurveWithMaxEfficiency =>
        OriginalCurve.MaxBy(
            dataCurve =>
                Calculate.Efficiency(
                    dataCurve.DcVolumeFlow,
                    dataCurve.DcTotalPressure,
                    dataCurve.DcPower
                )
        )
        ?? throw new InvalidOperationException(
            "Массив OriginalCurve не содержит ни одной DataCurve"
        );
}

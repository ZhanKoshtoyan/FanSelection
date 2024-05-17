using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Methods;
using SharpProp;

namespace Libraries.Fans;

public interface IFanCurves
{
    /// <summary>
    /// Количество точек на новой кривой
    /// </summary>
    private const int CountArray = 8;

    /// <summary>
    /// Расчет рабочих точек для оригинальной кривой
    /// </summary>
    public IEnumerable<DataCurve> OriginalCurve
    {
        get
        {
            var minVolumeFlow = ((IFan)this).Data.MinVolumeFlow;
            var maxVolumeFlow = ((IFan)this).Data.MaxVolumeFlow;
            var volumeFlowStep =
                (maxVolumeFlow - minVolumeFlow) / (CountArray - 1);

            for (var i = 0; i < CountArray; i++)
            {
                var volumeFlow = minVolumeFlow + volumeFlowStep * i;

                yield return Calculate.DataCurveCalculate(
                    volumeFlow,
                    ((IFan)this).Data.TotalPressureQvCoefficients,
                    ((IFan)this).Size,
                    ((IFan)this).ImpellerRotationSpeed,
                    ((IFan)this).Data.AirDensity,
                    ((IFan)this).Data.PowerQvCoefficients
                );
            }
        }
    }

    /*public IEnumerable<DataCurve> OriginalCurve =>
        Enumerable
            .Range(0, CountArray)
            .Select(
                i =>
                    i switch
                    {
                        0
                            => Calculate.DataCurveCalculate(
                                ((IFan)this).Data.MinVolumeFlow / ((IFan)this).UserInput.UserInputFan.NumberOfFans,
                                ((IFan)this).Data.TotalPressureQvCoefficients,
                                ((IFan)this).Size,
                                ((IFan)this).ImpellerRotationSpeed,
                                ((IFan)this).Data.PowerQvCoefficients
                            ),
                        CountArray - 1
                            => Calculate.DataCurveCalculate(
                                ((IFan)this).Data.MaxVolumeFlow / ((IFan)this).UserInput.UserInputFan.NumberOfFans,
                                ((IFan)this).Data.TotalPressureQvCoefficients,
                                ((IFan)this).Size,
                                ((IFan)this).ImpellerRotationSpeed,
                                ((IFan)this).Data.PowerQvCoefficients
                            ),
                        _
                            => Calculate.DataCurveCalculate(
                                (
                                    (
                                        ((IFan)this).Data.MaxVolumeFlow / ((IFan)this).UserInput.UserInputFan.NumberOfFans
                                        - ((IFan)this).Data.MinVolumeFlow / ((IFan)this).UserInput.UserInputFan.NumberOfFans
                                    )
                                        / (CountArray - 1)
                                        * i
                                    + ((IFan)this).Data.MinVolumeFlow / ((IFan)this).UserInput.UserInputFan.NumberOfFans
                                ),
                                ((IFan)this).Data.TotalPressureQvCoefficients,
                                ((IFan)this).Size,
                                ((IFan)this).ImpellerRotationSpeed,
                                ((IFan)this).Data.PowerQvCoefficients
                            )
                    }
            )
            .ToArray();*/

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
                            workPoint.DcSize,
                            ((IFan)this).ImpellerRotationSpeed,
                            ((IFan)this).Size
                        ),
                        DcTotalPressure = SimilarityCalculator.SimilarPressure(
                            workPoint.DcTotalPressure,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcSize,
                            workPoint.DcAir,
                            ((IFan)this).ImpellerRotationSpeed,
                            ((IFan)this).Size,
                            ((IFan)this)
                                .UserInput
                                .DataAir
                                .Density
                                .KilogramsPerCubicMeter
                        ),
                        DcSize = workPoint.DcSize,
                        DcImpellerRotationSpeed = (
                            (IFan)this
                        ).ImpellerRotationSpeed,
                        DcAir = ((IFan)this)
                            .UserInput
                            .DataAir
                            .Density
                            .KilogramsPerCubicMeter,
                        DcPower = SimilarityCalculator.SimilarPower(
                            workPoint.DcPower,
                            workPoint.DcImpellerRotationSpeed,
                            workPoint.DcSize,
                            workPoint.DcAir,
                            ((IFan)this).ImpellerRotationSpeed,
                            ((IFan)this).Size,
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

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using Microsoft.EntityFrameworkCore;

namespace Libraries.Fans;

public interface IFanNoise
{
    /// <summary>
    /// Список уровеней звуковой мощности Lw5 на входе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw5 =>
        CalcOctaveNoiseLw("Lw5");

    /// <summary>
    /// Список уровеней звуковой мощности Lw6 на выходе вентилятора по октавам
    /// </summary>
    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLw6 =>
        CalcOctaveNoiseLw("Lw6");

    private IEnumerable<(int Frequency, double Value)> CalcOctaveNoiseLw(
        string noiseType
    )
    {
        var octaveNoiseCoefficients = new List<PolynomialType?>();

        switch (noiseType)
        {
            case "Lw5":
                octaveNoiseCoefficients = new List<PolynomialType?>
                {
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients63,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients125,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients250,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients500,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients1000,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients2000,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients4000,
                    ((IFan)this).Data.OctaveNoiseLw5QvCoefficients8000
                };
                break;
            case "Lw6":
                octaveNoiseCoefficients = new List<PolynomialType?>
                {
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients63,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients125,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients250,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients500,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients1000,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients2000,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients4000,
                    ((IFan)this).Data.OctaveNoiseLw6QvCoefficients8000
                };
                break;
        }

        return octaveNoiseCoefficients
            .Select(
                (coefficients, frequency) =>
                    (
                        Frequency: OctaveNoise.Values[frequency],
                        Value: Calculate.MultipleFansNoise(
                            SimilarityCalculator.SimilarNoise(
                                Calculate.Polynomial(
                                    coefficients,
                                    ((IFan)this).VolumeFlowOnPolynomial
                                ),
                                ((IFan)this)
                                    .Data
                                    .ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                                ((IFan)this).ConditionalStandardSize,
                                (
                                    (IFan)this
                                ).ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                                ((IFan)this).ConditionalStandardSize
                            ),
                            ((IFan)this).NumberOfFans
                        )
                    )
            )
            .ToList();
    }

    //____________________________________________________________________________________________________________________________
    //Уровень шума по А

    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA5 =>
        CalcOctaveNoiseLwA(OctaveNoiseLw5);

    public IEnumerable<(int Frequency, double Value)> OctaveNoiseLwA6 =>
        CalcOctaveNoiseLwA(OctaveNoiseLw6);

    private static IEnumerable<(
        int Frequency,
        double Value
    )> CalcOctaveNoiseLwA(
        IEnumerable<(int Frequency, double Value)> octaveNoiseLw
    )
    {
        return octaveNoiseLw
            .Select(
                (item, count) =>
                    item with
                    {
                        Value = Math.Round(
                            item.Value + OctaveNoiseCorrectionA.Values[count],
                            1
                        )
                    }
            )
            .ToList();
    }

    /*     Конструкция "item with" в C# используется для создания нового объекта на основе существующего
             объекта, но с измененными значениями определенных свойств. В данном контексте, она применяется в
             методе доступа get для свойства OctaveNoiseA.
                   Когда мы используем "item with", мы фактически создаем копию объекта "item" из коллекции
             OctaveNoise, но при этом изменяем значение свойства "Value". В данном случае, значение "Value"
             увеличивается на значение из списка noiseCorrectionA по соответствующему индексу.
                  Эта конструкция позволяет нам элегантно изменять свойства объекта без явного создания нового
             экземпляра и копирования всех свойств по отдельности. Она удобна в случаях, когда требуется изменить
             только некоторые свойства объекта, оставляя остальные без изменений.*/

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA5 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA5 => Calculate.SumNoise(OctaveNoiseLwA5);

    /// <summary>
    ///     Суммарный уровень звуковой мощности LwA6 частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseLwA6 => Calculate.SumNoise(OctaveNoiseLwA6);
}

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public interface IFanNoise
{
    /// <summary>
    /// Список уровеней звуковой мощности на выходе вентилятора по октавам
    /// </summary>
    public List<(int Frequency, double Value)> OctaveNoise
    {
        get
        {
            var octaveNoiseCoefficients = new List<PolynomialType>
            {
                ((IFan)this).Data.OctaveNoiseQvCoefficients63,
                ((IFan)this).Data.OctaveNoiseQvCoefficients125,
                ((IFan)this).Data.OctaveNoiseQvCoefficients250,
                ((IFan)this).Data.OctaveNoiseQvCoefficients500,
                ((IFan)this).Data.OctaveNoiseQvCoefficients1000,
                ((IFan)this).Data.OctaveNoiseQvCoefficients2000,
                ((IFan)this).Data.OctaveNoiseQvCoefficients4000,
                ((IFan)this).Data.OctaveNoiseQvCoefficients8000
            };

            return octaveNoiseCoefficients
                .Select(
                    (coefficients, frequency) =>
                        (
                            Frequency: DescriptionOfObjects
                                .Parameters
                                .OctaveNoise
                                .Values[frequency],
                            Value: SimilarityCalculator.SimilarNoise(
                                Calculate.Polynomial(
                                    coefficients,
                                    ((IFan)this).VolumeFlowOnPolynomial
                                ),
                                ((IFan)this).Data.ImpellerRotationSpeed,
                                ((IFan)this).Size,
                                ((IFan)this).ImpellerRotationSpeed,
                                ((IFan)this).Size
                            )
                        )
                )
                .ToList();
        }
    }

    //____________________________________________________________________________________________________________________________
    //Уровень шума по А

    public List<(int Frequency, double Value)> OctaveNoiseA =>
        OctaveNoise
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
    ///     Суммарный уровень звуковой мощности частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseA => Calculate.SumNoise(OctaveNoiseA);
}

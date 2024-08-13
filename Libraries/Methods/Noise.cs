using Libraries.DescriptionOfObjects.Parameters;
using Libraries.StructureOfObjects;

namespace Libraries.Methods;

public static class Noise
{
    public static IEnumerable<(int Frequency, double Value)> CalcOctaveNoiseLw(
        string noiseType,
        FanData data,
        double volumeFlowOnPolynomial,
        double conditionalStandardSize,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint,
        int numberOfFans
    )
    {
        var octaveNoiseCoefficients = new List<PolynomialType?>();

        switch (noiseType)
        {
            case "Lw5":
                octaveNoiseCoefficients = new List<PolynomialType?>
                {
                    data.OctaveNoiseLw5QvCoefficients63,
                    data.OctaveNoiseLw5QvCoefficients125,
                    data.OctaveNoiseLw5QvCoefficients250,
                    data.OctaveNoiseLw5QvCoefficients500,
                    data.OctaveNoiseLw5QvCoefficients1000,
                    data.OctaveNoiseLw5QvCoefficients2000,
                    data.OctaveNoiseLw5QvCoefficients4000,
                    data.OctaveNoiseLw5QvCoefficients8000
                };
                break;
            case "Lw6":
                octaveNoiseCoefficients = new List<PolynomialType?>
                {
                    data.OctaveNoiseLw6QvCoefficients63,
                    data.OctaveNoiseLw6QvCoefficients125,
                    data.OctaveNoiseLw6QvCoefficients250,
                    data.OctaveNoiseLw6QvCoefficients500,
                    data.OctaveNoiseLw6QvCoefficients1000,
                    data.OctaveNoiseLw6QvCoefficients2000,
                    data.OctaveNoiseLw6QvCoefficients4000,
                    data.OctaveNoiseLw6QvCoefficients8000
                };
                break;
        }

        return octaveNoiseCoefficients
            .Select(
                (coefficients, frequency) =>
                    (
                        Frequency: OctaveNoise.Values[frequency],
                        Value: Calculate.MultipleFansNoise(
                            Similarity.SimilarNoise(
                                Calculate.Polynomial(
                                    coefficients,
                                    volumeFlowOnPolynomial
                                ),
                                data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                                conditionalStandardSize,
                                impellerRotationSpeedWithSlidingEngineForWorkPoint,
                                conditionalStandardSize
                            ),
                            numberOfFans
                        )
                    )
            )
            .ToList();
    }

    public static IEnumerable<(int Frequency, double Value)> CalcOctaveNoiseLwA(
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
}

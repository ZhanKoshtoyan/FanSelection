using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithFrequencyConverter : AbstractFan
{
    protected FanWithFrequencyConverter(FanData data, UserInput userInput)
        : base(data, userInput)
    {
        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Similarity.DerivedFromPvImpellerRotationSpeed(
                TotalPressureOnPolynomial,
                Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                ConditionalStandardSize,
                Data.AirDensity,
                UserInput.UserInputWorkPoint.TotalPressure,
                ConditionalStandardSize,
                UserInput.DataAir.Density.KilogramsPerCubicMeter
            );

        MinImpellerRotationFrequency = 35;
    }
    //TODO Проверить присвоение при инициализации и при переназначении!!!
}

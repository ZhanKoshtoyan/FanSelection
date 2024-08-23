using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithFrequencyConverter : AbstractFan
{
    protected FanWithFrequencyConverter(
        FanData data,
        UserInput userInput,
        int numberOfFans
    )
        : base(data, userInput, numberOfFans)
    {
        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Similarity.DerivedFromPvImpellerRotationSpeed(
                TotalPressureOnPolynomial,
                Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.OriginalFanDataConditionalStandardSize,
                Data.OriginalFanDataAirDensity,
                InputTotalNormalPressure,
                ConditionalStandardSize,
                Data.AirDensity
            );

        MinImpellerRotationFrequency = 35;
    }
}

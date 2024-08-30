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
                Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
                Data.OriginalFanDataAirDensity,
                InputTotalNormalPressure,
                Data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                Data.AirDensity
            );

        MinImpellerRotationFrequency = 35;
    }
}

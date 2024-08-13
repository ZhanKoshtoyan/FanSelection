using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithoutFrequencyConverter : AbstractFan
{
    protected FanWithoutFrequencyConverter(
        FanData data,
        UserInput userInput,
        int numberOfFans
    )
        : base(data, userInput, numberOfFans)
    {
        MinImpellerRotationFrequency = Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            Data.NominalImpellerRotationSpeedWithoutSlidingEngine
        );

        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint;
    }
}

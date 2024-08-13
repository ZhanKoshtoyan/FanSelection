using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithoutFrequencyConverter : AbstractFan
{
    protected FanWithoutFrequencyConverter(FanData data, UserInput userInput)
        : base(data, userInput)
    {
        MinImpellerRotationFrequency = Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            Data.NominalImpellerRotationSpeedWithoutSlidingEngine
        );

        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint;
    }
}

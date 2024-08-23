using Libraries.DescriptionOfObjects.UserInput;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class HighPressureFan : FanWithoutFrequencyConverter
{
    public HighPressureFan(
        FanData data,
        UserInput userInput,
        int numberOfFans = 1
    )
        : base(data, userInput, numberOfFans) =>
        ProjectId =
            $"ВВД.{FanOperatingMaxTemperature:000}.{Math.Round(ConditionalStandardSize * 100, 0,
                MidpointRounding.ToNegativeInfinity):000}.{FanBodyLength}.{ImpellerRotationDirection}.{
                NominalPower * 100:0000}.{NominalImpellerRotationSpeedWithoutSlidingEngine:0000}.{FanBodyExecutionMaterial}.Y2";
}

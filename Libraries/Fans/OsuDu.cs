using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class OsuDu : FanWithoutFrequencyConverter
{
    public OsuDu(FanData data, UserInput userInput)
        : base(data, userInput) =>
        ProjectId =
            $"ОСУ-ДУ.{FanOperatingMaxTemperature:000}.{Math.Round(ConditionalStandardSize * 100, 0,
                MidpointRounding.ToNegativeInfinity):000}.{FanBodyLength}.{ImpellerRotationDirection}.{
                NominalPower * 100:0000}.{NominalImpellerRotationSpeedWithoutSlidingEngine:0000}.{FanBodyExecutionMaterial}.Y2";
}

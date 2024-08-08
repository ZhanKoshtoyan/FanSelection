using Libraries.DescriptionOfObjects.UserInput;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class EuFan : FanWithFrequencyConverter
{
    public EuFan(FanData data, UserInput userInput)
        : base(data, userInput)
    {
        Data = data;
        UserInput = userInput;
        ProjectId =
            $"ЕУ.{((IFan)this).FanOperatingMaxTemperature:000}.{Math.Round(((IFan)this).ConditionalStandardSize * 100, 0,MidpointRounding.ToNegativeInfinity):000}.{((IFan)this)
            .FanBodyLength}.{((IFan)this).ImpellerRotationDirection}.{(
            (IFan)this).NominalPower * 100:0000}.{((IFan)this).NominalImpellerRotationSpeedWithoutSlidingEngine:0000}.{((IFan)this).FanBodyExecutionMaterial}.Y2";
    }
}

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class OsuDu : FanWithoutFrequencyConverter
{
    public OsuDu(FanData data, UserInput userInput)
        : base(data, userInput)
    {
        Data = data;
        UserInput = userInput;
        ProjectId =
            $"ОСУ-ДУ.{((IFan)this).FanOperatingMaxTemperature}.{((IFan) this).Size * 100:000}.{((IFan)this).FanBodyLength}.{((IFan)this).ImpellerRotationDirection}.{
                ((IFan) this).NominalPower * 100:0000}.{((IFan) this).NominalImpellerRotationSpeed:0000}.{((IFan)this).CaseExecutionMaterial}.Y2";
    }
}

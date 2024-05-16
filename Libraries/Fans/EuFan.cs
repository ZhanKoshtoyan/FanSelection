using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using System.Runtime.CompilerServices;

namespace Libraries.Fans;

public class EuFan : FanWithFrequencyConverter
{
    public EuFan(FanData data, UserInput userInput): base(data, userInput)
    {
        Data = data;
        UserInput = userInput;
        ProjectId = $"ЕУ.{((IFan)this).FanOperatingMaxTemperature}.{((IFan)this).Size * 100:000}.{((IFan)this).FanBodyLength}.{((IFan)this).ImpellerRotationDirection}.{(
            (IFan)this).NominalPower * 100:0000}.{((IFan)this).NominalImpellerRotationSpeed:0000}.{((IFan)this).CaseExecutionMaterial}.Y2";
    }
}

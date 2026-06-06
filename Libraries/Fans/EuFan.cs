using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class EuFan : FanWithFrequencyConverter
{
    public EuFan(FanData data, UserInput userInput, BladeType bladeType, BladeOrientation bladeOrientation, int numberOfFans = 1)
        : base(data, userInput, bladeType,  bladeOrientation,  numberOfFans)
    {
        /*Радиальное колесо с назад загнутыми лопатками
         Вроде бы постоянная работа с ПЧ*/

        ProjectId =
            $"ЕУ.{FanOperatingMaxTemperature:000}.{Math.Round(ConditionalStandardSize * 100, 0,
                MidpointRounding.ToNegativeInfinity):000}.{FanBodyLength}.{ImpellerRotationDirection
            }.{NominalPower * 100:0000}.{NominalImpellerRotationSpeedWithoutSlidingEngine:0000}.{FanBodyExecutionMaterial}.Y2";
    }

    public sealed override string? ProjectId { get; init; }
}
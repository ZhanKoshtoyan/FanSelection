using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class OsuDu : FanWithoutFrequencyConverter
{
    public OsuDu(
        FanData data,
        UserInput userInput,
        BladeType bladeType,
        BladeOrientation bladeOrientation,
        int numberOfFans = 1)
        : base(data, userInput, bladeType, bladeOrientation, numberOfFans)
    {
        /*Осевое колесо
         Работа без ПЧ*/
        ProjectId =
            $"ОСУ-ДУ.{FanOperatingMaxTemperature:000}.{Math.Round(ConditionalStandardSize * 100, 0,
                MidpointRounding.ToNegativeInfinity):000}.{FanBodyLength}.{ImpellerRotationDirection}.{
                NominalPower * 100:0000}.{NominalImpellerRotationSpeedWithoutSlidingEngine:0000}.{FanBodyExecutionMaterial}.Y2";

        /*EfficiencyGradeCoefficient = Calculate.PowerScaleEffectByFanEfficiencyGradeAsync(
            data.FanEfficiencyGradeList,
            data.OriginalFanDataEfficiencyMax,
            data.OriginalFanDataConditionalStandardSize,
            data.ConditionalStandardSize
        );*/
    }

    public sealed override string? ProjectId { get; init; }
}
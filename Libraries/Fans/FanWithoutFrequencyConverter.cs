using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithoutFrequencyConverter : AbstractFan
{
    protected FanWithoutFrequencyConverter(
        FanData data,
        UserInput userInput,
        BladeType bladeType,
        BladeOrientation bladeOrientation,
        int numberOfFans
    )
        : base(data, userInput, bladeType, bladeOrientation, numberOfFans)
    {
        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint;

        MinImpellerRotationFrequency = Calculate.ImpellerRotationFrequency(
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            Data.NominalImpellerRotationSpeedWithoutSlidingEngine
        );

        TotalEfficiency = Calculate.Efficiency(VolumeFlow, TotalPressure, Power);
    }

    public sealed override double ImpellerRotationSpeedWithSlidingEngineForWorkPoint { get; init; }

    public sealed override double MinImpellerRotationFrequency { get; protected init; }

    public sealed override double TotalEfficiency { get; init; }

    //public override string? ProjectId { get; init; }
}
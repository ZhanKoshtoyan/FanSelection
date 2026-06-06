using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public abstract class FanWithFrequencyConverter : AbstractFan
{
    protected FanWithFrequencyConverter(
        FanData data,
        UserInput userInput,
        BladeType bladeType,
        BladeOrientation bladeOrientation,
        int numberOfFans
    )
        : base(data, userInput, bladeType, bladeOrientation, numberOfFans)
    {
        ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
            Similarity.DerivedFromPvImpellerRotationSpeed(
                TotalPressureOnPolynomial,
                Data.OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                Data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
                Data.OriginalFanDataAirDensity,
                InputTotalNormalPressure,
                Data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                Data.AirDensity
            );

        MinImpellerRotationFrequency = 35;

        TotalEfficiency = Calculate.Efficiency(VolumeFlow, TotalPressure, Power) *
        Calculate.CompensationFactorForFanWithWithFrequencyConverter(Power);
    }

    public sealed override double ImpellerRotationSpeedWithSlidingEngineForWorkPoint { get; init; }

    public sealed override double MinImpellerRotationFrequency { get; protected init; }

    public sealed override double TotalEfficiency { get; init; }

    //public override string? ProjectId { get; init; }
}
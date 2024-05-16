using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class FanWithFrequencyConverter : IFan
{
    protected FanWithFrequencyConverter(FanData data, UserInput userInput)
    {
        Data = data;
        UserInput = userInput;
    }

    public FanData Data { get; protected init; }
    public UserInput UserInput { get; protected init; }
    public string? ProjectId { get; protected init; }

    double IFan.MinImpellerRotationFrequency => 35;

    double IFan.ImpellerRotationSpeed => SimilarityCalculator.DerivedFromPvImpellerRotationSpeed(
        ((IFan) this).TotalPressureOnPolynomial,
        Data.ImpellerRotationSpeed,
        ((IFan) this).Size,
        FanData.AirInTests,
        UserInput.UserInputWorkPoint.TotalPressure,
        ((IFan) this).Size,
        UserInput.DataAir
    );

    /*double IFan.ImpellerRotationSpeed
    {
        get
        {
            return _correctFanLogic switch
            {
                FanLogic.Values.Logic1
                    => SimilarityCalculator.DerivedFromPvImpellerRotationSpeed(
                        ((IFan)this).TotalPressureOnPolynomial,
                        Data.ImpellerRotationSpeed,
                        ((IFan)this).Size,
                        FanData.AirInTests,
                        UserInput.UserInputWorkPoint.TotalPressure,
                        ((IFan)this).Size,
                        UserInput.DataAir
                    ),
                FanLogic.Values.Logic2
                    => DimensionlessData.CalculatedSizeOrImpellerRotationSpeed(
                        ((IFan)this).Size,
                        UserInput.InputTotalNormalPressure,
                        (
                            (IFanDimensionlessData)this
                        ).TotalPressureCoefficientUserInput,
                        FanData.AirInTests
                    ),
                FanLogic.Values.Logic3
                    => DimensionlessData.CalculatedSizeOrImpellerRotationSpeed(
                        ((IFan)this).UserInput.UserInputFan.RequiredSize / 1000,
                        UserInput.InputTotalNormalPressure,
                        (
                            (IFanDimensionlessData)this
                        ).TotalPressureCoefficientUserInput,
                        FanData.AirInTests
                    ),
                _
                    => throw new InvalidOperationException(
                        "Invalid FanLogic value."
                    )
            };
        }
    }*/
}

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.Fans;

public class FanWithoutFrequencyConverter : IFan
{
    protected FanWithoutFrequencyConverter(FanData data, UserInput userInput)
    {
        Data = data;
        UserInput = userInput;
    }

    public FanData Data { get; protected init; }
    public UserInput UserInput { get; protected init; }
    public string? ProjectId { get; protected init; }

    double IFan.MinImpellerRotationFrequency =>
        Calculate.ImpellerRotationFrequency(
            Data.ImpellerRotationSpeed,
            Data.NominalImpellerRotationSpeed
        );

    double IFan.ImpellerRotationSpeed => Data.ImpellerRotationSpeed;
}

using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;
using Libraries.Methods;

namespace Libraries.Fans;

public interface IFanSpeed : IFan
{
    public double TotalNormalPressure =>
        SimilarityCalculator.SimilarPressure(
            UserInput.UserInputWorkPoint.TotalPressure,
            1,
            1,
            UserInputAir,
            1,
            1,
            FanData.AirInTests
        );

    public List<double> SizeCoefficientForInput =>
        ImpellerRotationSpeeds.Values
            .Select(
                speed =>
                    DimensionlessData.SpeedCoefficient(
                        speed,
                        UserInput.UserInputWorkPoint.VolumeFlow,
                        TotalNormalPressure
                    )
            )
            .ToList();
}

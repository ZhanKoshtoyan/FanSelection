using SharpProp;

namespace Libraries.Description_of_objects.Parameters;

public record DataCurve
{
    public required double VolumeFlow { get; init; }
    public required double TotalPressure { get; init; }
    public required double StaticPressure { get; init; }
    public required double DynamicPressure { get; init; }
    public required int Size { get; init; }
    public required int ImpellerRotationSpeed { get; init; }
    public required IHumidAir Air { get; init; }
    public required double TotalEfficiency { get; init; }
    public required double StaticEfficiency { get; init; }
    public required double Power { get; init; }
    public required double ConstDependencePq { get; init; }
}

using SharpProp;

namespace Libraries.DescriptionOfObjects.Parameters;

public record DataCurve
{
    public int DcIndex { get; init; }
    public required double DcVolumeFlow { get; init; }
    public required double DcTotalPressure { get; init; }
    public required double DcSize { get; init; }
    public required double DcImpellerRotationSpeed { get; init; }
    public required IHumidAir DcAir { get; init; }
    public required double DcPower { get; init; }

}

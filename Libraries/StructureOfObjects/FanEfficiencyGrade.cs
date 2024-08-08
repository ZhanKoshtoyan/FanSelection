using System.Text.Json.Serialization;

namespace Libraries.StructureOfObjects;

public record FanEfficiencyGrade
{
    public string Name { get; init; } = null!;

    public List<double> Values { get; init; } = null!;
}

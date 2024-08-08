using System.Collections.Generic;

namespace WpfApp1.Models;

public record FanParameterValuesForProjectId
{
    public required string FanName { get; init; } = null!;
    public required List<double> FanOperatingMaxTemperatureList { get; init; } =
        null!;
    private readonly List<double> _fanSizeList = new();
    public required List<double> FanSizeList
    {
        get => _fanSizeList;
        init
        {
            _fanSizeList.Add(0);
            _fanSizeList.AddRange(value);
        }
    }
    public required List<string> FanBodyLengthList { get; init; } = null!;
    public required List<string> ImpellerRotationDirectionList { get; init; } =
        null!;
    private readonly List<double> _nominalPowerList = new();
    public required List<double> NominalPowerList
    {
        get => _nominalPowerList;
        init
        {
            _nominalPowerList.Add(0);
            _nominalPowerList.AddRange(value);
        }
    }
    private readonly List<double> _nominalImpellerRotationSpeedList = new();
    public required List<double> NominalImpellerRotationSpeedList
    {
        get => _nominalImpellerRotationSpeedList;
        init
        {
            _nominalImpellerRotationSpeedList.Add(0);
            _nominalImpellerRotationSpeedList.AddRange(value);
        }
    }
    public required List<string> FanBodyExecutionMaterialList { get; init; } =
        null!;
}

using System.Collections.Generic;

namespace WpfApp1.Models;

public record FanParameterNamesForComboBoxes
{
    public required string FanName { get; init; } = null!;
    public required List<string> FanOperatingMaxTemperatureList { get; init; } =
        null!;

    private readonly List<string> _fanSizeList = new();
    public required List<string> FanSizeList
    {
        get => _fanSizeList;
        init
        {
            _fanSizeList.Add("");
            _fanSizeList.AddRange(value);
        }
    }

    public required List<string> FanBodyLengthList { get; init; } = null!;
    public required List<string> ImpellerRotationDirectionList { get; init; } =
        null!;
    private readonly List<string> _nominalPowerList = new();
    public required List<string> NominalPowerList
    {
        get => _nominalPowerList;
        init
        {
            _nominalPowerList.Add("");
            _nominalPowerList.AddRange(value);
        }
    }
    private readonly List<string> _nominalImpellerRotationSpeedList = new();
    public required List<string> NominalImpellerRotationSpeedList
    {
        get => _nominalImpellerRotationSpeedList;
        init
        {
            _nominalImpellerRotationSpeedList.Add("");
            _nominalImpellerRotationSpeedList.AddRange(value);
        }
    }
    public required List<string> FanBodyExecutionMaterialList { get; init; } =
        null!;
}

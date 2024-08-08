using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            Data.MaxImpellerRotationSpeedWithSlidingEngine,
            Data.NominalImpellerRotationSpeedWithoutSlidingEngine
        );

    double IFan.ImpellerRotationSpeedWithSlidingEngineForWorkPoint =>
        Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null
    )
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );
    }

    protected bool SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null
    )
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

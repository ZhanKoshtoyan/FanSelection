using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

    public int NumberOfFans { get; set; } = 1;

    //TODO Проверить присвоение при инициализации и при переназначении!!!

    double IFan.MinImpellerRotationFrequency => 35;

    double IFan.ImpellerRotationSpeedWithSlidingEngineForWorkPoint =>
        SimilarityCalculator.DerivedFromPvImpellerRotationSpeed(
            ((IFan)this).TotalPressureOnPolynomial,
            Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ((IFan)this).ConditionalStandardSize,
            ((IFan)this).Data.AirDensity,
            UserInput.UserInputWorkPoint.TotalPressure,
            ((IFan)this).ConditionalStandardSize,
            UserInput.DataAir.Density.KilogramsPerCubicMeter
        );

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

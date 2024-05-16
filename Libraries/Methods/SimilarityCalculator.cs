using SharpProp;

namespace Libraries.Methods;

public static class SimilarityCalculator
{
    public static double SimilarVolumeFlow(
        double oldVolumeFlow,
        double oldImpellerRotationSpeed,
        double oldSize,
        double newImpellerRotationSpeed,
        double newSize
    ) =>
        oldVolumeFlow
        * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 1)
        * Math.Pow(newSize / oldSize, 3);

    public static double DerivedFromQImpellerRotationSpeed(
        double oldVolumeFlow,
        double oldImpellerRotationSpeed,
        double oldSize,
        double newVolumeFlow,
        double newSize
    ) =>
        Math.Pow(newVolumeFlow / oldVolumeFlow, 1)
        * oldImpellerRotationSpeed
        * Math.Pow(oldSize / newSize, 3);

    public static double DerivedFromPvImpellerRotationSpeed(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newPressure,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        oldImpellerRotationSpeed
        * Math.Pow(newPressure / oldPressure, 0.5)
        * Math.Pow(oldSize / newSize, 1)
        * Math.Pow(oldAirDensity.Density.KilogramsPerCubicMeter / newAirDensity.Density.KilogramsPerCubicMeter, 0.5);

    public static double SimilarPower(
        double oldPower,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        oldPower
        * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 3)
        * Math.Pow(newSize / oldSize, 5)
        * Math.Pow(newAirDensity.Density.KilogramsPerCubicMeter / oldAirDensity.Density.KilogramsPerCubicMeter, 1);

    public static double SimilarPressure(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        oldPressure
        * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 2)
        * Math.Pow(newSize / oldSize, 2)
        * Math.Pow(newAirDensity.Density.KilogramsPerCubicMeter / oldAirDensity.Density.KilogramsPerCubicMeter, 1);

    public static double SimilarNoise(
        double oldNoise,
        double oldImpellerRotationSpeed,
        double oldSize,
        double newImpellerRotationSpeed,
        double newSize
    ) =>
        oldNoise
        + 50 * Math.Log10(newImpellerRotationSpeed / oldImpellerRotationSpeed)
        + 70 * Math.Log10(newSize / oldSize);
}

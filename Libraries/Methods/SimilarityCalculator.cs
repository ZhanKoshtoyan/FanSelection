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
        Math.Round(
            oldVolumeFlow
            * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 1)
            * Math.Pow(newSize / oldSize, 3),
            0
        );

    public static double DerivedFromQImpellerRotationSpeed(
        double oldVolumeFlow,
        double oldImpellerRotationSpeed,
        double oldSize,
        double newVolumeFlow,
        double newSize
    ) =>
        Math.Round(
            Math.Pow(newVolumeFlow / oldVolumeFlow, 1)
            * oldImpellerRotationSpeed
            * Math.Pow(oldSize / newSize, 3),
            0
        );

    public static double DerivedFromPvImpellerRotationSpeed(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newPressure,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        Math.Round(
            oldImpellerRotationSpeed
            *
            Math.Pow(newPressure / oldPressure, 0.5)
            *
            Math.Pow(oldSize / newSize, 1)
            *
            Math.Pow(oldAirDensity.Density / newAirDensity.Density, 0.5),
            0
        );

    public static double SimilarPower(
        double oldPower,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        Math.Round(
            oldPower
            * Math.Pow(
                newImpellerRotationSpeed / oldImpellerRotationSpeed,
                3
            )
            * Math.Pow(newSize / oldSize, 5)
            * Math.Pow(newAirDensity.Density / oldAirDensity.Density, 1),
            2
        );

    public static double SimilarPressure(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        IHumidAir oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        IHumidAir newAirDensity
    ) =>
        Math.Round(
            oldPressure
            * Math.Pow(
                newImpellerRotationSpeed / oldImpellerRotationSpeed,
                2
            )
            * Math.Pow(newSize / oldSize, 2)
            * Math.Pow(newAirDensity.Density / oldAirDensity.Density, 1),
            0
        );

    public static double SimilarNoise(
        double oldNoise,
        double oldImpellerRotationSpeed,
        double oldSize,
        double newImpellerRotationSpeed,
        double newSize
    ) =>
        Math.Round(
            oldNoise +
            50 * Math.Log10(
                newImpellerRotationSpeed / oldImpellerRotationSpeed
            )
            + 70 * Math.Log10(newSize / oldSize),
            2
        );
}

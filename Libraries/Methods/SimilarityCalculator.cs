using SharpProp;

namespace Libraries.Methods;

public static class SimilarityCalculator
{
    public static double SimilarVolumeFlow(
        double oldVolumeFlow,
        int oldImpellerRotationSpeed,
        int oldSize,
        int newImpellerRotationSpeed,
        int newSize
    ) =>
        Math.Round(
            oldVolumeFlow
            * Math.Pow((double) newImpellerRotationSpeed / oldImpellerRotationSpeed, 1)
            * Math.Pow((double) newSize / oldSize, 3),
            0
        );

    public static double SimilarImpellerRotationSpeed(
        double oldVolumeFlow,
        int oldImpellerRotationSpeed,
        int oldSize,
        double newVolumeFlow,
        int newSize
    ) =>
        Math.Round(
            Math.Pow(newVolumeFlow / oldVolumeFlow, 1)
            * oldImpellerRotationSpeed
            * Math.Pow((double) oldSize / newSize, 3),
            0
        );

    public static double SimilarPower(
        double oldPower,
        int oldImpellerRotationSpeed,
        int oldSize,
        IHumidAir oldAirDensity,
        int newImpellerRotationSpeed,
        int newSize,
        IHumidAir newAirDensity
    ) =>
        Math.Round(
            oldPower
            * Math.Pow(
                (double) newImpellerRotationSpeed / oldImpellerRotationSpeed,
                3
            )
            * Math.Pow((double) newSize / oldSize, 5)
            * Math.Pow(newAirDensity.Density / oldAirDensity.Density, 1),
            2
        );

    public static double SimilarPressure(
        double oldPressure,
        int oldImpellerRotationSpeed,
        int oldSize,
        IHumidAir oldAirDensity,
        int newImpellerRotationSpeed,
        int newSize,
        IHumidAir newAirDensity
    ) =>
        Math.Round(
            oldPressure
            * Math.Pow(
                (double) newImpellerRotationSpeed / oldImpellerRotationSpeed,
                2
            )
            * Math.Pow((double) newSize / oldSize, 2)
            * Math.Pow(newAirDensity.Density / oldAirDensity.Density, 1),
            0
        );

    public static double SimilarNoise(
        double oldNoise,
        int oldImpellerRotationSpeed,
        int oldSize,
        int newImpellerRotationSpeed,
        int newSize
    ) =>
        Math.Round(
            oldNoise +
            50 * Math.Log10(
                (double) newImpellerRotationSpeed / oldImpellerRotationSpeed
            )
            + 70 * Math.Log10((double) newSize / oldSize),
            1
        );
}

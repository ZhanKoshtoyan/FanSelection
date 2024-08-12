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

    public static double DerivedFromPvImpellerRotationSpeed(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        double oldAirDensity,
        double newPressure,
        double newSize,
        double newAirDensity
    ) =>
        oldImpellerRotationSpeed
        * Math.Pow(newPressure / oldPressure, 0.5)
        * Math.Pow(oldSize / newSize, 1)
        * Math.Pow(oldAirDensity / newAirDensity, 0.5);

    public static double SimilarPower(
        double oldPower,
        double oldImpellerRotationSpeed,
        double oldSize,
        double oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        double newAirDensity
    ) =>
        oldPower
        * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 3)
        * Math.Pow(newSize / oldSize, 5)
        * Math.Pow(newAirDensity / oldAirDensity, 1);

    public static double SimilarPressure(
        double oldPressure,
        double oldImpellerRotationSpeed,
        double oldSize,
        double oldAirDensity,
        double newImpellerRotationSpeed,
        double newSize,
        double newAirDensity
    ) =>
        oldPressure
        * Math.Pow(newImpellerRotationSpeed / oldImpellerRotationSpeed, 2)
        * Math.Pow(newSize / oldSize, 2)
        * Math.Pow(newAirDensity / oldAirDensity, 1);

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

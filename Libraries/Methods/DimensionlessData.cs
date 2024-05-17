using SharpProp;

namespace Libraries.Methods;

public static class DimensionlessData
{
    private const double AccelerationOfFreeFall = 9.80665;

    /// <summary>
    /// Oкружная скорость по концам лопаток [м/с] {size = м; impellerRotationSpeed = об/мин}
    /// </summary>
    /// <param name="size"></param>
    /// <param name="impellerRotationSpeed"></param>
    /// <returns></returns>
    public static double CircumferentialSpeed(
        double size,
        double impellerRotationSpeed
    ) => Math.PI * size * impellerRotationSpeed / 60;

    /// <summary>
    /// Площадь диска колеса по концам лопаток [м2] {size = м}
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static double AreaOfWheelDisc(double size) =>
        Math.PI * Math.Pow(size, 2) / 4;

    /// <summary>
    /// Коэффициент производительности φ [б/р] {volumeFlow = м3/с; areaOfWheelDisc = м2; circumferentialSpeed = м/с}
    /// </summary>
    /// <param name="volumeFlow"></param>
    /// <param name="areaOfWheelDisc"></param>
    /// <param name="circumferentialSpeed"></param>
    /// <returns></returns>
    public static double PerformanceCoefficient(
        double volumeFlow,
        double areaOfWheelDisc,
        double circumferentialSpeed
    ) => volumeFlow / (3600 * areaOfWheelDisc * circumferentialSpeed);

    public static double PressureCoefficient(
        double pressure,
        double airDensity,
        double circumferentialSpeed
    ) =>
        2
        * pressure
        / (airDensity * circumferentialSpeed);

    public static double PressureCoefficient(
        double pressure,
        double airDensity,
        double circumferentialSpeed,
        double compressibilityFactor
    ) =>
        2
        * pressure
        * compressibilityFactor
        / (airDensity * circumferentialSpeed);

    public static double PowerCoefficient(
        double power,
        double airDensity,
        double circumferentialSpeed,
        double areaOfWheelDisc
    ) =>
        2
        * power
        / (
            airDensity
            * Math.Pow(circumferentialSpeed, 3)
            * areaOfWheelDisc
        );

    public static double SpeedCoefficient(
        double performanceCoefficient,
        double totalPressureCoefficient
    ) =>
        137.58573
        * Math.Pow(performanceCoefficient, 0.5)
        * Math.Pow(totalPressureCoefficient, -0.75);

    public static double SpeedCoefficient(
        double impellerRotationSpeed,
        double volumeFlow,
        double totalNormalPressure
    ) =>
        impellerRotationSpeed
        * Math.Pow(volumeFlow / 3600, 0.5)
        * Math.Pow(totalNormalPressure / AccelerationOfFreeFall, -0.75);

    public static double SizeCoefficient(
        double performanceCoefficient,
        double totalPressureCoefficient
    ) =>
        0.56128879
        * Math.Pow(performanceCoefficient, -0.5)
        * Math.Pow(totalPressureCoefficient, 0.25);

    public static double SizeCoefficient(
        double size,
        double volumeFlow,
        double totalNormalPressure
    ) =>
        size / 1000
        * Math.Pow(volumeFlow / 3600, -0.5)
        * Math.Pow(totalNormalPressure / AccelerationOfFreeFall, 0.25);

    public static double CalculatedSizeOrImpellerRotationSpeed(
        double sizeOrImpellerRotationSpeed,
        double totalNormalPressure,
        double totalPressureCoefficient,
        IHumidAir air
    ) =>
        60
        / (Math.PI * sizeOrImpellerRotationSpeed)
        * Math.Pow(
            2
                * totalNormalPressure
                / (
                    totalPressureCoefficient
                    * air.Density.KilogramsPerCubicMeter
                ),
            0.5
        );
}

using SharpProp;

namespace Libraries.Methods;

public static class DimensionlessData
{
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
    ) => volumeFlow / (areaOfWheelDisc * circumferentialSpeed);

    public static double PressureCoefficient(
        double pressure,
        IHumidAir airDensity,
        double circumferentialSpeed
    ) =>
        2
        * pressure
        / (airDensity.Density.KilogramsPerCubicMeter * circumferentialSpeed);

    public static double PressureCoefficient(
        double pressure,
        IHumidAir airDensity,
        double circumferentialSpeed,
        double compressibilityFactor
    ) =>
        2
        * pressure
        * compressibilityFactor
        / (airDensity.Density.KilogramsPerCubicMeter * circumferentialSpeed);

    public static double PowerCoefficient(
        double power,
        IHumidAir airDensity,
        double circumferentialSpeed,
        double areaOfWheelDisc
    ) =>
        2
        * power
        / (
            airDensity.Density.KilogramsPerCubicMeter
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
        * Math.Pow(volumeFlow, 0.5)
        * Math.Pow(totalNormalPressure / 9.80665, -0.75);

    public static double SizeCoefficient(
        double performanceCoefficient,
        double totalPressureCoefficient
    ) =>
        0.56119365
        * Math.Pow(performanceCoefficient, -0.5)
        * Math.Pow(totalPressureCoefficient, 0.25);

    public static double SizeCoefficient(
        double size,
        double volumeFlow,
        double totalNormalPressure
    ) =>
        size
        * Math.Pow(volumeFlow, -0.5)
        * Math.Pow(totalNormalPressure / 9.80665, 0.25);
}

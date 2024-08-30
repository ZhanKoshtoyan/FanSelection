using SharpProp;

namespace Libraries.Methods;

public static class DimensionlessData
{
    private const double AccelerationOfFreeFall = 9.80665;

    /// <summary>
    /// Расчет окружной скорости по концам лопаток, [м/с]
    /// </summary>
    /// <param name="diameterOfTheImpellerAtTheEndsOfTheBlades">Диаметр рабочего колеса по концам лопаток, [мм]</param>
    /// <param name="impellerRotationSpeedWithSlidingEngineForWorkPoint">скорость вращения крыльчатки со скольжением двигателя, [об/мин]</param>
    /// <returns>Oкружная скорость по концам лопаток, [м/с]</returns>
    public static double CircumferentialSpeed(
        double diameterOfTheImpellerAtTheEndsOfTheBlades,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint
    ) =>
        Math.PI
        * diameterOfTheImpellerAtTheEndsOfTheBlades
        * impellerRotationSpeedWithSlidingEngineForWorkPoint
        / 60;

    /// <summary>
    /// Расчет площади диска колеса по концам лопаток, [м2]
    /// </summary>
    /// <param name="diameterOfTheImpellerAtTheEndsOfTheBlades">Диаметр рабочего колеса по концам лопаток, [мм]</param>
    /// <returns>площадь диска колеса по концам лопаток, [м2]</returns>
    public static double SquareOfWheelDisc(
        double diameterOfTheImpellerAtTheEndsOfTheBlades
    ) => Math.PI * Math.Pow(diameterOfTheImpellerAtTheEndsOfTheBlades, 2) / 4;

    /// <summary>
    /// Расчет коэффициента производительности φ (phi), [б/р]
    /// </summary>
    /// <param name="volumeFlow">Объемный расход воздуха, [м3/ч]</param>
    /// <param name="areaOfWheelDisc">площадь диска колеса по концам лопаток, [м2]</param>
    /// <param name="circumferentialSpeed">окружная скорость по концам лопаток, [м/с]</param>
    /// <returns>Коэффициент производительности φ, [б/р]</returns>
    public static double PhiCoefficient(
        double volumeFlow,
        double areaOfWheelDisc,
        double circumferentialSpeed
    ) => volumeFlow / (3600 * areaOfWheelDisc * circumferentialSpeed);

    /// <summary>
    /// Расчет коэффициента (полного, статического или динамического, соответственно) давления ψ, [б/р]
    /// </summary>
    /// <param name="pressure"> (полное, статическое или динамическое, соответственно) давление воздуха, [Па]</param>
    /// <param name="airDensity"> плотность воздуха, [кг/м3]</param>
    /// <param name="circumferentialSpeed">окружная скорость по концам лопаток, [м/с]</param>
    /// <returns>Коэффициент (полного, статического или динамического, соответственно) давления ψ (psi), [б/р]</returns>
    public static double PsiCoefficient(
        double pressure,
        double airDensity,
        double circumferentialSpeed
    ) => 2 * pressure / (airDensity * circumferentialSpeed);

    /// <summary>
    /// Расчет коэффициента (полного, статического или динамического, соответственно) давления ψ с учетом коэффициента учета сжимаемости, [б/р]
    /// </summary>
    /// <param name="pressure"> (полное, статическое или динамическое, соответственно) давление воздуха, [Па]</param>
    /// <param name="airDensity"> плотность воздуха, [кг/м3]</param>
    /// <param name="circumferentialSpeed">окружная скорость по концам лопаток, [м/с]</param>
    /// <param name="compressibilityFactor">коэффициент учета сжимаемости, [б/р]</param>
    /// <returns>Коэффициент (полного, статического или динамического, соответственно) давления ψ с учетом коэффициента учета сжимаемости, [б/р]</returns>
    public static double PsiCoefficient(
        double pressure,
        double airDensity,
        double circumferentialSpeed,
        double compressibilityFactor
    ) =>
        2
        * pressure
        * compressibilityFactor
        / (airDensity * circumferentialSpeed);

    /// <summary>
    /// Расчет коэффициента потребляемой мощности λ, [б/р]
    /// </summary>
    /// <param name="power">потребляемая мощность, [Вт]</param>
    /// <param name="airDensity"> плотность воздуха, [кг/м3]</param>
    /// <param name="circumferentialSpeed">окружная скорость по концам лопаток, [м/с]</param>
    /// <param name="areaOfWheelDisc">площадь диска колеса по концам лопаток, [м2]</param>
    /// <returns>коэффициент потребляемой мощности λ, [б/р]</returns>
    public static double PowerCoefficient(
        double power,
        double airDensity,
        double circumferentialSpeed,
        double areaOfWheelDisc
    ) =>
        2
        * power
        / (airDensity * Math.Pow(circumferentialSpeed, 3) * areaOfWheelDisc);

    public static double SpeedCoefficient(
        double performanceCoefficient,
        double totalPressureCoefficient
    ) =>
        137.58573
        * Math.Pow(performanceCoefficient, 0.5)
        * Math.Pow(totalPressureCoefficient, -0.75);

    /// <summary>
    /// Расчет быстроходности вентилятора по размерным характеристикам
    /// </summary>
    /// <param name="dataImpellerRotationSpeedWithSlidingEngineForWorkPoint">Скорость врашения крыльчатки, при которой был посчитан SpeedCoefficient по безразмерным характеристикам</param>
    /// <param name="inputVolumeFlow">Объемный расход воздуха, который ввел пользователь;  [м3/ч]</param>
    /// <param name="inputTotalNormalPressure">Полное давление воздуха, которое ввел пользователь; [Па]</param>
    /// <returns>Быстроходность вентилятора; [б/р]</returns>
    public static double SpeedCoefficient(
        double dataImpellerRotationSpeedWithSlidingEngineForWorkPoint,
        double inputVolumeFlow,
        double inputTotalNormalPressure
    ) =>
        dataImpellerRotationSpeedWithSlidingEngineForWorkPoint
        * Math.Pow(inputVolumeFlow / 3600, 0.5)
        * Math.Pow(inputTotalNormalPressure / AccelerationOfFreeFall, -0.75);

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
        size
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

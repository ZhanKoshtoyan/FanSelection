using Libraries.DescriptionOfObjects.Parameters;

namespace Libraries.Methods;

public static class FanCompensationCalculator
{
    private static readonly Dictionary<(BladeType, BladeOrientation), Func<double, double, double>> CalculationMethods = new()
    {
        [(BladeType.Axial, BladeOrientation.Axial)] = CalcMinEfficiencyForAxialOrRadialFanWithForwardCurvedBlades,
        [(BladeType.Radial, BladeOrientation.ForwardCurvedBlades)] = CalcMinEfficiencyForAxialOrRadialFanWithForwardCurvedBlades,
        [(BladeType.Radial, BladeOrientation.BackwardCurvedBlades)] = CalcMinEfficiencyForDiagonalOrRadialFanWithBackwardCurvedBlades,
        [(BladeType.Diagonal, BladeOrientation.Diagonal)] = CalcMinEfficiencyForDiagonalOrRadialFanWithBackwardCurvedBlades
    };

    /// <summary>
    /// В ГОСТ 33660-2015 рассчитывается показатель эффективности для вентиляторов с приводом на основе класса эффективности FMEG (в том числе с ПЧ) (стр.18-21 из 41, п.6.3.2 - 6.3.3).
    /// </summary>
    /// <param name="inputPowerOfTheBaseFanEngineInMaximumEfficiency"></param>
    /// <param name="efficiencyGradeOfTheBaseFan"></param>
    /// <param name="bladeType"></param>
    /// <param name="bladeOrientation"></param>
    /// <returns>Показатель эффективности в пределах выбранного класса эффективности (FMEG)</returns>
    /// <exception cref="ArgumentException"></exception>
    public static double GetMinEfficiencyFanByFanMotorEfficiencyGrade(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double efficiencyGradeOfTheBaseFan,
        BladeType bladeType,
        BladeOrientation bladeOrientation)
    {
        var key = (bladeType, bladeOrientation);

        if (!CalculationMethods.TryGetValue(key, out var calculationMethod))
        {
            throw new ArgumentException($"Для сочетания {bladeType}+{bladeOrientation} не создан расчет");
        }

        return calculationMethod(inputPowerOfTheBaseFanEngineInMaximumEfficiency, efficiencyGradeOfTheBaseFan);
    }

    private static double CalcMinEfficiencyForAxialOrRadialFanWithForwardCurvedBlades(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double efficiencyGradeOfTheBaseFan) =>
        CalculateEfficiencyOfFan(inputPowerOfTheBaseFanEngineInMaximumEfficiency, efficiencyGradeOfTheBaseFan,
            2.74, -6.33, 0.78, -1.88);

    private static double CalcMinEfficiencyForDiagonalOrRadialFanWithBackwardCurvedBlades(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double efficiencyGradeOfTheBaseFan) =>
        CalculateEfficiencyOfFan(inputPowerOfTheBaseFanEngineInMaximumEfficiency, efficiencyGradeOfTheBaseFan,
            4.56, -10.5, 1.1, -2.6);

    private static double CalculateEfficiencyOfFan(
        double inputPower,
        double efficiencyGrade,
        double coefficient1,
        double constant1,
        double coefficient2,
        double constant2)
    {
        double efficiency;
        if (inputPower <= 10)
        {
            efficiency = coefficient1 * Math.Log(inputPower) + constant1 + efficiencyGrade;
        }
        else
        {
            efficiency = coefficient2 * Math.Log(inputPower) + constant2 + efficiencyGrade;
        }

        return efficiency / 100;
    }
}
using Libraries.DescriptionOfObjects.Parameters;

namespace Libraries.Methods;

public static class EfficiencyGradeCalculator
{
    private static readonly Dictionary<(BladeType, BladeOrientation), Func<double, double, double>> CalculationMethods = new()
    {
        [(BladeType.Axial, BladeOrientation.Axial)] = CalcEfficiencyGradeForAxialOrRadialFanWithForwardCurvedBlades,
        [(BladeType.Radial, BladeOrientation.ForwardCurvedBlades)] = CalcEfficiencyGradeForAxialOrRadialFanWithForwardCurvedBlades,
        [(BladeType.Radial, BladeOrientation.BackwardCurvedBlades)] = CalcEfficiencyGradeForDiagonalOrRadialFanWithBackwardCurvedBlades,
        [(BladeType.Diagonal, BladeOrientation.Diagonal)] = CalcEfficiencyGradeForDiagonalOrRadialFanWithBackwardCurvedBlades
    };

    public static double GetEfficiencyGradeOfBaseFan(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double fanDataEfficiencyMax,
        BladeType bladeType,
        BladeOrientation bladeOrientation)
    {
        var key = (bladeType, bladeOrientation);

        if (!CalculationMethods.TryGetValue(key, out var calculationMethod))
        {
            throw new ArgumentException($"Для сочетания {bladeType}+{bladeOrientation} не создан расчет");
        }

        return calculationMethod(inputPowerOfTheBaseFanEngineInMaximumEfficiency, fanDataEfficiencyMax);
    }

    private static double CalcEfficiencyGradeForAxialOrRadialFanWithForwardCurvedBlades(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double fanDataEfficiencyMax) =>
        CalculateEfficiencyGrade(inputPowerOfTheBaseFanEngineInMaximumEfficiency, fanDataEfficiencyMax,
            2.74, -6.33, 0.78, -1.88);

    private static double CalcEfficiencyGradeForDiagonalOrRadialFanWithBackwardCurvedBlades(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double fanDataEfficiencyMax) =>
        CalculateEfficiencyGrade(inputPowerOfTheBaseFanEngineInMaximumEfficiency, fanDataEfficiencyMax,
            4.56, -10.5, 1.1, -2.6);

    private static double CalculateEfficiencyGrade(
        double powerInput,
        double fanDataEfficiencyMax,
        double coefficient1,
        double constant1,
        double coefficient2,
        double constant2
    )
    {
        double efficiencyGrade;
        if (powerInput <= 10)
        {
            efficiencyGrade = -(coefficient1 * Math.Log(powerInput) + constant1 - fanDataEfficiencyMax * 100);
        }
        else
        {
            efficiencyGrade = -(coefficient2 * Math.Log(powerInput) + constant2 - fanDataEfficiencyMax * 100);
        }

        return Convert.ToInt32(Math.Round(efficiencyGrade));
    }
}

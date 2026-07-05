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

    /// <summary>
    /// В ГОСТ 33660-2015 рассчитывается показатель эффективности для вентиляторов с приводом на основе класса эффективности FMEG (в том числе с ПЧ) (стр.18-21 из 41, п.6.3.2 - 6.3.3). На основе этих формул делаем обратные вычисления, чтобы узнать FMEG базового двигателя. 
    /// </summary>
    /// <param name="inputPowerOfTheBaseFanEngineInMaximumEfficiency"></param>
    /// <param name="fanDataEfficiencyMax"></param>
    /// <param name="bladeType"></param>
    /// <param name="bladeOrientation"></param>
    /// <returns>Класс эффективности FMEG базового двигателя. Результат - целое число, например, N =40 для FMEG40</returns>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// В ГОСТ 33660-2015 рассчитывается показатель эффективности для вентиляторов с приводом на основе класса эффективности FMEG (в том числе с ПЧ) (стр.18 из 41, п.6.3.2). На основе этих формул делаем обратные вычисления, чтобы узнать FMEG базового двигателя. 
    /// </summary>
    /// <param name="inputPowerOfTheBaseFanEngineInMaximumEfficiency"></param>
    /// <param name="fanDataEfficiencyMax"></param>
    /// <returns></returns>
    private static double CalcEfficiencyGradeForAxialOrRadialFanWithForwardCurvedBlades(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double fanDataEfficiencyMax) =>
        CalculateEfficiencyGrade(inputPowerOfTheBaseFanEngineInMaximumEfficiency, fanDataEfficiencyMax,
            2.74, -6.33, 0.78, -1.88);

    /// <summary>
    /// В ГОСТ 33660-2015 рассчитывается показатель эффективности для вентиляторов с приводом на основе класса эффективности FMEG (в том числе с ПЧ) (стр.20 из 41, п.6.3.3). На основе этих формул делаем обратные вычисления, чтобы узнать FMEG базового двигателя. 
    /// </summary>
    /// <param name="inputPowerOfTheBaseFanEngineInMaximumEfficiency"></param>
    /// <param name="fanDataEfficiencyMax"></param>
    /// <returns></returns>
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
            efficiencyGrade = -(coefficient1 * Math.Log(powerInput) + constant1 + fanDataEfficiencyMax * 100);
        }
        else
        {
            efficiencyGrade = -(coefficient2 * Math.Log(powerInput) + constant2 + fanDataEfficiencyMax * 100);
        }

        return Convert.ToInt32(Math.Round(efficiencyGrade));
    }
}
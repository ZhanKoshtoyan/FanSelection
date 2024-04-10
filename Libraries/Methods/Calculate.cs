using Libraries.Description_of_objects;

namespace Libraries.Methods;

public static class Calculate
{
    public static double SumNoise(IEnumerable<double> fullBandNoise)
    {
        var sum = fullBandNoise.Sum(
            singleOctave => Math.Pow(10, singleOctave / 10)
        );
        return Math.Round(10 * Math.Log10(sum), 2);
    }

    public static double MethodOfHalfDivision(
        double minVolumeFlow,
        double maxVolumeFlow,
        PolynomialType coefficients,
        double inputVolumeFlow,
        double inputTotalPressure
    )
    {
        var constDependencePq = inputTotalPressure / Math.Pow(inputVolumeFlow, 2);
        const double error = 0.00001;
        var desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        while (maxVolumeFlow - minVolumeFlow >= 2 * error)
        {
            if (
                (
                    Polynomial(coefficients, minVolumeFlow)
                    - constDependencePq * Math.Pow(minVolumeFlow, 2)
                )
                *
                (
                    Polynomial(coefficients, desiredValue)
                    - constDependencePq * Math.Pow(desiredValue, 2)
                )
                < 0
            )
            {
                maxVolumeFlow = desiredValue;
            }
            else
            {
                minVolumeFlow = desiredValue;
            }

            desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        }

        // Console.WriteLine("{0:0.00000000}", desiredValue);
        return Math.Round(desiredValue, 0);
    }

    /// <summary>
    /// Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficients"></param>
    /// <param name="inputVolumeFlow"></param>
    /// <returns></returns>
    public static double Polynomial(PolynomialType coefficients, double inputVolumeFlow) =>
        coefficients.SixthCoefficient * Math.Pow(inputVolumeFlow, 6)
        + coefficients.FifthCoefficient * Math.Pow(inputVolumeFlow, 5)
        + coefficients.FourthCoefficient * Math.Pow(inputVolumeFlow, 4)
        + coefficients.ThirdCoefficient * Math.Pow(inputVolumeFlow, 3)
        + coefficients.SecondCoefficient * Math.Pow(inputVolumeFlow, 2)
        + coefficients.FirstCoefficient * Math.Pow(inputVolumeFlow, 1)
        + coefficients.ZeroCoefficient;
}

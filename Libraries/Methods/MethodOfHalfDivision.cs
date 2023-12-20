using Libraries.Description_of_objects;

namespace Libraries.Methods;

public static class MethodOfHalfDivision
{
    public static double Calculate(
        double minVolumeFlow,
        double maxVolumeFlow,
        PolynomialType coefficients,
        double inputVolumeFlow,
        double inputTotalPressure
    )
    {
        var constDependencePq = inputTotalPressure / Math.Pow(inputVolumeFlow, 2);
        var error = 0.00001;
        var desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        while (maxVolumeFlow - minVolumeFlow >= 2 * error)
        {
            if (
                (
                    PolynomialCalculator.Calculate(coefficients, minVolumeFlow)
                    - constDependencePq * Math.Pow(minVolumeFlow, 2)
                )
                *
                (
                    PolynomialCalculator.Calculate(coefficients, desiredValue)
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
}

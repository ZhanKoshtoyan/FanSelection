using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Fans;
using Libraries.StructureOfObjects;
using SharpProp;
using System.Text;

namespace Libraries.Methods;

public static class Calculate
{
    public static double SumNoise(
        IEnumerable<(int Frequency, double Value)> octaveNoise
    )
    {
        var sum = octaveNoise.Sum(
            singleOctave => Math.Pow(10, singleOctave.Value / 10)
        );
        return 10 * Math.Log10(sum);
    }

    public static double MultipleFansNoise(
        double octaveNoiseAtFrequency,
        double numberOfFans
    ) => octaveNoiseAtFrequency + 10 * Math.Log10(numberOfFans);

    public static double MethodOfHalfDivisionVolumeFlow(
        double minVolumeFlow,
        double maxVolumeFlow,
        PolynomialType totalPressureCoefficients,
        double inputVolumeFlow,
        double inputTotalPressure
    )
    {
        var constDependencePq = FanSystemCharacteristicCoefficient(
            inputTotalPressure,
            inputVolumeFlow
        );
        const double error = 0.001;
        var desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        while (maxVolumeFlow - minVolumeFlow >= 2 * error)
        {
            if (
                (
                    Polynomial(totalPressureCoefficients, minVolumeFlow)
                    - constDependencePq * Math.Pow(minVolumeFlow, 2)
                )
                    * (
                        Polynomial(totalPressureCoefficients, desiredValue)
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
        return desiredValue;
    }

    /// <summary>
    /// Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficientsEntity"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static double Polynomial(
        PolynomialType coefficientsEntity,
        double entity
    ) =>
        coefficientsEntity.Coefficients[0] * Math.Pow(entity, 6)
        + coefficientsEntity.Coefficients[1] * Math.Pow(entity, 5)
        + coefficientsEntity.Coefficients[2] * Math.Pow(entity, 4)
        + coefficientsEntity.Coefficients[3] * Math.Pow(entity, 3)
        + coefficientsEntity.Coefficients[4] * Math.Pow(entity, 2)
        + coefficientsEntity.Coefficients[5] * Math.Pow(entity, 1)
        + coefficientsEntity.Coefficients[6];

    public static double Efficiency(
        double volumeFlow,
        double pressure,
        double power
    ) => volumeFlow * pressure / (3600 * 1000 * power) * 100;

    public static double AirVelocity(
        double volumeFlow,
        double inletCrossSection
    ) => volumeFlow / (3600 * inletCrossSection);

    public static double DynamicPressure(IHumidAir air, double airVelocity) =>
        0.5 * air.Density.KilogramsPerCubicMeter * Math.Pow(airVelocity, 2);

    public static double StaticPressure(
        double totalPressure,
        double dynamicPressure
    ) => totalPressure - dynamicPressure;

    public static double Deviation(
        double userInputValue,
        double calculatedValue
    ) => (1 - userInputValue / calculatedValue) * 100;

    /// <summary>
    /// Расчет основных параметров для расхода воздуха на OriginalCurve
    /// </summary>
    /// <param name="volumeFlow"></param>
    /// <param name="totalPressureCoefficients"></param>
    /// <param name="size"></param>
    /// <param name="impellerRotationSpeed"></param>
    /// <param name="oldAirDensity"></param>
    /// <param name="powerCoefficients"></param>
    /// <returns></returns>
    public static DataCurve DataCurveCalculate(
        double volumeFlow,
        PolynomialType totalPressureCoefficients,
        double size,
        double impellerRotationSpeed,
        double oldAirDensity,
        PolynomialType powerCoefficients
    ) =>
        new()
        {
            DcVolumeFlow = volumeFlow,
            DcTotalPressure = Polynomial(totalPressureCoefficients, volumeFlow),
            DcSize = size,
            DcImpellerRotationSpeed = impellerRotationSpeed,
            DcAir = oldAirDensity,
            DcPower = Polynomial(powerCoefficients, volumeFlow)
        };

    public static string GetOctaveNoiseAString(
        IEnumerable<(int Frequency, double Value)> octaveNoiseA
    )
    {
        var sb = new StringBuilder();
        foreach (var octave in octaveNoiseA)
        {
            sb.Append($"{octave.Value:0.0}; ");
        }
        return sb.ToString().TrimEnd(' ', ';');
    }

    public static double Share(double value1, double value2) =>
        Math.Abs(value1 - value2) / value2;

    public static double ImpellerRotationFrequency(
        double impellerRotationSpeed,
        double nominalImpellerRotationSpeed
    )
    {
        var index = Array.IndexOf(
            NominalImpellerRotationSpeeds.Values,
            nominalImpellerRotationSpeed
        );
        if (index == -1)
        {
            throw new ArgumentException(
                "Значение не найдено в массиве NominalImpellerRotationSpeeds.Values."
            );
        }
        return impellerRotationSpeed
            / 60
            * NominalImpellerRotationSpeeds.NumberOfPoles[index]
            / 2;
    }

    /// <summary>
    /// Коэффициент характеристики системы вентилятора, который учитывает
    /// отношение объемного потока воздуха и полного давления с учетом сети
    /// воздуховодов перед и после вентилятора;
    /// </summary>
    /// <param name="inputTotalPressure"></param>
    /// <param name="inputVolumeFlow"></param>
    /// <returns></returns>
    private static double FanSystemCharacteristicCoefficient(
        double inputTotalPressure,
        double inputVolumeFlow
    ) => inputTotalPressure / Math.Pow(inputVolumeFlow, 2);

    public static string ValueContainsOrFirst(string? userInputValue, List<string>? dataValue)
    {
        if (!string.IsNullOrEmpty(userInputValue) &&
            dataValue != null)
        {
            return dataValue.Contains(userInputValue)
                ? userInputValue : dataValue.First();
        }
        else
        {
            throw new ArgumentNullException($"({userInputValue} is null or empty) or ({dataValue} is null).");
        }
    }

    public static T GetValueOrDefault<T>(this T? nullable) where T : struct =>
        nullable ?? default(T);
}

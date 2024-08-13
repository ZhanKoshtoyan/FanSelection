using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Loader;
using Libraries.StructureOfObjects;
using SharpProp;
using System.Globalization;
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
        PolynomialType? totalPressureQvCoefficients,
        double inputVolumeFlow,
        double inputTotalPressure,
        double similarVolumeFlowCoefficient = 1,
        double similarTotalPressureCoefficient = 1
    )
    {
        var constDependencePq = FanSystemCharacteristicCoefficient(
            inputVolumeFlow,
            inputTotalPressure
        );
        const double error = 0.001;

        minVolumeFlow *= similarVolumeFlowCoefficient;
        maxVolumeFlow *= similarVolumeFlowCoefficient;

        var desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        while (maxVolumeFlow - minVolumeFlow >= 2 * error)
        {
            if (
                (
                    Polynomial(
                        totalPressureQvCoefficients,
                        minVolumeFlow / similarVolumeFlowCoefficient
                    ) * similarTotalPressureCoefficient
                    - constDependencePq * Math.Pow(minVolumeFlow, 2)
                )
                    * (
                        Polynomial(
                            totalPressureQvCoefficients,
                            desiredValue / similarVolumeFlowCoefficient
                        ) * similarTotalPressureCoefficient
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

    private static (
        double calcVolumeFlowMaxEfficiency,
        double calcMaxEfficiencyValue
    ) MethodOfHalfDivisionFindMaxEfficiency(
        double minVolumeFlow,
        double maxVolumeFlow,
        PolynomialType? totalPressureQvCoefficients,
        PolynomialType? powerQvCoefficients,
        double similarVolumeFlowCoefficient,
        double similarTotalPressureCoefficient,
        double similarPowerCoefficient
    )
    {
        const double error = 0.001;

        var volumeFlowLeft = minVolumeFlow;
        var volumeFlowRight = maxVolumeFlow;

        double efficiencyLeft;
        double efficiencyRight;
        double midVolumeFlowLeft;
        double midVolumeFlowRight;

        do
        {
            var midVolumeFlow = (volumeFlowLeft + volumeFlowRight) / 2;
            midVolumeFlowLeft = (midVolumeFlow + volumeFlowLeft) / 2;
            midVolumeFlowRight = (midVolumeFlow + volumeFlowRight) / 2;

            efficiencyLeft = Efficiency(
                midVolumeFlowLeft * similarVolumeFlowCoefficient,
                Polynomial(totalPressureQvCoefficients, midVolumeFlowLeft)
                    * similarTotalPressureCoefficient,
                Polynomial(powerQvCoefficients, midVolumeFlowLeft)
                    * similarPowerCoefficient
            );
            efficiencyRight = Efficiency(
                midVolumeFlowRight * similarVolumeFlowCoefficient,
                Polynomial(totalPressureQvCoefficients, midVolumeFlowRight)
                    * similarTotalPressureCoefficient,
                Polynomial(powerQvCoefficients, midVolumeFlowRight)
                    * similarPowerCoefficient
            );

            if (efficiencyLeft < efficiencyRight)
            {
                volumeFlowLeft = midVolumeFlowLeft;
            }
            else
            {
                volumeFlowRight = midVolumeFlowRight;
            }
        } while (Math.Abs(efficiencyLeft - efficiencyRight) >= error);

        var efficiencyList = new List<double>
        {
            efficiencyLeft,
            efficiencyRight
        };

        var volumeFlowList = new List<double>
        {
            midVolumeFlowLeft * similarVolumeFlowCoefficient,
            midVolumeFlowRight * similarVolumeFlowCoefficient
        };

        var resultValues = volumeFlowList
            .Zip(efficiencyList)
            .MaxBy(tuple => tuple.Second);

        /*var theoryResultEfficiency = (efficiencyLeft + efficiencyRight) / 2;
        var theoryResultVolumeFlow =
            (midVolumeFlowLeft + midVolumeFlowRight) / 2;
        var theoryResultTotalPressure =
            Polynomial(totalPressureQvCoefficients, theoryResultVolumeFlow)
            * similarTotalPressureCoefficient;

        var fanSystemCharacteristicCoefficient =
            FanSystemCharacteristicCoefficient(
                theoryResultVolumeFlow,
                theoryResultTotalPressure
            );
        var realResultVolumeFlow = Math.Pow(
            theoryResultTotalPressure / fanSystemCharacteristicCoefficient,
            0.5
        );

        var checkResultEfficiency = Efficiency(
            realResultVolumeFlow * similarVolumeFlowCoefficient,
            Polynomial(totalPressureQvCoefficients, realResultVolumeFlow)
                * similarTotalPressureCoefficient,
            Polynomial(powerQvCoefficients, realResultVolumeFlow)
                * similarPowerCoefficient
        );*/

        return resultValues;
    }

    /// <summary>
    /// Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficientsEntity"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static double Polynomial(
        PolynomialType? coefficientsEntity,
        double entity
    )
    {
        if (coefficientsEntity == null)
        {
            return 0;
        }

        return coefficientsEntity.Coefficients[0] * Math.Pow(entity, 6)
            + coefficientsEntity.Coefficients[1] * Math.Pow(entity, 5)
            + coefficientsEntity.Coefficients[2] * Math.Pow(entity, 4)
            + coefficientsEntity.Coefficients[3] * Math.Pow(entity, 3)
            + coefficientsEntity.Coefficients[4] * Math.Pow(entity, 2)
            + coefficientsEntity.Coefficients[5] * Math.Pow(entity, 1)
            + coefficientsEntity.Coefficients[6];
    }

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
    /// Расчет основных параметров воздуха для OriginalCurve
    /// </summary>
    /// <param name="volumeFlow"></param>
    /// <param name="totalPressureCoefficients"></param>
    /// <param name="conditionalStandardSize"></param>
    /// <param name="impellerRotationSpeed"></param>
    /// <param name="oldAirDensity"></param>
    /// <param name="powerQvCoefficients"></param>
    /// <param name="similarVolumeFlowCoefficient"></param>
    /// <param name="similarTotalPressureCoefficient"></param>
    /// <param name="similarPowerCoefficient"></param>
    /// <returns></returns>
    private static DataCurve DataCurveCalculate(
        double volumeFlow,
        PolynomialType? totalPressureCoefficients,
        double conditionalStandardSize,
        double impellerRotationSpeed,
        double oldAirDensity,
        PolynomialType? powerQvCoefficients,
        double similarVolumeFlowCoefficient,
        double similarTotalPressureCoefficient,
        double similarPowerCoefficient
    ) =>
        new()
        {
            DcVolumeFlow = volumeFlow * similarVolumeFlowCoefficient,
            DcTotalPressure =
                Polynomial(totalPressureCoefficients, volumeFlow)
                * similarTotalPressureCoefficient,
            DcConditionalStandardSize = conditionalStandardSize,
            DcImpellerRotationSpeed = impellerRotationSpeed,
            DcAir = oldAirDensity,
            DcPower =
                Polynomial(powerQvCoefficients, volumeFlow)
                * similarPowerCoefficient
        };

    public static IEnumerable<DataCurve> CreateDataCurves(
        double minVolumeFlow,
        double maxVolumeFlow,
        int numberOfDataCurves,
        PolynomialType? totalPressureCoefficients,
        double conditionalStandardSize,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint,
        double oldAirDensity,
        PolynomialType? powerQvCoefficients,
        double similarVolumeFlowCoefficient,
        double similarTotalPressureCoefficient,
        double similarPowerCoefficient
    )
    {
        var volumeFlowStep =
            (maxVolumeFlow - minVolumeFlow) / (numberOfDataCurves - 1);

        for (var i = 0; i < numberOfDataCurves; i++)
        {
            var volumeFlow = minVolumeFlow + volumeFlowStep * i;

            yield return DataCurveCalculate(
                volumeFlow,
                totalPressureCoefficients,
                conditionalStandardSize,
                impellerRotationSpeedWithSlidingEngineForWorkPoint,
                oldAirDensity,
                powerQvCoefficients,
                similarVolumeFlowCoefficient,
                similarTotalPressureCoefficient,
                similarPowerCoefficient
            );
        }
    }

    public static FanData CreateFanData(
        FanData oldFanData,
        double conditionalStandardSize,
        double weight,
        double nominalPower,
        double nominalImpellerRotationSpeedWithoutSlidingEngine,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint,
        double maxImpellerRotationSpeedWithSlidingEngine,
        double areaOfInletPipeOpening,
        double airDensity = 0,
        double altitude = 0,
        double currentTemperature = 0,
        double relativeHumidity = 0,
        List<double>? fanBodyLength = null,
        List<double>? fanOperatingMaxTemperature = null,
        List<double>? fanOperatingMinTemperature = null,
        List<string>? impellerRotationDirection = null,
        List<string>? caseExecutionMaterial = null
    //double minVolumeFlow,
    //double maxVolumeFlow,
    //PolynomialType totalPressureQvCoefficients,
    //PolynomialType powerQvCoefficients,
    //PolynomialType octaveNoiseLw5QvCoefficients63,
    //PolynomialType octaveNoiseLw5QvCoefficients125,
    //PolynomialType octaveNoiseLw5QvCoefficients250,
    //PolynomialType octaveNoiseLw5QvCoefficients500,
    //PolynomialType octaveNoiseLw5QvCoefficients1000,
    //PolynomialType octaveNoiseLw5QvCoefficients2000,
    //PolynomialType octaveNoiseLw5QvCoefficients4000,
    //PolynomialType octaveNoiseLw5QvCoefficients8000,
    //PolynomialType octaveNoiseLw6QvCoefficients63,
    //PolynomialType octaveNoiseLw6QvCoefficients125,
    //PolynomialType octaveNoiseLw6QvCoefficients250,
    //PolynomialType octaveNoiseLw6QvCoefficients500,
    //PolynomialType octaveNoiseLw6QvCoefficients1000,
    //PolynomialType octaveNoiseLw6QvCoefficients2000,
    //PolynomialType octaveNoiseLw6QvCoefficients4000,
    //PolynomialType octaveNoiseLw6QvCoefficients8000,
    //PolynomialType efficiencyPhiCoefficients,
    //double efficiencyMax,
    //double efficiencyMinLeft,
    //double efficiencyMinRight,
    //double phiEfficiencyMax,
    //double phiMin,
    //double phiMax,
    //PolynomialType psiPhiCoefficients,
    //PolynomialType lambdaPhiCoefficients,
    //PolynomialType specificSpeedPhiCoefficients,
    //PolynomialType specificSizePhiCoefficients
    )
    {
        var newFanData = new FanData
        {
            Version = oldFanData.Version,
            ConditionalStandardSize = conditionalStandardSize, //указываю
            Weight = weight, //указываю
            NominalPower = nominalPower, //указываю
            NominalImpellerRotationSpeedWithoutSlidingEngine =
                nominalImpellerRotationSpeedWithoutSlidingEngine, //указываю
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
                impellerRotationSpeedWithSlidingEngineForWorkPoint, // указать скорость, на которую будут пересчитаны данные
            MaxImpellerRotationSpeedWithSlidingEngine =
                maxImpellerRotationSpeedWithSlidingEngine, //указываю
            AreaOfInletPipeOpening = areaOfInletPipeOpening, //указываю
            AirDensity = airDensity == 0 ? oldFanData.AirDensity : airDensity,
            Altitude = altitude == 0 ? oldFanData.Altitude : altitude,
            FanOperatingCurrentTemperature =
                currentTemperature == 0
                    ? oldFanData.FanOperatingCurrentTemperature
                    : currentTemperature,
            RelativeHumidity =
                relativeHumidity == 0
                    ? oldFanData.RelativeHumidity
                    : relativeHumidity,
            FanBodyLength = fanBodyLength ?? oldFanData.FanBodyLength,
            FanOperatingMaxTemperature =
                fanOperatingMaxTemperature
                ?? oldFanData.FanOperatingMaxTemperature,
            FanOperatingMinTemperature =
                fanOperatingMinTemperature
                ?? oldFanData.FanOperatingMinTemperature,
            ImpellerRotationDirection =
                impellerRotationDirection
                ?? oldFanData.ImpellerRotationDirection,
            FanBodyExecutionMaterial =
                caseExecutionMaterial ?? oldFanData.FanBodyExecutionMaterial,
            MinVolumeFlow = oldFanData.MinVolumeFlow, // старые значения
            MaxVolumeFlow = oldFanData.MaxVolumeFlow, // старые значения
            TotalPressureQvCoefficients =
                oldFanData.TotalPressureQvCoefficients, // старые значения
            PowerQvCoefficients = oldFanData.PowerQvCoefficients, // старые значения
            OctaveNoiseLw5QvCoefficients63 = null,
            OctaveNoiseLw5QvCoefficients125 = null,
            OctaveNoiseLw5QvCoefficients250 = null,
            OctaveNoiseLw5QvCoefficients500 = null,
            OctaveNoiseLw5QvCoefficients1000 = null,
            OctaveNoiseLw5QvCoefficients2000 = null,
            OctaveNoiseLw5QvCoefficients4000 = null,
            OctaveNoiseLw5QvCoefficients8000 = null,
            OctaveNoiseLw6QvCoefficients63 = null,
            OctaveNoiseLw6QvCoefficients125 = null,
            OctaveNoiseLw6QvCoefficients250 = null,
            OctaveNoiseLw6QvCoefficients500 = null,
            OctaveNoiseLw6QvCoefficients1000 = null,
            OctaveNoiseLw6QvCoefficients2000 = null,
            OctaveNoiseLw6QvCoefficients4000 = null,
            OctaveNoiseLw6QvCoefficients8000 = null,
            EfficiencyPhiCoefficients = null, // ??? не используется
            EfficiencyMax = 0, // расчет ниже
            EfficiencyMinLeft = 0, // расчет ниже
            EfficiencyMinRight = 0, // расчет ниже
            PhiEfficiencyMax = 0, // расчет ниже
            PhiMin = 0, // расчет ниже
            PhiMax = 0, // расчет ниже
            PsiPhiCoefficients = oldFanData.PsiPhiCoefficients, // По-моему, они должны быть неизменны для одной АСВ
            LambdaPhiCoefficients = oldFanData.LambdaPhiCoefficients, // По-моему, они должны быть неизменны для одной АСВ
            SpecificSpeedPhiCoefficients =
                oldFanData.SpecificSpeedPhiCoefficients, // По-моему, они должны быть неизменны для одной АСВ
            SpecificSizePhiCoefficients = oldFanData.SpecificSizePhiCoefficients // По-моему, они должны быть неизменны для одной АСВ
        };

        newFanData.SimilarVolumeFlowCoefficient = Similarity.SimilarVolumeFlow(
            1.0,
            oldFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            oldFanData.ConditionalStandardSize,
            newFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            newFanData.ConditionalStandardSize
        );

        newFanData.SimilarTotalPressureCoefficient = Similarity.SimilarPressure(
            1.0,
            oldFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            oldFanData.ConditionalStandardSize,
            oldFanData.AirDensity,
            newFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            newFanData.ConditionalStandardSize,
            newFanData.AirDensity
        );

        newFanData.SimilarPowerCoefficient = Similarity.SimilarPower(
            1.0,
            oldFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            oldFanData.ConditionalStandardSize,
            oldFanData.AirDensity,
            newFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            newFanData.ConditionalStandardSize,
            newFanData.AirDensity
        );

        /*var calcMaxEfficiencyWithoutPowerScaleEffect =
            MethodOfHalfDivisionFindMaxEfficiency(
                oldFanData.MinVolumeFlow,
                oldFanData.MaxVolumeFlow,
                oldFanData.TotalPressureQvCoefficients,
                oldFanData.PowerQvCoefficients,
                newFanData.SimilarVolumeFlowCoefficient,
                newFanData.SimilarTotalPressureCoefficient,
                newFanData.SimilarPowerCoefficient
            );

        newFanData.EfficiencyMax =
            calcMaxEfficiencyWithoutPowerScaleEffect.calcMaxEfficiencyValue;*/

        var fanEfficiencyGradeCoefficient = PowerScaleEffect(
            oldFanData.EfficiencyMax,
            oldFanData.ConditionalStandardSize,
            newFanData.ConditionalStandardSize
        );

        newFanData.EfficiencyMax =
            oldFanData.EfficiencyMax * fanEfficiencyGradeCoefficient;
        newFanData.SimilarPowerCoefficient /= fanEfficiencyGradeCoefficient;

        /*var newMinVolumeFlow =
            oldFanData.MinVolumeFlow * newFanData.SimilarVolumeFlowCoefficient;
        var newMaxVolumeFlow =
            oldFanData.MaxVolumeFlow * newFanData.SimilarVolumeFlowCoefficient;*/

        /*newFanData.EfficiencyMinLeft = Efficiency(
            newMinVolumeFlow,
            Polynomial(
                oldFanData.TotalPressureQvCoefficients,
                oldFanData.MinVolumeFlow
            ) * newFanData.SimilarTotalPressureCoefficient,
            Polynomial(oldFanData.PowerQvCoefficients, oldFanData.MinVolumeFlow)
                * newFanData.SimilarPowerCoefficient
        );

        newFanData.EfficiencyMinRight = Efficiency(
            newMaxVolumeFlow,
            Polynomial(
                oldFanData.TotalPressureQvCoefficients,
                oldFanData.MaxVolumeFlow
            ) * newFanData.SimilarTotalPressureCoefficient,
            Polynomial(oldFanData.PowerQvCoefficients, oldFanData.MaxVolumeFlow)
                * newFanData.SimilarPowerCoefficient
        );*/
        newFanData.EfficiencyMinLeft = oldFanData.EfficiencyMinLeft;
        newFanData.EfficiencyMinRight = oldFanData.EfficiencyMinRight;

        /*newFanData.PhiEfficiencyMax = DimensionlessData.PhiCoefficient(
            calcMaxEfficiencyWithoutPowerScaleEffect.calcVolumeFlowMaxEfficiency,
            newFanData.AreaOfWheelDisc,
            newFanData.CircumferentialSpeed
        );*/
        newFanData.PhiEfficiencyMax = oldFanData.PhiEfficiencyMax;

        /*newFanData.PhiMin = DimensionlessData.PhiCoefficient(
            newMinVolumeFlow,
            newFanData.AreaOfWheelDisc,
            newFanData.CircumferentialSpeed
        );

        newFanData.PhiMax = DimensionlessData.PhiCoefficient(
            newMaxVolumeFlow,
            newFanData.AreaOfWheelDisc,
            newFanData.CircumferentialSpeed
        );*/
        newFanData.PhiMin = oldFanData.PhiMin;
        newFanData.PhiMax = oldFanData.PhiMax;

        return newFanData;
    }

    private static double PowerScaleEffect(
        double existingFanDataEfficiencyMax,
        double existingFanDataConditionalStandardSize,
        double newFanDataConditionalStandardSize
    )
    {
        var fanEfficiencyGradeList = new FanEfficiencyGradeCollection
        {
            FanEfficiencyGrades = JsonLoader.Download<FanEfficiencyGrade>(
                UserInput.PathFanEfficiencyGradeJsonFile
            )
        };

        if (fanEfficiencyGradeList.FanEfficiencyGrades == null)
        {
            throw new Exception(
                "Список fanEfficiencyGradeList.FanEfficiencyGrades == null"
            );
        }

        // Находим fanEfficiencyGrade.Name для fanData
        var fanEfficiencyGradeValueForExistingFanData =
            fanEfficiencyGradeList.FanEfficiencyGrades
                .Select(i => Convert.ToDouble(i.Name[3..]))
                .Where(
                    efficiencyMax =>
                        efficiencyMax >= existingFanDataEfficiencyMax
                )
                .MinBy(d => d);
        var fanEfficiencyGradeNameForExistingFanData = string.Concat(
            "FEG",
            fanEfficiencyGradeValueForExistingFanData
        );

        // Находим объект fanEfficiencyGrade из fanEfficiencyGradeList для fanData
        var fanEfficiencyGradeObjectForExistingFanData =
            fanEfficiencyGradeList.FanEfficiencyGrades.FirstOrDefault(
                grade => grade.Name == fanEfficiencyGradeNameForExistingFanData
            )
            ?? throw new Exception(
                "fanEfficiencyGradeObject не найден для объекта fanData"
            );

        var existingFanEfficiencyGradeCoefficient =
            FindFanEfficiencyGradeCoefficient(
                fanEfficiencyGradeList,
                fanEfficiencyGradeObjectForExistingFanData,
                existingFanDataConditionalStandardSize
            );
        var newFanEfficiencyGradeCoefficient =
            FindFanEfficiencyGradeCoefficient(
                fanEfficiencyGradeList,
                fanEfficiencyGradeObjectForExistingFanData,
                newFanDataConditionalStandardSize
            );
        return newFanEfficiencyGradeCoefficient
            / existingFanEfficiencyGradeCoefficient;
    }

    private static double FindFanEfficiencyGradeCoefficient(
        FanEfficiencyGradeCollection fanEfficiencyGradeList,
        FanEfficiencyGrade fanEfficiencyGradeObjectForExistingFanData,
        double fanDataConditionalStandardSize
    )
    {
        //Находим порядковый номер коэффициента FEG для fanSize в последовательности fanEfficiencyGradeList.FanSizeList для fanData
        var checkedConditionalStandardSize = fanDataConditionalStandardSize;
        if (checkedConditionalStandardSize > 1000)
        {
            checkedConditionalStandardSize = 1000;
        }
        var fanSizeIndex = fanEfficiencyGradeList.FanSizeList.FindIndex(
            i => Math.Abs(i - checkedConditionalStandardSize) < 0.05
        );

        // Находим сам коэффициент FEG для параметра fanSize объекта fanData
        return fanEfficiencyGradeObjectForExistingFanData.Values[fanSizeIndex];
    }

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

    public static IEnumerable<(
        int Frequency,
        double Value
    )> ConvertStringToOctaveNoiseA(string octaveNoiseString)
    {
        var octaveNoiseList = new List<(int Frequency, double Value)>();

        if (string.IsNullOrEmpty(octaveNoiseString))
        {
            return octaveNoiseList;
        }

        var octaveValues = octaveNoiseString.Split(';');

        foreach (var octaveValue in octaveValues)
        {
            var parts = octaveValue.Trim().Split(':');
            if (
                parts.Length == 2
                && int.TryParse(parts[0], out var frequency)
                && double.TryParse(parts[1], out var value)
            )
            {
                octaveNoiseList.Add((frequency, value));
            }
        }

        return octaveNoiseList;
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
    /// <param name="volumeFlow"></param>
    /// /// <param name="totalPressure"></param>
    /// <returns></returns>
    private static double FanSystemCharacteristicCoefficient(
        double volumeFlow,
        double totalPressure
    ) => totalPressure / Math.Pow(volumeFlow, 2);

    public static UserInput ProcessUserInput(
        string? volumeFlowTextBox,
        string? totalPressureTextBox,
        string? selectedFanOperatingMaxTemperature,
        string? selectedFanVersion,
        string? selectedFanLogic,
        string? selectedSize,
        string? selectedFanBodyLength,
        string? selectedImpellerRotationDirection,
        string? selectedNominalPower,
        string? selectedNominalImpellerRotationSpeed,
        string? selectedFanBodyExecutionMaterial,
        string? totalPressureDeviationTextBox,
        string? relativeHumidityTextBox,
        string? altitudeTextBox,
        string? fanOperatingCurrentTemperatureTextBox,
        string? selectedNumberOfFans
    )
    {
        var userInput = new UserInput
        {
            UserInputWorkPoint = new UserInputWorkPoint
            {
                VolumeFlow = ParseToDouble(volumeFlowTextBox),
                TotalPressure = ParseToDouble(totalPressureTextBox)
            },
            UserInputAir = new UserInputAir
            {
                FanOperatingMaxTemperature = ParseToDouble(
                    selectedFanOperatingMaxTemperature
                )
            },
            UserInputFan = new UserInputFan
            {
                FanVersion = ParseToInt(selectedFanVersion),
                FanLogic = ParseToInt(selectedFanLogic),
                ConditionalStandardSize = ParseToDouble(selectedSize),
                FanBodyLength = ParseToInt(selectedFanBodyLength),
                ImpellerRotationDirection = selectedImpellerRotationDirection,
                NominalPower = ParseToDouble(selectedNominalPower),
                NominalImpellerRotationSpeedWithoutSlidingEngine =
                    ParseToDouble(selectedNominalImpellerRotationSpeed),
                FanBodyExecutionMaterial = selectedFanBodyExecutionMaterial
            }
        };

        if (!string.IsNullOrEmpty(totalPressureDeviationTextBox))
        {
            userInput.UserInputWorkPoint.TotalPressureDeviation =
                Convert.ToDouble(totalPressureDeviationTextBox);
        }

        if (!string.IsNullOrEmpty(relativeHumidityTextBox))
        {
            userInput.UserInputAir.RelativeHumidity = ParseToDouble(
                relativeHumidityTextBox
            );
        }

        if (!string.IsNullOrEmpty(altitudeTextBox))
        {
            userInput.UserInputAir.Altitude = ParseToDouble(altitudeTextBox);
        }

        if (!string.IsNullOrEmpty(fanOperatingCurrentTemperatureTextBox))
        {
            userInput.UserInputAir.FanOperatingCurrentTemperature =
                ParseToDouble(fanOperatingCurrentTemperatureTextBox);
        }

        if (!string.IsNullOrEmpty(selectedImpellerRotationDirection))
        {
            userInput.UserInputFan.ImpellerRotationDirection =
                selectedImpellerRotationDirection;
        }

        if (!string.IsNullOrEmpty(selectedNumberOfFans))
        {
            userInput.UserInputFan.NumberOfFans = ParseToInt(
                selectedNumberOfFans
            );
        }

        return userInput;
    }

    private static int ParseToInt(string? strValue)
    {
        if (string.IsNullOrEmpty(strValue))
        {
            return 0;
        }

        return int.TryParse(strValue, out var intValue)
            ? intValue
            : throw new Exception(
                $"{strValue}, не удалось преобразовать к типу int"
            );
    }

    private static double ParseToDouble(string? strValue)
    {
        if (string.IsNullOrEmpty(strValue))
        {
            return 0.0;
        }

        return double.TryParse(
            strValue.Replace(',', '.'),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var doubleValue
        )
            ? doubleValue
            : throw new Exception(
                $"SelectedSize = {strValue}, не удалось преобразовать к типу double"
            );
    }

    public static List<T> UpdateFansCountWithClone<T>(List<T> fansTypeList)
        where T : AbstractFan, ICloneable
    {
        var resultList = new List<T>();

        foreach (var numberOfFans in NumberOfFans.Values)
        {
            foreach (var tFan in fansTypeList)
            {
                var newFan = (T)tFan.Clone(); // Клонируем объект
                newFan.NumberOfFans = numberOfFans; // Обновляем количество вентиляторов
                resultList.Add(newFan); // Добавляем в результирующий список
            }
        }

        return resultList;
    }
}

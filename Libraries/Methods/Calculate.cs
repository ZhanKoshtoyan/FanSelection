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
        FanData data,
        double inputVolumeFlow,
        double inputTotalPressure
    //double newFanDataConditionalStandardSize
    )
    {
        var inputVolumeFlowForNewDiameterOfTheImpellerAtTheEndsOfTheBlades =
            Similarity.SimilarVolumeFlow(
                inputVolumeFlow,
                1.0,
                data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                1.0,
                data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades
            );

        var inputTotalPressureForNewDiameterOfTheImpellerAtTheEndsOfTheBlades =
            Similarity.SimilarPressure(
                inputTotalPressure,
                1.0,
                data.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                1.0,
                1.0,
                data.OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades,
                1.0
            );

        var constDependencePq = FanSystemCharacteristicCoefficient(
            inputVolumeFlowForNewDiameterOfTheImpellerAtTheEndsOfTheBlades,
            inputTotalPressureForNewDiameterOfTheImpellerAtTheEndsOfTheBlades
        );
        const double error = 0.001;

        var minVolumeFlow = data.MinVolumeFlow;
        var maxVolumeFlow = data.MaxVolumeFlow;

        var desiredValue = (minVolumeFlow + maxVolumeFlow) / 2;
        while (maxVolumeFlow - minVolumeFlow >= 2 * error)
        {
            if (
                (
                    Polynomial(data.TotalPressureQvCoefficients, minVolumeFlow)
                    - constDependencePq * Math.Pow(minVolumeFlow, 2)
                )
                    * (
                        Polynomial(
                            data.TotalPressureQvCoefficients,
                            desiredValue
                        )
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

        var result =
            coefficientsEntity.Coefficients[0] * Math.Pow(entity, 6)
            + coefficientsEntity.Coefficients[1] * Math.Pow(entity, 5)
            + coefficientsEntity.Coefficients[2] * Math.Pow(entity, 4)
            + coefficientsEntity.Coefficients[3] * Math.Pow(entity, 3)
            + coefficientsEntity.Coefficients[4] * Math.Pow(entity, 2)
            + coefficientsEntity.Coefficients[5] * Math.Pow(entity, 1)
            + coefficientsEntity.Coefficients[6];

        return result;
    }

    public static double Efficiency(
        double volumeFlow,
        double pressure,
        double power
    ) => volumeFlow * pressure / (3600 * 1000 * power) * 100;

    public static double CompensationFactorForFanWithWithFrequencyConverter(double driveOrControlElectricalInputPower)
    {
        if (driveOrControlElectricalInputPower < 5000)
        {
            return -0.03 * Math.Log(driveOrControlElectricalInputPower) + 1.088;
        }

        return 1.04;
    }

    //TODO КПД для вентиляторов с частотником 33660-2015: вместо FEG д/б FMEG
    /*Вентилятор с открытым валом - Вентилятор без привода, оборудования и аксессуаров (принадлежностей).
     Показатель эффективности вентилятора FEG (fan efficiency grade)*/

    /*Вентилятор с приводом - Вентилятор с одним или несколькими рабочими колесами,
     оснащенный двигателем или подключенный к нему, с или без приводного механизма,
     с корпусом и средством изменения частоты вращения.
     Показатель энергоэффективности вентилятора с двигателем, FMEG (fan motor efficiency grade)
     Пример: "Свободное колесо"
     Алгоритм корректировки КПД для подобных колес:
     1. Зная КПД базового колеса, находим Ng - класс эффективности FMEG (целое число)
     из формул №12 и 14 (ГОСТ 33660-2015, пункт 6.3.3, формула №12, 14). Т.о. мы найдем Ng для всей серии геом.подобн.вентиляторов;
     2. Используя формулы №12, 14 и Ng для всей серии геом.подобн.вентиляторов мы сможем узнать минимальный критерий КПД
     для каждой входной мощности. */



    public static double AirVelocityOfOutletPipeOpening(
        double volumeFlow,
        double inletCrossSection
    )
    {
        if (inletCrossSection == 0)
        {
            return 0;
        }

        return volumeFlow / (3600 * inletCrossSection);
    }

    public static double DynamicPressure(IHumidAir air, double airVelocity) =>
        0.5 * air.Density.KilogramsPerCubicMeter * Math.Pow(airVelocity, 2);

    public static double StaticPressure(
        double totalPressure,
        double dynamicPressure
    ) => totalPressure - dynamicPressure;

    public static double Deviation(
        double valueX,
        double valueOneHundredPercent
    ) => (1 - valueX / valueOneHundredPercent) * 100;

    /// <summary>
    /// Расчет основных параметров воздуха для OriginalCurve
    /// </summary>
    /// <param name="volumeFlow"></param>
    /// <param name="totalPressureCoefficients"></param>
    /// <param name="diameterOfTheImpellerAtTheEndsOfTheBlades"></param>
    /// <param name="impellerRotationSpeed"></param>
    /// <param name="oldAirDensity"></param>
    /// <param name="powerQvCoefficients"></param>
    /// <returns></returns>
    private static DataCurve DataCurveCalculate(
        double volumeFlow,
        PolynomialType? totalPressureCoefficients,
        double diameterOfTheImpellerAtTheEndsOfTheBlades,
        double impellerRotationSpeed,
        double oldAirDensity,
        PolynomialType? powerQvCoefficients
    ) =>
        new()
        {
            DcVolumeFlow = volumeFlow,
            DcTotalPressure = Polynomial(totalPressureCoefficients, volumeFlow),
            DcDiameterOfTheImpellerAtTheEndsOfTheBlades =
                diameterOfTheImpellerAtTheEndsOfTheBlades,
            DcImpellerRotationSpeed = impellerRotationSpeed,
            DcAir = oldAirDensity,
            DcPower = Polynomial(powerQvCoefficients, volumeFlow)
        };

    public static IEnumerable<DataCurve> CreateDataCurves(
        double minVolumeFlow,
        double maxVolumeFlow,
        int numberOfDataCurves,
        PolynomialType? totalPressureCoefficients,
        double diameterOfTheImpellerAtTheEndsOfTheBlades,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint,
        double oldAirDensity,
        PolynomialType? powerQvCoefficients
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
                diameterOfTheImpellerAtTheEndsOfTheBlades,
                impellerRotationSpeedWithSlidingEngineForWorkPoint,
                oldAirDensity,
                powerQvCoefficients
            );
        }
    }

    public static FanData CreateFanData(
        FanData oldFanData,
        FanEfficiencyGradeCollection fanEfficiencyGradeList,
        double conditionalStandardSize,
        double diameterOfTheImpellerAtTheEndsOfTheBlades,
        double weight,
        double nominalPower,
        double nominalImpellerRotationSpeedWithoutSlidingEngine,
        double impellerRotationSpeedWithSlidingEngineForWorkPoint,
        double maxImpellerRotationSpeedWithSlidingEngine,
        double squareOfOutletPipeOpening,
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
            AerodynamicDesign = oldFanData.AerodynamicDesign,
            ConditionalStandardSize = conditionalStandardSize, //указываю
            DiameterOfTheImpellerAtTheEndsOfTheBlades =
                diameterOfTheImpellerAtTheEndsOfTheBlades / 1000, //указываю
            Weight = weight, //указываю
            NominalPower = nominalPower, //указываю
            NominalImpellerRotationSpeedWithoutSlidingEngine =
                nominalImpellerRotationSpeedWithoutSlidingEngine, //указываю
            ImpellerRotationSpeedWithSlidingEngineForWorkPoint =
                impellerRotationSpeedWithSlidingEngineForWorkPoint, // указать скорость, на которую будут пересчитаны данные. Скорость базового колеса
            MaxImpellerRotationSpeedWithSlidingEngine =
                maxImpellerRotationSpeedWithSlidingEngine, //указываю
            SquareOfOutletPipeOpening = squareOfOutletPipeOpening, //указываю
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
            OctaveNoiseLw5QvCoefficients63 =
                oldFanData.OctaveNoiseLw5QvCoefficients63, // старые значения
            OctaveNoiseLw5QvCoefficients125 =
                oldFanData.OctaveNoiseLw5QvCoefficients125, // старые значения
            OctaveNoiseLw5QvCoefficients250 =
                oldFanData.OctaveNoiseLw5QvCoefficients250, // старые значения
            OctaveNoiseLw5QvCoefficients500 =
                oldFanData.OctaveNoiseLw5QvCoefficients500, // старые значения
            OctaveNoiseLw5QvCoefficients1000 =
                oldFanData.OctaveNoiseLw5QvCoefficients1000, // старые значения
            OctaveNoiseLw5QvCoefficients2000 =
                oldFanData.OctaveNoiseLw5QvCoefficients2000, // старые значения
            OctaveNoiseLw5QvCoefficients4000 =
                oldFanData.OctaveNoiseLw5QvCoefficients4000, // старые значения
            OctaveNoiseLw5QvCoefficients8000 =
                oldFanData.OctaveNoiseLw5QvCoefficients8000, // старые значения
            OctaveNoiseLw6QvCoefficients63 =
                oldFanData.OctaveNoiseLw6QvCoefficients63, // старые значения
            OctaveNoiseLw6QvCoefficients125 =
                oldFanData.OctaveNoiseLw6QvCoefficients125, // старые значения
            OctaveNoiseLw6QvCoefficients250 =
                oldFanData.OctaveNoiseLw6QvCoefficients250, // старые значения
            OctaveNoiseLw6QvCoefficients500 =
                oldFanData.OctaveNoiseLw6QvCoefficients500, // старые значения
            OctaveNoiseLw6QvCoefficients1000 =
                oldFanData.OctaveNoiseLw6QvCoefficients1000, // старые значения
            OctaveNoiseLw6QvCoefficients2000 =
                oldFanData.OctaveNoiseLw6QvCoefficients2000, // старые значения
            OctaveNoiseLw6QvCoefficients4000 =
                oldFanData.OctaveNoiseLw6QvCoefficients4000, // старые значения
            OctaveNoiseLw6QvCoefficients8000 =
                oldFanData.OctaveNoiseLw6QvCoefficients8000, // старые значения
            EfficiencyPhiCoefficients = null, // ??? не используется
            EfficiencyMax = 0, // расчет в  абстрактном классе вентилятора
            EfficiencyMinLeft = oldFanData.EfficiencyMinLeft, // Неизменны для одной АСВ
            EfficiencyMinRight = oldFanData.EfficiencyMinRight, // Неизменны для одной АСВ
            PhiEfficiencyMax = oldFanData.PhiEfficiencyMax, // Неизменны для одной АСВ
            PhiMin = oldFanData.PhiMin, // Неизменны для одной АСВ
            PhiMax = oldFanData.PhiMax, // Неизменны для одной АСВ
            PsiPhiCoefficients = oldFanData.PsiPhiCoefficients, // Неизменны для одной АСВ
            LambdaPhiCoefficients = oldFanData.LambdaPhiCoefficients, // Неизменны для одной АСВ
            SpecificSpeedPhiCoefficients =
                oldFanData.SpecificSpeedPhiCoefficients, // Неизменны для одной АСВ
            SpecificSizePhiCoefficients =
                oldFanData.SpecificSizePhiCoefficients, // Неизменны для одной АСВ
            OriginalFanDataImpellerRotationSpeedWithSlidingEngineForWorkPoint =
                oldFanData.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            OriginalFanDataDiameterOfTheImpellerAtTheEndsOfTheBlades =
                oldFanData.DiameterOfTheImpellerAtTheEndsOfTheBlades / 1000,
            OriginalFanDataAirDensity = oldFanData.AirDensity,
            OriginalFanDataEfficiencyMax = oldFanData.OriginalFanDataEfficiencyMax,
            OriginalFanDataPowerByEfficiencyMax = oldFanData.OriginalFanDataPowerByEfficiencyMax,
            OriginalFanDataConditionalStandardSize = oldFanData.OriginalFanDataConditionalStandardSize,
            FanEfficiencyGradeList = fanEfficiencyGradeList
        };

    if (Equals(oldFanData.Version, FanVersion.Values.EuFan.ToString()))
        {
            /*var powerByEfficiencyMax = oldFanData.OriginalFanDataPowerByEfficiencyMax * oldFanData.SimilarPowerCoefficient;
            newFanData.FanEfficiencyGradeCoefficient = PowerScaleEffectByFanMotorEfficiencyGradeAsync(
                oldFanData.OriginalFanDataPowerByEfficiencyMax,
                oldFanData.EfficiencyMax,
                newFanData.OriginalFanDataPowerByEfficiencyMax);*/
        }
        else
        {
            /*newFanData.FanEfficiencyGradeCoefficient = PowerScaleEffectByFanEfficiencyGradeAsync(
                fanEfficiencyGradeList,
                oldFanData.EfficiencyMax,
                oldFanData.ConditionalStandardSize,
                newFanData.ConditionalStandardSize
            );*/
        }

        // newFanData.EfficiencyMax =
        //     oldFanData.EfficiencyMax * newFanData.FanEfficiencyGradeCoefficient;
        /*newFanData.SimilarPowerCoefficient /=
            newFanData.FanEfficiencyGradeCoefficient;*/

        return newFanData;
    }

    public static double PowerScaleEffectByFanMotorEfficiencyGrade(
        double inputPowerOfTheBaseFanEngineInMaximumEfficiency,
        double maximumEfficiencyOfTheBaseFan,
        double inputPowerOfTheNewFanEngineInMaximumEfficiency,
        BladeType bladeType,
        BladeOrientation bladeOrientation
        )
    {
        var efficiencyGradeOfTheBaseFan = EfficiencyGradeCalculator.GetEfficiencyGradeOfBaseFan(
            inputPowerOfTheBaseFanEngineInMaximumEfficiency,
            maximumEfficiencyOfTheBaseFan,
            bladeType,
            bladeOrientation);

        var minEfficiencyBaseFanByFanMotorEfficiencyGrade =
            FanCompensationCalculator.GetMinEfficiencyFanByFanMotorEfficiencyGrade(
                inputPowerOfTheBaseFanEngineInMaximumEfficiency,
                efficiencyGradeOfTheBaseFan,
                bladeType,
                bladeOrientation
            );

        var minEfficiencyNewFanByFanMotorEfficiencyGrade =
            FanCompensationCalculator.GetMinEfficiencyFanByFanMotorEfficiencyGrade(
            inputPowerOfTheNewFanEngineInMaximumEfficiency,
            efficiencyGradeOfTheBaseFan,
            bladeType,
            bladeOrientation
        );

        return minEfficiencyNewFanByFanMotorEfficiencyGrade
            / minEfficiencyBaseFanByFanMotorEfficiencyGrade;
    }

    public static double PowerScaleEffectByFanEfficiencyGradeAsync(
        FanEfficiencyGradeCollection fanEfficiencyGradeList,
        double existingFanDataEfficiencyMax,
        double existingFanDataConditionalStandardSize,
        double newFanDataConditionalStandardSize
    )
    {
        // Находим fanEfficiencyGrade.Name для fanData
        double fanEfficiencyGradeValueForExistingFanData;
        if (existingFanDataEfficiencyMax * 100 < 50)
        {
            fanEfficiencyGradeValueForExistingFanData = 0;
        }
        else
        {
            fanEfficiencyGradeValueForExistingFanData =
                fanEfficiencyGradeList.FanEfficiencyGrades!
                    .Select(i => Convert.ToDouble(i.Name[3..]))
                    .Where(
                        efficiencyMax =>
                            efficiencyMax / 100 >= existingFanDataEfficiencyMax
                    )
                    .MinBy(d => d);
        }

        var fanEfficiencyGradeNameForExistingFanData = string.Concat(
            "FEG",
            fanEfficiencyGradeValueForExistingFanData
        );

        // Находим объект fanEfficiencyGrade из fanEfficiencyGradeList для fanData
        var fanEfficiencyGradeObjectForExistingFanData =
            fanEfficiencyGradeList.FanEfficiencyGrades!.FirstOrDefault(
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
        (value1 - value2) / value1;

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
        string? specificDeviationLeftTextBox,
        string? specificDeviationRightTextBox,
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
            userInput.UserInputWorkPoint.VolumeFlowAndTotalPressureDeviation =
                Convert.ToDouble(totalPressureDeviationTextBox);
        }

        if (!string.IsNullOrEmpty(specificDeviationLeftTextBox))
        {
            userInput.UserInputWorkPoint.SpecificDeviationLeft =
                Convert.ToDouble(specificDeviationLeftTextBox);
        }

        if (!string.IsNullOrEmpty(specificDeviationRightTextBox))
        {
            userInput.UserInputWorkPoint.SpecificDeviationRight =
                Convert.ToDouble(specificDeviationRightTextBox);
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
            userInput.UserInputFan.NumberOfFans = ReturnCorrectOrDefaultIndex(
                NumberOfFans.Names,
                selectedNumberOfFans
            );
        }

        return userInput;
    }

    public static int ReturnCorrectOrDefaultIndex(
        IEnumerable<string> comboBoxArr,
        string selectedValue
    )
    {
        var selectedIndex = comboBoxArr
            .ToList()
            .FindIndex(i => Equals(i, selectedValue));
        var result = selectedIndex == -1 ? 0 : selectedIndex;
        return result;
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
}

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Methods;
using System.Globalization;

namespace Libraries.PrintFolder;

public static class ToPrint
{
    public static void Print<T>(List<T> sortFans, UserInput userInput)
        where T : AbstractFan
    {
        string? endingOfTheWord1;
        string? endingOfTheWord2;
        switch (sortFans.Count)
        {
            case 0:
                endingOfTheWord1 = "подобрано";
                endingOfTheWord2 = "вентиляторов";
                break;
            case 1:
                endingOfTheWord1 = "подобран";
                endingOfTheWord2 = "вентилятор";
                break;
            case 2:
            case 3:
            case 4:
                endingOfTheWord1 = "подобрано";
                endingOfTheWord2 = "вентилятора";
                break;
            default:
                endingOfTheWord1 = "подобраны";
                endingOfTheWord2 = "вентиляторов";
                break;
        }

        Console.WriteLine(
            $"\nВсего {endingOfTheWord1} {sortFans.Count} {endingOfTheWord2}."
        );

        foreach (var fan in sortFans)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {fan.ConditionalStandardSize * 1000:0}"
                    + $"\nНаименование: {fan.ProjectId}"
                    + $"\nОбъем воздуха, введенный пользователем: {userInput.UserInputWorkPoint.VolumeFlow} м3/ч;"
                    + $"\nПолное давление воздуха, введенное пользователем: {userInput.UserInputWorkPoint.TotalPressure} Па;"
                    + (
                        fan.NumberOfFans > 1
                            ? $"\nКоличество вентиляторов: {fan.NumberOfFans.ToString(CultureInfo.InvariantCulture)}"
                            : ""
                    )
                    + $"\nРасчетный объем воздуха: {fan.VolumeFlow:0} м3/ч;"
                    + $"\nРасчетное полное давление воздуха: {fan.TotalPressure:0} Па;"
                    + $"\nПогрешность подбора по объемному расходу воздуха: {fan.VolumeFlowDeviation: +0.0;-0.0;0} %;"
                    + $"\nПогрешность подбора по полному давлению воздуха: {fan.TotalPressureDeviation: +0.0;-0.0;0} %;"
                    + $"\n\nНаименование: {fan.Data.ExcelWorkSheetName}"
                    + $"\nРасход объемного воздуха на исходной кривой вентилятора: {fan.VolumeFlowOnPolynomial} м3/ч;"
                    + $"\nПолное давление воздуха на исходной кривой вентилятора: {fan.TotalPressureOnPolynomial} Па;"
                    + $"\n Номинальная скорость вращения крыльчатки {fan.Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint} об/мин;\n"
                    /*+ (FanLogic.Values)userInput.UserInputFan.FanLogic switch
                    {
                        FanLogic.Values.Logic1
                            => $"\nПогрешность подбора по объемному расходу воздуха: {fan.VolumeFlowDeviation: +0.0;-0.0;0} %;"
                                + $"\nПогрешность подбора по полному давлению воздуха: {fan.VolumeFlowAndTotalPressureDeviation: +0.0;-0.0;0} %;",
                        FanLogic.Values.Logic2
                            => $"\n\nРасход объемного воздуха на исходной кривой вентилятора: {fan
                        .VolumeFlowOnPolynomial} м3/ч;\nПолное давление воздуха на исходной кривой вентилятора: {fan
                        .TotalPressureOnPolynomial} Па;\n Номинальная скорость вращения крыльчатки {fan.Data.ImpellerRotationSpeed} об/мин;\n",
                        _
                            => throw new InvalidOperationException(
                                "Invalid FanLogic value."
                            )
                    }*/
                    + $"\nРасчетное статическое давление воздуха: {fan.StaticPressure:0} Па;"
                    + $"\nРасчетное динамическое давление воздуха: {fan.DynamicPressure:0} Па;"
                    + $"\nСкорость вращения крыльчатки: {fan.ImpellerRotationSpeedWithSlidingEngineForWorkPoint:0} об/мин;"
                    + $"\nЧастота вращения крыльчатки: {fan.ImpellerRotationFrequency:0} Гц;"
                    + $"\nРасчетная мощность в рабочей точке: {fan.Power:0.00} кВт;"
                    + $"\nРасчетный полный КПД вентилятора: {fan.TotalEfficiency:0.0} %;"
                    + $"\nСкорость воздуха в выпускной трубе: {fan.AirVelocityOfOutletPipeOpening:0.0} м/с;"
                    + $"\nНоминальная мощность: {fan.Data.NominalPower:0.00} кВт;"
                    + $"\nУровень звуковой мощности Lw5 на входе по октавам: {Calculate.GetOctaveNoiseAString(fan.OctaveNoiseLw5)} [дБ];"
                    + $"\nСуммарный уровень звуковой мощности Lw5 частот: {string.Join("; ", OctaveNoise.Names)} [Гц]  с корректировкой фильтра А на входе: {fan.SumNoiseLwA5:0.0} [дБ(А)];"
                    + $"\nУровень звуковой мощности Lw6 на выходе по октавам: {Calculate.GetOctaveNoiseAString(fan.OctaveNoiseLw6)} [дБ];"
                    + $"\nСуммарный уровень звуковой мощности Lw6 частот: {string.Join("; ", OctaveNoise.Names)} [Гц]  с корректировкой фильтра А на выходе: {fan.SumNoiseLwA6:0.0} [дБ(А)];"
                    + $"\nВес вентилятора {fan.Data.Weight} кг;"
                    + $"\n\nD = {fan.NewCurve[0].DcDiameterOfTheImpellerAtTheEndsOfTheBlades * 1000:0} [мм], "
                    + $"p = {fan.NewCurve[0].DcAir:0.000} [кг/м3], "
                    + $"n = {fan.NewCurve[0].DcImpellerRotationSpeed:0} [об/мин]:\n"
            );

            fan.NewCurve
                .ToList()
                .ForEach(
                    f =>
                        Console.WriteLine(
                            $"{f.DcIndex + 1}: "
                                + $"Q = {(fan.NumberOfFans > 1 ? $"{fan.NumberOfFans.ToString(CultureInfo.InvariantCulture)} * " : "")}{f.DcVolumeFlow} [м3/ч],"
                                + $"Pv = {f.DcTotalPressure} [Па],"
                                + $"N = {(fan.NumberOfFans > 1 ? $"{fan.NumberOfFans.ToString(CultureInfo.InvariantCulture)} * " : "")}{f.DcPower:0.00} [кВт],"
                                + $"Total Efficiency = {f.DcEfficiency:0.0} [%]"
                        )
                );
        }
    }
}

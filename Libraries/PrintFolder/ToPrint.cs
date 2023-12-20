using Libraries.Description_of_objects.UserInput;
using Libraries.Fans;

namespace Libraries.ToPrint;

public static class ToPrint
{
    public static void Print<T>(List<T> sortFans, UserInput userInput) where T : IFan
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

        Console.WriteLine($"\nВсего {endingOfTheWord1} {sortFans.Count} {endingOfTheWord2}.");

        foreach (var fan in sortFans)
        {
            Console.WriteLine(
                $"\n\nТипоразмер: {fan.Size}"
                + $"\nНаименование: {fan.ProjectId}"
                + $"\nОбъем воздуха, введенный пользователем: {userInput.UserInputWorkPoint.VolumeFlow} м3/ч;"
                + $"\nПолное давление воздуха, введенное пользователем: {userInput.UserInputWorkPoint.TotalPressure} Па;"
                + $"\nРасчетный объем воздуха: {fan.VolumeFlow} Па;"
                + $"\nРасчетное полное давление воздуха: {fan.TotalPressure} Па;"
                + $"\nПогрешность подбора по объемному расходу воздуха: {fan.VolumeFlowDeviation: +0.0;-0.0;0} %;"
                + $"\nПогрешность подбора по полному давлению воздуха: {fan.TotalPressureDeviation: +0.0;-0.0;0} %;"
                + $"\nРасчетное статическое давление воздуха: {fan.StaticPressure} Па;"
                + $"\nСкорость вращения крыльчатки: {fan.ImpellerRotationSpeed} об/мин;"
                + $"\nРасчетная мощность в рабочей точке: {fan.Power} кВт;"
                + $"\nРасчетный полный КПД вентилятора: {fan.Efficiency} %;"
                + $"\nСкорость воздуха: {fan.AirVelocity} м/с;"
                + $"\nНоминальная мощность: {fan.Data.NominalPower} кВт;"
                + $"\nУровень звуковой мощности на выходе по октавам: {fan.OctaveNoise63:0.0}; {fan.OctaveNoise125:0.0}; {fan
                    .OctaveNoise250:0.0}; {fan.OctaveNoise500:0.0}; {fan.OctaveNoise1000:0.0}; {fan.OctaveNoise2000:0.0}; {fan.OctaveNoise4000:0.0}; {fan.OctaveNoise8000:0.0} [дБ];"
                + $"\nСуммарный уровень звуковой мощности частот: 63; 125; 250; 500; 1к; 2к; 4к; 8к [Гц]  с корректировкой фильтра А на выходе: {fan.SumNoiseA:0.0} [дБ(А)]"
            );
        }
    }
}

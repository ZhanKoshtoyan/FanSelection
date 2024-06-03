using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Methods;
using Libraries.StructureOfObjects;

namespace Libraries.ValidateAndSort;

public abstract class SortFans2
{
    /// <summary>
    ///     В этом методе происходит проверка находится ли объем воздуха, который ввел пользователь, в диапазоне
    ///     производительности вентилятора. Если находится, то вентилятор добавляется в список. Следом происходит вторая
    ///     проверка: если допустимая погрешность подбора по полному давлению воздуха, которую ввел пользователь, удовлетворяет
    ///     выччисленную погрешность, то такой вентилятор и все его вычисленные свойства добавляется в список.
    /// </summary>
    /// <param name="fansList"></param>
    /// <param name="userInput"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<T> Sort<T>(List<FanData>? fansList, UserInput userInput)
        where T : IFan
    {
        //------------------------------------------------------------------------------------------------------------
        // Отбросим все FanData, которые выходят за пределы значений минимального и максимального значений VolumeFlow
        var correctFansList = fansList!
            .Where(
                f =>
                    userInput.UserInputWorkPoint.VolumeFlow / userInput.UserInputFan.NumberOfFans >= f.MinVolumeFlow
                    && userInput.UserInputWorkPoint.VolumeFlow / userInput.UserInputFan.NumberOfFans
                        <= f.MaxVolumeFlow
            )
            .ToList();

        if (correctFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {userInput.UserInputWorkPoint.VolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Количество вентиляторов в списке = 0"
            );
        }

        //------------------------------------------------------------------------------------------------------------
        if (userInput.UserInputFan.Size != 0)
        {
            correctFansList = correctFansList
                .Where(
                    f =>
                        userInput.UserInputFan.Size.ToString("D3") == f.Size
                )
                .ToList();
        }

        if (
            !string.IsNullOrEmpty(
                userInput.UserInputFan.ImpellerRotationDirection
            )
        )
        {
            correctFansList = correctFansList
                .Where(
                    f =>
                        f.ImpellerRotationDirection != null && f.ImpellerRotationDirection.Contains(userInput.UserInputFan.ImpellerRotationDirection)
                )
                .ToList();
        }

        if (userInput.UserInputFan.NominalPower != 0)
        {
            correctFansList = correctFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.UserInputFan.NominalPower
                                - f.NominalPower
                        ) < 0.05
                )
                .ToList();
        }

        if (userInput.UserInputFan.NominalImpellerRotationSpeed != 0)
        {
            correctFansList = correctFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.UserInputFan.NominalImpellerRotationSpeed
                                - f.NominalImpellerRotationSpeed
                        ) < 0.05
                )
                .ToList();
        }

        if (userInput.UserInputFan.FanBodyLength != 0)
        {
            correctFansList = correctFansList.Where(fan => fan
            .FanBodyLength?.Contains(userInput.UserInputFan.FanBodyLength) == true
            ).ToList();
        }

        //------------------------------------------------------------------------------------------------------------

        var fanTypeVersion = (FanVersion.Values)
            userInput.UserInputFan.FanVersion;

        List<T> fansTypeList =
            new(
                correctFansList
                    .Select(
                        elementFanData =>
                            fanTypeVersion switch
                            {
                                FanVersion.Values.OsuDu
                                    => (T) (object)new OsuDu(elementFanData, userInput),
                                FanVersion.Values.EuFan
                                    => (T) (object)new EuFan(elementFanData, userInput),
                                _ => throw new ArgumentOutOfRangeException($"Версии вентилятора с индексом {fanTypeVersion} не существует!")
                            }
                            /*fanTypeVersion == 0
                                ? (T)
                                    (object)new OsuDu(elementFanData, userInput)
                                : (T)
                                    (object)new EuFan(elementFanData, userInput)*/
                    )
                    .Where(fan => fan.Data.Version == fanTypeVersion.ToString())
                    .ToList()
            );

        //------------------------------------------------------------------------------------------------------------
        const double specificSpeedDeviation = 0.2;

        List<T>? fansTypeLogic = null;
        switch (userInput.UserInputFan.FanLogic)
        {
            case 0:
                //По быстроходности
                IEnumerable<(
                    double specificSpeed,
                    double specificSpeedEfficiencyMax,
                    double specificSpeedDevation,
                    T data
                )> numberOfHits = fansTypeList
                    .Select(
                        tFan =>
                            (
                                //Запишем быстроходность искомой рабочей точки
                                specificSpeed: tFan.SpecificSpeedCoefficientWithImpellerRotationSpeed,
                                //Запишем быстроходность при максимальном полном КПД
                                specificSpeedEfficiencyMax: tFan.SpecificSpeedEfficiencyMax,
                                //Посчитаем отклонение быстроходности при максимальном полном КПД от быстроходности искомой рабочей точки
                                specificSpeedDevation: Calculate.Share(
                                    tFan.SpecificSpeedEfficiencyMax,
                                    tFan.SpecificSpeedCoefficientWithImpellerRotationSpeed
                                ),
                                //Объект FanData
                                data: tFan
                            )
                    )
                    //Отбор объектов FanData удовлетворяющих условиям:
                    //Минимальная быстроходность FanData <= быстроходность рабочей точки (она различна для разной ImpellerRotationSpeed) <= Максимальная быстроходность FanData
                    .Where(
                        item =>
                            item.specificSpeed >= item.data.SpecificSpeedPhiMin
                            && item.specificSpeed
                                <= item.data.SpecificSpeedPhiMax
                    )
                    //Отбор объектов FanData удовлетворяющих условиям:
                    //Быстроходность FanData * 0,8 <= Быстроходность FanData <= Быстроходность FanData * 1,2;
                    .Where(
                        item =>
                            item.specificSpeed
                                >= (1 - specificSpeedDeviation)
                                    * item.specificSpeed
                            && item.specificSpeed
                                <= (1 + specificSpeedDeviation)
                                    * item.specificSpeed
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSpeedDevation)
                    .ToList();

                fansTypeLogic = numberOfHits.Select(nh => nh.data).ToList();
                break;
            case 1:
                //По габаритности
                IEnumerable<(
                    double specificSize,
                    double specificSizeEfficiencyMax,
                    double specificSizeDevation,
                    T data
                )> numberOfHits2 = fansTypeList
                    .Select(
                        tFan =>
                            (
                                //Запишем габаритность искомой рабочей точки
                                specificSize: tFan.SpecificSizeCoefficientWithRequiredSize,
                                //Запишем габаритность при максимальном полном КПД
                                specificSizeEfficiencyMax: tFan.SpecificSizeEfficiencyMax,
                                //Посчитаем отклонение габаритность при максимальном полном КПД от габаритность искомой рабочей точки
                                specificSizeDevation: Calculate.Share(
                                    tFan.SpecificSizeEfficiencyMax,
                                    tFan.SpecificSizeCoefficientWithRequiredSize
                                ),
                                //Объект FanData
                                data: tFan
                            )
                    )
                    //Отбор объектов FanData удовлетворяющих условиям:
                    //Минимальная габаритность FanData <= габаритность рабочей точки (она различна для разной ImpellerRotationSpeed) <= Максимальная габаритность FanData
                    .Where(
                        item =>
                            item.specificSize <= item.data.SpecificSizePhiMin
                            && item.specificSize >= item.data.SpecificSizePhiMax
                    )
                    //Отбор объектов FanData удовлетворяющих условиям:
                    //Габаритность FanData * 0,8 <= Габаритность FanData <= Габаритность FanData * 1,2;
                    .Where(
                        item =>
                            item.specificSize
                            >= (1 - specificSpeedDeviation)
                            * item.specificSize
                            && item.specificSize
                            <= (1 + specificSpeedDeviation)
                            * item.specificSize
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSizeDevation)
                    .ToList();

                fansTypeLogic = numberOfHits2.Select(nh => nh.data).ToList();
                break;
        }

        //По пересечению графиков
        fansTypeLogic = (
            fansTypeLogic
            ?? throw new InvalidOperationException(
                $"В результате исключения по {(FanLogic.Values)userInput.UserInputFan.FanLogic switch
                {
                    FanLogic.Values.Logic1
                        => "быстроходности",
                    FanLogic.Values.Logic2
                        => "габаритности",
                    _ => throw new ArgumentOutOfRangeException($"Ошибка - логика отсутствует")
                }} список вентиляторов пуст"
            )
        )
            //Отбор объектов FanData удовлетворяющих условиям:
            //Минимальная частота вращения FanData <= Частота вращения FanData <= Максимальная частота вращения FanData;
            .Where(
                item =>
                    item.ImpellerRotationFrequency
                    >= item.MinImpellerRotationFrequency
                    && item.ImpellerRotationFrequency
                    <= item.MaxImpellerRotationFrequency
            )
            .Where(
                fan =>
                    Math.Abs(fan.TotalPressureDeviation)
                    <= userInput.UserInputWorkPoint.TotalPressureDeviation
            )
            .OrderBy(fan => Math.Abs(fan.TotalPressureDeviation))
            .ThenBy(fan => Math.Abs(fan.VolumeFlowDeviation))
            .ToList();

        if (fansTypeLogic is null)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInput.UserInputWorkPoint.TotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        return new List<T>(fansTypeLogic);
    }
}

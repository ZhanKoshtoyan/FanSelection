using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;
using Libraries.Fans;

namespace Libraries.Validate_and_sort;

public abstract class SortFans
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
    public static List<T> Sort<T>(List<FanData>? fansList, UserInput userInput) where T : IFan
    {
        var sortInputFansList = fansList!
            .Where(
                f =>
                    userInput.UserInputWorkPoint.VolumeFlow >= f.MinVolumeFlow
                    &&
                    userInput.UserInputWorkPoint.VolumeFlow <= f.MaxVolumeFlow
            )
            .ToList();

        if (sortInputFansList.Count == 0)
        {
            throw new ArgumentException(
                $"Объем воздуха {userInput.UserInputWorkPoint.VolumeFlow} [м3/ч] выходит за границы производительности вентиляторов. Количество вентиляторов в списке = 0"
            );
        }

        //------------------------------------------------------------------------------------------------------------
        if (userInput.UserInputFan.Size != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInput.UserInputFan.Size
                            .GetValueOrDefault()
                            .ToString("D3") == f.Size
                )
                .ToList();
        }

        if (
            !string.IsNullOrEmpty(
                userInput.UserInputFan.ImpellerRotationDirection
            )
            || ReferenceEquals(
                userInput.UserInputFan.ImpellerRotationDirection,
                ImpellerRotationDirections.Values.GetValue(2)
            )
        )
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        userInput.UserInputFan.ImpellerRotationDirection
                        == f.ImpellerRotationDirection
                )
                .ToList();
        }

        if (userInput.UserInputFan.NominalPower != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.UserInputFan.NominalPower.GetValueOrDefault()
                            - f.NominalPower
                        ) < 0.05
                )
                .ToList();
        }

        if (userInput.UserInputFan.ImpellerRotationSpeed != 0)
        {
            sortInputFansList = sortInputFansList
                .Where(
                    f =>
                        Math.Abs(
                            userInput.UserInputFan.ImpellerRotationSpeed.GetValueOrDefault()
                            - f.NominalImpellerRotationSpeed
                        ) < 0.05
                )
                .ToList();
        }

        //------------------------------------------------------------------------------------------------------------

        List<T>? sortDeviationFansList = null;

        switch (userInput.UserInputFan.FanVersion)
        {
            case 0:
                sortDeviationFansList = new List<T>((IEnumerable<T>) sortInputFansList
                    .Select(
                        elementFanData =>
                            new OsuDu(elementFanData, userInput)
                    )
                    .Where(fan => ((IFan) fan).Data.Version == FanVersion.Values.OsuDu.ToString())
                    .Where(
                        fan =>
                            Math.Abs(((IFan) fan).TotalPressureDeviation)
                            <= userInput
                                .UserInputWorkPoint
                                .TotalPressureDeviation
                    )
                    .ToList()
                );
                break;
            case 1:
                sortDeviationFansList = new List<T>((IEnumerable<T>) sortInputFansList
                    .Select(
                        elementFanData =>
                            new EuFan(elementFanData, userInput)
                    )
                    .Where(fan => ((IFan) fan).Data.Version == FanVersion.Values.EuFan.ToString())
                    .Where(
                        fan =>
                            Math.Abs(((IFan) fan).TotalPressureDeviation)
                            <= userInput
                                .UserInputWorkPoint
                                .TotalPressureDeviation
                    )
                    .ToList()
                );
                break;
        }

        if (sortDeviationFansList is null)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInput.UserInputWorkPoint.TotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        var orderedNumbers = sortDeviationFansList.OrderBy(
            fan => Math.Abs(fan.TotalPressureDeviation)
        );

        return new List<T>(orderedNumbers);
    }
}

using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Loader;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using InvalidDataException = System.IO.InvalidDataException;

namespace Libraries.ValidateAndSort;

public abstract class CreatingListOfFans
{
    public static List<T> Create<T>(
        List<FanData>? fansList,
        UserInput userInput
    )
        where T : AbstractFan
    {
        if (fansList == null)
        {
            throw new InvalidDataException($"Список объектов FanData пуст");
        }

        var fanTypeVersion = (
            (FanVersion.Values)userInput.UserInputFan.FanVersion
        ).ToString();

        //1.Выбрать вентилятор, который соответствует типу fanVersion

        //1.1 Получаем список с кратким описанием вентиляторов из ShortDescriptionOfTheFans.json файла
        var shortDescriptionOfTheFanDataList =
            JsonLoader.Download<ShortDescriptionOfTheFanData>(
                UserInput.PathShortDescriptionOfTheFansJsonFile
            );

        // var stopwatch = Stopwatch.StartNew();
        // stopwatch.Start();
        //1.2 Создадим список объектов FanData с исполнением Version
        IEnumerable<ShortDescriptionOfTheFanData> sortedShortDescriptionOfTheFanDataList =
            (
                shortDescriptionOfTheFanDataList
                ?? throw new InvalidOperationException(
                    "shortDescriptionOfTheFanDataList == null"
                )
            )
                .Where(s => s.Version == fanTypeVersion)
                .ToList()
            ?? throw new Exception(
                $"В shortDescriptionOfTheFanDataList не найдено ни 1 объекта типа "
                    + $"{fanTypeVersion}"
            );
        // stopwatch.Stop();
        // Console.WriteLine(
        //     $"Время выполнения SomeMethod: {stopwatch.ElapsedMilliseconds} мс"
        // );

        //2.Выбрать вентилятор с исполнением impellerRotationDirection
        if (
            !string.IsNullOrEmpty(
                userInput.UserInputFan.ImpellerRotationDirection
            )
        )
        {
            sortedShortDescriptionOfTheFanDataList =
                sortedShortDescriptionOfTheFanDataList
                    .Where(
                        f =>
                            f.ImpellerRotationDirection?.Contains(
                                userInput.UserInputFan.ImpellerRotationDirection
                            ) == true
                    )
                    .ToList();
        }
        //3.Выбрать вентилятор с длиной корпуса fanBodyLength
        if (userInput.UserInputFan.FanBodyLength != 0)
        {
            sortedShortDescriptionOfTheFanDataList =
                sortedShortDescriptionOfTheFanDataList
                    .Where(
                        f =>
                            f.FanBodyLength?.Contains(
                                userInput.UserInputFan.FanBodyLength
                            ) == true
                    )
                    .ToList();
        }

        //4.Выбрать вентилятор с типоразмером ConditionalStandardSize
        if (userInput.UserInputFan.ConditionalStandardSize != 0)
        {
            sortedShortDescriptionOfTheFanDataList =
                sortedShortDescriptionOfTheFanDataList
                    .Where(
                        f =>
                            Math.Abs(
                                f.ConditionalStandardSize
                                    - userInput
                                        .UserInputFan
                                        .ConditionalStandardSize
                            ) < 0.05
                    )
                    .ToList();
        }

        //5.Выбрать вентилятор со скоростью вращения крыльчатки nominalImpellerRotationSpeedWithoutSlidingEngine
        if (
            userInput
                .UserInputFan
                .NominalImpellerRotationSpeedWithoutSlidingEngine != 0
        )
        {
            sortedShortDescriptionOfTheFanDataList =
                sortedShortDescriptionOfTheFanDataList
                    .Where(
                        f =>
                            Math.Abs(
                                userInput
                                    .UserInputFan
                                    .NominalImpellerRotationSpeedWithoutSlidingEngine
                                    - f.NominalImpellerRotationSpeedWithoutSlidingEngine
                            ) < 0.05
                    )
                    .ToList();
        }

        //6.Выбрать вентилятор с номинальной мощностью nominalPower
        if (userInput.UserInputFan.NominalPower != 0)
        {
            sortedShortDescriptionOfTheFanDataList =
                sortedShortDescriptionOfTheFanDataList
                    .Where(
                        f =>
                            Math.Abs(
                                userInput.UserInputFan.NominalPower
                                    - f.NominalPower
                            ) < 0.05
                    )
                    .ToList();
        }

        //7. Выбрать максимальное значение NumberOfFans, для которого будут созданы сочетания одновременно работающих вентиляторов

        var numberOfFansForCreate = userInput.UserInputFan.NumberOfFans switch
        {
            0 => NumberOfFans.Values.Length,
            _ => userInput.UserInputFan.NumberOfFans
        };

        //8. Запишем в список перечень АСВ всех вентиляторов
        var aerodynamicDesignList = sortedShortDescriptionOfTheFanDataList
            .Select(fan => fan.AerodynamicDesign)
            .Distinct()
            .ToList();

        //9. Создадим список с исполнением вентилятора FanData и аэродинамической схемой вентилятора AerodynamicDesign
        var newFanDataList = new List<FanData>();

        for (
            var aerodynamicDesignNumber = aerodynamicDesignList[0];
            aerodynamicDesignNumber <= aerodynamicDesignList.Count;
            aerodynamicDesignNumber++
        )
        {
            //9.1. Выбираем первый вентилятор с Version и AerodynamicDesign из Fans.json файла
            var fanDataWithVersionAndAerodynamicDesign = fansList
                .Where(fan => fan.Version == fanTypeVersion)
                .Where(fan => fan.AerodynamicDesign == aerodynamicDesignNumber)
                .Distinct()
                .First();

            //9.2. Создаем экземпляры вентиляторов из списка List<ShortDescriptionOfTheFanData> на основе fanDataWithVersionAndAerodynamicDesign
            IEnumerable<FanData> newFanDataListTemporary =
                sortedShortDescriptionOfTheFanDataList
                    .Where(s => s.Version == fanTypeVersion)
                    .Where(s => s.AerodynamicDesign == aerodynamicDesignNumber)
                    .Select(
                        el =>
                            Calculate.CreateFanData(
                                fanDataWithVersionAndAerodynamicDesign,
                                el.ConditionalStandardSize,
                                el.Weight,
                                el.NominalPower,
                                el.NominalImpellerRotationSpeedWithoutSlidingEngine,
                                el.MaxImpellerRotationSpeedWithSlidingEngine,
                                el.AreaOfInletPipeOpening
                            )
                    )
                    .ToList();
            newFanDataList.AddRange(newFanDataListTemporary);
        }

        //10.Создадим список с типом вентилятора OsuDu или EuFan
        var valueOfFanVersion = Convert.ToInt32(
            (FanVersion.Values)userInput.UserInputFan.FanVersion
        );

        var multipleTypedFansList = new List<T>();

        for (
            var numberOfFans = 1;
            numberOfFans <= numberOfFansForCreate;
            numberOfFans++
        )
        {
            var multipleTypedFansListTemporary = newFanDataList
                .Select(
                    elementFanData =>
                        valueOfFanVersion switch
                        {
                            0
                                => (T)
                                    (object)
                                        new OsuDu(
                                            elementFanData,
                                            userInput,
                                            numberOfFans
                                        ),
                            1
                                => (T)
                                    (object)
                                        new EuFan(
                                            elementFanData,
                                            userInput,
                                            numberOfFans
                                        ),
                            _
                                => throw new ArgumentOutOfRangeException(
                                    $"Версии вентилятора с индексом {valueOfFanVersion} не существует!"
                                )
                        }
                )
                .ToList();
            multipleTypedFansList.AddRange(multipleTypedFansListTemporary);
        }

        //11.Отобрать вентиляторы, которые соответствуют specificSpeedPhiCoefficients или specificSizePhiCoefficients

        var fanLogicNumber = userInput.UserInputFan.FanLogic;
        var valueOfFanLogic = (FanLogic.Values)userInput.UserInputFan.FanLogic;
        List<T>? listOfFansByTypeAndLogic = null;
        switch (fanLogicNumber)
        {
            case 0:
                //11.1. По быстроходности
                var specificSpeedList1 = multipleTypedFansList
                    .Select(
                        tFan =>
                            (
                                fanSize: tFan.ConditionalStandardSize,
                                numberOfFansIteration: tFan.NumberOfFans,
                                //Запишем быстроходность искомой рабочей точки по размерным характеристикам. Скорость вращения крыльчатки такая, при которой был посчитан SpeedCoefficient по безразмерным характеристикам
                                specificSpeed: tFan.SpecificSpeedCoefficientWithImpellerRotationSpeed,
                                //Запишем быстроходность при максимально полном КПД
                                specificSpeedEfficiencyMax: tFan.SpecificSpeedEfficiencyMax,
                                //11.1.1. Посчитаем отклонение быстроходности при максимальном полном КПД от быстроходности искомой рабочей точки
                                specificSpeedDeviation: Calculate.Share(
                                    tFan.SpecificSpeedEfficiencyMax,
                                    tFan.SpecificSpeedCoefficientWithImpellerRotationSpeed
                                ),
                                //Объект FanData
                                data: tFan
                            )
                    )
                    .ToList();
                //11.1.2. Отбор объектов FanData удовлетворяющих условиям: Быстроходность FanData * specificDeviationLeft <= Быстроходность FanData <= Быстроходность FanData * specificDeviationRight;
                var specificSpeedList2 = specificSpeedList1
                    .Where(
                        item =>
                            item.specificSpeedDeviation switch
                            {
                                < 0
                                    => Math.Abs(item.specificSpeedDeviation)
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationLeft / 100,
                                >= 0
                                    => item.specificSpeedDeviation
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationRight / 100,
                                _
                                    => throw new Exception(
                                        $"SpecificSpeedDeviation неправильно обработано"
                                    )
                            }
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSpeedDeviation)
                    .ToList();

                listOfFansByTypeAndLogic = specificSpeedList2
                    .Select(nh => nh.data)
                    .ToList();
                break;
            case 1:
                //11.2. По габаритности
                var specificSizeList1 = multipleTypedFansList.Select(
                    tFan =>
                        (
                            //Запишем габаритность искомой рабочей точки
                            specificSize: tFan.SpecificSizeCoefficientWithRequiredSize,
                            //Запишем габаритность при максимальном полном КПД
                            specificSizeEfficiencyMax: tFan.SpecificSizeEfficiencyMax,
                            //11.2.1. Посчитаем отклонение габаритность при максимальном полном КПД от габаритность искомой рабочей точки
                            specificSizeDeviation: Calculate.Share(
                                tFan.SpecificSizeEfficiencyMax,
                                tFan.SpecificSizeCoefficientWithRequiredSize
                            ),
                            //Объект FanData
                            data: tFan
                        )
                );
                //11.2.2. Отбор объектов FanData удовлетворяющих условиям: Габаритность FanData * specificDeviationLeft <= Габаритность FanData <= Габаритность FanData * specificDeviationRight;
                var specificSizeList2 = specificSizeList1
                    .Where(
                        item =>
                            item.specificSizeDeviation switch
                            {
                                < 0
                                    => Math.Abs(item.specificSizeDeviation)
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationLeft / 100,
                                >= 0
                                    => item.specificSizeDeviation
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationRight / 100,
                                _
                                    => throw new Exception(
                                        $"SpecificSpeedDeviation неправильно обработано"
                                    )
                            }
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSizeDeviation)
                    .ToList();

                listOfFansByTypeAndLogic = specificSizeList2
                    .Select(nh => nh.data)
                    .ToList();
                break;
        }

        //12. По пересечению графиков
        var listOfFansByTypeAndLogic1 = (
            listOfFansByTypeAndLogic
            ?? throw new InvalidOperationException(
                $"В результате исключения по {valueOfFanLogic switch
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
            .ToList();

        //TODO Для вентилятора с ПЧ это бессмысленный параметр. Для вентилятора без ПЧ нужно проверить.
        listOfFansByTypeAndLogic = listOfFansByTypeAndLogic1
            .Where(
                fan =>
                    Math.Abs(fan.TotalPressureDeviation)
                    <= userInput.UserInputWorkPoint.TotalPressureDeviation
            )
            .OrderByDescending(fan => fan.TotalEfficiency)
            .ThenBy(fan => Math.Abs(fan.TotalPressureDeviation))
            .ThenBy(fan => Math.Abs(fan.VolumeFlowDeviation))
            .ToList();

        if (listOfFansByTypeAndLogic is null)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInput.UserInputWorkPoint.TotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }

        return new List<T>(listOfFansByTypeAndLogic);
    }
}

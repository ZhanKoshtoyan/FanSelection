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
    public static async Task<List<T>> CreateAsync<T>(
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
            await JsonLoader.DownloadAsync<ShortDescriptionOfTheFanData>(
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


        sortedShortDescriptionOfTheFanDataList =
            sortedShortDescriptionOfTheFanDataList
                //2.Выбрать вентилятор с исполнением impellerRotationDirection
                .Where(!string.IsNullOrEmpty(userInput.UserInputFan.ImpellerRotationDirection),
                    f => f.ImpellerRotationDirection?.Contains(
                        userInput.UserInputFan.ImpellerRotationDirection ?? string.Empty
                    ) == true
                )
                //3.Выбрать вентилятор с длиной корпуса fanBodyLength
                .Where(userInput.UserInputFan.FanBodyLength != 0,
                    f =>
                        f.FanBodyLength?.Contains(userInput.UserInputFan.FanBodyLength) == true)
                //4.Выбрать вентилятор с типоразмером ConditionalStandardSize
                .Where(userInput.UserInputFan.ConditionalStandardSize != 0,
                    f =>
                        Math.Abs(
                            f.ConditionalStandardSize
                            - userInput
                                .UserInputFan
                                .ConditionalStandardSize
                        ) < 0.05)
                //5.Выбрать вентилятор со скоростью вращения крыльчатки nominalImpellerRotationSpeedWithoutSlidingEngine
                .Where(userInput
                        .UserInputFan
                        .NominalImpellerRotationSpeedWithoutSlidingEngine != 0,
                    f =>
                        Math.Abs(
                            userInput
                                .UserInputFan
                                .NominalImpellerRotationSpeedWithoutSlidingEngine
                            - f.NominalImpellerRotationSpeedWithoutSlidingEngine
                        ) < 0.05)
                //6.Выбрать вентилятор с номинальной мощностью nominalPower
                .Where(userInput.UserInputFan.NominalPower != 0,
                    f =>
                        Math.Abs(
                            userInput.UserInputFan.NominalPower
                            - f.NominalPower
                        ) < 0.05)
                .ToList();

        /*//2.Выбрать вентилятор с исполнением impellerRotationDirection
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
        }*/

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

        //*
        var fanEfficiencyGradeList = new FanEfficiencyGradeCollection
        {
            FanEfficiencyGrades =
                await JsonLoader.DownloadAsync<FanEfficiencyGrade>(
                    UserInput.PathFanEfficiencyGradeJsonFile
                )
        };

        if (fanEfficiencyGradeList.FanEfficiencyGrades == null)
        {
            throw new Exception(
                "Список fanEfficiencyGradeList.FanEfficiencyGrades == null"
            );
        }

        for (
            var aerodynamicDesignNumber = aerodynamicDesignList[0];
            aerodynamicDesignNumber
                <= aerodynamicDesignList[0] + aerodynamicDesignList.Count - 1;
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

            var newFanDataListTemporary = sortedShortDescriptionOfTheFanDataList
                .Where(
                    s =>
                        s.Version == fanTypeVersion
                        && s.AerodynamicDesign == aerodynamicDesignNumber
                )
                .Select(
                    el =>
                        Calculate.CreateFanData(
                            fanDataWithVersionAndAerodynamicDesign,
                            fanEfficiencyGradeList,
                            el.ConditionalStandardSize,
                            el.DiameterOfTheImpellerAtTheEndsOfTheBlades,
                            el.Weight,
                            el.NominalPower,
                            el.NominalImpellerRotationSpeedWithoutSlidingEngine,
                            Math.Abs(
                                el.ImpellerRotationSpeedWithSlidingEngineForWorkPoint
                                    - el.MaxImpellerRotationSpeedWithSlidingEngine
                            ) < 0.05
                                ? el.MaxImpellerRotationSpeedWithSlidingEngine
                                : fanDataWithVersionAndAerodynamicDesign.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            el.MaxImpellerRotationSpeedWithSlidingEngine,
                            el.SquareOfOutletPipeOpening
                        )
                )
                .ToList();
            newFanDataList.AddRange(newFanDataListTemporary);
        }

        //10.Создадим список с исполнением вентилятора, который выбрал пользователь
        //Запишем исполнение вентилятора в переменную
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
            var multipleTypedFansListTemporary1 = newFanDataList
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
                            2
                                => (T)
                                    (object)
                                        new HighPressureFan(
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

            multipleTypedFansList.AddRange(multipleTypedFansListTemporary1);
        }

        //11.Отобрать вентиляторы, которые соответствуют specificSpeedPhiCoefficients или specificSizePhiCoefficients

        var fanLogicNumber = userInput.UserInputFan.FanLogic;
        var valueOfFanLogic = (FanLogic.Values)userInput.UserInputFan.FanLogic;
        List<T>? listOfFansByTypeAndLogic = null;
        switch (fanLogicNumber)
        {
            case 0:
                //11.1. По быстроходности
                var specificSpeedList0 = multipleTypedFansList
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
                                specificSpeedDeviation: Calculate.Deviation(
                                    tFan.SpecificSpeedCoefficientWithImpellerRotationSpeed,
                                    tFan.SpecificSpeedEfficiencyMax
                                ),
                                //Объект FanData
                                data: tFan
                            )
                    )
                    .ToList();
                //11.1.2. Отбор объектов FanData удовлетворяющих условиям: Быстроходность FanData * specificDeviationLeft <= Быстроходность FanData <= Быстроходность FanData * specificDeviationRight;
                var specificSpeedList1 = multipleTypedFansList
                    .Where(
                        item =>
                            item.SpecificSpeedDeviation switch
                            {
                                < 0
                                    => Math.Abs(item.SpecificSpeedDeviation)
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationLeft,
                                >= 0
                                    => item.SpecificSpeedDeviation
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationRight,
                                _
                                    => throw new Exception(
                                        $"SpecificSpeedDeviation неправильно обработано"
                                    )
                            }
                    )
                    .OrderByDescending(item => item.TotalEfficiency)
                    .ThenBy(item => Math.Abs(item.SpecificSpeedDeviation))
                    .ToList();

                listOfFansByTypeAndLogic = specificSpeedList1.ToList();
                break;
            case 1:
                /*//11.2. По габаритности
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
                );*/
                //11.2.2. Отбор объектов FanData удовлетворяющих условиям: Габаритность FanData * specificDeviationLeft <= Габаритность FanData <= Габаритность FanData * specificDeviationRight;
                var specificSizeList1 = multipleTypedFansList
                    .Where(
                        item =>
                            item.SpecificSizeDeviation switch
                            {
                                < 0
                                    => Math.Abs(item.SpecificSizeDeviation)
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationLeft,
                                >= 0
                                    => item.SpecificSizeDeviation
                                        <= userInput
                                            .UserInputWorkPoint
                                            .SpecificDeviationRight,
                                _
                                    => throw new Exception(
                                        $"SpecificSpeedDeviation неправильно обработано"
                                    )
                            }
                    )
                    .OrderByDescending(item => item.TotalEfficiency)
                    .ThenBy(item => Math.Abs(item.SpecificSizeDeviation))
                    .ToList();

                listOfFansByTypeAndLogic = specificSizeList1.ToList();
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
        var listOfFansByTypeAndLogic2 = listOfFansByTypeAndLogic1.Where(
            fan =>
                Math.Abs(fan.TotalPressureDeviation)
                    <= userInput
                        .UserInputWorkPoint
                        .VolumeFlowAndTotalPressureDeviation
                && Math.Abs(fan.VolumeFlowDeviation)
                    <= userInput
                        .UserInputWorkPoint
                        .VolumeFlowAndTotalPressureDeviation
        );
        var listOfFansByTypeAndLogic3 = listOfFansByTypeAndLogic2
            .OrderBy(
                fan =>
                    Math.Abs(
                        Calculate.Share(
                            userInput.UserInputWorkPoint.VolumeFlow,
                            fan.VolumeFlow * fan.NumberOfFans
                        )
                            * Calculate.Share(
                                userInput.UserInputWorkPoint.TotalPressure,
                                fan.TotalPressure
                            )
                    )
            )
            .ThenBy(fan => Math.Abs(fan.SpecificSpeedDeviation))
            .ThenBy(fan => Math.Abs(fan.SpecificSizeDeviation))
            .ThenByDescending(fan => fan.TotalEfficiency)
            .ToList();

        listOfFansByTypeAndLogic = listOfFansByTypeAndLogic3;

        /*if (listOfFansByTypeAndLogic is null)
        {
            throw new ArgumentException(
                $"Условие не удовлетворяется: Погрешность подбора по полному давлению воздуха > {userInput.UserInputWorkPoint.VolumeFlowAndTotalPressureDeviation}%. Вентиляторы не могут быть подобраны."
            );
        }*/

        return new List<T>(listOfFansByTypeAndLogic);
    }
}

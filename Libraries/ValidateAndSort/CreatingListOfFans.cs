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

        var fanTypeVersion = (FanVersion.Values)
            userInput.UserInputFan.FanVersion;

        //1.Выбрать вентилятор, который соответствует типу fanVersion
        //1.1.Выбрать вентилятор, который соответствует типу fanVersion из Fans.json файла
        var correctFansList = fansList
            .Where(fan => fan.Version == fanTypeVersion.ToString())
            .ToList();

        //1.2 Получаем существующий объект для пересчета
        var existingFanData = correctFansList[0];

        //1.3 Получаем список с кратким описанием вентиляторов из ShortDescriptionOfTheFans.json файла
        var shortDescriptionOfTheFanDataList =
            JsonLoader.Download<ShortDescriptionOfTheFanData>(
                UserInput.PathShortDescriptionOfTheFansJsonFile
            );

        //1.4 Создадим список с типом вентилятора FanData
        IEnumerable<ShortDescriptionOfTheFanData> sortedShortDescriptionOfTheFanDataList =
            (
                shortDescriptionOfTheFanDataList
                ?? throw new InvalidOperationException(
                    "shortDescriptionOfTheFanDataList == null"
                )
            )
                .Where(s => s.Version == existingFanData.Version)
                .ToList()
            ?? throw new Exception(
                $"В shortDescriptionOfTheFanDataList не найдено ни 1 объекта типа "
                    + $"{existingFanData.Version}"
            );

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

        //4.Выбрать вентилятор с типоразмером size
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

        //7. Создадим список с типом вентилятора FanData
        IEnumerable<FanData> newFanDataList =
            sortedShortDescriptionOfTheFanDataList
                .Where(s => s.Version == existingFanData.Version)
                .Select(
                    el =>
                        Calculate.CreateFanData(
                            existingFanData,
                            el.ConditionalStandardSize,
                            el.Weight,
                            el.NominalPower,
                            el.NominalImpellerRotationSpeedWithoutSlidingEngine,
                            el.ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
                            el.MaxImpellerRotationSpeedWithSlidingEngine,
                            el.AreaOfInletPipeOpening
                        )
                )
                .ToList();

        //

        /*newFanDataList = newFanDataList
            .Where(
                f =>
                    userInput.UserInputWorkPoint.VolumeFlow
                        / userInput.UserInputFan.NumberOfFans
                        >= f.MinVolumeFlow * f.SimilarVolumeFlowCoefficient
                    && userInput.UserInputWorkPoint.VolumeFlow
                        / userInput.UserInputFan.NumberOfFans
                        <= f.MaxVolumeFlow * f.SimilarVolumeFlowCoefficient
            )
            .ToList();*/

        //9->8.Создадим список с типом вентилятора OsuDu или EuFan
        var valueOfFanVersion = (FanVersion.Values)
            userInput.UserInputFan.FanVersion;

        List<T> fansTypeList =
            new(
                newFanDataList
                    .Select(
                        elementFanData =>
                            valueOfFanVersion switch
                            {
                                FanVersion.Values.OsuDu
                                    => (T)
                                        (object)
                                            new OsuDu(
                                                elementFanData,
                                                userInput
                                            ),
                                FanVersion.Values.EuFan
                                    => (T)
                                        (object)
                                            new EuFan(
                                                elementFanData,
                                                userInput
                                            ),
                                _
                                    => throw new ArgumentOutOfRangeException(
                                        $"Версии вентилятора с индексом {valueOfFanVersion} не существует!"
                                    )
                            }
                    )
                    .ToList()
            );

        //8->9.Выбрать вентилятор где искомая точка находится в промежутке между minVolumeFlow и maxVolumeFlow

        /*var minMaxVolumeFlowFansTypeList1 = fansTypeList.SelectMany(
            f => NumberOfFans.Values.Select(v => new { TFan = f, Value = v })
        );

        var minMaxVolumeFlowFansTypeList2 = minMaxVolumeFlowFansTypeList1.Where(
            pair =>
                userInput.UserInputWorkPoint.VolumeFlow / pair.Value
                    >= pair.TFan.Data.MinVolumeFlow
                        * pair.TFan.Data.SimilarVolumeFlowCoefficient
                && userInput.UserInputWorkPoint.VolumeFlow / pair.Value
                    <= pair.TFan.Data.MaxVolumeFlow
                        * pair.TFan.Data.SimilarVolumeFlowCoefficient
        );

        fansTypeList = minMaxVolumeFlowFansTypeList2
            .Select(pair =>
            {
                pair.TFan.NumberOfFans = pair.Value;
                return pair.TFan;
            })
            .ToList();*/

        var minMaxVolumeFlowFansTypeList3 = new List<T>();

        foreach (var numberOfFans in NumberOfFans.Values)
        {
            var minMaxVolumeFlowFansTypeList1 = fansTypeList
                .Select(tFan =>
                {
                    tFan.NumberOfFans = numberOfFans;
                    return tFan;
                })
                .ToList();

            /*var minMaxVolumeFlowFansTypeList2 = minMaxVolumeFlowFansTypeList1
                .Where(
                    tFan =>
                        userInput.UserInputWorkPoint.VolumeFlow
                            / tFan.NumberOfFans
                            >= tFan.Data.MinVolumeFlow
                                * tFan.Data.SimilarVolumeFlowCoefficient
                        && userInput.UserInputWorkPoint.VolumeFlow
                            / tFan.NumberOfFans
                            <= tFan.Data.MaxVolumeFlow
                                * tFan.Data.SimilarVolumeFlowCoefficient
                )
                .ToList();*/

            minMaxVolumeFlowFansTypeList3.AddRange(
                minMaxVolumeFlowFansTypeList1
            );
        }

        fansTypeList = minMaxVolumeFlowFansTypeList3;

        //10.Отобрать вентиляторы, которые соответствуют specificSpeedPhiCoefficients или specificSizePhiCoefficients
        const double specificDeviation = 0.2;

        var fanLogicNumber = userInput.UserInputFan.FanLogic;
        var valueOfFanLogic = (FanLogic.Values)userInput.UserInputFan.FanLogic;
        List<T>? listOfFansByTypeAndLogic = null;
        switch (fanLogicNumber)
        {
            case 0:
                //10.1 По быстроходности
                var specificSpeedList1 = fansTypeList
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
                    );
                //Отбор объектов FanData удовлетворяющих условиям:
                //Быстроходность FanData * 0,8 <= Быстроходность FanData <= Быстроходность FanData * 1,2;
                IEnumerable<(
                    double specificSpeed,
                    double specificSpeedEfficiencyMax,
                    double specificSpeedDevation,
                    T data
                )> numberOfHits = specificSpeedList1
                    .Where(
                        item => item.specificSpeedDevation <= specificDeviation
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSpeedDevation)
                    .ToList();

                listOfFansByTypeAndLogic = numberOfHits
                    .Select(nh => nh.data)
                    .ToList();
                break;
            case 1:
                //10.2 По габаритности
                var specificSizeList1 = fansTypeList
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
                    .Where(
                        item =>
                            item.specificSize <= item.data.SpecificSizePhiMin
                            && item.specificSize >= item.data.SpecificSizePhiMax
                    );
                //Отбор объектов FanData удовлетворяющих условиям:
                //Минимальная габаритность FanData <= габаритность рабочей точки (она различна для разной ImpellerRotationSpeed) <= Максимальная габаритность FanData

                /*var sizeList2 = sizeList1.Where(
                    item =>
                        item.specificSize <= item.data.SpecificSizePhiMin
                        && item.specificSize >= item.data.SpecificSizePhiMax
                );*/
                //Отбор объектов FanData удовлетворяющих условиям:
                //Габаритность FanData * 0,8 <= Габаритность FanData <= Габаритность FanData * 1,2;
                IEnumerable<(
                    double specificSize,
                    double specificSizeEfficiencyMax,
                    double specificSizeDevation,
                    T data
                )> numberOfHits2 = specificSizeList1
                    .Where(
                        item => item.specificSizeDevation <= specificDeviation
                    )
                    .OrderByDescending(item => item.data.TotalEfficiency)
                    .ThenBy(item => item.specificSizeDevation)
                    .ToList();

                listOfFansByTypeAndLogic = numberOfHits2
                    .Select(nh => nh.data)
                    .ToList();
                break;
        }

        //10.3 По пересечению графиков
        listOfFansByTypeAndLogic = (
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

        listOfFansByTypeAndLogic = listOfFansByTypeAndLogic
            .Where(
                fan =>
                    Math.Abs(fan.TotalPressureDeviation)
                    <= userInput.UserInputWorkPoint.TotalPressureDeviation
            )
            .OrderBy(fan => Math.Abs(fan.TotalPressureDeviation))
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

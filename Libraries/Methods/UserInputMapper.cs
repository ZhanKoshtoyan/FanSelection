using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;

namespace Libraries.Methods;

/// <summary>
/// Маппер для преобразования UserInputFormData (строки с UI) в доменный объект UserInput.
/// 
/// Работает по принципу:
/// 1. Получает DTO с данными (как есть на форме - строки)
/// 2. Валидирует обязательные поля
/// 3. Парсит строки в типизированные значения
/// 4. Строит объекты UserInputWorkPoint, UserInputAir, UserInputFan
/// 5. Возвращает готовый объект UserInput
/// </summary>
public class UserInputMapper
{
    private readonly IStringParser _stringParser;

    /// <summary>
    /// Инициализирует маппер с парсером.
    /// </summary>
    /// <param name="stringParser">Парсер для преобразования строк (по умолчанию LocalizedStringParser)</param>
    public UserInputMapper(IStringParser? stringParser = null)
    {
        _stringParser = stringParser ?? new LocalizedStringParser();
    }

    /// <summary>
    /// Преобразует данные формы в объект UserInput.
    /// 
    /// Процесс:
    /// 1. Валидирует обязательные поля (VolumeFlow, TotalPressure, FanVersion)
    /// 2. Парсит основные параметры
    /// 3. Парсит опциональные параметры (если они заполнены)
    /// 4. Строит три подобъекта: WorkPoint, Air, Fan
    /// 5. Возвращает готовый UserInput
    /// </summary>
    /// <exception cref="ArgumentException">Если обязательный параметр отсутствует</exception>
    /// <exception cref="FormatException">Если не удалось распарсить значение</exception>
    public UserInput Map(UserInputFormData formData)
    {
        // Шаг 1: Валидировать обязательные поля
        ValidateRequiredFields(formData);

        // Шаг 2-4: Создать подобъекты через специализированные методы
        return new UserInput
        {
            UserInputWorkPoint = MapToWorkPoint(formData),
            UserInputAir = MapToAir(formData),
            UserInputFan = MapToFan(formData)
        };
    }

    /// <summary>
    /// Маппирует данные в объект параметров рабочей точки.
    /// Обрабатывает: VolumeFlow, TotalPressure, отклонения.
    /// </summary>
    private UserInputWorkPoint MapToWorkPoint(UserInputFormData formData)
    {
        var workPoint = new UserInputWorkPoint
        {
            // Обязательные параметры
            VolumeFlow = _stringParser.ParseDouble(formData.VolumeFlow, "Объемный расход воздуха"),
            TotalPressure = _stringParser.ParseDouble(formData.TotalPressure, "Полное давление воздуха")
        };

        // Опциональный параметр: погрешность давления
        if (!string.IsNullOrEmpty(formData.TotalPressureDeviation))
        {
            workPoint.VolumeFlowAndTotalPressureDeviation =
                _stringParser.ParseDouble(formData.TotalPressureDeviation,
                    "Допустимая погрешность подбора по давлению");
        }

        // Опциональный параметр: отклонение слева
        if (!string.IsNullOrEmpty(formData.SpecificDeviationLeft))
        {
            workPoint.SpecificDeviationLeft =
                _stringParser.ParseDouble(formData.SpecificDeviationLeft,
                    "Погрешность быстроходности слева");
        }

        // Опциональный параметр: отклонение справа
        if (!string.IsNullOrEmpty(formData.SpecificDeviationRight))
        {
            workPoint.SpecificDeviationRight =
                _stringParser.ParseDouble(formData.SpecificDeviationRight,
                    "Погрешность быстроходности справа");
        }

        return workPoint;
    }

    /// <summary>
    /// Маппирует данные в объект параметров воздуха.
    /// Обрабатывает: температуру, влажность, высоту.
    /// </summary>
    private UserInputAir MapToAir(UserInputFormData formData)
    {
        var air = new UserInputAir
        {
            // Обязательный параметр
            FanOperatingMaxTemperature =
                _stringParser.ParseDouble(formData.FanOperatingMaxTemperature,
                    "Температура перемещаемой среды")
        };

        // Опциональный параметр: текущая температура
        if (!string.IsNullOrEmpty(formData.FanOperatingCurrentTemperature))
        {
            air.FanOperatingCurrentTemperature =
                _stringParser.ParseDouble(formData.FanOperatingCurrentTemperature,
                    "Температура ежедневной эксплуатации");
        }

        // Опциональный параметр: относительная влажность
        if (!string.IsNullOrEmpty(formData.RelativeHumidity))
        {
            air.RelativeHumidity =
                _stringParser.ParseDouble(formData.RelativeHumidity,
                    "Относительная влажность");
        }

        // Опциональный параметр: высота над уровнем моря
        if (!string.IsNullOrEmpty(formData.Altitude))
        {
            air.Altitude =
                _stringParser.ParseDouble(formData.Altitude,
                    "Высота над уровнем моря");
        }

        return air;
    }

    /// <summary>
    /// Маппирует данные в объект параметров вентилятора.
    /// Обрабатывает: версию, логику, размеры, характеристики.
    /// </summary>
    private UserInputFan MapToFan(UserInputFormData formData)
    {
        var fan = new UserInputFan
        {
            // Обязательные параметры
            FanVersion = _stringParser.ParseInt(formData.FanVersion,
                "Номер исполнения вентилятора"),
            FanLogic = _stringParser.ParseInt(formData.FanLogic,
                "Номер логики подбора"),

            // Опциональные параметры (парсятся, если заполнены)
            ConditionalStandardSize = _stringParser.ParseDouble(formData.Size,
                "Условный типоразмер крыльчатки"),
            FanBodyLength = _stringParser.ParseInt(formData.FanBodyLength,
                "Длина корпуса функциональной сборки"),
            NominalPower = _stringParser.ParseDouble(formData.NominalPower,
                "Номинальная мощность двигателя"),
            NominalImpellerRotationSpeedWithoutSlidingEngine =
                _stringParser.ParseDouble(formData.NominalImpellerRotationSpeed,
                    "Условное число оборотов двигателя"),

            // Строковые параметры (не требуют парсинга)
            ImpellerRotationDirection = formData.ImpellerRotationDirection ?? string.Empty,
            FanBodyExecutionMaterial = formData.FanBodyExecutionMaterial ?? string.Empty,

            // Количество вентиляторов (индекс в списке)
            NumberOfFans = GetNumberOfFansIndex(formData.NumberOfFans)
        };

        return fan;
    }

    /// <summary>
    /// Получает индекс количества вентиляторов из предопределенного списка.
    /// Если значение не найдено или не указано, возвращает 0 (первый элемент).
    /// </summary>
    private static int GetNumberOfFansIndex(string? numberOfFans)
    {
        if (string.IsNullOrEmpty(numberOfFans))
            return 0;

        var index = NumberOfFans.Names.ToList().FindIndex(i => Equals(i, numberOfFans));
        return index == -1 ? 0 : index;
    }

    /// <summary>
    /// Валидирует, что все обязательные поля заполнены.
    /// </summary>
    /// <exception cref="ArgumentException">Если обязательное поле не заполнено</exception>
    private static void ValidateRequiredFields(UserInputFormData formData)
    {
        if (string.IsNullOrWhiteSpace(formData.VolumeFlow))
            throw new ArgumentException(
                "Объемный расход воздуха (VolumeFlow) не может быть пустым");

        if (string.IsNullOrWhiteSpace(formData.TotalPressure))
            throw new ArgumentException(
                "Полное давление воздуха (TotalPressure) не может быть пустым");

        if (string.IsNullOrWhiteSpace(formData.FanVersion))
            throw new ArgumentException(
                "Номер исполнения вентилятора (FanVersion) не может быть пустым");

        if (string.IsNullOrWhiteSpace(formData.FanLogic))
            throw new ArgumentException(
                "Номер логики подбора (FanLogic) не может быть пустым");

        if (string.IsNullOrWhiteSpace(formData.FanOperatingMaxTemperature))
            throw new ArgumentException(
                "Максимальная температура (FanOperatingMaxTemperature) не может быть пустой");
    }
}
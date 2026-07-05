using Libraries;
using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;

// ============================================================================
// ПРОЦЕСС СБОРА ДАННЫХ ОТ ПОЛЬЗОВАТЕЛЯ
// Данные собираются в объект UserInputFormData (DTO), который затем
// преобразуется в объект UserInput через UserInputMapper
// ============================================================================

var formData = new UserInputFormData();

// ОБЯЗАТЕЛЬНЫЕ ПАРАМЕТРЫ
Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");
formData.VolumeFlow = Console.ReadLine();

Console.WriteLine("Введите полное давление воздуха, [Па]: ");
formData.TotalPressure = Console.ReadLine();

Console.WriteLine(
    $"Введите номер исполнения вентилятора:\n({string.Join(", \n", FanVersion.Names)}): "
);
formData.FanVersion = Console.ReadLine();

Console.WriteLine(
    $"Введите номер логики подбора вентилятора:\n({string.Join(", \n", FanLogic.Names)}): "
);
formData.FanLogic = Console.ReadLine();

Console.WriteLine(
    $"Введите количество вентиляторов:\n({string.Join(", \n", NumberOfFans.Names)}): "
);
formData.NumberOfFans = Console.ReadLine();

Console.WriteLine(
    "Введите допустимую погрешность подбора по полному давлению воздуха (<=30; по умолчанию = 30), [%]: "
);
formData.TotalPressureDeviation = Console.ReadLine();

Console.WriteLine(
    "Введите погрешность быстроходности/ габаритности слева, [%]: "
);
formData.SpecificDeviationLeft = Console.ReadLine();

Console.WriteLine(
    "Введите погрешность быстроходности/ габаритности справа, [%]: "
);
formData.SpecificDeviationRight = Console.ReadLine();

Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");
formData.FanOperatingCurrentTemperature = Console.ReadLine();

// ОПЦИОНАЛЬНЫЕ ПАРАМЕТРЫ
Console.WriteLine(
    "Хотите ли Вы ввести дополнительные параметры? ['y' == 'yes']"
);
var inputAddParameters = Console.ReadLine();

if (inputAddParameters == "y")
{
    Console.WriteLine(
        "Введите относительную влажность этой температуры, [%]: "
    );
    formData.RelativeHumidity = Console.ReadLine();

    Console.WriteLine("Введите высоту над уровнем моря, [м]: ");
    formData.Altitude = Console.ReadLine();

    Console.WriteLine(
        $"Введите условный типоразмер крыльчатки ({string.Join("; ", Sizes.NamesForOsuDu)}): "
    );
    formData.Size = Console.ReadLine();

    Console.WriteLine(
        $"Введите длину корпуса функциональной сборки ({string.Join(", ", FanBodyLengths.NamesForOsuDu)}): "
    );
    formData.FanBodyLength = Console.ReadLine();

    Console.WriteLine(
        $"Введите температуру перемещаемой среды ({string.Join(", ", FanOperatingMaxTemperatures.NamesForOsuDu)} [°C]): "
    );
    formData.FanOperatingMaxTemperature = Console.ReadLine();

    Console.WriteLine(
        $"Введите направление вращения крыльчатки ({string.Join(", ", ImpellerRotationDirections.NamesForOsuDu)}): "
    );
    formData.ImpellerRotationDirection = Console.ReadLine();

    Console.WriteLine(
        $"Введите номинальную мощность двигателя, [кВт] ({string.Join("; ", NominalPowers.Names)}): "
    );
    formData.NominalPower = Console.ReadLine();

    Console.WriteLine(
        $"Введите условное число оборотов двигателя, [об/мин] ({string.Join(", ", NominalImpellerRotationSpeeds.Names)}): "
    );
    formData.NominalImpellerRotationSpeed = Console.ReadLine();

    Console.WriteLine(
        $"Введите материал корпуса функциональной сборки ({string.Join(", ", CaseExecutionMaterials.Names)}): "
    );
    formData.FanBodyExecutionMaterial = Console.ReadLine();
}

// ============================================================================
// ПРЕОБРАЗОВАНИЕ ДАННЫХ
// UserInputFormData (DTO со строками) -> UserInput (доменный объект)
// ============================================================================

try
{
    var userInput = Calculate.ProcessUserInput(formData);
    await FanSelector.ProcessTheRequest(userInput);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Ошибка валидации: {ex.Message}");
    Environment.Exit(1);
}
catch (FormatException ex)
{
    Console.WriteLine($"Ошибка ввода: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
    Environment.Exit(1);
}
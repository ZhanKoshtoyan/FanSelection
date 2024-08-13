using Libraries;
using Libraries.DescriptionOfObjects.Parameters;
using Libraries.Methods;

string? inputImpellerRotationDirection = default;
string? inputFanBodyExecutionMaterial = default;
string? inputRelativeHumidity = default;
string? inputAltitude = default;
string? inputSize = default;
string? inputBodyLength = default;
string? inputFanOperatingMaxTemperature = default;
string? inputNominalPower = default;
string? inputNominalImpellerRotationSpeed = default;

Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");

var inputVolumeFlow = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите полное давление воздуха, [Па]: ");

var inputTotalPressure = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите номер исполнения вентилятора:\n({string.Join(", \n", FanVersion.Names)}): "
);
var inputFanVersion = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите номер логики подбора вентилятора:\n({string.Join(", \n", FanLogic.Names)}): "
);
var inputFanLogic = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите количество вентиляторов:\n({string.Join(", \n", NumberOfFans.Names)}): "
);
var inputNumberOfFans = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    "Введите допустимую погрешность подбора по полному давлению воздуха (<=30; по умолчанию = 30), [%]: "
);

var inputTotalPressureDeviation = Console.ReadLine();

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");

var inputFanOperatingCurrentTemperature = Console.ReadLine();

//==========================================================================================================
Console.WriteLine(
    "Хотите ли Вы ввести дополнительные параметры? ['y' == 'yes']"
);
var inputAddParameters = Console.ReadLine();
if (inputAddParameters == "y")
{
    //==========================================================================================================
    Console.WriteLine(
        "Введите относительную влажность этой температуры, [%]: "
    );

    inputRelativeHumidity = Console.ReadLine();

    //==========================================================================================================

    Console.WriteLine("Введите высоту над уровнем моря, [м]: ");

    inputAltitude = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите условный типоразмер крыльчатки ({string.Join("; ", Sizes.NamesForOsuDu)}): "
    );

    inputSize = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите длину корпуса функциональной сборки ({string.Join(", ", FanBodyLengths.NamesForOsuDu)}): "
    );

    inputBodyLength = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите температуру перемещаемой среды ({string.Join(", ", FanOperatingMaxTemperatures.NamesForOsuDu)} [°C]): "
    );

    inputFanOperatingMaxTemperature = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите направление вращения крыльчатки ({string.Join(", ", ImpellerRotationDirections.NamesForOsuDu)}): "
    );
    inputImpellerRotationDirection = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите номинальную мощность двигателя, [кВт] ({string.Join("; ", NominalPowers.Names)}): "
    );

    inputNominalPower = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите условное число оборотов двигателя, [об/мин] ({string.Join(", ", NominalImpellerRotationSpeeds.Names)}): "
    );

    inputNominalImpellerRotationSpeed = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите материал корпуса функциональной сборки ({string.Join(", ", CaseExecutionMaterials.Names)}): "
    );

    inputFanBodyExecutionMaterial = Console.ReadLine();
}

//==========================================================================================================


var userInput = Calculate.ProcessUserInput(
    inputVolumeFlow,
    inputTotalPressure,
    inputFanOperatingMaxTemperature,
    inputFanVersion,
    inputFanLogic,
    inputSize,
    inputBodyLength,
    inputImpellerRotationDirection,
    inputNominalPower,
    inputNominalImpellerRotationSpeed,
    inputFanBodyExecutionMaterial,
    inputTotalPressureDeviation,
    inputRelativeHumidity,
    inputAltitude,
    inputFanOperatingCurrentTemperature,
    inputNumberOfFans
);

FanSelector.DoIt(userInput);

// Console.WriteLine("Нажмите любую клавишу чтобы закрыть программу.");
// Console.ReadLine();

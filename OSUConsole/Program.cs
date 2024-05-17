using Libraries;
using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Methods;

string? stringImpellerRotationDirection = default;
string? stringCaseMaterial = default;
string? inputRelativeHumidity = default;
double doubleRelativeHumidity = default;
string? inputAltitude = default;
double doubleAltitude = default;
int intSize = default;
int intCaseLength = default;
double doubleFanOperatingMaxTemperature = default;
double doubleNominalPower = default;
double doubleImpellerRotationSpeed = default;
string? stringRequiredSize = default;
double doubleRequiredSize = default;

Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");

var inputVolumeFlow = Console.ReadLine();

var result = double.TryParse(
    inputVolumeFlow?.Replace(".", ","),
    out var doubleVolumeFlow
);
if (!result)
{
    throw new ArgumentException("Значение 'Объем воздуха' не является числом.");
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите полное давление воздуха, [Па]: ");

var inputTotalPressure = Console.ReadLine();

result = double.TryParse(
    inputTotalPressure?.Replace(".", ","),
    out var doubleTotalPressure
);
if (!result)
{
    throw new ArgumentException(
        "Значение 'Полное давление воздуха' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите номер исполнения вентилятора:\n({string.Join(", \n", FanVersion.Names)}): "
);
var stringFanVersion = Console.ReadLine();

result = int.TryParse(stringFanVersion, out var intFanVersion);
if (!result)
{
    throw new ArgumentException(
        "Значение 'Номер исполнения вентилятора' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите номер логики подбора вентилятора:\n({string.Join(", \n", FanLogic.Names)}): "
);
var stringFanLogic = Console.ReadLine();

result = int.TryParse(stringFanLogic, out var intFanLogic);
if (!result)
{
    throw new ArgumentException(
        "Значение 'Номер логики подбора вентилятора' не является числом."
    );
}
//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите количество вентиляторов:\n({string.Join(", \n", NumberOfFans.Names)}): "
);
var stringNumberOfFans = Console.ReadLine();

result = double.TryParse(stringNumberOfFans, out var intNumberOfFans);
if (!result && !string.IsNullOrEmpty(stringNumberOfFans))
{
    throw new ArgumentException(
        "Значение 'Количество вентиляторов' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    "Введите допустимую погрешность подбора по полному давлению воздуха (<=30; по умолчанию = 30), [%]: "
);

var inputTotalPressureDeviation = Console.ReadLine();

result = double.TryParse(
    inputTotalPressureDeviation?.Replace(".", ","),
    out var doubleTotalPressureDeviation
);
if (!result && !string.IsNullOrEmpty(inputTotalPressureDeviation))
{
    throw new ArgumentException(
        "Значение 'Допустимая погрешность подбора' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
if (intFanLogic == 2)
{
    Console.WriteLine(
        $"Введите условный типоразмер крыльчатки ({string.Join("; ", Sizes.Names)}), которое требуется подобрать, [мм]: "
    );

    stringRequiredSize = Console.ReadLine();

    result = double.TryParse(
        stringRequiredSize?.Replace(".", ","),
        out doubleRequiredSize
    );
    if (!result && !string.IsNullOrEmpty(stringRequiredSize))
    {
        throw new ArgumentException(
            "Значение 'Условный типоразмер крыльчатки' не является числом."
        );
    }
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");

var inputFanOperatingMinTemperature = Console.ReadLine();

result = double.TryParse(
    inputFanOperatingMinTemperature?.Replace(".", ","),
    out var doubleFanOperatingMinTemperature
);
if (!result && !string.IsNullOrEmpty(inputFanOperatingMinTemperature))
{
    throw new ArgumentException(
        "Значение 'Температура ежедневной эксплуатации' не является числом."
    );
}

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
    result = double.TryParse(
        inputRelativeHumidity?.Replace(".", ","),
        out doubleRelativeHumidity
    );
    if (!result && !string.IsNullOrEmpty(inputRelativeHumidity))
    {
        throw new ArgumentException(
            "Значение 'Относительная влажность воздуха' не является числом."
        );
    }

    //==========================================================================================================

    Console.WriteLine("Введите высоту над уровнем моря, [м]: ");

    inputAltitude = Console.ReadLine();
    result = double.TryParse(
        inputAltitude!.Replace(".", ","),
        out doubleAltitude
    );
    if (!result && !string.IsNullOrEmpty(inputAltitude))
    {
        throw new ArgumentException(
            "Значение 'Высота над уровнем моря' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите условный типоразмер крыльчатки ({string.Join("; ", Sizes.Names)}): "
    );

    var inputSize = Console.ReadLine();
    result = int.TryParse(inputSize, out intSize);
    if (!result && !string.IsNullOrEmpty(inputSize))
    {
        throw new ArgumentException(
            "Значение 'Условный типоразмер крыльчатки' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите длину корпуса функциональной сборки ({string.Join(", ", FanBodyLengths.Names)}): "
    );

    var inputCaseLength = Console.ReadLine();
    result = int.TryParse(inputCaseLength, out intCaseLength);
    if (!result && !string.IsNullOrEmpty(inputCaseLength))
    {
        throw new ArgumentException(
            "Значение 'Длина корпуса функциональной сборки' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите температуру перемещаемой среды ({string.Join(", ", FanOperatingMaxTemperatures.Names)} [°C]): "
    );

    var inputFanOperatingMaxTemperature = Console.ReadLine();
    result = double.TryParse(
        inputFanOperatingMaxTemperature,
        out doubleFanOperatingMaxTemperature
    );
    if (!result && !string.IsNullOrEmpty(inputFanOperatingMaxTemperature))
    {
        throw new ArgumentException(
            "Значение 'Температура перемещаемой среды' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите направление вращения крыльчатки ({string.Join(", ", ImpellerRotationDirections.Names)}): "
    );
    stringImpellerRotationDirection = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите номинальную мощность двигателя, [кВт] ({string.Join("; ", NominalPowers.Names)}): "
    );

    var inputNominalPower = Console.ReadLine();
    result = double.TryParse(
        inputNominalPower!.Replace(".", ","),
        out doubleNominalPower
    );
    if (!result && !string.IsNullOrEmpty(inputNominalPower))
    {
        throw new ArgumentException(
            "Значение 'Номинальная мощность двигателя' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите условное число оборотов двигателя, [об/мин] ({string.Join(", ", NominalImpellerRotationSpeeds.Names)}): "
    );

    var inputImpellerRotationSpeed = Console.ReadLine();
    result = double.TryParse(
        inputImpellerRotationSpeed,
        out doubleImpellerRotationSpeed
    );
    if (!result && !string.IsNullOrEmpty(inputImpellerRotationSpeed))
    {
        throw new ArgumentException(
            "Значение 'Условное число оборотов двигателя' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите материал корпуса функциональной сборки ({string.Join(", ", CaseExecutionMaterials.Names)}): "
    );

    stringCaseMaterial = Console.ReadLine();
}

//==========================================================================================================

var userInput = new UserInput
{
    UserInputWorkPoint = new UserInputWorkPoint
    {
        VolumeFlow = doubleVolumeFlow,
        TotalPressure = doubleTotalPressure
    },
    UserInputAir = new UserInputAir
    {
        FanOperatingMaxTemperature = doubleFanOperatingMaxTemperature
    },
    UserInputFan = new UserInputFan
    {
        FanVersion = intFanVersion,
        FanLogic = intFanLogic,
        Size = intSize,
        FanBodyLength = intCaseLength,
        ImpellerRotationDirection = stringImpellerRotationDirection,
        NominalPower = doubleNominalPower,
        NominalImpellerRotationSpeed = doubleImpellerRotationSpeed,
        CaseExecutionMaterial = stringCaseMaterial
    }
};


if (!string.IsNullOrEmpty(inputTotalPressureDeviation))
{
    userInput.UserInputWorkPoint.TotalPressureDeviation = doubleTotalPressureDeviation;
}

if (!string.IsNullOrEmpty(inputRelativeHumidity))
{
    userInput.UserInputAir.RelativeHumidity = doubleRelativeHumidity;
}

if (!string.IsNullOrEmpty(inputAltitude))
{
    userInput.UserInputAir.Altitude = doubleAltitude;
}

if (!string.IsNullOrEmpty(inputAltitude))
{
    userInput.UserInputAir.Altitude = doubleAltitude;
}

if (!string.IsNullOrEmpty(inputFanOperatingMinTemperature))
{
    userInput.UserInputAir.FanOperatingMinTemperature = doubleFanOperatingMinTemperature;
}

if (!string.IsNullOrEmpty(stringImpellerRotationDirection))
{
    userInput.UserInputFan.ImpellerRotationDirection = stringImpellerRotationDirection;
}

if (!string.IsNullOrEmpty(stringRequiredSize))
{
    userInput.UserInputFan.RequiredSize = doubleRequiredSize;
}

if (!string.IsNullOrEmpty(stringNumberOfFans))
{
    userInput.UserInputFan.NumberOfFans = intNumberOfFans;
}

FanSelector.DoIt(userInput);

// Console.WriteLine("Нажмите любую клавишу чтобы закрыть программу.");
// Console.ReadLine();

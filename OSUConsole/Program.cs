using Libraries;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;

double doubleRelativeHumidity = default;
double doubleAltitude = default;
int intSize = default;
int intCaseLength = default;
int intTemperatureFan = default;
string? stringImpellerRotationDirection = default;
double doubleNominalPower = default;
int intImpellerRotationSpeed = default;
string? stringCaseMaterial = default;

Console.WriteLine("Введите объемный расход воздуха, [м3/ч]: ");

var inputVolumeFlow = Console.ReadLine();

// var inputVolumeFlow = "4000";
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

// var inputTotalPressure = "350";
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
    "Введите допустимую погрешность подбора по полному давлению воздуха (<=30), [%]: "
);

var inputTotalPressureDeviation = Console.ReadLine();

// var inputTotalPressureDeviation = "30";
result = double.TryParse(
    inputTotalPressureDeviation?.Replace(".", ","),
    out var doubleTotalPressureDeviation
);
if (!result)
{
    throw new ArgumentException(
        "Значение 'Допустимая погрешность подбора' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine(
    $"Введите исполнение вентилятора ({string.Join(", ", FanVersion.Names)}): "
);
var stringFanVersion = Console.ReadLine();

result = int.TryParse(stringFanVersion, out var intFanVersion);
if (!result)
{
    throw new ArgumentException(
        "Значение 'Исполнение вентилятора' не является числом."
    );
}

//-----------------------------------------------------------------------------------------------------------
Console.WriteLine("Введите температуру ежедневной эксплуатации, [°C]: ");

var inputTemperature = Console.ReadLine();

// var inputTemperature = "20";
result = double.TryParse(
    inputTemperature?.Replace(".", ","),
    out var doubleTemperature
);
if (!result)
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

    var inputRelativeHumidity = Console.ReadLine();
    result = double.TryParse(
        inputRelativeHumidity?.Replace(".", ","),
        out doubleRelativeHumidity
    );
    if (!result && !string.IsNullOrEmpty(inputRelativeHumidity))
    {
        throw new ArgumentException(
            "Значение 'Относительная влажность' не является числом."
        );
    }

    //==========================================================================================================

    Console.WriteLine("Введите высоту над уровнем моря, [м]: ");

    var inputAltitude = Console.ReadLine();
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
        $"Введите условный типоразмер ОВД ({string.Join("; ", Sizes.Names)}): "
    );

    var inputSize = Console.ReadLine();
    result = int.TryParse(inputSize, out intSize);
    if (!result && !string.IsNullOrEmpty(inputSize))
    {
        throw new ArgumentException(
            "Значение 'Условный типоразмер ОВД' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите длину корпуса ОВД ({string.Join(", ", FanBodyLengths.Names)}): "
    );

    var inputCaseLength = Console.ReadLine();
    result = int.TryParse(inputCaseLength, out intCaseLength);
    if (!result && !string.IsNullOrEmpty(inputCaseLength))
    {
        throw new ArgumentException(
            "Значение 'Длина корпуса ОВД' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите температуру перемещаемой среды ОВД ({string.Join(", ", FanOperatingMaxTemperatures.Names)} [°C]): "
    );

    var inputTemperatureFan = Console.ReadLine();
    result = int.TryParse(inputTemperatureFan, out intTemperatureFan);
    if (!result && !string.IsNullOrEmpty(inputTemperatureFan))
    {
        throw new ArgumentException(
            "Значение 'Температура перемещаемой среды ОВД' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите направление вращения рабочего колеса ОВД ({string.Join(", ", ImpellerRotationDirections.Names)}): "
    );
    stringImpellerRotationDirection = Console.ReadLine();

    //==========================================================================================================
    Console.WriteLine(
        $"Введите номинальную мощность двигателя ОВД, [кВт] ({string.Join("; ", NominalPowers.Names)}): "
    );

    var inputNominalPower = Console.ReadLine();
    result = double.TryParse(
        inputNominalPower!.Replace(".", ","),
        out doubleNominalPower
    );
    if (!result && !string.IsNullOrEmpty(inputNominalPower))
    {
        throw new ArgumentException(
            "Значение 'Номинальную мощность двигателя ОВД' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите условное число оборотов двигателя ОВД, [об/мин] ({string.Join(", ", ImpellerRotationSpeeds.Names)}): "
    );

    var inputImpellerRotationSpeed = Console.ReadLine();
    result = int.TryParse(
        inputImpellerRotationSpeed,
        out intImpellerRotationSpeed
    );
    if (!result && !string.IsNullOrEmpty(inputImpellerRotationSpeed))
    {
        throw new ArgumentException(
            "Значение 'Условное число оборотов двигателя ОВД' не является числом."
        );
    }

    //==========================================================================================================
    Console.WriteLine(
        $"Введите материал корпуса ОВД ({string.Join(", ", CaseExecutionMaterials.Names)}): "
    );

    stringCaseMaterial = Console.ReadLine();
}

//==========================================================================================================

var userInput = new UserInput
{
    UserInputWorkPoint = new UserInputWorkPoint
    {
        VolumeFlow = doubleVolumeFlow,
        TotalPressure = doubleTotalPressure,
        TotalPressureDeviation = doubleTotalPressureDeviation
    },
    UserInputAir = new UserInputAir
    {
        RelativeHumidity = doubleRelativeHumidity,
        Altitude = doubleAltitude,
        FanOperatingMinTemperature = doubleTemperature,
        FanOperatingMaxTemperature = intTemperatureFan
    },
    UserInputFan = new UserInputFan
    {
        FanVersion = intFanVersion,
        Size = intSize,
        FanBodyLength = intCaseLength,
        ImpellerRotationDirection = stringImpellerRotationDirection,
        NominalPower = doubleNominalPower,
        ImpellerRotationSpeed = intImpellerRotationSpeed,
        CaseExecutionMaterial = stringCaseMaterial
    }
};

FanSelector.DoIt(userInput);

// Console.WriteLine("Нажмите любую клавишу чтобы закрыть программу.");
// Console.ReadLine();

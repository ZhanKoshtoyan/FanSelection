namespace Libraries.DescriptionOfObjects.Parameters;

public static class FanVersion
{
    /// <summary>
    /// Исполнение вентилятора: короткое название (enum)
    /// </summary>
    public enum Values
    {
        OsuDu,
        EuFan,
        HighPressureFan
    }

    /// <summary>
    ///     Исполнение вентилятора: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "0 = 'ОСУ-ДУ' - Осевой вентилятор дымоудаления",
        "1 = 'ЕУ' - Вентилятор для ЕУКЦ",
        "2 = 'ВВД' - Вентилятор высокого давления"
    };

    public static readonly string[] ValuesForComboBox =
    {
        "ОСУ-ДУ",
        "ЕУ",
        "ВВД"
    };

    public static readonly string[] NamesForComboBox =
    {
        "'ОСУ-ДУ' - Осевой вентилятор дымоудаления",
        "'ЕУ' - Вентилятор для ЕУКЦ",
        "'ВВД' - Вентилятор высокого давления"
    };

    public static readonly string[] Altitude = { "20", "213", "0" };
}

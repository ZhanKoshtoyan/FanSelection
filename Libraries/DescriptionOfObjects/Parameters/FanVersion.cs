namespace Libraries.DescriptionOfObjects.Parameters;

public static class FanVersion
{
    public enum Values
    {
        OsuDu,
        EuFan
    }

    /// <summary>
    ///     Исполнение вентилятора: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "1 = 'ОСУ-ДУ' - Осевой вентилятор дымоудаления",
        "2 = 'ЕУ' - Вентилятор для ЕУКЦ"
    };
}

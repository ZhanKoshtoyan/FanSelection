namespace Libraries.DescriptionOfObjects.Parameters;

public static class FanLogic
{
    public enum Values
    {
        Logic1,
        Logic2
    }

    /// <summary>
    ///     Исполнение вентилятора: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "1 = Через показатель быстроходности (удельной скорости вращения крыльчатки)",
        "2 = Через показатель габаритности (удельного диаметра)"
    };
}

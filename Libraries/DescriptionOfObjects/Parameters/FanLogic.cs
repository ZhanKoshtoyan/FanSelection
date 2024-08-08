namespace Libraries.DescriptionOfObjects.Parameters;

public static class FanLogic
{
    /// <summary>
    /// Логика подбора вентилятора: короткое название (enum)
    /// </summary>
    public enum Values
    {
        Logic1,
        Logic2
    }

    /// <summary>
    /// Логика подбора вентилятора: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "0 = Через показатель быстроходности (удельной скорости вращения крыльчатки)",
        "1 = Через показатель габаритности (удельного диаметра)"
    };

    /// <summary>
    /// Логика подбора вентилятора: полное название для ComboBox
    /// </summary>
    public static readonly string[] NamesForComboBox =
    {
        "Через показатель быстроходности (удельной скорости вращения крыльчатки)",
        "Через показатель габаритности (удельного диаметра)"
    };
}

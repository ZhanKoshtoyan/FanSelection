namespace Libraries.DescriptionOfObjects.Parameters;

public static class NumberOfFans
{
    /// <summary>
    /// Количество вентиляторов, обеспечивающих рабочую точку: короткое название
    /// </summary>
    public static readonly int[] Values =
    {
        1,
        2,
        3
    };

    /// <summary>
    ///     Количество вентиляторов, обеспечивающих рабочую точку: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "1 = один вентилятор обеспечивают рабочую точку",
        "2 = два вентилятора обеспечивают рабочую точку",
        "3 = три вентилятора обеспечивают рабочую точку"
    };
}

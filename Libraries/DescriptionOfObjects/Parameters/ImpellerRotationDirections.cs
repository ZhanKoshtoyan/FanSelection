namespace Libraries.DescriptionOfObjects.Parameters;

public static class ImpellerRotationDirections
{
    /// <summary>
    ///     Направление движения крыльчатки для OsuDu: короткое название
    /// </summary>
    public static readonly string[] ValuesForOsuDu = { "RRO", "LRO", "REV" };

    /// <summary>
    ///     Направление движения крыльчатки для OsuDu: полное название
    /// </summary>
    public static readonly string[] NamesForOsuDu =
    {
        "RRO - поток на мотор",
        "LRO - поток на колесо",
        "REV - реверс"
    };

    /// <summary>
    ///     Направление движения крыльчатки для EuFan: короткое название
    /// </summary>
    public static readonly string[] ValuesForEuFan = { "RRO", "LRO" };

    /// <summary>
    ///     Направление движения крыльчатки для EuFan: полное название
    /// </summary>
    public static readonly string[] NamesForEuFan =
    {
        "RRO - Правое исполнение",
        "LRO - Левое исполнение"
    };
}

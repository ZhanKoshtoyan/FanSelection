namespace Libraries.DescriptionOfObjects.Parameters;

public static class ImpellerRotationDirections
{
    /// <summary>
    ///     Направление движения крыльчатки: короткое название
    /// </summary>
    public static readonly string[] Values =
    {
        "RRO",
        "LRO",
        "REV"
    };

    /// <summary>
    ///     Направление движения крыльчатки: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "RRO - поток на мотор",
        "LRO - поток на колесо",
        "REV - реверс"
    };
}

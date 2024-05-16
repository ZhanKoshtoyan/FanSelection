namespace Libraries.DescriptionOfObjects.Parameters;

public abstract record OctaveNoise
{
    public static readonly int[] Values =
    {
        63,
        125,
        250,
        500,
        1000,
        2000,
        4000,
        8000
    };

    public static readonly string[] Names =
    {
        "63",
        "125",
        "250",
        "500",
        "1к",
        "2к",
        "4к",
        "8к"
    };
}

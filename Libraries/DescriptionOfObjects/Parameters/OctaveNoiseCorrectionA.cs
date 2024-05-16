namespace Libraries.DescriptionOfObjects.Parameters;

public static class OctaveNoiseCorrectionA
{
    /// <summary>
    /// Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    public static readonly double[] Values =
    {
        -26.2,
        -16.1,
        -8.6,
        -3.2,
        0,
        1.2,
        1.0,
        -1.1
    };
}

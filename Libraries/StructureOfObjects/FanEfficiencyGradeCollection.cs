namespace Libraries.StructureOfObjects;

public class FanEfficiencyGradeCollection
{
    private readonly List<int> _fanSizeList =
        new()
        {
            125,
            132,
            140,
            150,
            160,
            170,
            180,
            190,
            200,
            212,
            224,
            236,
            250,
            265,
            280,
            300,
            315,
            335,
            355,
            375,
            400,
            425,
            450,
            475,
            500,
            530,
            560,
            600,
            630,
            670,
            710,
            750,
            800,
            850,
            900,
            950,
            1000
        };

    public List<int> FanSizeList => new(_fanSizeList);
    public List<FanEfficiencyGrade>? FanEfficiencyGrades { get; init; }
}

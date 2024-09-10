namespace Libraries.Methods;

public static class NewExtensions
{
    public static IEnumerable<T> Where<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> predicate
    ) => condition ? source.Where(predicate) : source;
}

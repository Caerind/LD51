using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static IEnumerable<T> Randomize<T>(this List<T> source)
    {
        System.Random rnd = new System.Random();
        return source.OrderBy((item) => rnd.Next()).ToList<T>();
    }

    public static void AddUnique<T>(this List<T> source, T item)
    {
        if (!source.Contains(item))
        {
            source.Add(item);
        }
    }
}

using System.Collections;

namespace Communicator.Server.Utils.Collections;

// Source - https://stackoverflow.com/a/34362604
// Posted by Servy
// Retrieved 2026-08-31, License - CC BY-SA 3.0

public class ReadOnlyCollectionWrapper<T>(ICollection<T> collection) : IReadOnlyCollection<T>
{
    public int Count => collection.Count;

    public IEnumerator<T> GetEnumerator()
    {
        return collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return collection.GetEnumerator();
    }
}

public static class ReadOnlyCollectionWrapper
{
    public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
    {
        return new ReadOnlyCollectionWrapper<T>(collection);
    }
}

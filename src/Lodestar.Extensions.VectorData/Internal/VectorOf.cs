namespace Lodestar.Extensions.VectorData;

/// <summary>How a search value of type <typeparamref name="T"/> held by value is read as a vector, if it is one.</summary>
/// <typeparam name="T">The type the caller declared the search value as.</typeparam>
/// <remarks>
/// Built once per type, so the reader itself boxes nothing on any runtime, where a type pattern over a value-type
/// generic boxes it on a JIT that does not fold box and isinst. The search's own null test runs before it.
/// </remarks>
internal static class VectorOf<T>
{
    /// <summary>The reader, or null when <typeparamref name="T"/> is not a vector type held by value.</summary>
    internal static readonly Func<T, ReadOnlyMemory<float>>? Read = Reader();

    // Each delegate is cast through object to the type it already is, which is why the typeof tests guard each.
    private static Func<T, ReadOnlyMemory<float>>? Reader()
    {
        if (typeof(T) == typeof(ReadOnlyMemory<float>))
        {
            return (Func<T, ReadOnlyMemory<float>>)(object)new Func<ReadOnlyMemory<float>, ReadOnlyMemory<float>>(vector => vector);
        }

        // A nullable one is held; an empty one is refused here too, as the search's null guard refuses it, whatever
        // order a caller of this reader puts the two in.
        if (typeof(T) == typeof(ReadOnlyMemory<float>?))
        {
            return (Func<T, ReadOnlyMemory<float>>)(object)new Func<ReadOnlyMemory<float>?, ReadOnlyMemory<float>>(
                searchValue => searchValue ?? throw new ArgumentNullException(nameof(searchValue)));
        }

        return null;
    }
}

namespace Lodestar.Extensions.VectorData;

/// <summary>The two sifts of an array-backed binary heap whose root is its largest element.</summary>
/// <remarks>
/// What keeps the smallest <c>n</c> of a stream: the root is the largest kept, so an element that
/// cannot enter costs one comparison. <see cref="TopHits"/> and <see cref="FusedTop"/> both keep one.
/// </remarks>
internal static class MaxHeap
{
    /// <summary>Restores the heap after the element at <paramref name="at"/> was set, moving it rootwards.</summary>
    public static void SiftUp<T>(T[] heap, int at)
        where T : IComparable<T>
    {
        while (at > 0)
        {
            int parent = (at - 1) / 2;
            if (heap[at].CompareTo(heap[parent]) <= 0)
            {
                return;
            }

            (heap[at], heap[parent]) = (heap[parent], heap[at]);
            at = parent;
        }
    }

    /// <summary>Restores the first <paramref name="size"/> elements after the root was replaced.</summary>
    public static void SiftDown<T>(T[] heap, int size)
        where T : IComparable<T>
    {
        int at = 0;
        while (true)
        {
            int left = (2 * at) + 1;
            if (left >= size)
            {
                return;
            }

            int larger = left + 1 < size && heap[left + 1].CompareTo(heap[left]) > 0 ? left + 1 : left;
            if (heap[larger].CompareTo(heap[at]) <= 0)
            {
                return;
            }

            (heap[at], heap[larger]) = (heap[larger], heap[at]);
            at = larger;
        }
    }
}

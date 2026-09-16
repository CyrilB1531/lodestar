namespace Lodestar.Cluster.Internal;

/// <summary>Labels from a merge tree cut at a cluster count — <c>sklearn.cluster._agglomerative._hc_cut</c>.</summary>
/// <remarks>
/// The reference splits the newest node first, keeping the frontier in a <c>heapq</c> of negated
/// node ids, and a sample's label is <strong>its cluster's position in the heap's array</strong>,
/// not in any sorted order. So the heap is reproduced operation for operation — the same sift-up
/// and sift-down as CPython's <c>heapq</c> — and a sorted frontier would number the same clusters
/// differently. Checked against every cut of six fixtures, from one cluster to one per sample.
/// </remarks>
internal static class TreeCut
{
    /// <summary>The label of every sample when the tree is cut into <paramref name="clusterCount"/> clusters.</summary>
    public static int[] Labels(int[] children, int sampleCount, int clusterCount)
    {
        var heap = new List<int>(clusterCount + 1);
        int root = Math.Max(children[children.Length - 2], children[children.Length - 1]) + 1;
        heap.Add(-root);

        for (int split = 0; split < clusterCount - 1; split++)
        {
            int row = -heap[0] - sampleCount;
            Push(heap, -children[2 * row]);
            PushPop(heap, -children[(2 * row) + 1]);
        }

        var labels = new int[sampleCount];
        for (int position = 0; position < heap.Count; position++)
        {
            Mark(labels, children, sampleCount, -heap[position], position);
        }

        return labels;
    }

    private static void Mark(int[] labels, int[] children, int sampleCount, int node, int label)
    {
        var pending = new Stack<int>();
        pending.Push(node);
        while (pending.Count > 0)
        {
            int current = pending.Pop();
            if (current < sampleCount)
            {
                labels[current] = label;
                continue;
            }

            int row = current - sampleCount;
            pending.Push(children[2 * row]);
            pending.Push(children[(2 * row) + 1]);
        }
    }

    /// <summary><c>heapq.heappush</c>.</summary>
    private static void Push(List<int> heap, int item)
    {
        heap.Add(item);
        SiftDown(heap, 0, heap.Count - 1);
    }

    /// <summary><c>heapq.heappushpop</c>: push, then pop the smallest, without growing past one step.</summary>
    private static void PushPop(List<int> heap, int item)
    {
        if (heap.Count > 0 && heap[0] < item)
        {
            heap[0] = item;
            SiftUp(heap, 0);
        }
    }

    /// <summary><c>heapq._siftdown</c>: moves the item at <paramref name="position"/> towards the root.</summary>
    private static void SiftDown(List<int> heap, int start, int position)
    {
        int item = heap[position];
        while (position > start)
        {
            int parent = (position - 1) >> 1;
            if (item >= heap[parent])
            {
                break;
            }

            heap[position] = heap[parent];
            position = parent;
        }

        heap[position] = item;
    }

    /// <summary><c>heapq._siftup</c>: sinks to a leaf along the smaller child, then sifts back down.</summary>
    private static void SiftUp(List<int> heap, int position)
    {
        int end = heap.Count;
        int start = position;
        int item = heap[position];
        int child = (2 * position) + 1;
        while (child < end)
        {
            int right = child + 1;
            if (right < end && heap[child] >= heap[right])
            {
                child = right;
            }

            heap[position] = heap[child];
            position = child;
            child = (2 * position) + 1;
        }

        heap[position] = item;
        SiftDown(heap, start, position);
    }
}

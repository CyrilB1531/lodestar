using System.Diagnostics.CodeAnalysis;
using ILGPU;
using ILGPU.Runtime;
using Lodestar.Embeddings.Search;

namespace Lodestar.Gpu.Compute;

/// <summary>Sweeps a resident matrix with a batch of queries: cosine similarity, then top-k.</summary>
/// <remarks>
/// Two kernels on one stream. The first tiles the query vector through shared memory so a
/// group reads it once rather than once per thread; the second keeps the selection on the
/// accelerator instead of copying every score back. Each lane of the second holds a heap of
/// its own best rows, and the group merges the lanes' sorted lists, so a query costs one pass
/// over its scores rather than one per hit (#1214). Ties break by row index ascending,
/// matching the CPU path.
/// </remarks>
public sealed class TiledCosineTopK
{
    /// <summary>The widest group the kernels are compiled for, and the shared tile's size.</summary>
    /// <remarks>
    /// ILGPU sizes static shared memory at compile time, so this is the ceiling rather than
    /// the group actually launched: <see cref="_groupSize"/> clamps to what the accelerator
    /// allows. Measured — ILGPU's CPU accelerator caps a group dimension at 16, so a kernel
    /// hard-coded to 256 threads runs on a GPU and refuses to launch where correctness is
    /// tested.
    /// </remarks>
    private const int MaxGroupSize = 256;

    private readonly GpuContext _context;
    private readonly int _groupSize;
    private readonly int _queriesPerLaunch;
    private readonly Action<KernelConfig, ArrayView<float>, ArrayView<float>, ArrayView<float>, int, int, int> _score;
    private readonly Action<KernelConfig, ArrayView<float>, ArrayView<int>, ArrayView<int>, ArrayView<float>,
        int, int, int> _select;

    /// <summary>Loads both kernels onto the accelerator.</summary>
    /// <param name="context">The accelerator to compile for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="ObjectDisposedException"><paramref name="context"/> was disposed.</exception>
    /// <remarks>
    /// Loading compiles, so build this once and reuse it. A first launch on a freshly
    /// loaded kernel measures the compiler, which is why src/Lodestar.Gpu/performance.md's gate
    /// excludes the warm-up.
    /// </remarks>
    public TiledCosineTopK(GpuContext context)
        : this(context, RowLaunch.Limit(context))
    {
    }

    /// <summary>Loads both kernels, scoring at most <paramref name="queriesPerLaunch"/> queries at once.</summary>
    /// <remarks>Exists so a test can split a launch on an accelerator that never needs to.</remarks>
    internal TiledCosineTopK(GpuContext context, int queriesPerLaunch)
    {
        Guard.NotNull(context);
        context.EnsureNotDisposed();
        _context = context;
        _groupSize = LargestPowerOfTwo(Math.Min(MaxGroupSize, context.Accelerator.MaxGroupSize.X));
        _queriesPerLaunch = queriesPerLaunch;
        _score = context.Accelerator
            .LoadStreamKernel<ArrayView<float>, ArrayView<float>, ArrayView<float>, int, int, int>(ScoreKernel);
        _select = context.Accelerator
            .LoadStreamKernel<ArrayView<float>, ArrayView<int>, ArrayView<int>, ArrayView<float>, int, int, int>(
                SelectKernel);
    }

    /// <summary>The best <paramref name="k"/> rows for each query, best first.</summary>
    /// <param name="matrix">The resident matrix to sweep.</param>
    /// <param name="queries">
    /// <paramref name="queryCount"/> × <c>matrix.Dimension</c> values, row-major. Normalized
    /// on upload, as the matrix rows were.
    /// </param>
    /// <param name="queryCount">How many queries the batch holds.</param>
    /// <param name="k">How many hits per query; fewer come back when the matrix is smaller.</param>
    /// <returns>One list per query, in the batch's own order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="queryCount"/> or <paramref name="k"/> is below 1.</exception>
    /// <exception cref="ObjectDisposedException"><paramref name="matrix"/>, or the context it and this kernel share, was disposed.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="queries"/> is not exactly the batch or holds a non-finite value, or
    /// <paramref name="matrix"/> was uploaded to another context.
    /// </exception>
    public IReadOnlyList<IReadOnlyList<SearchResult>> Search(
        DeviceEmbeddingMatrix matrix, ReadOnlySpan<float> queries, int queryCount, int k)
    {
        Guard.NotNull(matrix);
        _context.EnsureNotDisposed();
        matrix.EnsureUsableBy(_context, nameof(matrix));
        Guard.NotLessThan(queryCount, 1);
        Guard.NotLessThan(k, 1);
        if (queries.Length != (long)queryCount * matrix.Dimension)
        {
            throw new ArgumentException(
                $"queries holds {queries.Length} values, not {queryCount} x {matrix.Dimension}.",
                nameof(queries));
        }

        DeviceEmbeddingMatrix.RefuseNonFinite(queries, nameof(queries));
        int take = Math.Min(k, matrix.Count);
        // A lane sees every width-th row, so it never holds more than that many candidates.
        int capacity = Math.Min(take, (matrix.Count + _groupSize - 1) / _groupSize);
        float[] staged = queries.ToArray();
        DeviceEmbeddingMatrix.NormalizeRows(staged, queryCount, matrix.Dimension);

        Accelerator accelerator = _context.Accelerator;
        using MemoryBuffer1D<float, Stride1D.Dense> queryBuffer = accelerator.Allocate1D(staged);
        using MemoryBuffer1D<float, Stride1D.Dense> scores =
            accelerator.Allocate1D<float>((long)queryCount * matrix.Count);
        using MemoryBuffer1D<int, Stride1D.Dense> heaps =
            accelerator.Allocate1D<int>((long)queryCount * _groupSize * capacity);
        using MemoryBuffer1D<int, Stride1D.Dense> hitIndices =
            accelerator.Allocate1D<int>((long)queryCount * take);
        using MemoryBuffer1D<float, Stride1D.Dense> hitScores =
            accelerator.Allocate1D<float>((long)queryCount * take);

        int groups = (matrix.Count + _groupSize - 1) / _groupSize;
        for (int first = 0; first < queryCount; first += _queriesPerLaunch)
        {
            int batch = Math.Min(_queriesPerLaunch, queryCount - first);
            _score(
                new KernelConfig(new Index2D(groups, batch), new Index2D(_groupSize, 1)),
                matrix.Buffer.View, queryBuffer.View, scores.View, matrix.Dimension, matrix.Count, first);
        }

        _select(
            new KernelConfig(new Index1D(queryCount), new Index1D(_groupSize)),
            scores.View, heaps.View, hitIndices.View, hitScores.View, matrix.Count, take, capacity);
        accelerator.Synchronize();

        int[] flatIndices = hitIndices.GetAsArray1D();
        float[] flatScores = hitScores.GetAsArray1D();
        var results = new IReadOnlyList<SearchResult>[queryCount];
        for (int query = 0; query < queryCount; query++)
        {
            var hits = new SearchResult[take];
            for (int slot = 0; slot < take; slot++)
            {
                int at = (query * take) + slot;
                hits[slot] = new SearchResult(flatIndices[at], flatScores[at]);
            }

            results[query] = hits;
        }

        return results;
    }

    /// <summary>One thread per row, one group per row block, the query tiled through shared memory.</summary>
    // long-comment: why a kernel is excluded from coverage instrumentation. Coverlet
    // rewrites an instrumented method to record each hit through a mutable static array,
    // and ILGPU refuses device code that reads a static field which is not read only. So
    // every kernel in this package failed to compile under coverage while passing without
    // it: measured, thirty-six of forty-six tests failed in that job and none locally.
    // The exclusion covers the device method alone; the host code around it is
    // instrumented as usual.
    [ExcludeFromCodeCoverage]
    private static void ScoreKernel(
        ArrayView<float> matrix, ArrayView<float> queries, ArrayView<float> scores, int dimension, int count,
        int firstQuery)
    {
        ArrayView<float> tile = SharedMemory.Allocate1D<float>(MaxGroupSize);
        int width = Group.DimX;
        int row = (Grid.IdxX * width) + Group.IdxX;
        int query = firstQuery + Grid.IdxY;
        long queryBase = (long)query * dimension;
        float accumulator = 0.0f;

        for (int offset = 0; offset < dimension; offset += width)
        {
            int span = Math.Min(width, dimension - offset);
            if (Group.IdxX < span)
            {
                tile[Group.IdxX] = queries[queryBase + offset + Group.IdxX];
            }

            Group.Barrier();
            if (row < count)
            {
                long rowBase = ((long)row * dimension) + offset;
                for (int i = 0; i < span; i++)
                {
                    accumulator += matrix[rowBase + i] * tile[i];
                }
            }

            Group.Barrier();
        }

        if (row < count)
        {
            scores[((long)query * count) + row] = accumulator;
        }
    }

    /// <summary>One group per query: a heap per lane, then a merge of the lanes' lists.</summary>
    /// <remarks>
    /// A lane keeps the best of rows <c>lane, lane + w, …</c> in a heap worst first — n/w reads, and
    /// a log k sift only when a row beats the root — then heapsorts it best first. The group runs k
    /// rounds of a parallel argmax over the lanes' heads, the winner's lane stepping on: k·log w,
    /// where the argmax this replaced rescanned all n rows per hit. No score is NaN — the inputs are
    /// finite, and a finite sum overflows only to an infinity — so the order is total.
    /// </remarks>
    // Same reason as the kernel above: coverage instrumentation reads a mutable static.
    [ExcludeFromCodeCoverage]
    private static void SelectKernel(
        ArrayView<float> scores, ArrayView<int> heaps, ArrayView<int> hitIndices, ArrayView<float> hitScores,
        int count, int take, int capacity)
    {
        ArrayView<float> bestScore = SharedMemory.Allocate1D<float>(MaxGroupSize);
        ArrayView<int> bestIndex = SharedMemory.Allocate1D<int>(MaxGroupSize);
        int width = Group.DimX;
        int query = Grid.IdxX;
        int lane = Group.IdxX;
        ArrayView<float> row = scores.SubView((long)query * count, count);
        ArrayView<int> heap = heaps.SubView((((long)query * width) + lane) * capacity, capacity);

        int size = KeepBest(row, heap, lane, width, count, capacity);
        SortBestFirst(row, heap, size);

        int head = 0;
        for (int slot = 0; slot < take; slot++)
        {
            // An exhausted lane offers negative infinity at int.MaxValue, which any real row
            // outranks: a row scoring negative infinity wins the tie on its lower index.
            bestScore[lane] = head < size ? row[heap[head]] : float.NegativeInfinity;
            bestIndex[lane] = head < size ? heap[head] : int.MaxValue;
            Group.Barrier();

            for (int stride = width / 2; stride > 0; stride >>= 1)
            {
                if (lane < stride)
                {
                    Merge(bestScore, bestIndex, lane, lane + stride);
                }

                Group.Barrier();
            }

            int winner = bestIndex[0];
            if (lane == 0)
            {
                long at = ((long)query * take) + slot;
                hitIndices[at] = winner;
                hitScores[at] = bestScore[0];
            }

            // Rows are dealt to lanes by index modulo the width, so the winner names its lane.
            if (winner % width == lane)
            {
                head++;
            }

            Group.Barrier();
        }
    }

    /// <summary>The best <paramref name="capacity"/> of rows <paramref name="lane"/>, + width, …, as a heap worst first.</summary>
    /// <returns>How many rows the heap holds.</returns>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static int KeepBest(
        ArrayView<float> row, ArrayView<int> heap, int lane, int width, int count, int capacity)
    {
        int size = 0;
        for (int candidate = lane; candidate < count; candidate += width)
        {
            if (size < capacity)
            {
                SiftUp(row, heap, size, candidate);
                size++;
            }
            else if (Outranks(row, candidate, heap[0]))
            {
                SiftDown(row, heap, size, candidate);
            }
        }

        return size;
    }

    /// <summary>Heapsorts the first <paramref name="size"/> entries in place, best first.</summary>
    /// <remarks>Moving the root, the worst, to the end of a shrinking heap is what leaves the best at the front.</remarks>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static void SortBestFirst(ArrayView<float> row, ArrayView<int> heap, int size)
    {
        for (int end = size - 1; end > 0; end--)
        {
            int last = heap[end];
            heap[end] = heap[0];
            SiftDown(row, heap, end, last);
        }
    }

    /// <summary>Whether row <paramref name="a"/> ranks above row <paramref name="b"/>: score, then lower index.</summary>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static bool Outranks(ArrayView<float> row, int a, int b)
    {
        // S1244: an exact tie is the case this decides, and the CPU path's sort decides it
        // the same way -- a tolerance here would make the two disagree on which row wins.
#pragma warning disable S1244
        return row[a] > row[b] || (row[a] == row[b] && a < b);
#pragma warning restore S1244
    }

    /// <summary>Inserts <paramref name="candidate"/> at <paramref name="size"/>, keeping the worst at the root.</summary>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static void SiftUp(ArrayView<float> row, ArrayView<int> heap, int size, int candidate)
    {
        int at = size;
        while (at > 0)
        {
            int parent = (at - 1) / 2;
            if (!Outranks(row, heap[parent], candidate))
            {
                break;
            }

            heap[at] = heap[parent];
            at = parent;
        }

        heap[at] = candidate;
    }

    /// <summary>Puts <paramref name="candidate"/> at the root of the first <paramref name="size"/> entries and sinks it.</summary>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static void SiftDown(ArrayView<float> row, ArrayView<int> heap, int size, int candidate)
    {
        int at = 0;
        while (true)
        {
            int child = (2 * at) + 1;
            if (child >= size)
            {
                break;
            }

            if (child + 1 < size && Outranks(row, heap[child], heap[child + 1]))
            {
                child++;
            }

            if (!Outranks(row, candidate, heap[child]))
            {
                break;
            }

            heap[at] = heap[child];
            at = child;
        }

        heap[at] = candidate;
    }

    /// <summary>The largest power of two at or below <paramref name="limit"/>, at least one.</summary>
    /// <remarks>The reduction halves its stride, so a group that is not a power of two would
    /// leave the top lanes unmerged and lose whatever they held.</remarks>
    private static int LargestPowerOfTwo(int limit)
    {
        int size = 1;
        while (size * 2 <= limit)
        {
            size *= 2;
        }

        return size;
    }

    /// <summary>Keeps the better of two candidates in <paramref name="lane"/>, lower index on a tie.</summary>
    // Called from a kernel, so it is device code and carries the same exclusion.
    [ExcludeFromCodeCoverage]
    private static void Merge(ArrayView<float> bestScore, ArrayView<int> bestIndex, int lane, int other)
    {
        // S1244: an exact tie is the case this decides, and the CPU path's sort decides it
        // the same way -- a tolerance here would make the two disagree on which row wins.
#pragma warning disable S1244
        bool better = bestScore[other] > bestScore[lane]
            || (bestScore[other] == bestScore[lane] && bestIndex[other] < bestIndex[lane]);
#pragma warning restore S1244
        if (better)
        {
            bestScore[lane] = bestScore[other];
            bestIndex[lane] = bestIndex[other];
        }
    }
}

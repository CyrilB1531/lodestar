using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>Sweeps a resident matrix with a batch of queries: cosine similarity, then top-k.</summary>
/// <remarks>
/// Two kernels on one stream. The first tiles the query vector through shared memory so a
/// group reads it once rather than once per thread; the second selects the best k by
/// repeated parallel argmax, which keeps the selection on the accelerator instead of
/// copying every score back. Ties break by row index ascending, matching the CPU path.
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
    private readonly Action<KernelConfig, ArrayView<float>, ArrayView<float>, ArrayView<float>, int, int> _score;
    private readonly Action<KernelConfig, ArrayView<float>, ArrayView<int>, ArrayView<float>, int, int> _select;

    /// <summary>Loads both kernels onto the accelerator.</summary>
    /// <param name="context">The accelerator to compile for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <remarks>
    /// Loading compiles, so build this once and reuse it. A first launch on a freshly
    /// loaded kernel measures the compiler, which is why decision 0102 asks a benchmark
    /// for an explicit warm-up.
    /// </remarks>
    public TiledCosineTopK(GpuContext context)
    {
        Guard.NotNull(context);
        _context = context;
        _groupSize = LargestPowerOfTwo(Math.Min(MaxGroupSize, context.Accelerator.MaxGroupSize.X));
        _score = context.Accelerator
            .LoadStreamKernel<ArrayView<float>, ArrayView<float>, ArrayView<float>, int, int>(ScoreKernel);
        _select = context.Accelerator
            .LoadStreamKernel<ArrayView<float>, ArrayView<int>, ArrayView<float>, int, int>(SelectKernel);
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
    /// <exception cref="ArgumentException"><paramref name="queries"/> is not exactly the batch.</exception>
    public IReadOnlyList<IReadOnlyList<GpuSearchResult>> Search(
        DeviceEmbeddingMatrix matrix, ReadOnlySpan<float> queries, int queryCount, int k)
    {
        Guard.NotNull(matrix);
        Guard.NotLessThan(queryCount, 1);
        Guard.NotLessThan(k, 1);
        if (queries.Length != (long)queryCount * matrix.Dimension)
        {
            throw new ArgumentException(
                $"queries holds {queries.Length} values, not {queryCount} x {matrix.Dimension}.",
                nameof(queries));
        }

        int take = Math.Min(k, matrix.Count);
        float[] staged = queries.ToArray();
        DeviceEmbeddingMatrix.NormalizeRows(staged, queryCount, matrix.Dimension);

        Accelerator accelerator = _context.Accelerator;
        using MemoryBuffer1D<float, Stride1D.Dense> queryBuffer = accelerator.Allocate1D(staged);
        using MemoryBuffer1D<float, Stride1D.Dense> scores =
            accelerator.Allocate1D<float>((long)queryCount * matrix.Count);
        using MemoryBuffer1D<int, Stride1D.Dense> hitIndices =
            accelerator.Allocate1D<int>((long)queryCount * take);
        using MemoryBuffer1D<float, Stride1D.Dense> hitScores =
            accelerator.Allocate1D<float>((long)queryCount * take);

        int groups = (matrix.Count + _groupSize - 1) / _groupSize;
        _score(
            new KernelConfig(new Index2D(groups, queryCount), new Index2D(_groupSize, 1)),
            matrix.Buffer.View, queryBuffer.View, scores.View, matrix.Dimension, matrix.Count);
        _select(
            new KernelConfig(new Index1D(queryCount), new Index1D(_groupSize)),
            scores.View, hitIndices.View, hitScores.View, matrix.Count, take);
        accelerator.Synchronize();

        int[] flatIndices = hitIndices.GetAsArray1D();
        float[] flatScores = hitScores.GetAsArray1D();
        var results = new IReadOnlyList<GpuSearchResult>[queryCount];
        for (int query = 0; query < queryCount; query++)
        {
            var hits = new GpuSearchResult[take];
            for (int slot = 0; slot < take; slot++)
            {
                int at = (query * take) + slot;
                hits[slot] = new GpuSearchResult(flatIndices[at], flatScores[at]);
            }

            results[query] = hits;
        }

        return results;
    }

    /// <summary>One thread per row, one group per row block, the query tiled through shared memory.</summary>
    private static void ScoreKernel(
        ArrayView<float> matrix, ArrayView<float> queries, ArrayView<float> scores, int dimension, int count)
    {
        ArrayView<float> tile = SharedMemory.Allocate1D<float>(MaxGroupSize);
        int width = Group.DimX;
        int row = (Grid.IdxX * width) + Group.IdxX;
        int query = Grid.IdxY;
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

    /// <summary>One group per query, selecting k winners by repeated parallel argmax.</summary>
    /// <remarks>
    /// The taken row is masked to negative infinity so the next pass cannot see it, which
    /// is why <c>scores</c> is scratch and never read again by the caller.
    /// </remarks>
    private static void SelectKernel(
        ArrayView<float> scores, ArrayView<int> hitIndices, ArrayView<float> hitScores, int count, int take)
    {
        ArrayView<float> bestScore = SharedMemory.Allocate1D<float>(MaxGroupSize);
        ArrayView<int> bestIndex = SharedMemory.Allocate1D<int>(MaxGroupSize);
        int width = Group.DimX;
        int query = Grid.IdxX;
        int lane = Group.IdxX;
        long rowBase = (long)query * count;

        for (int slot = 0; slot < take; slot++)
        {
            float localScore = float.NegativeInfinity;
            int localIndex = int.MaxValue;
            // Strided so a lane's own candidates arrive in ascending index order, which
            // makes a strict > enough to keep the lower index on a tie.
            for (int row = lane; row < count; row += width)
            {
                float candidate = scores[rowBase + row];
                if (candidate > localScore)
                {
                    localScore = candidate;
                    localIndex = row;
                }
            }

            bestScore[lane] = localScore;
            bestIndex[lane] = localIndex;
            Group.Barrier();

            for (int stride = width / 2; stride > 0; stride >>= 1)
            {
                if (lane < stride)
                {
                    Merge(bestScore, bestIndex, lane, lane + stride);
                }

                Group.Barrier();
            }

            if (lane == 0)
            {
                long at = ((long)query * take) + slot;
                hitIndices[at] = bestIndex[0];
                hitScores[at] = bestScore[0];
                scores[rowBase + bestIndex[0]] = float.NegativeInfinity;
            }

            Group.Barrier();
        }
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

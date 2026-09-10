using Lodestar.Embeddings.Search;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>The kernel against the SIMD path it is meant to replace.</summary>
/// <remarks>
/// Every test forces ILGPU's CPU accelerator. Correctness is the question these answer and
/// it must be answerable where there is no GPU; the 5–10× gate is a different question,
/// measured on a named machine (decision 0102).
/// </remarks>
public sealed class TiledCosineTopKTests
{
    private const float Tolerance = 1e-6f;

    [Fact]
    public void Single_query_matches_the_simd_baseline()
    {
        const int count = 500;
        const int dimension = 64;
        const int k = 10;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4441);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4442);

        IReadOnlyList<SearchResult> expected = CosineCorpus.Baseline(rows, count, dimension, query, k);
        IReadOnlyList<GpuSearchResult> actual = RunKernel(rows, count, dimension, query, 1, k)[0];

        AssertSameRanking(expected, actual);
    }

    [Fact]
    public void A_batch_of_queries_matches_the_baseline_query_by_query()
    {
        const int count = 300;
        const int dimension = 48;
        const int queries = 7;
        const int k = 5;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4443);
        float[] batch = CosineCorpus.Rows(queries, dimension, seed: 4444);

        IReadOnlyList<IReadOnlyList<GpuSearchResult>> actual =
            RunKernel(rows, count, dimension, batch, queries, k);

        Assert.Equal(queries, actual.Count);
        for (int query = 0; query < queries; query++)
        {
            IReadOnlyList<SearchResult> expected = CosineCorpus.Baseline(
                rows, count, dimension, batch.AsSpan(query * dimension, dimension), k);
            AssertSameRanking(expected, actual[query]);
        }
    }

    [Fact]
    public void A_dimension_past_one_tile_still_matches()
    {
        // 700 spans three shared-memory tiles of 256, with the last one partial --
        // the case an off-by-one in the tile loop gets wrong and 64 never reaches.
        const int count = 200;
        const int dimension = 700;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4445);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4446);

        IReadOnlyList<SearchResult> expected = CosineCorpus.Baseline(rows, count, dimension, query, 8);
        AssertSameRanking(expected, RunKernel(rows, count, dimension, query, 1, 8)[0]);
    }

    [Fact]
    public void A_count_that_is_not_a_group_multiple_still_matches()
    {
        // 257 leaves one thread of a second group live and 255 dead, which is where a
        // missing bounds check writes past the score buffer or reads a stale row.
        const int count = 257;
        const int dimension = 32;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4447);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4448);

        IReadOnlyList<SearchResult> expected = CosineCorpus.Baseline(rows, count, dimension, query, 12);
        AssertSameRanking(expected, RunKernel(rows, count, dimension, query, 1, 12)[0]);
    }

    [Fact]
    public void Asking_for_more_hits_than_rows_returns_the_rows()
    {
        const int count = 5;
        const int dimension = 16;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4449);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4450);

        IReadOnlyList<GpuSearchResult> actual = RunKernel(rows, count, dimension, query, 1, 50)[0];

        Assert.Equal(count, actual.Count);
        Assert.Equal(count, actual.Select(hit => hit.Index).Distinct().Count());
    }

    [Fact]
    public void Identical_rows_break_their_tie_by_index_ascending()
    {
        // Every row is the same vector, so the order is decided entirely by the tie-break
        // -- what the CPU sort and the kernel reduction must agree on by construction.
        const int count = 300;
        const int dimension = 8;
        var rows = new float[count * dimension];
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = ((i % dimension) + 1) / 10.0f;
        }

        float[] query = rows[..dimension];
        IReadOnlyList<GpuSearchResult> actual = RunKernel(rows, count, dimension, query, 1, 6)[0];

        Assert.Equal([0, 1, 2, 3, 4, 5], actual.Select(hit => hit.Index).ToArray());
    }

    [Fact]
    public void The_cpu_accelerator_is_what_these_tests_run_on()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.False(context.IsHardwareGpu);
        Assert.NotEmpty(context.DeviceName);
    }

    [Fact]
    public void A_runtime_over_a_processor_is_not_reported_as_graphics_hardware()
    {
        // long-comment: the defect this pins cost a whole benchmark run. ILGPU enumerated
        // an OpenCL device named cpu-skylake-avx512 beside a CUDA card, and the earlier
        // check asked only for the accelerator's type -- which is OpenCL, not CPU -- so it
        // called a processor a GPU and the report carried its figures under a graphics
        // heading. IsHardwareGpu asks OpenCL for the device type instead.
        using var forced = GpuContext.Create(preferCpu: true);
        using var preferred = GpuContext.Create();

        Assert.False(forced.IsHardwareGpu);
        if (!preferred.IsHardwareGpu)
        {
            Assert.Equal(forced.DeviceName, preferred.DeviceName);
        }
    }

    [Fact]
    public void A_second_search_on_one_instance_agrees_with_the_first()
    {
        // The select kernel masks taken rows to negative infinity, so this fails the day
        // the scores buffer stops being per-call scratch and gets reused.
        const int count = 120;
        const int dimension = 24;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4451);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4452);

        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);
        var kernel = new TiledCosineTopK(context);

        IReadOnlyList<GpuSearchResult> first = kernel.Search(matrix, query, 1, 4)[0];
        IReadOnlyList<GpuSearchResult> second = kernel.Search(matrix, query, 1, 4)[0];

        Assert.Equal(first, second);
    }

    [Fact]
    public void A_freshly_loaded_kernel_answers_what_a_warmed_one_does()
    {
        // long-comment: why the obvious version of this test was deleted rather than
        // fixed. Decision 0102 asks a benchmark to warm up because ILGPU compiles on
        // first launch, and asserting that warm < cold is a wall-clock comparison on a
        // shared machine -- it failed once here and passed on re-run, which is the
        // definition of a flaky test. What a test can pin is that warming changes only
        // the cost: a kernel loaded again must answer identically, or [GlobalSetup]
        // would be hiding a difference rather than a compile.
        const int count = 64;
        const int dimension = 16;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4453);
        float[] query = CosineCorpus.Rows(1, dimension, seed: 4454);

        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);

        IReadOnlyList<GpuSearchResult> cold = new TiledCosineTopK(context).Search(matrix, query, 1, 4)[0];
        var warmed = new TiledCosineTopK(context);
        warmed.Search(matrix, query, 1, 4);
        IReadOnlyList<GpuSearchResult> warm = warmed.Search(matrix, query, 1, 4)[0];

        Assert.Equal(cold, warm);
    }

    [Fact]
    public void An_empty_query_batch_is_refused()
    {
        const int count = 8;
        const int dimension = 4;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4455);
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);
        var kernel = new TiledCosineTopK(context);

        Assert.Throws<ArgumentOutOfRangeException>(() => kernel.Search(matrix, rows, 0, 1));
    }

    [Fact]
    public void A_query_batch_of_the_wrong_width_is_refused()
    {
        const int count = 8;
        const int dimension = 4;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 4456);
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);
        var kernel = new TiledCosineTopK(context);

        Assert.Throws<ArgumentException>(() => kernel.Search(matrix, rows.AsSpan(0, 3), 1, 1));
    }

    private static IReadOnlyList<IReadOnlyList<GpuSearchResult>> RunKernel(
        float[] rows, int count, int dimension, ReadOnlySpan<float> queries, int queryCount, int k)
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);
        var kernel = new TiledCosineTopK(context);
        return kernel.Search(matrix, queries, queryCount, k);
    }

    private static void AssertSameRanking(
        IReadOnlyList<SearchResult> expected, IReadOnlyList<GpuSearchResult> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int slot = 0; slot < expected.Count; slot++)
        {
            Assert.Equal(expected[slot].Index, actual[slot].Index);
            Assert.Equal(expected[slot].Score, actual[slot].Score, Tolerance);
        }
    }
}

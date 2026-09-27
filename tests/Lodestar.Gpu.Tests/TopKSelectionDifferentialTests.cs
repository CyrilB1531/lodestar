using Lodestar.Embeddings.Search;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random draws the cases so that a failure
// replays, and there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>The selection against an exact CPU ranking, over hundreds of random cases.</summary>
/// <remarks>
/// Rows are small integers uploaded unnormalized, and a query of 1, 4 or 16 ones normalizes to a
/// binary fraction, so every score is exact in <c>float</c> in any summation order: the reference
/// is the kernel's own score, and a difference can only be the selection's. Small integers tie
/// constantly, which is what a selection gets wrong. The order is <c>EmbeddingIndex.Search</c>'s:
/// score descending, then row index ascending.
/// </remarks>
public sealed class TopKSelectionDifferentialTests
{
    private const int Cases = 400;

    [Fact]
    public void Random_cases_match_an_exact_cpu_ranking()
    {
        var random = new Random(1214);
        using var context = GpuContext.Create(preferCpu: true);
        var kernel = new TiledCosineTopK(context);

        for (int trial = 0; trial < Cases; trial++)
        {
            int count = random.Next(1, 700);
            int dimension = random.Next(16, 40);
            int queryCount = random.Next(1, 4);
            int k = random.Next(1, count + 6);
            float[] rows = IntegerRows(random, count, dimension);
            float[] queries = BinaryFractionQueries(random, queryCount, dimension);

            using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension, normalize: false);
            IReadOnlyList<IReadOnlyList<SearchResult>> actual = kernel.Search(matrix, queries, queryCount, k);

            for (int query = 0; query < queryCount; query++)
            {
                SearchResult[] expected = ExactRanking(
                    rows, count, dimension, queries.AsSpan(query * dimension, dimension), k);
                Assert.True(
                    expected.SequenceEqual(actual[query]),
                    $"trial {trial}: {count} rows of {dimension}, k = {k}, query {query} of {queryCount}");
            }
        }
    }

    /// <summary>Integers in [-3, 3], with an occasional row that overflows to an infinity.</summary>
    private static float[] IntegerRows(Random random, int count, int dimension)
    {
        var rows = new float[count * dimension];
        for (int row = 0; row < count; row++)
        {
            Span<float> values = rows.AsSpan(row * dimension, dimension);
            // One row in fifty is ±3e38 throughout: the queries are non-negative, so its
            // products share a sign and the sum overflows to that infinity in any order.
            if (random.Next(50) == 0)
            {
                values.Fill(random.Next(2) == 0 ? 3e38f : -3e38f);
                continue;
            }

            for (int i = 0; i < dimension; i++)
            {
                values[i] = random.Next(-3, 4);
            }
        }

        return rows;
    }

    /// <summary>Queries of 1, 4 or 16 ones, which normalize to 1, 1/2 or 1/4 exactly.</summary>
    private static float[] BinaryFractionQueries(Random random, int queryCount, int dimension)
    {
        int[] sizes = [1, 4, 16];
        var queries = new float[queryCount * dimension];
        for (int query = 0; query < queryCount; query++)
        {
            int ones = sizes[random.Next(sizes.Length)];
            int[] positions = Enumerable.Range(0, dimension).OrderBy(_ => random.Next()).Take(ones).ToArray();
            foreach (int position in positions)
            {
                queries[(query * dimension) + position] = 1f;
            }
        }

        return queries;
    }

    /// <summary>Every row scored in <c>double</c>, sorted by score descending then index ascending.</summary>
    private static SearchResult[] ExactRanking(
        float[] rows, int count, int dimension, ReadOnlySpan<float> query, int k)
    {
        double norm = 0.0;
        foreach (float value in query)
        {
            norm += value * value;
        }

        norm = Math.Sqrt(norm);
        var hits = new SearchResult[count];
        for (int row = 0; row < count; row++)
        {
            double sum = 0.0;
            for (int i = 0; i < dimension; i++)
            {
                sum += rows[(row * dimension) + i] * (double)(float)(query[i] / norm);
            }

            hits[row] = new SearchResult(row, (float)sum);
        }

        return hits
            .OrderByDescending(hit => hit.Score)
            .ThenBy(hit => hit.Index)
            .Take(k)
            .ToArray();
    }
}

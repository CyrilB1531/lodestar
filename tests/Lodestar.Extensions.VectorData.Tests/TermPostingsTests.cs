using Lodestar.Abstractions;
using Xunit;

// CA5394/S2245 (insecure randomness): a seeded Random builds a reproducible corpus, and nothing
// here is a secret — the same matrices must come back on every run for a failure to be readable.
#pragma warning disable CA5394, S2245

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>
/// The transposed postings answer what the per-query scan over every stored count answered (#993).
/// </summary>
public sealed class TermPostingsTests
{
    /// <summary>What the hybrid search did before #993: read every stored count, per query.</summary>
    private static int[] ByScanningEveryCount(CsrMatrix counts, IReadOnlyList<int> terms)
    {
        bool[] queried = new bool[counts.ColumnCount];
        foreach (int term in terms)
        {
            queried[term] = true;
        }

        bool[] matched = new bool[counts.RowCount];
        for (int row = 0; row < matched.Length; row++)
        {
            for (int k = counts.RowPointers[row]; k < counts.RowPointers[row + 1] && !matched[row]; k++)
            {
                matched[row] = queried[counts.ColumnIndices[k]];
            }
        }

        return [.. Enumerable.Range(0, matched.Length).Where(row => matched[row])];
    }

    [Fact]
    public void The_postings_match_a_scan_over_every_stored_count()
    {
        var random = new Random(993);
        for (int matrix = 0; matrix < 200; matrix++)
        {
            int rows = random.Next(1, 9);
            int columns = random.Next(1, 9);
            var values = new List<double>();
            var indices = new List<int>();
            var pointers = new List<int> { 0 };
            for (int row = 0; row < rows; row++)
            {
                foreach (int column in Enumerable.Range(0, columns).Where(_ => random.Next(3) == 0))
                {
                    values.Add(1.0);
                    indices.Add(column);
                }

                pointers.Add(values.Count);
            }

            var counts = new CsrMatrix(rows, columns, [.. values], [.. indices], [.. pointers]);
            TermPostings postings = TermPostings.Of(counts);
            int[] terms = [.. Enumerable.Range(0, columns).Where(_ => random.Next(2) == 0)];

            Assert.Equal(ByScanningEveryCount(counts, terms), postings.Matching(terms));
        }
    }

    [Fact]
    public void A_term_no_record_holds_matches_nothing()
    {
        var counts = new CsrMatrix(2, 3, [1.0, 1.0], [0, 2], [0, 1, 2]);

        Assert.Empty(TermPostings.Of(counts).Matching([1]));
    }

    [Fact]
    public void A_record_stored_twice_under_one_term_is_matched_once_and_stays_matched()
    {
        var counts = new CsrMatrix(2, 2, [1.0, 2.0, 1.0], [0, 0, 1], [0, 2, 3]);

        Assert.Equal([0], TermPostings.Of(counts).Matching([0]));
    }
}

#pragma warning restore CA5394, S2245

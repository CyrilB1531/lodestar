using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Xunit;

// CA5394/S2245 (insecure randomness): a seeded Random builds a reproducible corpus, and nothing
// here is a secret — the same matrices must come back on every run for a failure to be readable.
#pragma warning disable CA5394, S2245

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>
/// The transposed postings answer what the per-query scan over every stored count answered (#993),
/// and rank what they match in <c>Bm25Index.Top</c>'s order (#1039).
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

    [Fact]
    public void Several_terms_matching_one_record_name_it_once()
    {
        var counts = new CsrMatrix(3, 3, [1.0, 1.0, 1.0, 1.0], [0, 1, 1, 2], [0, 2, 3, 4]);

        Assert.Equal([0, 1], TermPostings.Of(counts).Matching([0, 1]));
    }

    [Fact]
    public void The_ranking_puts_a_higher_score_first_whatever_its_index_and_a_tie_in_index_order()
    {
        // Records 0-2 hold term 0 (IDF positive over seven); 2 is shortest so scores highest, 0 and 1
        // tie. Score order alone would accept [2, 1, 0]; an inverted comparison gives [0, 1, 2].
        var counts = new CsrMatrix(
            7, 2,
            [1.0, 3.0, 1.0, 3.0, 1.0, 2.0, 2.0, 2.0, 2.0],
            [0, 1, 0, 1, 0, 1, 1, 1, 1],
            [0, 2, 4, 5, 6, 7, 8, 9]);
        var scorer = new Bm25Index(counts);
        double[] scores = scorer.Score([0]);
        Assert.True(scores[2] > scores[0], "the fixture needs record 2 to outscore the others");
        Assert.Equal(scores[0], scores[1]);

        Assert.Equal([2, 0, 1], TermPostings.Of(counts).Ranked([0], scorer));
    }

    [Fact]
    public void The_ranking_is_top_over_the_matched_records_on_random_corpora()
    {
        var random = new Random(1039);
        for (int corpus = 0; corpus < 2_000; corpus++)
        {
            int rows = random.Next(1, 30);
            int columns = random.Next(1, 6);
            var values = new List<double>();
            var indices = new List<int>();
            var pointers = new List<int> { 0 };
            for (int row = 0; row < rows; row++)
            {
                // Frequencies from a narrow range, so equal lengths and equal scores are common.
                foreach (int column in Enumerable.Range(0, columns).Where(_ => random.Next(3) == 0))
                {
                    values.Add(random.Next(1, 3));
                    indices.Add(column);
                }

                pointers.Add(values.Count);
            }

            var counts = new CsrMatrix(rows, columns, [.. values], [.. indices], [.. pointers]);
            var scorer = new Bm25Index(counts);
            TermPostings postings = TermPostings.Of(counts);
            int[] terms = [.. Enumerable.Range(0, columns).Where(_ => random.Next(2) == 0)];
            var matched = new HashSet<int>(postings.Matching(terms));

            int[] expected = [.. scorer.Top(terms, rows).Select(hit => hit.Document).Where(matched.Contains)];

            Assert.Equal(expected, postings.Ranked(terms, scorer));
        }
    }
}

#pragma warning restore CA5394, S2245

using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>
/// The vectorizers count and hash through span sinks; these check each matrix against the term
/// list <see cref="TextAnalyzer.Analyze(string)"/> returns, counted the plain way.
/// </summary>
public sealed class TermSinkTests
{
    private static readonly string[] Documents =
    [
        "The cat and the dog, the CAT again",
        "",
        "  caf\u00e9 na\u00efve \ud83d\ude00\ud83d\ude00 \u4e2d\u6587 dog dog  ",
        "a b c d e f g the and of",
        "running runs run ran \t runner",

        // Longer than the 256-byte encoding buffer holds, so the hashing sink has to grow it.
        "short " + new string('z', 120) + " short",
    ];

    public static TheoryData<AnalyzerKind, int, int, bool> Shapes => new()
    {
        { AnalyzerKind.Word, 1, 1, false },
        { AnalyzerKind.Word, 1, 3, true },
        { AnalyzerKind.Word, 2, 2, true },
        { AnalyzerKind.Char, 1, 3, false },
        { AnalyzerKind.CharWordBoundary, 2, 5, false },
    };

    [Theory]
    [MemberData(nameof(Shapes))]
    public void Count_matrix_counts_the_analyzed_terms(AnalyzerKind kind, int min, int max, bool stopWords)
    {
        var options = new CountVectorizerOptions
        {
            Analyzer = kind,
            NgramRange = (min, max),
            StopWords = stopWords ? StopWords.English : null,
        };
        TextAnalyzer analyzer = Analyzer(options);
        var vectorizer = new CountVectorizer(options);
        CsrMatrix fitted = vectorizer.FitTransform(Documents);
        CsrMatrix transformed = vectorizer.Transform(Documents);
        IReadOnlyList<string> names = vectorizer.GetFeatureNames();

        for (int row = 0; row < Documents.Length; row++)
        {
            var expected = new SortedDictionary<string, int>(StringComparer.Ordinal);
            foreach (string term in analyzer.Analyze(Documents[row]))
            {
                expected[term] = expected.TryGetValue(term, out int count) ? count + 1 : 1;
            }

            Assert.Equal(expected, Row(fitted, row, names));
            Assert.Equal(expected, Row(transformed, row, names));
        }
    }

    [Theory]
    [MemberData(nameof(Shapes))]
    public void Hashing_matrix_sums_the_signed_buckets(AnalyzerKind kind, int min, int max, bool stopWords)
    {
        var count = new CountVectorizerOptions
        {
            Analyzer = kind,
            NgramRange = (min, max),
            StopWords = stopWords ? StopWords.English : null,
        };
        // Eight buckets, so terms collide and cancel, which is the case the sort-and-sum path must get right.
        var options = new HashingVectorizerOptions { Count = count, NumFeatures = 8, Norm = null };
        CsrMatrix matrix = new HashingVectorizer(options).Transform(Documents);
        TextAnalyzer analyzer = Analyzer(count);

        for (int row = 0; row < Documents.Length; row++)
        {
            var expected = new SortedDictionary<int, double>();
            foreach (string term in analyzer.Analyze(Documents[row]))
            {
                int h = MurmurHash3.Hash32(System.Text.Encoding.UTF8.GetBytes(term));
                int column = (int)(Math.Abs((long)h) % 8);
                expected[column] = (expected.TryGetValue(column, out double v) ? v : 0.0) + (h < 0 ? -1.0 : 1.0);
            }

            var actual = new SortedDictionary<int, double>();
            for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
            {
                actual[matrix.ColumnIndices[i]] = matrix.Values[i];
            }

            // S1244: sums of +1 and -1 are exact, and a stored entry is one that is not exactly zero.
#pragma warning disable S1244
            var nonZero = new SortedDictionary<int, double>(expected.Where(entry => entry.Value != 0.0).ToDictionary(e => e.Key, e => e.Value));
#pragma warning restore S1244
            Assert.Equal(nonZero, actual);
        }
    }

    private static TextAnalyzer Analyzer(CountVectorizerOptions options) =>
        new(options.Lowercase, options.StripAccents, options.Analyzer, options.NgramRange, options.TokenPattern, options.StopWords);

    private static SortedDictionary<string, int> Row(CsrMatrix matrix, int row, IReadOnlyList<string> names)
    {
        var counts = new SortedDictionary<string, int>(StringComparer.Ordinal);
        int previous = -1;
        for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
        {
            Assert.True(matrix.ColumnIndices[i] > previous, "columns ascend within a row");
            previous = matrix.ColumnIndices[i];
            counts[names[matrix.ColumnIndices[i]]] = (int)matrix.Values[i];
        }

        return counts;
    }
}

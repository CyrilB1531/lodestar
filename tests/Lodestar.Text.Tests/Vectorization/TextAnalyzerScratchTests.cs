using System.Collections;
using System.Globalization;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>
/// The scratch list a long document grows is kept for the rest of the call and let go once it ends: kept for good, it
/// slowed every later operation on the thread; let go per document, a fit of long documents regrew it each (#1643).
/// </summary>
public sealed class TextAnalyzerScratchTests
{
    private static readonly string Long = Words(5_000);

    [Fact]
    public void A_fit_keeps_a_long_documents_list_for_the_next_and_lets_it_go_at_the_end()
    {
        var documents = new Recording([Long, Long]);
        new CountVectorizer().Fit(documents);

        Assert.True(documents.CapacityBeforeSecond > 4096, $"capacity before the second document: {documents.CapacityBeforeSecond}");
        Assert.Null(TextAnalyzer.ScratchCapacity);
    }

    [Fact]
    public void Every_call_over_documents_lets_a_long_documents_list_go()
    {
        var count = new CountVectorizer().Fit(["a few words"]);
        count.Transform([Long]);
        Assert.Null(TextAnalyzer.ScratchCapacity);

        new HashingVectorizer().Transform([Long]);
        Assert.Null(TextAnalyzer.ScratchCapacity);

        new TfidfVectorizer().Fit([Long]);
        Assert.Null(TextAnalyzer.ScratchCapacity);
    }

    [Fact]
    public void An_ordinary_documents_list_is_kept_across_calls()
    {
        new CountVectorizer().Fit(["the quick brown fox", "jumps over the lazy dog"]);
        Assert.NotNull(TextAnalyzer.ScratchCapacity);
    }

    private static string Words(int count) =>
        string.Join(" ", Enumerable.Range(0, count).Select(i => "w" + i.ToString(CultureInfo.InvariantCulture)));

    /// <summary>Documents that note the scratch list's capacity whenever the second is read.</summary>
    private sealed class Recording(string[] documents) : IReadOnlyList<string>
    {
        public int? CapacityBeforeSecond { get; private set; }

        public int Count => documents.Length;

        public string this[int index]
        {
            get
            {
                if (index == 1)
                {
                    CapacityBeforeSecond = TextAnalyzer.ScratchCapacity;
                }
                return documents[index];
            }
        }

        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)documents).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

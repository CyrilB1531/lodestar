using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>The record both the scorer and the fuser hand back.</summary>
internal static class SearchHitSample
{
    public static void Run()
    {
        Console.WriteLine("SearchHit (Lodestar.Text.Search)");

        var vectorizer = new CountVectorizer();
        CsrMatrix counts = vectorizer.FitTransform(Corpus.Documents);

        foreach (SearchHit hit in new Bm25Index(counts).Top([Corpus.Column(vectorizer, "sat")], 3))
        {
            Console.WriteLine($"  doc {hit.Document} scored {Inv.F4(hit.Score)}");
        }

        // A hit is an ordinary record, so a caller can build one to seed or splice a ranking.
        SearchHit manual = new(Document: 7, Score: 1.25);
        Console.WriteLine($"  built by hand: doc {manual.Document} scored {Inv.F4(manual.Score)}");

        Console.WriteLine();
    }
}

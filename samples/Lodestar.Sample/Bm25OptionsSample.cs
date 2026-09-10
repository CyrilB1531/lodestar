using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>What k1, b and the IDF variant each change.</summary>
internal static class Bm25OptionsSample
{
    public static void Run()
    {
        Console.WriteLine("BM25 options (Lodestar.Text.Search)");

        var defaults = new Bm25Options();
        Console.WriteLine($"  defaults          : k1={Inv.F1(defaults.K1)} b={Inv.F3(defaults.B)} idf={defaults.Idf} eps={Inv.F3(defaults.Epsilon)}");

        var vectorizer = new CountVectorizer();
        CsrMatrix counts = vectorizer.FitTransform(Corpus.Documents);
        int sat = Corpus.Column(vectorizer, "sat");

        // The two IDF variants are different numbers, not a rescaling of one another.
        double floored = new Bm25Index(counts).Score([sat])[1];
        double lucene = new Bm25Index(counts, defaults with { Idf = Bm25Idf.Lucene }).Score([sat])[1];
        Console.WriteLine($"  floored Robertson : {Inv.F4(floored)}");
        Console.WriteLine($"  Lucene            : {Inv.F4(lucene)}");
        Console.WriteLine();
    }
}

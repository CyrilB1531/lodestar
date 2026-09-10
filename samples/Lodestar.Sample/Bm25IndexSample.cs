using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>BM25 over the matrix CountVectorizer already produced.</summary>
internal static class Bm25IndexSample
{
    public static void Run()
    {
        Console.WriteLine("BM25 (Lodestar.Text.Search)");

        var vectorizer = new CountVectorizer();
        CsrMatrix counts = vectorizer.FitTransform(Corpus.Documents);
        var index = new Bm25Index(counts);

        Console.WriteLine($"  documents / terms : {index.DocumentCount} / {index.TermCount}");
        Console.WriteLine($"  average length    : {Inv.F3(index.AverageDocumentLength)}");

        // A rare term separates; a term in most documents barely moves the score.
        double[] rare = index.Score([Corpus.Column(vectorizer, "cat")]);
        double[] common = index.Score([Corpus.Column(vectorizer, "sat")]);
        Console.WriteLine($"  \"cat\" on doc 0    : {Inv.F4(rare[0])}");
        Console.WriteLine($"  \"sat\" on doc 1    : {Inv.F4(common[1])}");
        Console.WriteLine();
    }
}

using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>
/// The analyzer divergences docs/equivalence.md records against scikit-learn 1.9.1 (#879), pinned
/// so that a change to either side is noticed. Each comment gives what scikit-learn returns.
/// </summary>
public sealed class AnalyzerDivergenceTests
{
    [Fact]
    public void Lowercasing_is_simple_case_mapping()
    {
        // scikit-learn: ['stanbul', 'οδος'] -- full mapping ends the word with a final sigma, and
        // lowercases the dotted capital I to i plus a combining dot the token pattern splits on.
        Assert.Equal(["\u0130stanbul", "\u03BF\u03B4\u03BF\u03C3"], Features(new CountVectorizerOptions(), "\u039F\u0394\u039F\u03A3 \u0130stanbul"));
    }

    [Fact]
    public void Word_characters_are_pythons_class()
    {
        // A mark is not a word character and a number of any kind is, over code points (#1239).
        Assert.Equal(["nai", "ve"], Features(new CountVectorizerOptions(), "nai\u0308ve"));
        Assert.Equal(["\u0928\u092E\u0938"], Features(new CountVectorizerOptions(), "\u0928\u092E\u0938\u094D\u0924\u0947"));
        Assert.Equal(["x\u00B2y"], Features(new CountVectorizerOptions(), "x\u00B2y"));
        Assert.Equal(["de", "\U0001D400bc"], Features(new CountVectorizerOptions(), "\U0001D400bc de"));
        Assert.Equal(["cd"], Features(new CountVectorizerOptions(), "a\u203Fb cd"));
    }

    [Fact]
    public void A_custom_pattern_reads_its_classes_as_python_does()
    {
        // scikit-learn: ['nai', "ve's", 'x²', '𝐀'], through the translated regex rather than the scanner.
        var options = new CountVectorizerOptions { TokenPattern = @"[\w']+", Lowercase = false };

        Assert.Equal(["nai", "ve's", "x\u00B2", "\U0001D400"], Features(options, "nai\u0308ve's x\u00B2 \U0001D400"));
    }

    [Fact]
    public void Scikit_learns_own_default_pattern_is_accepted()
    {
        // token_pattern=r"(?u)\b\w\w+\b" as scikit-learn spells it; .NET alone refuses the flag.
        var options = new CountVectorizerOptions { TokenPattern = @"(?u)\b\w\w+\b" };

        Assert.Equal(["nai", "ve"], Features(options, "nai\u0308ve"));
    }

    [Fact]
    public void A_single_tab_survives_char_analysis_as_in_scikit_learn()
    {
        // scikit-learn: ['\t', '\tb', 'a', 'a\t', 'b'] -- only \s\s+ collapses.
        var options = new CountVectorizerOptions { Analyzer = AnalyzerKind.Char, NgramRange = (1, 2) };

        Assert.Equal(["\t", "\tb", "a", "a\t", "b"], Features(options, "a\tb"));
    }

    private static IReadOnlyList<string> Features(CountVectorizerOptions options, string document)
    {
        var vectorizer = new CountVectorizer(options);
        vectorizer.Fit([document]);
        return vectorizer.GetFeatureNames();
    }
}

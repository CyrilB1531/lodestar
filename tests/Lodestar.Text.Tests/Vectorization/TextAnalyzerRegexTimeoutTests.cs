using System.Text.RegularExpressions;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>
/// <see cref="TextAnalyzer"/> accepts a caller-supplied pattern and runs it over
/// caller-supplied text, so catastrophic backtracking is reachable from the public
/// API. These tests pin the bound over a text without surrogates: a pathological pair
/// must fail, not hang. Over one holding a surrogate, a pattern read on Python's
/// grammar has no bound, as Python's has none (#1666).
/// </summary>
public sealed class TextAnalyzerRegexTimeoutTests
{
    // Nested quantifier over a non-matching tail: the classic exponential case.
    private const string CatastrophicPattern = @"(a+)+$";

    private static TextAnalyzer Analyzer(string pattern) =>
        new(lowercase: false,
            stripAccents: false,
            kind: AnalyzerKind.Word,
            ngramRange: (1, 1),
            tokenPattern: pattern,
            stopWords: null,
            optionsName: "options");

    [Fact]
    public void Pathological_pattern_times_out_instead_of_hanging()
    {
        // Long enough that unbounded backtracking would not finish in any
        // reasonable time, so reaching the assert at all proves the bound holds.
        string input = new string('a', 40) + "!";

        Assert.Throws<RegexMatchTimeoutException>(() => Analyzer(CatastrophicPattern).Analyze(input));
    }

    [Fact]
    public void Ordinary_documents_are_unaffected()
    {
        List<string> terms = Analyzer(@"\b\w\w+\b").Analyze("the quick brown fox jumps over the lazy dog");

        Assert.Equal(
            ["the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"],
            terms);
    }
}

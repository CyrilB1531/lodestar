namespace Lodestar.Text.Vectorization;

// CA1720 (identifier contains type name): AnalyzerKind.Char mirrors
// scikit-learn's analyzer='char', which is the name a reader arrives with, and it
// has been public since 0.1.0 — renaming it breaks consumers for a naming rule.
#pragma warning disable CA1720
/// <summary>The kind of tokens a vectorizer extracts.</summary>
public enum AnalyzerKind
{
    /// <summary>Word tokens (via the token pattern), then word n-grams.</summary>
    Word,

    /// <summary>Character n-grams over the whole preprocessed string.</summary>
    Char,

    /// <summary>Character n-grams that do not cross word boundaries (words padded with spaces).</summary>
    CharWordBoundary,
}
#pragma warning restore CA1720

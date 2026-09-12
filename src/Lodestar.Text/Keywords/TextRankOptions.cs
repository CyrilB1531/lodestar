namespace Lodestar.Text.Keywords;

/// <summary>What <see cref="TextRank"/> is built with.</summary>
public sealed record TextRankOptions
{
    /// <summary>The stop words dropped before the graph is built. Null takes <c>StopWords.English</c>.</summary>
    public IReadOnlyCollection<string>? StopWords { get; init; }

    /// <summary>How many tokens share a co-occurrence window. 2 pairs adjacent tokens only.</summary>
    public int Window { get; init; } = 2;

    /// <summary>The random-surfer damping of the reference implementation.</summary>
    public double Damping { get; init; } = 0.85;

    /// <summary>
    /// How close two successive iterates must be before the ranking is taken as converged.
    /// </summary>
    /// <remarks>
    /// This implementation's, not the reference's: summa solves the eigenproblem outright and
    /// has no tolerance to expose.
    /// </remarks>
    public double Tolerance { get; init; } = 1e-12;

    /// <summary>Iterations allowed before <c>Extract</c> gives up rather than return a half-ranked vector.</summary>
    public int MaxIterations { get; init; } = 1_000;

    /// <summary>What proportion of the ranked words to keep. Ignored when <see cref="Words"/> is set.</summary>
    public double Ratio { get; init; } = 0.2;

    /// <summary>How many ranked words to keep, overriding <see cref="Ratio"/>.</summary>
    public int? Words { get; init; }

    /// <summary>What counts as a word.</summary>
    public string TokenPattern { get; init; } = @"\b\w+\b";

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated
    /// equality would otherwise treat two lists of the same words as unequal. Decision 0112 has
    /// the rule.
    /// </remarks>
    public bool Equals(TextRankOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Window != other.Window
            || MaxIterations != other.MaxIterations
            || Words != other.Words
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        // S1244: value equality between two configurations, where "the same damping" means the
        // same bits. double.Equals also makes NaN equal NaN, which equality must.
#pragma warning disable S1244
        if (!Damping.Equals(other.Damping)
            || !Tolerance.Equals(other.Tolerance)
            || !Ratio.Equals(other.Ratio))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present, for the reason
    /// <see cref="Equals(TextRankOptions)"/> gives: a set's count is not preserved by its own
    /// equality, and equal objects must hash alike.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Window;
            hash = (hash * 31) + MaxIterations;
            hash = (hash * 31) + (Words ?? -1);
            hash = (hash * 31) + Damping.GetHashCode();
            hash = (hash * 31) + Tolerance.GetHashCode();
            hash = (hash * 31) + Ratio.GetHashCode();
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
}

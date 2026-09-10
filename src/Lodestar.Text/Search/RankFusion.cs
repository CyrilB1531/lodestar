namespace Lodestar.Text.Search;

/// <summary>Combining several rankings of the same documents into one.</summary>
/// <remarks>
/// Reciprocal rank fusion, from Cormack, Clarke and Buettcher (2009). <strong>There is no
/// canonical Python library to oracle this against</strong>, so it is pinned by tests that
/// state the definition rather than by a frozen corpus — the same way
/// <c>Lodestar.Metrics</c>' mean reciprocal rank is pinned. Thread-safe.
/// </remarks>
public static class RankFusion
{
    /// <summary>The constant the published formula uses, and what it defaults to.</summary>
    public const int DefaultK = 60;

    /// <summary>Fuses two or more rankings by reciprocal rank.</summary>
    /// <param name="rankings">One sequence of document identifiers per ranking, each ordered
    /// best first. A document absent from a ranking contributes nothing from it, which is what
    /// lets rankings of different lengths be fused without padding.</param>
    /// <param name="k">The rank offset, defaulting to <see cref="DefaultK"/>. Larger flattens
    /// the weight a top position carries; must be positive.</param>
    /// <returns>Every document that appeared in any ranking, best fused score first.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="rankings"/>, or one of them, is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="k"/> is not positive.</exception>
    /// <remarks>
    /// The score is <c>Σ 1 / (k + rank)</c> over the rankings that hold the document, with
    /// <strong>rank counted from 1</strong>: at <c>k = 60</c> the first position is worth
    /// <c>1/61</c>, not the <c>1/60</c> counting from zero would give. A document repeated
    /// inside one ranking is scored at its first position there, because a ranking that lists
    /// a document twice ranks it once. Ties break by score, then by the order first seen.
    /// </remarks>
    public static IReadOnlyList<SearchHit> Rrf(
        IEnumerable<IEnumerable<int>> rankings, int k = DefaultK)
    {
        Guard.NotNull(rankings);
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(k), k, "The rank offset is positive; zero would divide by zero at rank zero.");
        }

        Dictionary<int, double> fused = [];
        List<int> order = [];

        foreach (IEnumerable<int> ranking in rankings)
        {
            Guard.NotNull(ranking);
            HashSet<int> seenHere = [];
            int rank = 0;
            foreach (int document in ranking)
            {
                rank++;
                if (!seenHere.Add(document))
                {
                    continue;
                }

                if (!fused.ContainsKey(document))
                {
                    fused[document] = 0.0;
                    order.Add(document);
                }

                fused[document] += 1.0 / (k + rank);
            }
        }

        // OrderByDescending is documented stable, so first-seen order survives a tie and
        // the result does not depend on the dictionary's enumeration order.
        return [.. order.OrderByDescending(document => fused[document])
                        .Select(document => new SearchHit(document, fused[document]))];
    }
}

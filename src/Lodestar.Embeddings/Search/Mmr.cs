namespace Lodestar.Embeddings.Search;

/// <summary>
/// Maximal Marginal Relevance: greedy selection that trades relevance to a query
/// against redundancy with what is already selected.
/// </summary>
/// <remarks>
/// Knows nothing about text. The candidates are vectors and the result is their
/// indices, so the same call serves keyword selection, passage reranking and any
/// other list a caller wants spread out rather than clustered.
/// </remarks>
public static class Mmr
{
    /// <summary>Selects up to <paramref name="count"/> candidates.</summary>
    /// <param name="query">What relevance is measured against.</param>
    /// <param name="candidates">The candidate vectors, all of <paramref name="query"/>'s length.</param>
    /// <param name="count">How many to select. More than there are selects them all.</param>
    /// <param name="lambda">1 is pure relevance, 0 pure diversity.</param>
    /// <returns>The chosen indices, <b>in selection order</b>.</returns>
    /// <remarks>
    /// <see cref="VectorMath.Dot"/> sums in a different order on net10 (SIMD) than on netstandard2.0 (scalar), so a genuine
    /// near-tie can select a different index on the two targets. A dot or norm whose float sum overflowed or underflowed is
    /// recomputed in <see cref="double"/>: <c>[1e20f]</c> and <c>[1e-23f]</c> were refused though their norms are finite (#1355).
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="candidates"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative, or <paramref name="lambda"/> is outside <c>[0, 1]</c> or <c>NaN</c>.</exception>
    /// <exception cref="ArgumentException">A candidate is null, of a different length than <paramref name="query"/>, or has a zero or non-finite norm; so does <paramref name="query"/> itself. Cosine is undefined in either case.</exception>
    public static int[] Select(
        ReadOnlySpan<float> query,
        IReadOnlyList<float[]> candidates,
        int count,
        double lambda = 0.5)
    {
        Guard.NotNull(candidates);
        Guard.NotLessThan(count, 0);
        // NaN passes both comparisons, and a NaN score picks no candidate and indexes -1 (#886).
        if (double.IsNaN(lambda) || lambda is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lambda), lambda, "Lambda must lie in [0, 1].");
        }

        // Validated before the count short-circuit below: an invalid query must throw
        // whether or not anything would end up selected, not only when count > 0.
        double[] norms = ComputeNorms(query, candidates);
        double queryNorm = Norm(query);
        // NaN and infinity spelled out rather than left to a negated comparison: `!(x > 0)`
        // rejects both too, but reads as if it meant `x <= 0` (SplitConformal.cs's choice).
        if (double.IsNaN(queryNorm) || double.IsInfinity(queryNorm) || queryNorm <= 0)
        {
            throw new ArgumentException("The query has a zero or non-finite norm, whose cosine is undefined.", nameof(query));
        }

        int n = Math.Min(count, candidates.Count);
        if (n == 0)
        {
            return [];
        }

        double[] toQuery = ComputeQuerySimilarities(query, candidates, norms, queryNorm);
        return SelectIndices(candidates, norms, toQuery, n, lambda);
    }

    // Split from Select so each half stays under the cognitive-complexity cap: this one
    // owns validation -- a candidate's shape, and a norm too degenerate to give a cosine.
    private static double[] ComputeNorms(ReadOnlySpan<float> query, IReadOnlyList<float[]> candidates)
    {
        var norms = new double[candidates.Count];
        for (int i = 0; i < candidates.Count; i++)
        {
            float[]? candidate = candidates[i];
            if (candidate is null || candidate.Length != query.Length)
            {
                throw new ArgumentException(
                    $"Candidate at index {i} is null or is not {query.Length} wide.", nameof(candidates));
            }

            norms[i] = Norm(candidate);
            if (double.IsNaN(norms[i]) || double.IsInfinity(norms[i]) || norms[i] <= 0)
            {
                throw new ArgumentException(
                    $"Candidate at index {i} has a zero or non-finite norm, whose cosine is undefined.", nameof(candidates));
            }
        }

        return norms;
    }

    private static double[] ComputeQuerySimilarities(
        ReadOnlySpan<float> query, IReadOnlyList<float[]> candidates, double[] norms, double queryNorm)
    {
        var toQuery = new double[candidates.Count];
        for (int i = 0; i < candidates.Count; i++)
        {
            toQuery[i] = Cosine(query, candidates[i], queryNorm, norms[i]);
        }

        return toQuery;
    }

    // The first pick has nothing to be redundant with yet, so it is chosen by relevance
    // alone rather than folded into the loop below against a not-yet-seeded redundancy array.
    private static int[] SelectIndices(
        IReadOnlyList<float[]> candidates, double[] norms, double[] toQuery, int n, double lambda)
    {
        var chosen = new int[n];
        var taken = new bool[candidates.Count];
        // Seeded to negative infinity, not zero: keybert/_mmr.py:48 takes the raw max
        // similarity with no floor, so a pointing-away candidate must score negative here.
        var redundancy = new double[candidates.Count];
        for (int i = 0; i < redundancy.Length; i++)
        {
            redundancy[i] = double.NegativeInfinity;
        }

        int first = PickMostRelevant(toQuery);
        chosen[0] = first;
        taken[first] = true;
        UpdateRedundancy(candidates, norms, redundancy, first);

        for (int k = 1; k < n; k++)
        {
            int best = PickByMmrScore(taken, toQuery, redundancy, lambda);
            chosen[k] = best;
            taken[best] = true;
            UpdateRedundancy(candidates, norms, redundancy, best);
        }

        return chosen;
    }

    private static int PickMostRelevant(double[] toQuery)
    {
        int best = 0;
        for (int i = 1; i < toQuery.Length; i++)
        {
            if (toQuery[i] > toQuery[best])
            {
                best = i;
            }
        }

        return best;
    }

    private static int PickByMmrScore(bool[] taken, double[] toQuery, double[] redundancy, double lambda)
    {
        int best = -1;
        double bestScore = double.NegativeInfinity;
        for (int i = 0; i < taken.Length; i++)
        {
            if (taken[i])
            {
                continue;
            }

            double score = (lambda * toQuery[i]) - ((1 - lambda) * redundancy[i]);
            if (score > bestScore)
            {
                bestScore = score;
                best = i;
            }
        }

        return best;
    }

    private static void UpdateRedundancy(
        IReadOnlyList<float[]> candidates, double[] norms, double[] redundancy, int justChosen)
    {
        for (int i = 0; i < candidates.Count; i++)
        {
            redundancy[i] = Math.Max(
                redundancy[i], Cosine(candidates[i], candidates[justChosen], norms[i], norms[justChosen]));
        }
    }

    private static double Cosine(ReadOnlySpan<float> a, ReadOnlySpan<float> b, double normA, double normB)
    {
        // The float SIMD dot, as before, unless it overflowed or its terms may have underflowed (#1355).
        double scale = normA * normB;
        if (scale >= VectorMath.SmallestExactSquares)
        {
            float dot = VectorMath.Dot(a, b);

            // NaN too: products overflowing to +inf and -inf sum to it, from finite vectors.
            if (!float.IsInfinity(dot) && !float.IsNaN(dot))
            {
                return dot / scale;
            }
        }

        return Dot(a, b) / scale;
    }

    private static double Dot(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += (double)a[i] * b[i];
        }

        return sum;
    }

    /// <summary>The norm in double: the float one, unless its sum of squares overflowed or underflowed.</summary>
    private static double Norm(ReadOnlySpan<float> v)
    {
        float squares = VectorMath.Dot(v, v);
        return squares >= VectorMath.SmallestExactSquares && !float.IsInfinity(squares)
            ? Math.Sqrt(squares)
            : Math.Sqrt(VectorMath.SquaresInDouble(v));
    }
}

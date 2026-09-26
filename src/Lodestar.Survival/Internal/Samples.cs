namespace Lodestar.Survival.Internal;

/// <summary>Validates the times, flags, weights and entries of a parametric fit into a <see cref="CensoredSample"/>.</summary>
internal static class Samples
{
    /// <summary>Builds the sample; for right and left censoring <paramref name="lower"/> and <paramref name="upper"/> are the same durations.</summary>
    // S107: the public fits' columns, and the name the refusals report them under.
#pragma warning disable S107
    public static CensoredSample Build(
        Censoring censoring,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        string timesName)
#pragma warning restore S107
    {
        int n = upper.Length;
        bool interval = censoring == Censoring.Interval;
        if (n == 0 || lower.Length != n || (!interval && eventObserved.Length != n))
        {
            throw new ArgumentException(
                $"One time and one event flag are needed per subject; got {lower.Length} and {(interval ? n : eventObserved.Length)} for {n}.",
                timesName);
        }

        var low = new double[n];
        var high = new double[n];
        var events = new bool[n];
        for (int i = 0; i < n; i++)
        {
            (low[i], high[i], events[i]) = interval ? Bounds(lower[i], upper[i], i, timesName) : Duration(upper[i], eventObserved[i], i, timesName);
        }

        return new CensoredSample(censoring, low, high, events, Weights(weights, n), Entries(entries, n, high));
    }

    private static (double Lower, double Upper, bool Event) Duration(double duration, bool observed, int i, string name)
    {
        if (!(duration > 0.0) || double.IsInfinity(duration))
        {
            throw new ArgumentException($"A duration is positive and finite; {name}[{i}] is {duration}.", name);
        }

        return (duration, duration, observed);
    }

    /// <summary>lifelines' interval: the bounds clipped to <c>[1e-20, 1e25]</c>, and an event exactly where they coincide.</summary>
    private static (double Lower, double Upper, bool Event) Bounds(double lower, double upper, int i, string name)
    {
        if (!(lower >= 0.0) || double.IsInfinity(lower) || double.IsNaN(upper) || !(upper >= lower) || !(upper > 0.0))
        {
            throw new ArgumentException(
                $"An interval has a finite, non-negative lower bound and an upper bound at least as large and positive; interval {i} is [{lower}, {upper}].",
                name);
        }

        // S1244: lifelines calls an interval an event exactly when its bounds are the same number.
#pragma warning disable S1244
        return (Math.Max(lower, 1e-20), Math.Min(upper, 1e25), lower == upper);
#pragma warning restore S1244
    }

    private static double[] Weights(ReadOnlySpan<double> weights, int n)
    {
        if (weights.IsEmpty)
        {
            return [.. Enumerable.Repeat(1.0, n)];
        }

        if (weights.Length != n)
        {
            throw new ArgumentException($"weights holds {weights.Length} values for {n} subjects.", nameof(weights));
        }

        foreach (double w in weights)
        {
            if (!(w > 0.0) || double.IsInfinity(w))
            {
                throw new ArgumentException($"A weight is positive and finite; weights holds {w}.", nameof(weights));
            }
        }

        return weights.ToArray();
    }

    private static double[] Entries(ReadOnlySpan<double> entries, int n, double[] times)
    {
        if (entries.IsEmpty)
        {
            return new double[n];
        }

        if (entries.Length != n)
        {
            throw new ArgumentException($"entries holds {entries.Length} values for {n} subjects.", nameof(entries));
        }

        for (int i = 0; i < n; i++)
        {
            if (!(entries[i] >= 0.0) || entries[i] > times[i])
            {
                throw new ArgumentException(
                    $"An entry time is non-negative and at most the subject's time; entries[{i}] is {entries[i]}.", nameof(entries));
            }
        }

        return entries.ToArray();
    }
}

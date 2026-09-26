namespace Lodestar.Survival.Internal;

/// <summary>A Cox sample as lifelines fits it: sorted by stratum, duration and event, the covariates standardised.</summary>
/// <remarks>
/// lifelines sorts with a stable multi-key sort, so subjects equal on every key keep their input order, and the
/// row-level score residuals of tied subjects depend on it; this sort is stable too. Each covariate is centred on
/// its mean and divided by its sample standard deviation, the scale lifelines' penalty is written on.
/// </remarks>
internal sealed class CoxData
{
    private CoxData(int count, int featureCount)
    {
        Count = count;
        FeatureCount = featureCount;
        Design = new double[checked(count * featureCount)];
        Durations = new double[count];
        Events = new bool[count];
        Weights = new double[count];
        Order = new int[count];
        Means = new double[featureCount];
        Deviations = new double[featureCount];
    }

    public int Count { get; }

    public int FeatureCount { get; }

    /// <summary>The standardised covariates, row-major, in sorted order.</summary>
    public double[] Design { get; }

    public double[] Durations { get; }

    public bool[] Events { get; }

    public double[] Weights { get; }

    /// <summary>Whether any weight differs from one.</summary>
    public bool Weighted { get; private set; }

    /// <summary>The input index of each sorted row.</summary>
    public int[] Order { get; }

    /// <summary>Each stratum's label, and its sorted rows as a start and an end, labels ascending; one stratum when none is given.</summary>
    public (int Label, int Start, int End)[] Strata { get; private set; } = [];

    /// <summary>Each sorted row's cluster, or <see langword="null"/> when none is given.</summary>
    public int[]? Clusters { get; private set; }

    public double[] Means { get; }

    /// <summary>Each covariate's sample standard deviation, with one degree of freedom removed, as pandas takes it.</summary>
    public double[] Deviations { get; }

    /// <summary>Validates and sorts the inputs.</summary>
    /// <exception cref="ArgumentException">A span disagrees in length, a weight is not positive and finite, or a covariate is constant.</exception>
    public static CoxData Build(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<int> strata,
        ReadOnlySpan<int> clusters,
        int featureCount)
    {
        int n = durations.Length;
        Check(weights, n, nameof(weights));
        Check(strata, n, nameof(strata));
        Check(clusters, n, nameof(clusters));
        var data = new CoxData(n, featureCount);
        int[] order = SortedOrder(durations, eventObserved, strata);
        for (int position = 0; position < n; position++)
        {
            int subject = order[position];
            data.Order[position] = subject;
            data.Durations[position] = durations[subject];
            data.Events[position] = eventObserved[subject];
            double weight = weights.IsEmpty ? 1.0 : weights[subject];
            if (!(weight > 0.0) || double.IsInfinity(weight))
            {
                throw new ArgumentException($"A weight is positive and finite; weights[{subject}] is {weight}.", nameof(weights));
            }

            // S1244: a weight of exactly one is no weighting at all.
#pragma warning disable S1244
            data.Weighted |= weight != 1.0;
#pragma warning restore S1244
            data.Weights[position] = weight;
        }

        if (!clusters.IsEmpty)
        {
            data.Clusters = new int[n];
            for (int position = 0; position < n; position++)
            {
                data.Clusters[position] = clusters[order[position]];
            }
        }

        data.Strata = Ranges(order, strata);
        data.Standardise(design, order);
        return data;
    }

    /// <summary>Row <paramref name="position"/>'s standardised covariates.</summary>
    public ReadOnlySpan<double> Row(int position) => Design.AsSpan(position * FeatureCount, FeatureCount);

    private static void Check<T>(ReadOnlySpan<T> values, int count, string name)
    {
        if (!values.IsEmpty && values.Length != count)
        {
            throw new ArgumentException($"{name} holds {values.Length} values for {count} subjects; pass one per subject, or none.", name);
        }
    }

    /// <summary>The stable order by stratum, then duration, then event, censorings before events at a time.</summary>
    private static int[] SortedOrder(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<int> strata)
    {
        int n = durations.Length;
        var keys = new (int Stratum, double Duration, int Event, int Index)[n];
        for (int i = 0; i < n; i++)
        {
            keys[i] = (strata.IsEmpty ? 0 : strata[i], durations[i], eventObserved[i] ? 1 : 0, i);
        }

        // The input index as the last key makes the sort stable.
        Array.Sort(keys);
        return [.. keys.Select(key => key.Index)];
    }

    private static (int Label, int Start, int End)[] Ranges(int[] order, ReadOnlySpan<int> strata)
    {
        if (strata.IsEmpty)
        {
            return [(0, 0, order.Length)];
        }

        var ranges = new List<(int Label, int Start, int End)>();
        int start = 0;
        for (int position = 1; position <= order.Length; position++)
        {
            if (position == order.Length || strata[order[position]] != strata[order[start]])
            {
                ranges.Add((strata[order[start]], start, position));
                start = position;
            }
        }

        return [.. ranges];
    }

    /// <summary>The sample with its covariates as given: mean zero, deviation one, for evaluating a likelihood at raw coefficients.</summary>
    public static CoxData Raw(
        ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount)
    {
        CoxData data = Build(design, durations, eventObserved, [], [], [], featureCount);
        data.UseDesignAsGiven(design);
        return data;
    }

    /// <summary>Replaces the standardised covariates by <paramref name="design"/> as given, in sorted order.</summary>
    public void UseDesignAsGiven(ReadOnlySpan<double> design)
    {
        int p = FeatureCount;
        for (int position = 0; position < Count; position++)
        {
            design.Slice(Order[position] * p, p).CopyTo(Design.AsSpan(position * p));
        }

        Array.Clear(Means, 0, p);
        for (int a = 0; a < p; a++)
        {
            Deviations[a] = 1.0;
        }
    }

    /// <summary>Centres each covariate on its mean and divides it by its deviation, into sorted order.</summary>
    private void Standardise(ReadOnlySpan<double> design, int[] order)
    {
        int n = Count;
        int p = FeatureCount;
        for (int a = 0; a < p; a++)
        {
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                sum += design[(i * p) + a];
            }

            double mean = sum / n;
            double squares = 0.0;
            for (int i = 0; i < n; i++)
            {
                double gap = design[(i * p) + a] - mean;
                squares += gap * gap;
            }

            double deviation = Math.Sqrt(squares / (n - 1));
            if (!(deviation > 0.0))
            {
                throw new ArgumentException(
                    $"Covariate {a} does not vary, so it carries nothing a hazard ratio could measure; drop it.", nameof(design));
            }

            Means[a] = mean;
            Deviations[a] = deviation;
        }

        for (int position = 0; position < n; position++)
        {
            int subject = order[position];
            for (int a = 0; a < p; a++)
            {
                Design[(position * p) + a] = (design[(subject * p) + a] - Means[a]) / Deviations[a];
            }
        }
    }
}

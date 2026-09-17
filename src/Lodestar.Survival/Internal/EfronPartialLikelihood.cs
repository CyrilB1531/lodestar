namespace Lodestar.Survival.Internal;

/// <summary>The Cox log partial likelihood with Efron's handling of ties, its score and its observed information.</summary>
/// <remarks>
/// Reference behavior: the objective <c>lifelines.CoxPHFitter</c> 0.30.3 maximizes, which agrees with
/// this one to 0.0 at the same coefficients. At a time carrying <c>m</c> events, the <c>l</c>-th
/// divides by <c>S₀ − (l/m)·S̃₀</c>, the tilde summing over the tied events alone; with no tie that
/// is the ordinary Breslow term. The subjects are ordered once, at construction, by descending
/// duration, so every evaluation grows the risk set by one suffix per distinct time.
/// </remarks>
internal sealed class EfronPartialLikelihood
{
    private readonly double[] _design;
    private readonly bool[] _events;
    private readonly int[] _groupEnds;
    private readonly int _featureCount;

    // Scratch reused by every evaluation, so a Newton iteration allocates nothing here; one fit owns one instance.
    private readonly double[] _eta;
    private readonly double[] _adjustedFirst;
    private readonly Sums _risk;
    private readonly Sums _tied;

    /// <summary>Orders the subjects by descending duration and records where each distinct time ends.</summary>
    internal EfronPartialLikelihood(
        ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount)
    {
        int count = durations.Length;
        _featureCount = featureCount;

        double[] keys = new double[count];
        int[] order = new int[count];
        for (int i = 0; i < count; i++)
        {
            keys[i] = -durations[i];
            order[i] = i;
        }

        Array.Sort(keys, order);

        _design = new double[checked(count * featureCount)];
        _events = new bool[count];
        var groupEnds = new List<int>();
        for (int position = 0; position < count; position++)
        {
            int subject = order[position];
            design.Slice(subject * featureCount, featureCount)
                .CopyTo(_design.AsSpan(position * featureCount, featureCount));
            _events[position] = eventObserved[subject];

            // S1244: a tie is the same recorded duration, and a tolerance would merge two times.
#pragma warning disable S1244
            if (position + 1 == count || keys[position + 1] != keys[position])
#pragma warning restore S1244
            {
                groupEnds.Add(position + 1);
            }
        }

        _groupEnds = [.. groupEnds];
        _eta = new double[count];
        _adjustedFirst = new double[featureCount];
        _risk = new Sums(featureCount);
        _tied = new Sums(featureCount);
    }

    /// <summary>Evaluates the log partial likelihood at <paramref name="coefficients"/>.</summary>
    /// <param name="coefficients">The log hazard ratios, one per column of the design.</param>
    /// <param name="score">Receives the gradient of the log-likelihood.</param>
    /// <param name="information">Receives the observed information, the negated Hessian, row-major.</param>
    /// <returns>The log partial likelihood.</returns>
    internal double Evaluate(ReadOnlySpan<double> coefficients, Span<double> score, Span<double> information)
    {
        int p = _featureCount;

        double[] eta = LinearPredictor(coefficients);

        // Every weight is exp(η − shift): the shift cancels in each ratio the score and the
        // information take, and is added back to each logarithm, so no weight overflows.
        double shift = double.NegativeInfinity;
        foreach (double value in eta)
        {
            shift = Math.Max(shift, value);
        }

        Sums risk = _risk;
        Sums tied = _tied;
        risk.Clear();
        score.Clear();
        information.Clear();
        double logLikelihood = 0.0;
        int start = 0;
        foreach (int end in _groupEnds)
        {
            tied.Clear();
            int eventCount = 0;
            for (int position = start; position < end; position++)
            {
                ReadOnlySpan<double> row = _design.AsSpan(position * p, p);
                double weight = Math.Exp(eta[position] - shift);
                risk.Add(weight, row);
                if (_events[position])
                {
                    tied.Add(weight, row);
                    eventCount++;
                    logLikelihood += eta[position];
                    for (int a = 0; a < p; a++)
                    {
                        score[a] += row[a];
                    }
                }
            }

            for (int l = 0; l < eventCount; l++)
            {
                logLikelihood -= AccumulateTiedTerm(risk, tied, (double)l / eventCount, score, information) + shift;
            }

            start = end;
        }

        return logLikelihood;
    }

    /// <summary>Subtracts one tied event's share of the risk set, returning the logarithm of its denominator.</summary>
    private double AccumulateTiedTerm(Sums risk, Sums tied, double fraction, Span<double> score, Span<double> information)
    {
        int p = _featureCount;
        double denominator = risk.Zero - (fraction * tied.Zero);
        double squared = denominator * denominator;
        double[] first = _adjustedFirst;
        for (int a = 0; a < p; a++)
        {
            first[a] = risk.First[a] - (fraction * tied.First[a]);
        }

        for (int a = 0; a < p; a++)
        {
            double firstA = first[a];
            score[a] -= firstA / denominator;
            for (int b = 0; b < p; b++)
            {
                double second = risk.Second[(a * p) + b] - (fraction * tied.Second[(a * p) + b]);
                information[(a * p) + b] += (second / denominator) - (firstA * first[b] / squared);
            }
        }

        return Math.Log(denominator);
    }

    private double[] LinearPredictor(ReadOnlySpan<double> coefficients)
    {
        int p = _featureCount;
        double[] eta = _eta;
        for (int position = 0; position < eta.Length; position++)
        {
            double sum = 0.0;
            for (int a = 0; a < p; a++)
            {
                sum += coefficients[a] * _design[(position * p) + a];
            }

            eta[position] = sum;
        }

        return eta;
    }

    /// <summary>The weighted zeroth, first and second moments of a set of rows.</summary>
    private sealed class Sums(int featureCount)
    {
        internal double Zero { get; private set; }

        internal double[] First { get; } = new double[featureCount];

        internal double[] Second { get; } = new double[checked(featureCount * featureCount)];

        internal void Add(double weight, ReadOnlySpan<double> row)
        {
            Zero += weight;
            for (int a = 0; a < featureCount; a++)
            {
                double weighted = weight * row[a];
                First[a] += weighted;
                for (int b = 0; b < featureCount; b++)
                {
                    Second[(a * featureCount) + b] += weighted * row[b];
                }
            }
        }

        internal void Clear()
        {
            Zero = 0.0;
            Array.Clear(First, 0, First.Length);
            Array.Clear(Second, 0, Second.Length);
        }
    }
}

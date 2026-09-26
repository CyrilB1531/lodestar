namespace Lodestar.Survival.Internal;

/// <summary>The Cox log partial likelihood with Efron's handling of ties, its score and its observed information.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.CoxPHFitter._get_efron_values_single</c> 0.30.3, summed over the strata. At a
/// time carrying <c>m</c> events the <c>l</c>-th divides by <c>S₀ − (l/m)·S̃₀</c>, the tilde summing over the tied
/// events alone. With weights the sums are weighted and each time's terms are multiplied by the mean weight of its
/// events, lifelines' reading of a weighted Efron. The risk set grows backwards from the last time of each stratum.
/// </remarks>
internal sealed class EfronPartialLikelihood : IPartialLikelihood
{
    private readonly CoxData _data;

    // Scratch reused by every evaluation, so a Newton iteration allocates nothing here; one fit owns one instance.
    private readonly double[] _eta;
    private readonly double[] _adjustedFirst;
    private readonly double[] _deathSum;
    private readonly Sums _risk;
    private readonly Sums _tied;

    /// <summary>The likelihood of the covariates as given, unstandardised, unweighted and unstratified.</summary>
    internal EfronPartialLikelihood(
        ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount)
        : this(CoxData.Raw(design, durations, eventObserved, featureCount))
    {
    }

    internal EfronPartialLikelihood(CoxData data)
    {
        _data = data;
        int p = data.FeatureCount;
        _eta = new double[data.Count];
        _adjustedFirst = new double[p];
        _deathSum = new double[p];
        _risk = new Sums(p);
        _tied = new Sums(p);
    }

    /// <summary>Evaluates the log partial likelihood at <paramref name="coefficients"/>, on the standardised covariates.</summary>
    /// <param name="coefficients">The log hazard ratios of the standardised covariates.</param>
    /// <param name="score">Receives the gradient of the log-likelihood.</param>
    /// <param name="information">Receives the observed information, the negated Hessian, row-major.</param>
    /// <returns>The log partial likelihood.</returns>
    public double Evaluate(ReadOnlySpan<double> coefficients, Span<double> score, Span<double> information)
    {
        double[] eta = LinearPredictor(coefficients);

        // Every weight is exp(η − shift): the shift cancels in each ratio the score and the
        // information take, and is added back to each logarithm, so no weight overflows.
        double shift = double.NegativeInfinity;
        foreach (double value in eta)
        {
            shift = Math.Max(shift, value);
        }

        score.Clear();
        information.Clear();
        double logLikelihood = 0.0;
        foreach ((_, int start, int end) in _data.Strata)
        {
            logLikelihood += Stratum(start, end, eta, shift, score, information);
        }

        return logLikelihood;
    }

    /// <summary>One stratum's terms, walked from its last time back to its first.</summary>
    private double Stratum(int start, int end, double[] eta, double shift, Span<double> score, Span<double> information)
    {
        int p = _data.FeatureCount;
        Sums risk = _risk;
        Sums tied = _tied;
        risk.Clear();
        double logLikelihood = 0.0;
        int groupEnd = end;
        while (groupEnd > start)
        {
            int groupStart = GroupStart(start, groupEnd);
            (int eventCount, double weightCount, double eventTerm) = AddGroup(groupStart, groupEnd, eta, shift);
            logLikelihood += eventTerm;
            if (eventCount > 0)
            {
                double mean = weightCount / eventCount;
                for (int a = 0; a < p; a++)
                {
                    score[a] += _deathSum[a];
                }

                for (int l = 0; l < eventCount; l++)
                {
                    logLikelihood -= mean * (AccumulateTiedTerm(risk, tied, (double)l / eventCount, mean, score, information) + shift);
                }
            }

            groupEnd = groupStart;
        }

        return logLikelihood;
    }

    /// <summary>Adds one time's subjects to the risk set and its events to the tied sums, walking backwards as lifelines does.</summary>
    /// <returns>The events at the time, their total weight, and their weighted linear predictors' sum.</returns>
    private (int Events, double Weight, double EventTerm) AddGroup(int groupStart, int groupEnd, double[] eta, double shift)
    {
        int p = _data.FeatureCount;
        _tied.Clear();
        Array.Clear(_deathSum, 0, p);
        int eventCount = 0;
        double weightCount = 0.0;
        double eventTerm = 0.0;
        for (int position = groupEnd - 1; position >= groupStart; position--)
        {
            ReadOnlySpan<double> row = _data.Row(position);
            double w = _data.Weights[position];
            double phi = w * Math.Exp(eta[position] - shift);
            _risk.Add(phi, row);
            if (!_data.Events[position])
            {
                continue;
            }

            _tied.Add(phi, row);
            eventCount++;
            weightCount += w;
            eventTerm += w * eta[position];
            for (int a = 0; a < p; a++)
            {
                _deathSum[a] += w * row[a];
            }
        }

        return (eventCount, weightCount, eventTerm);
    }

    /// <summary>Where the run of equal durations ending just before <paramref name="groupEnd"/> starts.</summary>
    private int GroupStart(int start, int groupEnd)
    {
        double time = _data.Durations[groupEnd - 1];
        int groupStart = groupEnd - 1;
        // S1244: a tie is the same recorded duration, and a tolerance would merge two times.
#pragma warning disable S1244
        while (groupStart > start && _data.Durations[groupStart - 1] == time)
#pragma warning restore S1244
        {
            groupStart--;
        }

        return groupStart;
    }

    /// <summary>Subtracts one tied event's share of the risk set, returning the logarithm of its denominator.</summary>
    private double AccumulateTiedTerm(
        Sums risk, Sums tied, double fraction, double mean, Span<double> score, Span<double> information)
    {
        int p = _data.FeatureCount;
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
            score[a] -= mean * firstA / denominator;
            for (int b = 0; b < p; b++)
            {
                double second = risk.Second[(a * p) + b] - (fraction * tied.Second[(a * p) + b]);
                information[(a * p) + b] += mean * ((second / denominator) - (firstA * first[b] / squared));
            }
        }

        return Math.Log(denominator);
    }

    private double[] LinearPredictor(ReadOnlySpan<double> coefficients)
    {
        int p = _data.FeatureCount;
        double[] eta = _eta;
        for (int position = 0; position < eta.Length; position++)
        {
            ReadOnlySpan<double> row = _data.Row(position);
            double sum = 0.0;
            for (int a = 0; a < p; a++)
            {
                sum += coefficients[a] * row[a];
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

namespace Lodestar.Survival.Internal;

/// <summary>The Efron partial likelihood of start-stop intervals, lifelines' <c>CoxTimeVaryingFitter._get_gradients</c>.</summary>
/// <remarks>
/// At each distinct event time <c>t</c> of a stratum, the risk set is every interval with <c>start &lt; t ≤ stop</c>
/// and the events are those whose interval stops at <c>t</c>; the terms are the proportional hazards fit's, weighted
/// the same way. The rows are standardised as lifelines standardises them, over every interval.
/// </remarks>
internal sealed class TimeVaryingLikelihood : IPartialLikelihood
{
    private readonly double[] _design;
    private readonly double[] _starts;
    private readonly double[] _stops;
    private readonly bool[] _events;
    private readonly double[] _weights;
    private readonly int _p;

    /// <summary>Each stratum's rows, latest stop first, so an event time's risk set lies within a prefix of them: those
    /// of the prefix that started before it.</summary>
    private readonly int[][] _strata;

    /// <summary>Each stratum's distinct event times, ascending.</summary>
    private readonly double[][] _eventTimes;

    internal TimeVaryingLikelihood(
        double[] design, double[] starts, double[] stops, bool[] events, double[] weights, int[][] strata, int featureCount)
    {
        _design = design;
        _starts = starts;
        _stops = stops;
        _events = events;
        _weights = weights;
        _p = featureCount;
        _strata = [.. strata.Select(rows => rows.OrderByDescending(i => stops[i]).ToArray())];
        _eventTimes = [.. strata.Select(rows => rows.Where(i => events[i]).Select(i => stops[i]).Distinct().OrderBy(t => t).ToArray())];
    }

    public double Evaluate(ReadOnlySpan<double> coefficients, Span<double> score, Span<double> information)
    {
        score.Clear();
        information.Clear();
        // Each row's linear predictor, read once per row, not per event time; local, so that the likelihood stays
        // re-entrant.
        var eta = new double[_stops.Length];
        for (int i = 0; i < eta.Length; i++)
        {
            eta[i] = Eta(_design.AsSpan(i * _p, _p), coefficients);
        }

        double logLikelihood = 0.0;
        for (int s = 0; s < _strata.Length; s++)
        {
            foreach (double time in _eventTimes[s])
            {
                logLikelihood += AtTime(_strata[s], time, eta, score, information);
            }
        }

        return logLikelihood;
    }

    /// <summary>One event time's terms: the risk set at <paramref name="time"/> and the events stopping there.</summary>
    private double AtTime(int[] rows, double time, double[] etas, Span<double> score, Span<double> information)
    {
        int p = _p;
        double shift = Shift(rows, time, etas);
        var risk = new Moments(p);
        var tied = new Moments(p);
        var deathSum = new double[p];
        int deaths = 0;
        double weightCount = 0.0;
        double eventTerm = 0.0;
        foreach (int i in rows)
        {
            if (!(time <= _stops[i]))
            {
                break;
            }

            if (!(_starts[i] < time))
            {
                continue;
            }

            ReadOnlySpan<double> row = _design.AsSpan(i * p, p);
            double eta = etas[i];
            double phi = _weights[i] * Math.Exp(eta - shift);
            risk.Add(phi, row);
            if (DiesAt(i, time))
            {
                tied.Add(phi, row);
                deaths++;
                weightCount += _weights[i];
                eventTerm += _weights[i] * eta;
                for (int a = 0; a < p; a++)
                {
                    deathSum[a] += _weights[i] * row[a];
                }
            }
        }

        double mean = weightCount / deaths;
        double logLikelihood = eventTerm;
        for (int a = 0; a < p; a++)
        {
            score[a] += deathSum[a];
        }

        for (int l = 0; l < deaths; l++)
        {
            double fraction = deaths > 1 ? (double)l / deaths : 0.0;
            logLikelihood -= mean * (Tied(risk, tied, fraction, mean, score, information) + shift);
        }

        return logLikelihood;
    }

    /// <summary>The largest η at risk at <paramref name="time"/>: every weight is exp(η − shift), added back to the
    /// logarithm, where unshifted a row at η = −1274 alone at risk read its sum as zero and a design with a maximum as
    /// separated (#1756).</summary>
    private double Shift(int[] rows, double time, double[] etas)
    {
        double shift = double.NegativeInfinity;
        foreach (int i in rows)
        {
            if (!(time <= _stops[i]))
            {
                break;
            }

            shift = _starts[i] < time ? Math.Max(shift, etas[i]) : shift;
        }

        return shift;
    }

    private static double Eta(ReadOnlySpan<double> row, ReadOnlySpan<double> beta)
    {
        double eta = 0.0;
        for (int a = 0; a < row.Length; a++)
        {
            eta += row[a] * beta[a];
        }

        return eta;
    }

    /// <summary>Whether interval <paramref name="i"/> ends in an event at <paramref name="time"/>.</summary>
    // S1244: an event belongs to the time its interval stops at, exactly.
#pragma warning disable S1244
    private bool DiesAt(int i, double time) => _events[i] && _stops[i] == time;
#pragma warning restore S1244

    /// <summary>One tied event's share, returning the logarithm of its denominator.</summary>
    private double Tied(Moments risk, Moments tied, double fraction, double mean, Span<double> score, Span<double> information)
    {
        int p = _p;
        double denominator = risk.Zero - (fraction * tied.Zero);
        var first = new double[p];
        for (int a = 0; a < p; a++)
        {
            first[a] = risk.First[a] - (fraction * tied.First[a]);
        }

        for (int a = 0; a < p; a++)
        {
            score[a] -= mean * first[a] / denominator;
            for (int b = 0; b < p; b++)
            {
                double second = risk.Second[(a * p) + b] - (fraction * tied.Second[(a * p) + b]);
                information[(a * p) + b] += mean * ((second / denominator) - (first[a] / denominator * (first[b] / denominator)));
            }
        }

        return Math.Log(denominator);
    }

    private sealed class Moments(int p)
    {
        public double Zero { get; private set; }

        public double[] First { get; } = new double[p];

        public double[] Second { get; } = new double[TableLength.Of(p, p, ResultTable.FeatureCount)];

        public void Add(double weight, ReadOnlySpan<double> row)
        {
            Zero += weight;
            for (int a = 0; a < p; a++)
            {
                First[a] += weight * row[a];
                for (int b = 0; b < p; b++)
                {
                    Second[(a * p) + b] += weight * row[a] * row[b];
                }
            }
        }
    }
}

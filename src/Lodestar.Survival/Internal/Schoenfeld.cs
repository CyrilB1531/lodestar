namespace Lodestar.Survival.Internal;

/// <summary>lifelines' <c>proportional_hazard_test</c>: scaled Schoenfeld residuals against a transform of time.</summary>
internal static class Schoenfeld
{
    /// <summary>One chi-squared statistic per covariate, on one degree of freedom each.</summary>
    /// <param name="data">The sample, sorted as the fit sorted it, with its covariates as given.</param>
    /// <param name="fit">The fitted model.</param>
    /// <param name="covariance">The fit's model-based covariance, row-major.</param>
    /// <param name="transform">The time scale.</param>
    /// <remarks>
    /// long-comment: lifelines' arithmetic, which is not R's since 2019.
    /// The Schoenfeld residual of each event is its covariates less the Efron-weighted mean of its risk set, at the
    /// fitted coefficients and on the covariates as given; scaled, it is <c>d · r · V</c>, <c>d</c> the number of
    /// events and <c>V</c> the model-based covariance whatever the fit's own. For covariate <c>k</c> the statistic is
    /// <c>(Σ gᵢ sᵢₖ)² / (d · seₖ² · Σ gᵢ²)</c>, <c>g</c> the transformed event times less their mean and <c>se</c> the
    /// fit's standard errors, robust where the fit was; the rank transform counts events along the fit's sorted order.
    /// </remarks>
    internal static double[] Statistics(CoxData data, CoxSummary fit, double[] covariance, CoxTimeTransform transform)
    {
        int p = data.FeatureCount;
        double[] residuals = Residuals(data, [.. fit.Coefficients]);
        double[] times = Times(data, transform);
        int deaths = data.Events.Count(e => e);
        var scaled = new double[deaths * p];
        var eventTimes = new double[deaths];
        int k = 0;
        for (int position = 0; position < data.Count; position++)
        {
            if (!data.Events[position])
            {
                continue;
            }

            eventTimes[k] = times[position];
            for (int b = 0; b < p; b++)
            {
                double sum = 0.0;
                for (int a = 0; a < p; a++)
                {
                    sum += residuals[(position * p) + a] * covariance[(a * p) + b];
                }

                scaled[(k * p) + b] = deaths * sum;
            }

            k++;
        }

        double mean = eventTimes.Average();
        double squares = eventTimes.Sum(t => (t - mean) * (t - mean));
        var statistics = new double[p];
        for (int b = 0; b < p; b++)
        {
            double cross = 0.0;
            for (int i = 0; i < deaths; i++)
            {
                cross += (eventTimes[i] - mean) * scaled[(i * p) + b];
            }

            double error = fit.StandardErrors[b];
            statistics[b] = cross * cross / (deaths * error * error * squares);
        }

        return statistics;
    }

    /// <summary>Each sorted row's Schoenfeld residual, zero for a censored one; lifelines' <c>_compute_schoenfeld_within_strata</c>.</summary>
    private static double[] Residuals(CoxData data, double[] beta)
    {
        int p = data.FeatureCount;
        var residuals = new double[data.Count * p];
        var riskX = new double[p];
        var tieX = new double[p];
        foreach ((_, int start, int end) in data.Strata)
        {
            double risk = 0.0;
            Array.Clear(riskX, 0, p);
            int groupEnd = end;
            while (groupEnd > start)
            {
                int groupStart = groupEnd - 1;
                // S1244: a tie is the same recorded duration.
#pragma warning disable S1244
                while (groupStart > start && data.Durations[groupStart - 1] == data.Durations[groupEnd - 1])
#pragma warning restore S1244
                {
                    groupStart--;
                }

                (double tie, int ties) = AddGroup(data, beta, (groupStart, groupEnd), ref risk, riskX, tieX);
                WriteGroup(data, (groupStart, groupEnd), (risk, riskX), (tie, tieX, ties), residuals);
                groupEnd = groupStart;
            }
        }

        return residuals;
    }

    /// <summary>Adds one time's rows to the risk sums and its events to the tied sums, walking backwards as lifelines does.</summary>
    private static (double Tie, int Ties) AddGroup(
        CoxData data, double[] beta, (int Start, int End) group, ref double risk, double[] riskX, double[] tieX)
    {
        int p = data.FeatureCount;
        double tie = 0.0;
        int ties = 0;
        Array.Clear(tieX, 0, p);
        for (int position = group.End - 1; position >= group.Start; position--)
        {
            double phi = data.Weights[position] * Math.Exp(CoxReport.Eta(data, position, beta));
            ReadOnlySpan<double> row = data.Row(position);
            risk += phi;
            bool observed = data.Events[position];
            tie += observed ? phi : 0.0;
            ties += observed ? 1 : 0;
            for (int a = 0; a < p; a++)
            {
                riskX[a] += phi * row[a];
                tieX[a] += observed ? phi * row[a] : 0.0;
            }
        }

        return (tie, ties);
    }

    /// <summary>The residuals of one time's events: their covariates less the Efron-weighted mean of the risk set.</summary>
    private static void WriteGroup(
        CoxData data,
        (int Start, int End) group,
        (double Zero, double[] First) risk,
        (double Zero, double[] First, int Count) tied,
        double[] residuals)
    {
        if (tied.Count == 0)
        {
            return;
        }

        int p = data.FeatureCount;
        var mean = new double[p];
        for (int l = 0; l < tied.Count; l++)
        {
            double denominator = risk.Zero - (l * tied.Zero / tied.Count);
            for (int a = 0; a < p; a++)
            {
                mean[a] += (risk.First[a] - (l * tied.First[a] / tied.Count)) / (denominator * tied.Count);
            }
        }

        for (int position = group.Start; position < group.End; position++)
        {
            if (!data.Events[position])
            {
                continue;
            }

            ReadOnlySpan<double> row = data.Row(position);
            for (int a = 0; a < p; a++)
            {
                residuals[(position * p) + a] = row[a] - mean[a];
            }
        }
    }

    /// <summary>The transformed time of each sorted row, lifelines' <c>TimeTransformers</c>.</summary>
    private static double[] Times(CoxData data, CoxTimeTransform transform)
    {
        var times = new double[data.Count];
        switch (transform)
        {
            case CoxTimeTransform.Rank:
                int count = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    count += data.Events[i] ? 1 : 0;
                    times[i] = count;
                }

                break;
            case CoxTimeTransform.KaplanMeier:
                double[] survival = WeightedKaplanMeier(data);
                for (int i = 0; i < data.Count; i++)
                {
                    times[i] = 1.0 - survival[i];
                }

                break;
            case CoxTimeTransform.Log:
                for (int i = 0; i < data.Count; i++)
                {
                    times[i] = Math.Log(data.Durations[i]);
                }

                break;
            default:
                Array.Copy(data.Durations, times, data.Count);
                break;
        }

        return times;
    }

    /// <summary>The weighted Kaplan-Meier estimate at each row's own duration, pooled over the strata as lifelines pools it.</summary>
    /// <remarks>
    /// lifelines' <c>_additive_estimate</c>: the risk set is the total weight less the weight removed before each time,
    /// the estimate a running sum of <c>log(n − d) − log(n)</c> exponentiated, and a term whose <c>n − d</c> rounds
    /// below zero, as fractional weights can make it where the last subjects all die, is skipped, not zero.
    /// </remarks>
    private static double[] WeightedKaplanMeier(CoxData data)
    {
        int[] order = [.. Enumerable.Range(0, data.Count).OrderBy(i => data.Durations[i])];
        double total = KahanSum(Enumerable.Range(0, data.Count).Select(i => data.Weights[i]));
        double removedBefore = 0.0;
        double logSurvival = 0.0;
        var survival = new double[data.Count];
        int at = 0;
        while (at < order.Length)
        {
            double time = data.Durations[order[at]];
            int end = at;
            // S1244: a tie is the same recorded duration.
#pragma warning disable S1244
            while (end < order.Length && data.Durations[order[end]] == time)
#pragma warning restore S1244
            {
                end++;
            }

            int[] group = [.. order.Skip(at).Take(end - at)];
            double removed = KahanSum(group.Select(i => data.Weights[i]));
            double deaths = KahanSum(group.Select(i => data.Events[i] ? data.Weights[i] : 0.0));

            double atRisk = total - removedBefore;
            double term = Math.Log(atRisk - deaths) - Math.Log(atRisk);
            logSurvival += double.IsNaN(term) ? 0.0 : term;
            removedBefore += removed;
            for (int k = at; k < end; k++)
            {
                survival[order[k]] = Math.Exp(logSurvival);
            }

            at = end;
        }

        return survival;
    }

    /// <summary>pandas' <c>groupby().sum()</c>: Kahan's compensated sum, which lifelines' event table is built with.</summary>
    private static double KahanSum(IEnumerable<double> values)
    {
        double sum = 0.0;
        double compensation = 0.0;
        foreach (double value in values)
        {
            double y = value - compensation;
            double t = sum + y;
            compensation = t - sum - y;
            sum = t;
        }

        return sum;
    }
}

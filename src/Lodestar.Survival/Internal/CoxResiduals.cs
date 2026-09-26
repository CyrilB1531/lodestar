namespace Lodestar.Survival.Internal;

/// <summary>The residuals lifelines builds its robust variance and its proportional hazards test from.</summary>
internal static class CoxResiduals
{
    /// <summary>The Huber sandwich covariance on the original scale, clusters summed when there are any.</summary>
    /// <param name="data">The sorted, standardised sample.</param>
    /// <param name="beta">The coefficients of the standardised covariates.</param>
    /// <param name="inverse">The inverse of the fit's information on the standardised scale.</param>
    /// <remarks>
    /// lifelines' <c>_compute_sandwich_estimator</c>: each row's weighted score residual times the inverse
    /// information, divided by the covariates' deviations, summed within a cluster, then <c>DᵀD</c>. Its score
    /// residual is a row-level one that takes no account of ties, so tied subjects' residuals depend on their
    /// order, which the stable sort keeps as lifelines keeps it.
    /// </remarks>
    internal static double[] Sandwich(CoxData data, double[] beta, double[] inverse)
    {
        int p = data.FeatureCount;
        double[] scores = Scores(data, beta);
        var deltas = new Dictionary<int, double[]>();
        var order = new List<int>();
        for (int row = 0; row < data.Count; row++)
        {
            int key = data.Clusters?[row] ?? row;
            if (!deltas.TryGetValue(key, out double[]? delta))
            {
                delta = new double[p];
                deltas.Add(key, delta);
                order.Add(key);
            }

            for (int b = 0; b < p; b++)
            {
                double sum = 0.0;
                for (int a = 0; a < p; a++)
                {
                    sum += scores[(row * p) + a] * inverse[(a * p) + b];
                }

                delta[b] += sum / data.Deviations[b];
            }
        }

        var sandwich = new double[p * p];
        foreach (int key in order)
        {
            double[] delta = deltas[key];
            for (int a = 0; a < p; a++)
            {
                for (int b = 0; b < p; b++)
                {
                    sandwich[(a * p) + b] += delta[a] * delta[b];
                }
            }
        }

        return sandwich;
    }

    /// <summary>lifelines' <c>_compute_score_within_strata</c> for every row, weighted, row-major.</summary>
    /// <remarks>
    /// Row <c>i</c>'s residual is <c>w_i · (e_i (x_i − m_i) − φ_i Σ_{k≤i} e_k w_k (x_i − m_k) / R_k)</c>, with
    /// <c>R_k</c> and <c>m_k</c> the weighted risk sum and mean over the rows from <c>k</c> on in the stratum. The
    /// inner sum is split into <c>x_i Σ c_k − Σ c_k m_k</c> and accumulated forwards, rather than rebuilt per row.
    /// </remarks>
    private static double[] Scores(CoxData data, double[] beta)
    {
        int p = data.FeatureCount;
        var scores = new double[data.Count * p];
        foreach ((_, int start, int end) in data.Strata)
        {
            (double[] phi, double[] risk, double[] riskX) = RiskSums(data, beta, start, end);
            double weightSum = 0.0;
            var meanSum = new double[p];
            for (int i = 0; i < end - start; i++)
            {
                int position = start + i;
                bool observed = data.Events[position];
                if (observed)
                {
                    double c = data.Weights[position] / risk[i];
                    weightSum += c;
                    for (int a = 0; a < p; a++)
                    {
                        meanSum[a] += c * riskX[(i * p) + a] / risk[i];
                    }
                }

                ReadOnlySpan<double> row = data.Row(position);
                for (int a = 0; a < p; a++)
                {
                    double own = observed ? row[a] - (riskX[(i * p) + a] / risk[i]) : 0.0;
                    scores[(position * p) + a] = (own - (phi[i] * ((row[a] * weightSum) - meanSum[a]))) * data.Weights[position];
                }
            }
        }

        return scores;
    }

    /// <summary>Each row's hazard, and the weighted risk sums over it and the rows after it in its stratum.</summary>
    private static (double[] Phi, double[] Risk, double[] RiskX) RiskSums(CoxData data, double[] beta, int start, int end)
    {
        int p = data.FeatureCount;
        int length = end - start;
        var phi = new double[length];
        var risk = new double[length + 1];
        var riskX = new double[(length + 1) * p];
        for (int k = length - 1; k >= 0; k--)
        {
            ReadOnlySpan<double> row = data.Row(start + k);
            phi[k] = Math.Exp(CoxReport.Eta(data, start + k, beta));
            double weighted = data.Weights[start + k] * phi[k];
            risk[k] = weighted + risk[k + 1];
            for (int a = 0; a < p; a++)
            {
                riskX[(k * p) + a] = (weighted * row[a]) + riskX[((k + 1) * p) + a];
            }
        }

        return (phi, risk, riskX);
    }
}

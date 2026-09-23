namespace Lodestar.Preprocessing.Internal;

/// <summary>The two power families and the log-likelihoods their exponents are fitted by.</summary>
internal static class PowerLikelihood
{
    /// <summary>Yeo and Johnson's transform at one exponent.</summary>
    /// <remarks>
    /// Four branches, because the family is defined piecewise about zero and degenerates at
    /// <c>lambda = 0</c> on the non-negative side and at <c>lambda = 2</c> on the other. It is
    /// exactly the reference's own definition.
    /// </remarks>
    internal static double YeoJohnson(double value, double lambda)
    {
        if (value >= 0.0)
        {
            // S1244: the exponent at which the power form is a logarithm, and only that value.
#pragma warning disable S1244
            return lambda == 0.0
#pragma warning restore S1244
                ? Log1P(value)
                : (Math.Pow(value + 1.0, lambda) - 1.0) / lambda;
        }

#pragma warning disable S1244
        return lambda == 2.0
#pragma warning restore S1244
            ? -Log1P(-value)
            : -((Math.Pow(1.0 - value, 2.0 - lambda) - 1.0) / (2.0 - lambda));
    }

    /// <summary>Box and Cox's transform at one exponent; the value must be positive.</summary>
    internal static double BoxCox(double value, double lambda)
    {
        // S1244: as above, the exponent at which the power form is a logarithm.
#pragma warning disable S1244
        return lambda == 0.0
#pragma warning restore S1244
            ? Math.Log(value)
            : (Math.Pow(value, lambda) - 1.0) / lambda;
    }

    /// <summary>The negative log-likelihood Yeo-Johnson's exponent minimises.</summary>
    internal static double NegativeYeoJohnson(ReadOnlySpan<double> column, double lambda)
    {
        var transformed = new double[column.Length];
        double shifted = 0.0;
        for (int i = 0; i < column.Length; i++)
        {
            transformed[i] = YeoJohnson(column[i], lambda);
            shifted += Math.Sign(column[i]) * Log1P(Math.Abs(column[i]));
        }

        return -((-0.5 * column.Length * Math.Log(PopulationVariance(transformed)))
            + ((lambda - 1.0) * shifted));
    }

    /// <summary>The negative log-likelihood Box-Cox's exponent minimises.</summary>
    /// <remarks>
    /// The variance is taken of the transform without its constant offset, as the reference
    /// takes it: the offset shifts every value alike and so cannot change a variance, while
    /// subtracting it costs digits when the exponent is small.
    /// </remarks>
    internal static double NegativeBoxCox(ReadOnlySpan<double> column, double lambda)
    {
        var scaled = new double[column.Length];
        double logSum = 0.0;
        for (int i = 0; i < column.Length; i++)
        {
            double logValue = Math.Log(column[i]);
            logSum += logValue;

            // S1244: the same exponent as above.
#pragma warning disable S1244
            scaled[i] = lambda == 0.0 ? logValue : Math.Pow(column[i], lambda) / lambda;
#pragma warning restore S1244
        }

        return -(((lambda - 1.0) * logSum)
            - (0.5 * column.Length * Math.Log(PopulationVariance(scaled))));
    }

    /// <summary>The bounds the reference searches Yeo-Johnson's exponent over.</summary>
    /// <remarks>
    /// Derived from the data so the transform cannot overflow inside the search: the widest
    /// value expected is twenty times the largest observed, and the exponent is bounded so that
    /// raising it stays inside half the exponent range of a double. The reference computes them
    /// the same way, and reflects them about one when every value is negative.
    /// </remarks>
    internal static (double Low, double High) YeoJohnsonBounds(ReadOnlySpan<double> column)
    {
        double largest = 0.0;
        bool anyNegative = false;
        bool allNegative = true;
        for (int i = 0; i < column.Length; i++)
        {
            largest = Math.Max(largest, Math.Abs(column[i]));
            anyNegative |= column[i] < 0.0;
            allNegative &= column[i] < 0.0;
        }

        double spread = Log1P(20.0 * largest);
        double logEpsilon = Math.Log(Math.Pow(2.0, -52.0));
        double logTiny = (Math.Log(2.2250738585072014e-308) - logEpsilon) / 2.0;
        double logLargest = (Math.Log(double.MaxValue) + logEpsilon) / 2.0;

        double low = logTiny / spread;
        double high = logLargest / spread;
        if (allNegative)
        {
            return (2.0 - high, 2.0 - low);
        }

        return anyNegative ? (Math.Max(2.0 - high, low), Math.Min(2.0 - low, high)) : (low, high);
    }

    /// <summary>The variance a likelihood reads: divided by the count, not by one less.</summary>
    internal static double PopulationVariance(ReadOnlySpan<double> values)
    {
        double sum = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        double mean = sum / values.Length;
        double squares = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            double gap = values[i] - mean;
            squares += gap * gap;
        }

        return squares / values.Length;
    }

    /// <summary>log(1 + x), kept accurate for a small x where the sum would round it away.</summary>
    private static double Log1P(double x)
    {
        double sum = 1.0 + x;

        // S1244: the sum having rounded back to exactly one is the case the series is for.
#pragma warning disable S1244
        if (sum == 1.0)
#pragma warning restore S1244
        {
            return x;
        }

        return Math.Log(sum) * x / (sum - 1.0);
    }
}

namespace Lodestar.Preprocessing.Internal;

/// <summary>The negative log-likelihood Box-Cox's exponent minimises, <c>-scipy.stats.boxcox_llf</c>, over one column.</summary>
/// <remarks>
/// The variance is taken of the transform without its constant offset and in logarithms, as scipy 1.18.1's
/// <c>_log_var</c> takes it: an exponent the bracket search reaches far from zero reads a finite likelihood where
/// <c>x^λ</c> would overflow (#1227). The logs and their sum, which no exponent changes, are read once, and the
/// search's forty-odd steps reuse two buffers rather than allocating per step.
/// </remarks>
internal sealed class BoxCoxObjective
{
    private readonly double[] _logs;
    private readonly double _logSum;
    private readonly double[] _scaled;
    private readonly double[] _work;

    public BoxCoxObjective(ReadOnlySpan<double> column)
    {
        _logs = new double[column.Length];
        for (int i = 0; i < column.Length; i++)
        {
            _logs[i] = Math.Log(column[i]);
        }

        _logSum = NumpyReduction.Sum(_logs);
        _scaled = new double[column.Length];
        _work = new double[column.Length];
    }

    /// <summary>The negative log-likelihood at <paramref name="lambda"/>.</summary>
    public double Negative(double lambda)
    {
        double logVariance;

        // S1244: the exponent at which the transform is a logarithm, and only that value, as scipy tests it.
#pragma warning disable S1244
        if (lambda == 0.0)
#pragma warning restore S1244
        {
            logVariance = Math.Log(NumpyReduction.Variance(_logs, _work, _logSum / _logs.Length));
        }
        else
        {
            logVariance = PowerLikelihood.ScaledLogVariance(_logs, lambda, _scaled, _work) - (2.0 * Math.Log(Math.Abs(lambda)));
        }

        return -(((lambda - 1.0) * _logSum) - (_logs.Length / 2.0 * logVariance));
    }
}

/// <summary>The negative log-likelihood Yeo-Johnson's exponent minimises, <c>yeojohnson_normmax</c>'s <c>_neg_llf</c>.</summary>
/// <remarks>
/// scipy 1.18.1's <c>yeojohnson_llf</c>: on one-signed data the variance is taken in logarithms, as Box-Cox's is,
/// so a transform that rounds every value alike reads a finite likelihood rather than <c>-∞</c>; on mixed data it
/// is taken directly and a subnormal one reads <c>-∞</c>. An infinite likelihood is rejected as <c>_neg_llf</c>
/// rejects it, so the search never runs to a bound (#1227). What no exponent changes is read once.
/// </remarks>
internal sealed class YeoJohnsonObjective
{
    private readonly double[] _column;
    private readonly double[] _logs;
    private readonly double _signedSum;
    private readonly bool _allNonNegative;
    private readonly bool _allNegative;
    private readonly double[] _scratch;
    private readonly double[] _work;

    /// <param name="column">Held, not copied: the objective lives only through the search over it.</param>
    public YeoJohnsonObjective(double[] column)
    {
        _column = column;
        _logs = new double[column.Length];
        _scratch = new double[column.Length];
        _work = new double[column.Length];
        _allNonNegative = true;
        _allNegative = true;
        for (int i = 0; i < column.Length; i++)
        {
            _allNonNegative &= column[i] >= 0.0;
            _allNegative &= column[i] < 0.0;
            _logs[i] = NearZero.Log1P(Math.Abs(column[i]));
            _scratch[i] = Math.Sign(column[i]) * _logs[i];
        }

        // The signed logs summed in the scratch every evaluation overwrites, not an array of their own.
        _signedSum = NumpyReduction.Sum(_scratch);
    }

    /// <summary>The negative log-likelihood at <paramref name="lambda"/>, <c>+∞</c> where the likelihood is infinite.</summary>
    public double Negative(double lambda)
    {
        double exponent = _allNonNegative ? lambda : 2.0 - lambda;
        double logVariance = _allNonNegative || _allNegative
            ? OneSignedLogVariance(exponent)
            : MixedLogVariance(lambda);

        double likelihood = (-_column.Length / 2.0 * logVariance) + ((lambda - 1.0) * _signedSum);
        return double.IsInfinity(likelihood) ? double.PositiveInfinity : -likelihood;
    }

    /// <summary>The log-variance of a one-signed column's transform, from <c>exponent · log1p(|x|)</c>.</summary>
    private double OneSignedLogVariance(double exponent)
    {
        if (Math.Abs(exponent) < NearZero.MachineEpsilon)
        {
            // One-signed, the held signed sum is the logs' own sum, or its exact negation: no second pass for the mean.
            double logSum = _allNonNegative ? _signedSum : -_signedSum;
            return Math.Log(NumpyReduction.Variance(_logs, _scratch, logSum / _logs.Length));
        }

        return PowerLikelihood.ScaledLogVariance(_logs, exponent, _scratch, _work) - (2.0 * Math.Log(Math.Abs(exponent)));
    }

    /// <summary>The log-variance of a mixed-sign column's transform, taken directly; <c>-∞</c> below the smallest normal.</summary>
    private double MixedLogVariance(double lambda)
    {
        for (int i = 0; i < _column.Length; i++)
        {
            _scratch[i] = PowerLikelihood.YeoJohnson(_column[i], lambda);
        }

        double variance = NumpyReduction.Variance(_scratch, _work);
        return variance >= NearZero.SmallestNormal ? Math.Log(variance) : double.NegativeInfinity;
    }
}

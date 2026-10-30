namespace Lodestar.Internal;

/// <summary>The coefficients of Temme's uniform expansion of the incomplete gamma tail, <c>c_k(η)</c> as series in η.</summary>
/// <remarks>
/// <c>c_0 = 1/(λ − 1) − 1/η</c> and <c>c_k = c_(k−1)′/η + β_k/(λ − 1)</c>, β_k fixed by regularity at η = 0, which makes
/// them Stirling's coefficients for Γ*; λ − 1 reverted from <c>η²/2 = λ − 1 − log λ</c>. Each by exact rational
/// arithmetic, then rounded: c_0 starts −1/3, 1/12, −2/135, c_1 at −1/540, DLMF 8.12.10's values. Twelve rows of 61:
/// Lodestar.Stats' incomplete gamma reads sixteen of each in double and 27 − 2k of row k in double-double, Lodestar.Survival's
/// generalized gamma quantile seven whole and its likelihood all twelve.
/// </remarks>
internal static partial class TemmeSeries
{
    /// <summary>The terms in each row, the degree in η plus one.</summary>
    internal const int Degree = 61;

    /// <summary>The rows, c_0 to c_11.</summary>
    internal const int Rows = 12;

    /// <summary>Row k's series at <paramref name="eta"/>, of its first <paramref name="terms"/> terms.</summary>
    // Inlined, over a span of the row: a call indexing the table cost ChiSquaredSf(1000, 1000) 6%, 51 against 48 ns.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static double Value(int k, int terms, double eta)
    {
        ReadOnlySpan<double> coefficients = Row(k, terms);
        double value = coefficients[terms - 1];
        for (int j = terms - 2; j >= 0; j--)
        {
            value = (value * eta) + coefficients[j];
        }
        return value;
    }

    /// <summary>Row k's series at <paramref name="eta"/>, of its first <paramref name="terms"/> terms, and its derivative.</summary>
    internal static (double Value, double Derivative) Evaluate(int k, int terms, double eta)
    {
        ReadOnlySpan<double> coefficients = Row(k, terms);
        double value = 0.0;
        double derivative = 0.0;
        for (int j = terms - 1; j >= 0; j--)
        {
            derivative = (derivative * eta) + value;
            value = (value * eta) + coefficients[j];
        }
        return (value, derivative);
    }

    /// <summary>Row k's first <paramref name="terms"/> coefficients, for a caller that evaluates them its own way.</summary>
    internal static ReadOnlySpan<double> Row(int k, int terms) => Coefficients.AsSpan(k * Degree, terms);
}

namespace Lodestar.Internal;

// Stats' own part of Temme's series: the low parts its 106-bit tails read, which Survival's quantile does not.
internal static partial class TemmeSeries
{
    /// <summary>Row k's coefficient of η^j.</summary>
    internal static double Coefficient(int k, int j) => Coefficients[(k * Degree) + j];

    /// <summary>The rows whose first coefficients carry low parts, and the terms of each row held.</summary>
    internal const int LowRows = 4;

    internal const int LowTerms = 10;

    /// <summary>The low part of row k's coefficient of η^j, below LowRows and LowTerms: with Coefficient, the exact
    /// rational to 106 bits.</summary>
    internal static double CoefficientLow(int k, int j) => Lows[(k * LowTerms) + j];

    // The low parts of the first coefficients, whose doubles are Coefficients': each the exact rational of c_k's recursion
    // less its double, c_0's −1/3 less −0.3333333333333333 the first. Rows of LowTerms, 10 − 2k of row k used.
    private static readonly double[] Lows =
    [
        -1.850371707708594e-17, 4.625929269271485e-18, 5.653913551331816e-19, 6.424901762877063e-20,
        -2.3787433907794843e-20, -1.2452708902909642e-20, 1.1215426647085746e-21, -1.796679213731138e-22,
        5.2664960679965244e-24, -5.099923629038616e-23,
        7.06739193916477e-20, -1.927470528863119e-19, 1.4685489743719e-19, -4.051257500480815e-20,
        4.194033095211416e-21, -8.191470889084797e-24, -1.1807071831874762e-21, 2.61839989546201e-22, 0.0, 0.0,
        2.294607772456094e-19, 1.113649638898691e-19, 4.283267841918042e-20, -1.708008823681511e-22,
        -6.413920180411989e-21, 3.7791977548669234e-22, 0.0, 0.0, 0.0, 0.0,
        5.050686663595025e-20, 9.124252850752496e-21, -1.0352913158647245e-20, -9.999957329345695e-22, 0.0, 0.0, 0.0, 0.0,
        0.0, 0.0,
    ];
}

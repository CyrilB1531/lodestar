namespace Lodestar.Stats.Internal;

/// <summary>The standard normal's quantile as <c>scipy.special.ndtri</c> computes it, which <c>norm.ppf</c> calls.</summary>
/// <remarks>
/// Cephes' rational approximations, by Stephen L. Moshier (BSD, via scipy's xsf; decision 0002): one in
/// <c>p − 0.5</c> on the centre, two in <c>1 / √(−2 ln p)</c> on the tails. Ported statement for statement, so
/// the answer is scipy's to the last bit where <see cref="Math.Log(double)"/> is glibc's, as on Linux, to 1e-14
/// relative elsewhere; DistributionsEdgeTests pins nine points across both tails and the centre against scipy 1.18.1,
/// where inverting <see cref="Normal.Sf"/> by Newton landed up to 5.5e-13 relative away near one half (#1684).
/// </remarks>
internal static class Ndtri
{
    // exp(-2): below it, and above 1 - exp(-2) by symmetry, the tail approximations take over.
    private const double TailEdge = 0.13533528323661269189;

    private const double SquareRootTwoPi = 2.50662827463100050242;

    /// <summary>The <c>z</c> with <c>P(Z ≤ z) = p</c>, for <c>p</c> strictly inside <c>(0, 1)</c>.</summary>
    internal static double Of(double p)
    {
        bool lower = true;
        double y = p;
        if (y > 1.0 - TailEdge)
        {
            y = 1.0 - y;
            lower = false;
        }

        if (y > TailEdge)
        {
            y -= 0.5;
            double y2 = y * y;
            double centre = y + (y * Centre(y2));
            return centre * SquareRootTwoPi;
        }

        double x = Math.Sqrt(-2.0 * Math.Log(y));
        double x0 = x - (Math.Log(x) / x);
        double z = 1.0 / x;
        double x1 = x < 8.0
            ? Near(z)
            : Far(z);
        double tail = x0 - x1;
        return lower ? -tail : tail;
    }

    // Cephes' polevl and p1evl written out: Horner's rule over the same coefficients in the same order, numerator
    // times the variable before the division as Cephes has it, so the bits are theirs without a loop per term.
    /// <summary>The centre, <c>|p − 0.5| &lt; 0.5 − exp(−2)</c>, inside <see cref="TailEdge"/>: <c>y² · P0(y²) / Q0(y²)</c>.</summary>
    private static double Centre(double y2)
    {
        double numerator = -5.99633501014107895267E1;
        numerator = (numerator * y2) + 9.80010754185999661536E1;
        numerator = (numerator * y2) - 5.66762857469070293439E1;
        numerator = (numerator * y2) + 1.39312609387279679503E1;
        numerator = (numerator * y2) - 1.23916583867381258016E0;

        double denominator = y2 + 1.95448858338141759834E0;
        denominator = (denominator * y2) + 4.67627912898881538453E0;
        denominator = (denominator * y2) + 8.63602421390890590575E1;
        denominator = (denominator * y2) - 2.25462687854119370527E2;
        denominator = (denominator * y2) + 2.00260212380060660359E2;
        denominator = (denominator * y2) - 8.20372256168333339912E1;
        denominator = (denominator * y2) + 1.59056225126211695515E1;
        denominator = (denominator * y2) - 1.18331621121330003142E0;

        return y2 * numerator / denominator;
    }

    /// <summary>The tail for <c>√(−2 ln p)</c> between 2 and 8, p between exp(−2) and exp(−32).</summary>
    private static double Near(double z)
    {
        double numerator = 4.05544892305962419923E0;
        numerator = (numerator * z) + 3.15251094599893866154E1;
        numerator = (numerator * z) + 5.71628192246421288162E1;
        numerator = (numerator * z) + 4.40805073893200834700E1;
        numerator = (numerator * z) + 1.46849561928858024014E1;
        numerator = (numerator * z) + 2.18663306850790267539E0;
        numerator = (numerator * z) - 1.40256079171354495875E-1;
        numerator = (numerator * z) - 3.50424626827848203418E-2;
        numerator = (numerator * z) - 8.57456785154685413611E-4;

        double denominator = z + 1.57799883256466749731E1;
        denominator = (denominator * z) + 4.53907635128879210584E1;
        denominator = (denominator * z) + 4.13172038254672030440E1;
        denominator = (denominator * z) + 1.50425385692907503408E1;
        denominator = (denominator * z) + 2.50464946208309415979E0;
        denominator = (denominator * z) - 1.42182922854787788574E-1;
        denominator = (denominator * z) - 3.80806407691578277194E-2;
        denominator = (denominator * z) - 9.33259480895457427372E-4;

        return z * numerator / denominator;
    }

    /// <summary>The tail for <c>√(−2 ln p)</c> between 8 and 64, p between exp(−32) and exp(−2048).</summary>
    private static double Far(double z)
    {
        double numerator = 3.23774891776946035970E0;
        numerator = (numerator * z) + 6.91522889068984211695E0;
        numerator = (numerator * z) + 3.93881025292474443415E0;
        numerator = (numerator * z) + 1.33303460815807542389E0;
        numerator = (numerator * z) + 2.01485389549179081538E-1;
        numerator = (numerator * z) + 1.23716634817820021358E-2;
        numerator = (numerator * z) + 3.01581553508235416007E-4;
        numerator = (numerator * z) + 2.65806974686737550832E-6;
        numerator = (numerator * z) + 6.23974539184983293730E-9;

        double denominator = z + 6.02427039364742014255E0;
        denominator = (denominator * z) + 3.67983563856160859403E0;
        denominator = (denominator * z) + 1.37702099489081330271E0;
        denominator = (denominator * z) + 2.16236993594496635890E-1;
        denominator = (denominator * z) + 1.34204006088543189037E-2;
        denominator = (denominator * z) + 3.28014464682127739104E-4;
        denominator = (denominator * z) + 2.89247864745380683936E-6;
        denominator = (denominator * z) + 6.79019408009981274425E-9;

        return z * numerator / denominator;
    }
}

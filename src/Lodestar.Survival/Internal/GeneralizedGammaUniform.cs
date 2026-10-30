namespace Lodestar.Survival.Internal;

/// <summary>The generalized gamma's log density and log survival near its log-normal limit, in the standardized log time
/// z, by the uniform expansion <see cref="GeneralizedGammaQuantile"/> inverts.</summary>
/// <remarks>
/// Through the gamma at shape 1/λ² the log density cancels terms of order 1/λ² to an O(1) value, and its derivative in
/// λ terms of order 1/λ³: 10,000 log-normal times read the log-likelihood to 1e-5 at λ = 9e-4 and its gradient to
/// noise, and Newton stalled (#1222). Here, with <c>ζ = z √ρ(λz)</c>, <c>ρ(t) = 2(e^t − 1 − t)/t²</c> and <c>η = λζ</c>,
/// the density is <c>φ(ζ) e^(−R)</c>, R Stirling's remainder at 1/λ², and the survival
/// <c>Φ̄(ζ) + λ φ(ζ) Σ C_k(η) λ^(2k)</c>, both smooth through λ = 0.
/// </remarks>
internal static class GeneralizedGammaUniform
{
    // Stirling's series for log Γ(a) − (a − ½) log a + a − ½ log 2π, in 1/a = λ²: the first term left out,
    // 854513/63756 λ^42, is 1e-18 at λ = 0.35.
    private static readonly double[] Stirling =
    [
        1.0 / 12.0, -1.0 / 360.0, 1.0 / 1260.0, -1.0 / 1680.0, 1.0 / 1188.0, -691.0 / 360360.0, 1.0 / 156.0,
        -3617.0 / 122400.0, 43867.0 / 244188.0, -174611.0 / 125400.0,
    ];

    // Below it the expansion meets the gamma's tail to 3e-13; past it the Richardson differences in a = 1/λ², chained by
    // (2/λ³)², held H_λλ to 2e-6 at its edge for z in [−5, 5], and to 9e-6 at λ = 0.25 with a third of their step.
    private const double LikelihoodShape = 0.35;

    /// <summary>The log of lifelines' clip of the gamma's variate to <c>[1e-300, 1e20]</c>, which its branch applies too.</summary>
    internal static readonly double LowestVariate = Math.Log(1e-300);

    internal static readonly double HighestVariate = Math.Log(1e20);

    // ρ's series below |t| = 1, 2/(n+2)!: past its 22nd term, 2/23!, the rest falls under 1e-22.
    private static readonly double[] RatioSeries = RatioCoefficients(22);

    /// <summary>ζ for <paramref name="z"/> at <paramref name="lambda"/>, or null where the survival's expansion does not
    /// hold: past <see cref="LikelihoodShape"/>, or where η leaves the reach of TemmeSeries' rows, or at a NaN.</summary>
    internal static Jet? Standardize(Jet z, Jet lambda) =>
        Zeta(z, lambda) is { } zeta
        && (double.IsInfinity(zeta.Value) || Math.Abs(lambda.Value * zeta.Value) <= GeneralizedGammaQuantile.SeriesReach)
            ? zeta
            : null;

    /// <summary>ζ for <paramref name="z"/> at <paramref name="lambda"/>, or null past <see cref="LikelihoodShape"/>: the
    /// density's closed form needs no series, so it holds at every z.</summary>
    internal static Jet? Zeta(Jet z, Jet lambda)
    {
        if (!(Math.Abs(lambda.Value) < LikelihoodShape))
        {
            return null;
        }
        // At a time of zero or infinity ζ is z, whatever λ: λz alone read 0·∞ at λ = 0 and fell to the gamma at an
        // infinite shape, which threw.
        if (double.IsInfinity(z.Value))
        {
            return Jet.Constant(z.Value, z.Size);
        }
        Jet t = lambda * z;
        // Where λz itself carries the variate e^(λz)/λ² past lifelines' clip to [1e-300, 1e20], the gamma's branch
        // clips it as lifelines does: unclipped, an event at z = 200 read −2e31 at λ = 0.3499 and −1e20 at 0.3501.
        double logX = t.Value - (2.0 * Math.Log(Math.Abs(lambda.Value)));
        if (Math.Abs(t.Value) > 1.0 && !(logX >= LowestVariate && logX <= HighestVariate))
        {
            return null;
        }
        (double value, double first, double second) = Ratio(t.Value);
        double root = Math.Sqrt(value);
        return z * Jet.Apply(t, root, first / (2.0 * root), (second / (2.0 * root)) - (first * first / (4.0 * value * root)));
    }

    /// <summary>The log density of z, <c>log φ(ζ) − R(λ)</c>.</summary>
    internal static Jet LogDensity(Jet zeta, Jet lambda)
    {
        // R = λ² P(λ⁴), in doubles and lifted once: R′ = 2λP + 4λ⁵P′, R″ = 2P + 28λ⁴P′ + 16λ⁸P″.
        double l = lambda.Value;
        double square = l * l;
        double fourth = square * square;
        (double p, double p1, double p2) = PowerSeries.Curvature(Stirling, fourth);
        Jet remainder = Jet.Apply(
            lambda,
            square * p,
            (2.0 * l * p) + (4.0 * fourth * l * p1),
            (2.0 * p) + (28.0 * fourth * p1) + (16.0 * fourth * fourth * p2));
        return Jet.NormalLogPdf(zeta) - remainder;
    }

    /// <summary>The log survival, <c>log Φ̄(ζ) + log(1 + λ h(ζ) Σ C_k(η) λ^(2k))</c>, h the normal hazard, its value
    /// clipped as lifelines clips it away from its log-normal limit.</summary>
    internal static Jet LogSurvival(Jet zeta, Jet lambda)
    {
        // S1244: at λ = 0 exactly lifelines takes the log-normal's own form, which it does not clip.
#pragma warning disable S1244
        bool logNormal = lambda.Value == 0.0;
#pragma warning restore S1244
        if (double.IsInfinity(zeta.Value))
        {
            double limit = Special.NormalLogSf(zeta.Value);
            return Jet.Constant(logNormal || !Special.Clips(limit, out double edge) ? limit : edge, zeta.Size);
        }
        Jet eta = lambda * zeta;
        (double sum, double dEta, double dLambda, double dEtaEta, double dEtaLambda, double dLambdaLambda) =
            Series(eta.Value, lambda.Value);
        Jet series = Jet.Apply(eta, lambda, sum, (dEta, dLambda), (dEtaEta, dEtaLambda, dLambdaLambda));
        (Jet logSf, Jet hazard) = Jet.NormalLogSfAndHazard(zeta);
        Jet logSurvival = logSf + Jet.Log1p(lambda * hazard * series);
        // Clipped as the gamma's branch and lifelines clip the survival, not at λ = 0; its derivatives stop there with the
        // value, where lifelines' autograd_gamma keeps its own, not the clipped value's (docs/equivalence.md).
        return !logNormal && Special.Clips(logSurvival.Value, out double clipped) ? Jet.Constant(clipped, zeta.Size) : logSurvival;
    }

    /// <summary>Σ C_k(η) λ^(2k) and its partials in η and λ, in doubles, for one jet: as jets each row allocated four.</summary>
    private static (double Sum, double DEta, double DLambda, double DEtaEta, double DEtaLambda, double DLambdaLambda) Series(
        double e, double l)
    {
        int terms = GeneralizedGammaQuantile.Terms(e);
        double size = Math.Abs(l);
        double sum = 0.0;
        double dEta = 0.0;
        double dEtaEta = 0.0;
        double dLambda = 0.0;
        double dLambdaLambda = 0.0;
        double dEtaLambda = 0.0;
        double below = 1.0;
        for (int k = 0; k < TemmeSeries.Rows; k++)
        {
            // Row k enters as λ^(2k+1) C_k, whose second derivative in λ, (2k+1) 2k λ^(2k−1) C_k, is below 1e-15
            // past it: at λ = 0.05 eight rows are read, at 0.35 all twelve.
            if (k > 0 && (2 * k) * ((2 * k) + 1) * Math.Pow(size, (2 * k) - 1) < 1e-15)
            {
                break;
            }
            (double c, double c1, double c2) = PowerSeries.Curvature(TemmeSeries.Row(k, terms), e);
            // λ^(2k), its first and second derivatives, from below = λ^(2k−2).
            double power = k == 0 ? 1.0 : below * l * l;
            double first = k == 0 ? 0.0 : 2 * k * below * l;
            double second = k == 0 ? 0.0 : 2 * k * ((2 * k) - 1) * below;
            sum += c * power;
            dEta += c1 * power;
            dEtaEta += c2 * power;
            dLambda += c * first;
            dEtaLambda += c1 * first;
            dLambdaLambda += c * second;
            below = k == 0 ? 1.0 : power;
        }
        return (sum, dEta, dLambda, dEtaEta, dEtaLambda, dLambdaLambda);
    }

    /// <summary><c>ρ(t)</c> alone, for the quantile's Newton steps: <see cref="Ratio"/>'s value, by the same routes.</summary>
    internal static double RatioValue(double t) =>
        Math.Abs(t) < 1.0 ? PowerSeries.Value(RatioSeries.AsSpan(0, RatioTerms(t)), t) : RatioByDifference(t).Value;

    /// <summary><c>ρ(t) = 2(e^t − 1 − t)/t²</c> and its two derivatives: its series below one, where the difference lost
    /// log2(2/t) bits, and the difference above; the value is <see cref="RatioValue"/>'s, so both read one ρ.</summary>
    internal static (double Value, double First, double Second) Ratio(double t) =>
        Math.Abs(t) < 1.0 ? PowerSeries.Curvature(RatioSeries.AsSpan(0, RatioTerms(t)), t) : RatioByDifference(t);

    /// <summary>ρ and its derivatives from the difference itself, at |t| from one or a NaN.</summary>
    private static (double Value, double First, double Second) RatioByDifference(double t)
    {
        double expm1 = NearZero.Expm1(t);
        double square = t * t;
        double value = 2.0 * (expm1 - t) / square;
        double first = (2.0 * expm1 / square) - (2.0 * value / t);
        double second = (2.0 * (expm1 + 1.0) / square) - (4.0 * expm1 / (square * t)) - (2.0 * first / t)
            + (2.0 * value / square);
        return (value, first, second);
    }

    /// <summary>The terms of ρ's series at |t| below one: up to the first whose second derivative, n(n − 1) c_n t^(n−2),
    /// falls under 1e-17, so the value and both derivatives keep every term they need.</summary>
    private static int RatioTerms(double t)
    {
        double size = Math.Abs(t);
        int terms = 3;
        double power = size;
        while (terms < RatioSeries.Length && terms * (terms - 1) * RatioSeries[terms] * power > 1e-17)
        {
            terms++;
            power *= size;
        }
        return terms;
    }

    private static double[] RatioCoefficients(int count)
    {
        var coefficients = new double[count];
        coefficients[0] = 1.0;
        for (int n = 1; n < count; n++)
        {
            coefficients[n] = coefficients[n - 1] / (n + 2);
        }
        return coefficients;
    }
}

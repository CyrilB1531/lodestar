namespace Lodestar.Stats.Internal;

/// <summary>A number held as two doubles, <c>High + Low</c> with <c>|Low|</c> at most half an ulp of High: some 106 bits.</summary>
/// <remarks>
/// For a tail's exponent, whose rounding of 1e-16 relative is <c>|z|</c>·1e-16 in the tail: at F shapes (5, 5) an
/// exponent of tens of nats left tens of ulps of noise, and the tail fell as f rose by an ulp (#1745). Dekker's and
/// Knuth's error-free sums and products (Dekker, 1971), the product's error from <see cref="ExactProduct"/>.
/// </remarks>
internal readonly struct DoubleDouble(double high, double low)
{
    // ln 2 as two doubles, the second the first's rounding: 0.6931471805599453094172321214581765680755...
    private const double Ln2High = 0.6931471805599453;
    private const double Ln2Low = 2.3190468138462996e-17;

    internal double High { get; } = high;

    internal double Low { get; } = low;

    /// <summary>The two parts, for a deconstruction.</summary>
    internal void Deconstruct(out double high, out double low)
    {
        high = High;
        low = Low;
    }

    /// <summary><c>left + right</c> exactly, by Knuth's two-sum.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static DoubleDouble Sum(double left, double right)
    {
        double sum = left + right;
        double virtualRight = sum - left;
        double error = (left - (sum - virtualRight)) + (right - virtualRight);
        return new DoubleDouble(sum, error);
    }

    /// <summary><c>left + right</c> exactly where <c>|left| ≥ |right|</c> or left is zero, by Dekker's fast two-sum.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static DoubleDouble QuickSum(double left, double right)
    {
        double sum = left + right;
        return new DoubleDouble(sum, right - (sum - left));
    }

    /// <summary><c>left · right</c> exactly, its error the product's residual.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static DoubleDouble Product(double left, double right)
    {
        double product = left * right;
        return new DoubleDouble(product, ExactProduct.ProductError(product, left, right));
    }

    /// <summary><paramref name="result"/>, or <paramref name="plain"/>, the operation in double, where it is not finite.</summary>
    /// <remarks>An infinite part keeps a zero error, as a double's ±∞ flows on through exp to 0 or ∞: its ∞ − ∞ error
    /// read NaN, and F's tails at dfs (1.7e308, 10) NaN where main read 0 or 1 (#1745's Review A). Tested once an
    /// operation, not in each two-sum: there it cost the tails 15%.</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static DoubleDouble OrPlain(DoubleDouble result, double plain) =>
        Math.Abs(result.High) <= double.MaxValue ? result : new DoubleDouble(plain, 0.0);

    // The renormalising sums below take Dekker's fast form: each one's first part dominates, as the QD library has it.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator +(DoubleDouble left, DoubleDouble right)
    {
        DoubleDouble high = Sum(left.High, right.High);
        DoubleDouble low = Sum(left.Low, right.Low);
        DoubleDouble partial = QuickSum(high.High, high.Low + low.High);
        return OrPlain(QuickSum(partial.High, partial.Low + low.Low), left.High + right.High);
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator -(DoubleDouble value) => new(-value.High, -value.Low);

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator -(DoubleDouble left, DoubleDouble right) => left + (-right);

    /// <summary><c>left · right</c> to some 106 bits, a double factor exact.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator *(double left, DoubleDouble right)
    {
        DoubleDouble product = Product(left, right.High);
        return OrPlain(QuickSum(product.High, product.Low + (left * right.Low)), product.High);
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator *(DoubleDouble left, DoubleDouble right)
    {
        DoubleDouble product = Product(left.High, right.High);
        return OrPlain(QuickSum(product.High, product.Low + ((left.High * right.Low) + (left.Low * right.High))), product.High);
    }

    /// <summary><c>left / right</c> to some 106 bits.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static DoubleDouble operator /(DoubleDouble left, DoubleDouble right)
    {
        double quotient = left.High / right.High;
        // What the quotient leaves of left, exact to the product's rounding: left − quotient·right.
        DoubleDouble rest = left - (quotient * right);
        return OrPlain(QuickSum(quotient, rest.High / right.High), quotient);
    }

    /// <summary><c>log value</c> for a positive finite double, to some 1e-31 of itself.</summary>
    /// <remarks>
    /// <c>value = m 2^k</c> with m in [√½, √2), and <c>log m = log c + 2 atanh u</c>, c = i/128 the nearest and
    /// <c>u = (m − c)/(m + c)</c> under 0.003: m − c is exact (Sterbenz), the quotient formed to 106 bits, and the series
    /// to u¹⁴ in double-double. Summed from m with its tail in double it held 1e-27, and log Γ 1e-24, a Q of 2e-19 6e-7
    /// off at a shape of 5e-21 (#1745).
    /// </remarks>
    internal static DoubleDouble Log(double value)
    {
        // 0, ∞ and NaN as Math.Log has them: no mantissa to look up.
        if (!(value > 0.0 && value < double.PositiveInfinity))
        {
            return new DoubleDouble(Math.Log(value), 0.0);
        }
        (double mantissa, int exponent) = PowerOfTwo.Split(value);
        // PowerOfTwo.Split leaves the mantissa in [1, 2): past √2 it is halved, exactly.
        if (mantissa > 1.4142135623730951)
        {
            mantissa *= 0.5;
            exponent++;
        }
        int index = (int)Math.Round(mantissa * TableSteps);
        double centre = index / (double)TableSteps;
        DoubleDouble u = new DoubleDouble(mantissa - centre, 0.0) / Sum(mantissa, centre);
        DoubleDouble square = u * u;
        // u (1 + u²/3 + ⋯ + u¹⁴/15): u¹⁶ is under 1e-40.
        DoubleDouble series = OddReciprocals[LogSeriesTerms];
        for (int k = LogSeriesTerms - 1; k >= 0; k--)
        {
            series = (square * series) + OddReciprocals[k];
        }
        DoubleDouble logMantissa = LogTable[index - TableFirst] + (2.0 * (u * series));
        return (exponent * new DoubleDouble(Ln2High, Ln2Low)) + logMantissa;
    }

    /// <summary><c>log(High + Low)</c>, Low being at most an ulp of High: log High and log(1 + r), r = Low/High, to its
    /// second order; a zero low part log High alone, so zero's log is −∞, not 0/0's NaN. To the first order alone, at a
    /// High of one, λ − 1 − log λ read zero for r²/2, the whole of Temme's exponent there (#1745's Review A).</summary>
    internal static DoubleDouble Log(DoubleDouble value)
    {
#pragma warning disable S1244 // A low part of exactly zero adds nothing: High is the whole value.
        if (value.Low == 0.0)
#pragma warning restore S1244
        {
            return Log(value.High);
        }
        double ratio = value.Low / value.High;
        return Log(value.High) + Sum(ratio, -0.5 * ratio * ratio);
    }

    // 1/(2k + 1) for k = 0 to SlowLogTerms, the table's series; Log's reads them to k = LogSeriesTerms.
    private static readonly DoubleDouble[] OddReciprocals = BuildOddReciprocals();

    // Log's last term past u⁰, u¹⁴/15: about its table's centres u¹⁶ is under 1e-40.
    private const int LogSeriesTerms = 7;

    private static DoubleDouble[] BuildOddReciprocals()
    {
        var table = new DoubleDouble[SlowLogTerms + 1];
        for (int k = 0; k < table.Length; k++)
        {
            table[k] = new DoubleDouble(1.0, 0.0) / new DoubleDouble((2 * k) + 1, 0.0);
        }
        return table;
    }

    // (−1)^n / (n! (2n + 1)), the erf series' coefficients; below ErfSeriesSquare its 75th term is under 1e-35 of the sum.
    private static readonly DoubleDouble[] ErfTerms = BuildErfTerms();

    private static DoubleDouble[] BuildErfTerms()
    {
        var table = new DoubleDouble[80];
        DoubleDouble inverseFactorial = new(1.0, 0.0);
        table[0] = inverseFactorial;
        for (int n = 1; n < table.Length; n++)
        {
            inverseFactorial = -inverseFactorial / new DoubleDouble(n, 0.0);
            table[n] = inverseFactorial / new DoubleDouble((2 * n) + 1, 0.0);
        }
        return table;
    }

    // log(i/128) for i from 90 to 182, which cover [√½, √2) to the nearest step, built from the whole series once.
    private const int TableSteps = 128;
    private const int TableFirst = 90;
    private const int TableLast = 182;
    private static readonly DoubleDouble[] LogTable = BuildLogTable();

    private static DoubleDouble[] BuildLogTable()
    {
        var table = new DoubleDouble[TableLast - TableFirst + 1];
        for (int i = TableFirst; i <= TableLast; i++)
        {
            table[i - TableFirst] = SlowLog(i / (double)TableSteps);
        }
        return table;
    }

    // Terms past u⁰ of SlowLog's series: across the table u² is at most 0.031, and its 25th power under 1e-37.
    private const int SlowLogTerms = 24;

    /// <summary><c>log c</c> for c in the table's [0.70, 1.42] as <c>2 atanh((c − 1)/(c + 1))</c>, every term in
    /// double-double, from OddReciprocals: on every entry their 25 divisions took 93 times (#1745's Review A).</summary>
    private static DoubleDouble SlowLog(double centre)
    {
        DoubleDouble u = new DoubleDouble(centre - 1.0, 0.0) / Sum(centre, 1.0);
        DoubleDouble square = u * u;
        DoubleDouble series = new(0.0, 0.0);
        for (int k = SlowLogTerms; k >= 0; k--)
        {
            series = (square * series) + OddReciprocals[k];
        }
        return 2.0 * (u * series);
    }

    /// <summary><c>e^value</c> to some 106 bits, 0 below the doubles and +∞ past them.</summary>
    /// <remarks>
    /// <c>value = (64n + j) ln 2 / 64 + r</c>, <c>|r| ≤ ln 2 / 128</c>: 2^(j/64) from a table, <c>e^r − 1</c> by its
    /// Taylor series to r¹¹, under 1e-33 there, and 2^n applied last. From r/256 and eight squarings it took 0.5 us.
    /// </remarks>
    internal static DoubleDouble ExpExact(DoubleDouble value) => ExpExact(value, 0);

    /// <summary><c>e^value</c> times 2^<paramref name="lift"/>, scaled before its one rounding: a tail below 2^-969 kept a
    /// subnormal low part, and FisherSnedecorCdf(2.27e-154, 4, 5) read an ulp off (#1745's Review A).</summary>
    internal static DoubleDouble ExpExact(DoubleDouble value, int lift)
    {
        double shift = lift * Ln2High;
        if (value.High > 709.79 - shift)
        {
            return new DoubleDouble(double.PositiveInfinity, 0.0);
        }
        if (value.High < -745.2 - shift)
        {
            return new DoubleDouble(0.0, 0.0);
        }
        double k = Math.Round(value.High * (ExpSteps / Ln2High));
        DoubleDouble reduced = value - (k * new DoubleDouble(Ln2High / ExpSteps, Ln2Low / ExpSteps));
        int step = (int)k;
        int index = ((step % ExpSteps) + ExpSteps) % ExpSteps;
        int exponent = (step - index) / ExpSteps;
        DoubleDouble small = SmallExpm1(reduced);
        DoubleDouble power = ExpTable[index];
        DoubleDouble whole = power + (power * small);
        return ScaledOnce(whole, exponent + lift);
    }

    /// <summary><c>e^value − 1</c> to some 106 bits of itself, however small: 1 − e^(−δ) through ExpExact kept one double
    /// of δ = 1e-200, and F's tail at dfs (5, 1e-200) fell by an ulp as f rose (#1745).</summary>
    internal static DoubleDouble Expm1Exact(DoubleDouble value)
    {
        if (Math.Abs(value.High) <= SmallExpReach)
        {
            return SmallExpm1(value);
        }
        return ExpExact(value) - new DoubleDouble(1.0, 0.0);
    }

    // The table's steps per power of two, and the reach of the series it leaves: ln 2 / 128.
    private const int ExpSteps = 64;
    private const double SmallExpReach = 0.0054152123481245725;

    /// <summary><c>e^r − 1</c> for <c>|r| ≤ ln 2 / 128</c> by Taylor's series to r¹¹.</summary>
    private static DoubleDouble SmallExpm1(DoubleDouble reduced)
    {
        DoubleDouble sum = InverseFactorials[SmallTerms];
        for (int n = SmallTerms - 1; n >= 1; n--)
        {
            sum = (reduced * sum) + InverseFactorials[n];
        }
        return reduced * sum;
    }

    // Taylor's terms SmallExpm1 takes.
    private const int SmallTerms = 11;

    private static DoubleDouble[] BuildExpTable()
    {
        var table = new DoubleDouble[ExpSteps];
        for (int j = 0; j < ExpSteps; j++)
        {
            DoubleDouble small = ReducedExpm1(j * new DoubleDouble(Ln2High / ExpSteps, Ln2Low / ExpSteps));
            table[j] = Sum(1.0, small.High) + new DoubleDouble(small.Low, 0.0);
        }
        return table;
    }

    /// <summary><c>e^r − 1</c> for <c>|r| ≤ ln 2</c>: the series at r/256, squared back as <c>2s + s²</c>, for the table.</summary>
    private static DoubleDouble ReducedExpm1(DoubleDouble reduced)
    {
        reduced = new DoubleDouble(reduced.High / 256.0, reduced.Low / 256.0);
        DoubleDouble sum = InverseFactorials[TaylorTerms];
        for (int n = TaylorTerms - 1; n >= 1; n--)
        {
            sum = (reduced * sum) + InverseFactorials[n];
        }
        DoubleDouble small = reduced * sum;
        for (int i = 0; i < 8; i++)
        {
            small = (2.0 * small) + (small * small);
        }
        return small;
    }

    // 1/n! to n = 12 as double-doubles, from 1/0!.
    private const int TaylorTerms = 12;
    private static readonly DoubleDouble[] InverseFactorials = BuildInverseFactorials();

    // 2^(j/64) for j from 0 to 63, each from the slow form once: after InverseFactorials, which builds it.
    private static readonly DoubleDouble[] ExpTable = BuildExpTable();

    private static DoubleDouble[] BuildInverseFactorials()
    {
        var table = new DoubleDouble[TaylorTerms + 1];
        table[0] = new DoubleDouble(1.0, 0.0);
        for (int n = 1; n <= TaylorTerms; n++)
        {
            table[n] = table[n - 1] / new DoubleDouble(n, 0.0);
        }
        return table;
    }

    /// <summary><paramref name="value"/>, positive, times 2^<paramref name="exponent"/>, in two steps where one would leave
    /// the doubles, and rounded once where that is subnormal.</summary>
    internal static DoubleDouble ScaledOnce(DoubleDouble value, int exponent)
    {
        int first = exponent / 2;
        int second = exponent - first;
        double firstScale = PowerOfTwo.Of(first);
        double secondScale = PowerOfTwo.Of(second);
        double high = value.High * firstScale * secondScale;
        // A high part rounding to zero at 2^-1075 may still owe a granule to its low part (#1745's Review A).
        return high >= 0.0 && high < NearZero.SmallestNormal && secondScale > 0.0
            ? new DoubleDouble(SubnormalRounded(value, high, firstScale, secondScale), 0.0)
            : new DoubleDouble(high, value.Low * firstScale * secondScale);
    }

    /// <summary>A positive value scaled onto the subnormals, rounded once from both its parts: the high part alone, rounded
    /// there, dropped the low, and a tail near 1e-308 rounded twice (#1745's Review A).</summary>
    private static double SubnormalRounded(DoubleDouble value, double high, double firstScale, double secondScale)
    {
        // What the high part's rounding left, and the granule, both back at the unscaled size, exactly. The rest is a
        // multiple of the high part's ulp, over twice the low part, so the low part decides only a tie (#1745's Review A).
        double rest = value.High - (high / secondScale / firstScale);
        // Twice the rest against the granule: the half granule underflowed to 0 unscaled, and ScaledOnce(3ε, 0) read 4ε.
        double granule = double.Epsilon / secondScale / firstScale;
        bool odd = (BitConverter.DoubleToInt64Bits(high) & 1L) != 0;
        if (2.0 * rest > granule || (2.0 * rest >= granule && (value.Low > 0.0 || (value.Low == 0.0 && odd))))
        {
            return high + double.Epsilon;
        }
        return 2.0 * rest < -granule || (2.0 * rest <= -granule && (value.Low < 0.0 || (value.Low == 0.0 && odd))) ? high - double.Epsilon : high;
    }

    // ½ log 2π as two doubles.
    private const double HalfLogTwoPiHigh = 0.9189385332046728;
    private const double HalfLogTwoPiLow = -3.8782941580672414e-17;

    // Below it log Γ shifts its argument up by recurrence before Stirling's series.
    private const double StirlingFloor = 20.0;

    // From it the three products of LogBeta's steps stay inside the doubles together.
    private const double MergedStepsShape = 1e-100;

    // B_2k / (2k (2k − 1)) for k = 1 to 12, as numerator and denominator: past 20 the 13th term is under 1e-29.
    private static readonly double[] StirlingNumerators = [1, -1, 1, -1, 1, -691, 1, -3617, 43867, -174611, 77683, -236364091];
    private static readonly double[] StirlingDenominators = [12, 360, 1260, 1680, 1188, 360360, 156, 122400, 244188, 125400, 5796, 1506960];
    private static readonly DoubleDouble[] StirlingCoefficients = BuildStirlingCoefficients();

    private static DoubleDouble[] BuildStirlingCoefficients()
    {
        var table = new DoubleDouble[StirlingNumerators.Length];
        for (int k = 0; k < table.Length; k++)
        {
            table[k] = new DoubleDouble(StirlingNumerators[k], 0.0) / new DoubleDouble(StirlingDenominators[k], 0.0);
        }
        return table;
    }

    /// <summary><c>log Γ(value)</c> for a positive value, to some 1e-29 absolute plus 106 bits of its size.</summary>
    /// <remarks>Stirling's series past 20, below it the argument raised by recurrence and the product of the steps divided out.</remarks>
    internal static DoubleDouble LogGamma(DoubleDouble value)
    {
        bool shifted = value.High < StirlingFloor;
        DoubleDouble stirling = ShiftedStirling(value, out DoubleDouble steps);
        return shifted ? stirling - Log(steps) : stirling;
    }

    /// <summary>Stirling's log Γ at the value raised past StirlingFloor by recurrence, and the product of the steps,
    /// <paramref name="steps"/>, whose logarithm it is over log Γ(value).</summary>
    private static DoubleDouble ShiftedStirling(DoubleDouble value, out DoubleDouble steps)
    {
        DoubleDouble one = new(1.0, 0.0);
        steps = one;
        while (value.High < StirlingFloor)
        {
            steps *= value;
            value += one;
        }
        DoubleDouble result = ((value - new DoubleDouble(0.5, 0.0)) * Log(value)) - value + new DoubleDouble(HalfLogTwoPiHigh, HalfLogTwoPiLow);
        return result + StirlingSeries(value);
    }

    // Euler's γ as two doubles, and ζ(2)/2, ζ(3)/3 for log Γ(1 + s)'s next terms.
    private const double EulerHigh = 0.5772156649015329;
    private const double EulerLow = -4.942915152430645e-18;
    private const double HalfZeta2 = 0.8224670334241132;
    private const double ThirdZeta3 = 0.40068563438653143;

    // Below it log Γ(1 + s) is its series −γ s + ζ(2) s²/2 − ζ(3) s³/3, the next term s³ of the first's size.
    private const double SeriesShape = 1e-8;

    /// <summary><c>log Γ(1 + s)</c> to some 1e-21 of itself: below SeriesShape its series, whose −γ s Stirling's form held
    /// only to its 6e-30 absolute, 2e-9 of it at s = 5e-21; past it LogGamma at 1 + s, 6e-30 absolute.</summary>
    internal static DoubleDouble LogGammaOnePlus(double s) =>
        s < SeriesShape
            ? (s * new DoubleDouble(-EulerHigh, -EulerLow)) + new DoubleDouble(s * s * (HalfZeta2 - (ThirdZeta3 * s)), 0.0)
            : LogGamma(Sum(1.0, s));

    /// <summary>Stirling's correction <c>Σ B_2k / (2k (2k − 1) z^(2k−1))</c>, to 1e-29 past 20 and 2e-22 from 10.</summary>
    internal static DoubleDouble StirlingSeries(DoubleDouble value)
    {
        DoubleDouble inverse = new DoubleDouble(1.0, 0.0) / value;
        DoubleDouble inverseSquare = inverse * inverse;
        DoubleDouble sum = StirlingCoefficients[StirlingCoefficients.Length - 1];
        for (int k = StirlingCoefficients.Length - 2; k >= 0; k--)
        {
            sum = (sum * inverseSquare) + StirlingCoefficients[k];
        }
        return sum * inverse;
    }

    /// <summary><c>log B(a, b)</c> to some 106 bits of its parts.</summary>
    /// <remarks>
    /// Past 20 in the larger shape, <c>log Γ(small) − (log Γ(large + small) − log Γ(large))</c>, the increment formed as
    /// <c>(large − ½) log(1 + small/large) + small log(large + small) − small</c> and the series' difference, so no log Γ
    /// of order <c>large log large</c> cancels: in double log B(9.95, 5e13), −301, carried 3e-14 into F's density (#1745).
    /// </remarks>
    internal static DoubleDouble LogBeta(double a, double b)
    {
        // The last pair's on this thread, as an inversion or a search holds its shapes: forming it took a microsecond of each
        // tail. Symmetric, so keyed on the ordered pair, and in thread-static fields: an entry object allocated 48 B a miss.
        (a, b) = (Math.Min(a, b), Math.Max(a, b));
        if (ShapeKey.Matches(a, b, cachedA, cachedB))
        {
            return cachedLogBeta;
        }
        DoubleDouble value = UncachedLogBeta(a, b);
        (cachedA, cachedB, cachedLogBeta) = (a, b, value);
        return value;
    }

    [ThreadStatic]
    private static double cachedA;

    [ThreadStatic]
    private static double cachedB;

    [ThreadStatic]
    private static DoubleDouble cachedLogBeta;

    /// <summary>LogBeta's value, its shapes ordered.</summary>
    private static DoubleDouble UncachedLogBeta(double small, double large)
    {
        DoubleDouble sum = Sum(small, large);
        if (large < StirlingFloor && small >= MergedStepsShape)
        {
            // One logarithm of the three products of steps, not three: each cost a tenth of a microsecond.
            DoubleDouble first = ShiftedStirling(new DoubleDouble(small, 0.0), out DoubleDouble firstSteps);
            DoubleDouble second = ShiftedStirling(new DoubleDouble(large, 0.0), out DoubleDouble secondSteps);
            DoubleDouble whole = ShiftedStirling(sum, out DoubleDouble wholeSteps);
            return first + second - whole - Log(firstSteps * secondSteps / wholeSteps);
        }
        if (large < StirlingFloor)
        {
            return LogGamma(new DoubleDouble(small, 0.0)) + LogGamma(new DoubleDouble(large, 0.0)) - LogGamma(sum);
        }
        DoubleDouble ratio = new DoubleDouble(small, 0.0) / new DoubleDouble(large, 0.0);
        // log1p of the ratio, to its own 106 bits: read on 1 + ratio, below 1e-16 it lost its low part, 4e-17 of log B(0.5,
        // 5e299) (#1746's Review A). Below the normal doubles small (1 − ½/large): FisherSnedecorPdf(1, 2e-5, 1.7e308) 2 ulps.
        DoubleDouble spread = ratio.High >= NearZero.SmallestNormal
            ? Sum(large, -0.5) * Log1P(ratio)
            : small * Sum(1.0, -0.5 / large);
        DoubleDouble series = StirlingSeries(sum) - StirlingSeries(new DoubleDouble(large, 0.0));
        if (small >= StirlingFloor)
        {
            // log Γ(small) less small log(sum) as small log(small/sum) and Stirling's rest: each alone overflowed past shapes
            // of 2.5e305, and LogBeta(5e306, 5e307) read NaN (#1746's Review A).
            var whole = new DoubleDouble(small, 0.0);
            return (small * Log(whole / sum)) - (0.5 * Log(whole)) + new DoubleDouble(HalfLogTwoPiHigh, HalfLogTwoPiLow)
                + StirlingSeries(whole) - spread - series;
        }
        DoubleDouble increment = spread + (small * Log(sum)) - new DoubleDouble(small, 0.0) + series;
        return LogGamma(new DoubleDouble(small, 0.0)) - increment;
    }

    // 1/√π and 2/√π as two doubles.
    private const double InverseSqrtPiHigh = 0.5641895835477563;
    private const double InverseSqrtPiLow = 7.66772980658294e-18;
    private const double TwoInverseSqrtPiHigh = 1.1283791670955126;
    private const double TwoInverseSqrtPiLow = 1.533545961316588e-17;

    // Below this square erfc is one less the series of erf, whose largest term, near e to the power of the square, costs
    // at most 3 of its 32 digits; past it Lentz's fraction for erfc, which needs few terms there.
    private const double ErfSeriesSquare = 9.0;

    /// <summary><c>log(1 + value)</c> for a value above −1: its series below 1e-3 in size, whose terms Log's 1e-32 absolute
    /// would swamp for a tiny value, the logarithm of the sum past it.</summary>
    internal static DoubleDouble Log1P(DoubleDouble value)
    {
        DoubleDouble one = new(1.0, 0.0);
        // |value|, as a negative value near −1 ran the series a hundred thousand terms, and −1 never stopped.
        if (!(Math.Abs(value.High) < Log1PSeriesReach))
        {
            return Log(one + value);
        }
        DoubleDouble power = value;
        DoubleDouble sum = value;
        double n = 1.0;
        while (Math.Abs(power.High) > 1e-33 * Math.Abs(sum.High))
        {
            n += 1.0;
            power *= -value;
            sum += power / new DoubleDouble(n, 0.0);
        }
        return sum;
    }

    // Below it Log1P's series, at most a dozen terms to 1e-33.
    private const double Log1PSeriesReach = 1e-3;

    /// <summary><c>√value</c> to some 106 bits: the double root and one Newton step from the exact residual.</summary>
    internal static DoubleDouble Sqrt(DoubleDouble value)
    {
        double root = Math.Sqrt(value.High);
        if (!(root > 0.0 && root < double.PositiveInfinity))
        {
            return new DoubleDouble(root, 0.0);
        }
        DoubleDouble residual = value - Product(root, root);
        return Sum(root, residual.High / (2.0 * root));
    }

    /// <summary><c>erfc(√value)</c> for a non-negative value, to some 1e-25 of itself.</summary>
    /// <remarks>Temme's tail is ½ erfc of its exponent's root, whose rounding in double that exponent's size multiplied: tens
    /// of ulps of F's tail at dfs of 1e6 (#1745).</remarks>
    internal static DoubleDouble ErfcOfSquareRoot(DoubleDouble value) =>
        // A NaN answers itself, and +∞ the limit 0, before the fraction, which ran its 2,000 terms on either.
        value.High < double.PositiveInfinity
            ? FiniteErfcOfSquareRoot(value)
            : new DoubleDouble(Math.Exp(-value.High), 0.0);

    /// <summary>ErfcOfSquareRoot at a finite value.</summary>
    private static DoubleDouble FiniteErfcOfSquareRoot(DoubleDouble value)
    {
        DoubleDouble one = new(1.0, 0.0);
        DoubleDouble root = Sqrt(value);
        if (value.High < ErfSeriesSquare)
        {
            // The series of erf z: 2/√π times z times the sum over n of (−z²)^n over n! (2n + 1), the coefficients from
            // ErfTerms: two divisions a term took erfc(√8.9) 1.87 us (#1745's Review A).
            DoubleDouble power = one;
            DoubleDouble sum = one;
            for (int n = 1; n < ErfTerms.Length; n++)
            {
                power *= value;
                DoubleDouble step = power * ErfTerms[n];
                sum += step;
                if (Math.Abs(step.High) < 1e-34 * Math.Abs(sum.High))
                {
                    break;
                }
            }
            return one - (new DoubleDouble(TwoInverseSqrtPiHigh, TwoInverseSqrtPiLow) * (root * sum));
        }
        // Laplace's fraction for erfc z, e^(−z²)/√π over z plus ½ over z plus 1 over z plus 3/2 and so on, by modified Lentz.
        DoubleDouble tiny = new(1e-300, 0.0);
        DoubleDouble h = root;
        DoubleDouble c = root;
        DoubleDouble d = new(0.0, 0.0);
        for (int k = 1; k < 2000; k++)
        {
            DoubleDouble numerator = new(0.5 * k, 0.0);
            d = root + (numerator * d);
            d = Math.Abs(d.High) < 1e-300 ? tiny : d;
            c = root + (numerator / c);
            c = Math.Abs(c.High) < 1e-300 ? tiny : c;
            d = one / d;
            DoubleDouble delta = c * d;
            h *= delta;
            // 1e-30, ten widths of delta's own rounding: at 1e-32 the stop waited on that noise.
            if (Math.Abs((delta - one).High) < 1e-30)
            {
                break;
            }
        }
        return new DoubleDouble(InverseSqrtPiHigh, InverseSqrtPiLow) * ExpExact(-value) / h;
    }

    /// <summary>The value times <paramref name="power"/>, a power of two, both parts exactly while they stay normal.</summary>
    internal DoubleDouble Scaled(double power) => new(High * power, Low * power);

    /// <summary>The double nearest the value, its low part added once.</summary>
    internal double Rounded => High + Low;
}

namespace Lodestar.Stats.Internal;

/// <summary>Powers of a number below the normal doubles, held as a mantissa and an exponent of two until the last step.</summary>
/// <remarks>
/// <c>e^(s log y)</c> takes its exponent's rounding <c>|s log y|</c> times, 3e-14 at <c>y^½</c> of a subnormal y, as
/// FisherSnedecorCdf(1e-310, 1, 3) read (#1729's Review A). Here <c>y = m 2^e</c>: <c>m^s</c> by Math.Pow, <c>e s</c>
/// split exactly into its integer and its fraction, and the integer applied once, at the end, where it rounds once.
/// </remarks>
internal static class PowerOfTwo
{
    // 2^64, which takes a subnormal into the normal doubles for Split, exactly.
    private const double SubnormalLift = 18446744073709551616.0;

    // The masks that keep a double's top 41 or 33 significant bits: times an exponent below 2^12, or 2^20, a product of
    // 53 bits at most, exact.
    private const long HighBits = ~0xFFFL;
    private const long WideHighBits = ~0xFFFFFL;

    // Past it in log2 the power is 0 or +∞ to every caller, whose other factors stay within 2^1900 of one: from 2^-3000
    // a product needs 2^1926 of them to reach the smallest double.
    private const double MaxLog2 = 3000.0;

    /// <summary>The largest |k| <see cref="Power"/> answers: its exponent's whole part lies within MaxPartLog2 of a log2
    /// within MaxLog2, and past MaxLog2 it answers one times 2^±MaxExponent.</summary>
    internal const int MaxExponent = 4001;

    // The most m^s may hold in log2 and stay a normal double, with room for 2 to the fraction.
    private const double MaxPartLog2 = 1000.0;

    private const double Sqrt2 = 1.4142135623730951;

    /// <summary><paramref name="x"/>, positive and finite, as <c>m 2^e</c> with <c>m</c> in [1, 2), exactly.</summary>
    internal static (double Mantissa, int Exponent) Split(double x)
    {
        // 0, ∞ and NaN have no such form, and their bits would read as 2^-1087 or 2^1024: NaN, which no caller hides.
        if (!(x > 0.0 && x < double.PositiveInfinity))
        {
            return (double.NaN, 0);
        }
        long bits = BitConverter.DoubleToInt64Bits(x);
        int biased = (int)((bits >> 52) & 0x7FF);
        int shift = 0;
        if (biased == 0)
        {
            bits = BitConverter.DoubleToInt64Bits(x * SubnormalLift);
            biased = (int)((bits >> 52) & 0x7FF);
            shift = 64;
        }
        double mantissa = BitConverter.Int64BitsToDouble((bits & 0x000FFFFFFFFFFFFFL) | 0x3FF0000000000000L);
        return (mantissa, biased - 1023 - shift);
    }

    /// <summary><c>(m 2^e)^s</c> as <c>v 2^k</c>, <c>v</c> within a few powers of two of one where <c>s</c> is moderate.</summary>
    internal static (double Value, int Exponent) Power(double mantissa, int exponent, double s)
    {
        (double m, int shift) = Split(mantissa);
        // A NaN mantissa, Split's answer to 0 or ∞, or a NaN shape is NaN, which the overflow test below would read as a side.
        if (double.IsNaN(m) || double.IsNaN(s))
        {
            return (double.NaN, 0);
        }
        int e = exponent + shift;
        if (m > Sqrt2)
        {
            m /= 2.0;
            e++;
        }
        double high = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(s) & (Math.Abs(e) < 4096 ? HighBits : WideHighBits));
        double whole = e * high;
        // Past MaxLog2 in log2 of the power nothing a caller multiplies in brings it back, and m^s alone could overflow:
        // at s = 2500, +∞ times a factor already 0 read FisherSnedecorSf(1e40, 1e300, 5000) NaN (#1729's Review A).
        double log2Power = whole + (s * Math.Log(m) / Beta.Ln2);
        if (!(Math.Abs(log2Power) <= MaxLog2))
        {
            // A NaN is e s overflowing against s log m, which |log2 m| ≤ ½ never outweighs: e's sign then decides.
            double side = double.IsNaN(log2Power) ? whole : log2Power;
            // One times 2^±MaxExponent, not 0 or +∞, so a caller's factor of 0 makes 0 rather than NaN where scaled.
            return (1.0, side < 0.0 ? -MaxExponent : MaxExponent);
        }
        double floor = Math.Floor(whole);
        double fraction = (whole - floor) + (e * (s - high));
        (double part, int partExponent) = MantissaPower(m, s, log2Power - whole);
        return (part * Math.Pow(2.0, fraction), (int)floor + partExponent);
    }

    /// <summary><c>m^s</c> as <c>v 2^k</c>, <paramref name="log2"/> being about <c>s log2 m</c>: past 2^±MaxPartLog2, where
    /// Math.Pow would leave the doubles, as <c>m^(s/2^j)</c> squared j times, each square split into mantissa and
    /// exponent; 2 to the rounded log2 instead carried |log2| ε, 3e-13 at 3000 (#1729's Review A).</summary>
    private static (double Value, int Exponent) MantissaPower(double m, double s, double log2)
    {
        int squarings = 0;
        while (!(Math.Abs(log2) <= MaxPartLog2))
        {
            s /= 2.0;
            log2 /= 2.0;
            squarings++;
        }
        double value = Math.Pow(m, s);
        int exponent = 0;
        for (int i = 0; i < squarings; i++)
        {
            (double mantissa, int binary) = Split(value);
            value = mantissa * mantissa;
            exponent = 2 * (exponent + binary);
        }
        return (value, exponent);
    }

    /// <summary>The product of <paramref name="numerators"/> over that of <paramref name="denominators"/>, times
    /// <c>2^exponent</c>, as mantissas and one exponent of two applied last: no partial product leaves the doubles, and
    /// only the scale rounds into the subnormals. Its factors are non-negative: zeros and +∞ read as the reals do, NaN
    /// where they meet, and a negative factor, which no caller forms, reads NaN.</summary>
    internal static double Quotient(ReadOnlySpan<double> numerators, ReadOnlySpan<double> denominators, int exponent)
    {
        (bool nanAbove, bool zeroAbove, bool infiniteAbove) = Kinds(numerators);
        (bool nanBelow, bool zeroBelow, bool infiniteBelow) = Kinds(denominators);
        if (nanAbove || nanBelow || (zeroAbove && infiniteAbove) || (zeroBelow && infiniteBelow)
            || (zeroAbove && zeroBelow) || (infiniteAbove && infiniteBelow))
        {
            return double.NaN;
        }
        if (zeroAbove || infiniteBelow)
        {
            return 0.0;
        }
        if (infiniteAbove || zeroBelow)
        {
            return double.PositiveInfinity;
        }
        double mantissa = 1.0;
        long total = exponent;
        foreach (double value in numerators)
        {
            (double m, int e) = Split(value);
            (mantissa, int carry) = Split(mantissa * m);
            total += e + carry;
        }
        foreach (double value in denominators)
        {
            (double m, int e) = Split(value);
            (mantissa, int carry) = Split(mantissa / m);
            total += carry - e;
        }
        // Past ±MaxExponent's double the answer is 0 or +∞ whatever the mantissa, and the int stays in range.
        return Scale(mantissa, (int)Math.Max(-2 * MaxExponent, Math.Min(2 * MaxExponent, total)));
    }

    private static (bool NaN, bool Zero, bool Infinite) Kinds(ReadOnlySpan<double> values)
    {
        bool nan = false;
        bool zero = false;
        bool infinite = false;
        foreach (double value in values)
        {
            nan |= double.IsNaN(value);
            zero |= !(Math.Abs(value) > 0.0) && !double.IsNaN(value);
            infinite |= double.IsPositiveInfinity(value);
        }
        return (nan, zero, infinite);
    }

    /// <summary><c>2^exponent</c>, from -1074 to 1023.</summary>
    internal static double Of(int exponent)
    {
        if (exponent > 1023)
        {
            return double.PositiveInfinity;
        }
        if (exponent >= -1022)
        {
            return BitConverter.Int64BitsToDouble((long)(exponent + 1023) << 52);
        }
        // Below 2^-1074 the shift would be negative, which C# masks to six bits: a sign bit or a stray subnormal.
        return exponent >= -1074 ? BitConverter.Int64BitsToDouble(1L << (exponent + 1074)) : 0.0;
    }

    /// <summary><paramref name="value"/> times <c>2^exponent</c>, rounding once, where the result leaves the normal doubles.</summary>
#if NET
    internal static double Scale(double value, int exponent) => Math.ScaleB(value, exponent);
#else
    // Math.ScaleB is not on netstandard2.0: exact steps to 2^-1022, then the one rounding.
    internal static double Scale(double value, int exponent)
    {
        // As Math.ScaleB, a negative value scaled by its magnitude.
        if (value < 0.0)
        {
            return -Scale(-value, exponent);
        }
        if (!(value > 0.0) || double.IsInfinity(value) || exponent == 0)
        {
            return value;
        }
        (_, int own) = Split(value);
        int target = own + exponent;
        if (target > 1023)
        {
            return double.PositiveInfinity;
        }
        if (target >= -1022)
        {
            return Steps(value, exponent);
        }
        // Brought to 2^-1022 exactly first, so the one step into the subnormals is the only one that rounds.
        int toEdge = -1022 - own;
        double atEdge = Steps(value, toEdge);
        int rest = exponent - toEdge;
        return rest < -1074 ? 0.0 : atEdge * Of(rest);
    }

    private static double Steps(double value, int exponent)
    {
        while (exponent > 1023)
        {
            value *= Of(1023);
            exponent -= 1023;
        }
        while (exponent < -1022)
        {
            value *= Of(-1022);
            exponent += 1022;
        }
        return value * Of(exponent);
    }
#endif
}

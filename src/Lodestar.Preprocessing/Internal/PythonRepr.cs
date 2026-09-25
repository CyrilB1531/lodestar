using System.Globalization;

namespace Lodestar.Preprocessing.Internal;

/// <summary>A floating value written as Python's <c>str</c> writes it, which is what scikit-learn's feature names carry.</summary>
/// <remarks>
/// The shortest digits that round-trip, found by the smallest <c>"G"</c> precision that parses back to the value —
/// <c>"R"</c> is the shortest only from .NET Core 3.0, and the netstandard2.0 build also runs on .NET Framework —
/// laid out by Python's rule: positional
/// between <c>1e-4</c> and <c>1e16</c> with at least one decimal (<c>1.0</c>), scientific outside it with a
/// two-digit exponent at least (<c>1e-05</c>, <c>1.5e+16</c>), and <c>nan</c>, <c>inf</c> by name.
/// </remarks>
internal static class PythonRepr
{
    public static string Double(double value)
    {
        for (int precision = 1; precision < 17; precision++)
        {
            string text = value.ToString("G" + precision.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            if (BitConverter.DoubleToInt64Bits(double.Parse(text, CultureInfo.InvariantCulture)) == BitConverter.DoubleToInt64Bits(value))
            {
                return Float(value, text, single: false);
            }
        }

        return Float(value, value.ToString("G17", CultureInfo.InvariantCulture), single: false);
    }

    public static string Single(float value)
    {
        for (int precision = 1; precision < 9; precision++)
        {
            string text = value.ToString("G" + precision.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            if (BitConverter.DoubleToInt64Bits(float.Parse(text, CultureInfo.InvariantCulture)) == BitConverter.DoubleToInt64Bits(value))
            {
                return Float(value, text, single: true);
            }
        }

        return Float(value, value.ToString("G9", CultureInfo.InvariantCulture), single: true);
    }

    /// <remarks>
    /// Python's rule reads the shortest digits' exponent, positional from <c>1e-4</c> up to <c>1e16</c>; numpy's
    /// <c>float32</c> reads the value itself and stops at <c>1e6</c>, so <c>float32(0.0001)</c>, just below
    /// <c>1e-4</c>, is <c>1e-04</c> there — measured on numpy 2.5.3.
    /// </remarks>
    private static string Float(double value, string shortest, bool single)
    {
        if (double.IsNaN(value))
        {
            return "nan";
        }

        if (double.IsInfinity(value))
        {
            return value > 0 ? "inf" : "-inf";
        }

        // The sign from the bits, not the text: .NET Framework writes -0.0 as "0" and reads "-0" back as +0.
        string sign = BitConverter.DoubleToInt64Bits(value) < 0 ? "-" : string.Empty;
        string body = shortest.TrimStart('-');
        int e = body.AsSpan().IndexOf('E');
        int exponent = e < 0 ? 0 : int.Parse(body[(e + 1)..], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        string mantissa = e < 0 ? body : body[..e];
        int point = mantissa.AsSpan().IndexOf('.');
        string whole = point < 0 ? mantissa : mantissa[..point];
        string all = whole + (point < 0 ? string.Empty : mantissa[(point + 1)..]);
        string digits = all.TrimStart('0').TrimEnd('0');
        if (digits.Length == 0)
        {
            return sign + "0.0";
        }

        // The power of ten of the first significant digit.
        int leading = all.Length - all.TrimStart('0').Length;
        int power = whole.Length - 1 - leading + exponent;
        bool positional = single
            ? Math.Abs(value) >= 1e-4 && Math.Abs(value) < 1e6
            : power is >= -4 and < 16;
        return sign + (positional ? Positional(digits, power) : Scientific(digits, power));
    }

    private static string Positional(string digits, int power)
    {
        if (power < 0)
        {
            return "0." + new string('0', -power - 1) + digits;
        }

        string whole = digits.Length > power + 1 ? digits[..(power + 1)] : digits.PadRight(power + 1, '0');
        string fraction = digits.Length > power + 1 ? digits[(power + 1)..] : "0";
        return whole + "." + fraction;
    }

    private static string Scientific(string digits, int power)
    {
        string mantissa = digits.Length == 1 ? digits : digits[0] + "." + digits[1..];
        string magnitude = Math.Abs(power).ToString("00", CultureInfo.InvariantCulture);
        return mantissa + "e" + (power < 0 ? "-" : "+") + magnitude;
    }
}

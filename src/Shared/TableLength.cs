namespace Lodestar.Internal;

/// <summary>The length of a row-major table whose two sizes come from the caller.</summary>
internal static class TableLength
{
    /// <summary><c>Array.MaxLength</c>, which netstandard2.0 does not declare.</summary>
    public const long MaxLength = 0x7FFFFFC7;

    /// <summary>The cell count, refused rather than wrapped in <c>int</c> past the largest array (#1314).</summary>
    /// <exception cref="ArgumentException">The table holds more cells than one array can.</exception>
    public static int Of(int rows, int columns, string paramName)
    {
        long length = (long)rows * columns;
        if (length > MaxLength)
        {
            throw new ArgumentException(
                $"{rows} rows of {columns} is {length} cells, more than one array holds.", paramName);
        }

        return (int)length;
    }
}

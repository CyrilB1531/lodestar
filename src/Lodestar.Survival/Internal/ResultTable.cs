namespace Lodestar.Survival.Internal;

/// <summary>The length of a row-major table whose two sizes both come from the caller.</summary>
internal static class ResultTable
{
    /// <summary>The public parameter a table sized by the covariates or parameters is blamed on.</summary>
    public const string FeatureCount = "featureCount";

    /// <summary>The public parameter a table sized by the groups is blamed on.</summary>
    public const string Groups = "groups";

    /// <summary><c>Array.MaxLength</c>, which netstandard2.0 does not declare.</summary>
    private const long MaxLength = 0x7FFFFFC7;

    /// <summary>The cell count, refused rather than wrapped past the largest array (#1308).</summary>
    public static int Length(int rows, int columns, string paramName)
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

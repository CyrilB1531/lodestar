namespace Lodestar.Stats.Regression.Internal;

/// <summary>The checks the options records share, each written once.</summary>
/// <remarks>
/// <c>OlsOptions</c>, <c>GlmOptions</c> and <c>MultinomialLogitOptions</c> take the same three settings with the same
/// bounds and the same messages. Three copies of each <c>init</c> accessor is what SonarCloud reported as 4.6%
/// duplication on #788, and a caller reading two of those records would have to check that they still agreed.
/// </remarks>
internal static class OptionGuards
{
    /// <summary>A confidence level, strictly inside (0, 1).</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not.</exception>
    public static double ConfidenceLevel(double value)
    {
        if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), value, "A confidence level lies strictly inside (0, 1).");
        }

        return value;
    }

    /// <summary>An iteration budget, one or more.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public static int MaximumIterations(int value)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), value, "An iteration budget is one or more.");
        }

        return value;
    }

    /// <summary>A convergence tolerance, above zero.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not.</exception>
    public static double Tolerance(double value)
    {
        if (double.IsNaN(value) || value <= 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), value, "A convergence tolerance is above zero.");
        }

        return value;
    }
}

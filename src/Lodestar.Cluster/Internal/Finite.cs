namespace Lodestar.Cluster.Internal;

/// <summary>The one refusal every fit shares: a value no distance can be taken to.</summary>
internal static class Finite
{
    /// <summary>Refuses a <c>NaN</c> or infinite value, naming the first one found.</summary>
    /// <remarks>
    /// scikit-learn's <c>check_array</c> refuses them with a <c>ValueError</c> before fitting. Let
    /// through, a <c>NaN</c> centre or an infinite merge height comes back as a clustering, or an
    /// index goes out of range where no candidate beats an infinite distance (#896).
    /// </remarks>
    /// <exception cref="ArgumentException">A value is <c>NaN</c> or infinite.</exception>
    public static void Require(ReadOnlySpan<double> values, string parameterName)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (double.IsNaN(values[i]) || double.IsInfinity(values[i]))
            {
                throw new ArgumentException(
                    $"{parameterName}[{i}] is {values[i]}; every value must be finite.", parameterName);
            }
        }
    }
}

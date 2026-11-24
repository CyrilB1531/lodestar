namespace Lodestar.Stats.Regression.Internal;

/// <summary>A robust covariance and the parts it was built from: <c>Factor · Bread · FᵀF · Bread</c>.</summary>
/// <param name="Covariance">The coefficients' covariance, row-major.</param>
/// <param name="Bread"><c>(XᵀX)⁻¹</c>, row-major.</param>
/// <param name="Factor">The small-sample factor, one where the type has none.</param>
/// <param name="Scores">The score rows whose cross product is the filling, built when called.</param>
internal sealed record SandwichParts(double[] Covariance, double[] Bread, double Factor, Func<ScoreRows> Scores)
{
    /// <summary>The most rank the filling can hold, or null where its construction sets none below the coefficients.</summary>
    public int? LargestRank { get; init; }

    /// <summary>Whether the covariance is zero: every residual exactly zero and no HC2 or HC3 divisor zero, or the rows interpolated (#1779, #1784).</summary>
    public bool ZeroBlock { get; init; }

    /// <summary>An exact fit's covariance: zero, with no bread, factor or score rows a test could read.</summary>
    public static SandwichParts Zero(int parameterCount) => new(
        new double[parameterCount * parameterCount],
        [],
        1.0,
        static () => throw new InvalidOperationException("An exact fit's zero covariance has no score rows to read."))
    {
        LargestRank = 0,
        ZeroBlock = true,
    };
}

/// <summary>The score rows <c>F</c>, <c>FᵀF</c> the filling, each row-major, and the size of the terms each entry sums.</summary>
/// <param name="Rows">The rows.</param>
/// <param name="Magnitudes">Each entry's terms in absolute value, residuals' rounding included, summed: what rounding scales with.</param>
/// <param name="Count">How many rows.</param>
internal readonly record struct ScoreRows(double[] Rows, double[] Magnitudes, int Count);

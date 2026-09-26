namespace Lodestar.Survival;

/// <summary>What a <see cref="ParametricSurvival"/> fit may be told.</summary>
/// <remarks>Each setting is checked where it is set, as <see cref="CoxOptions"/> checks its own.</remarks>
public sealed record ParametricOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;
    private double[] _breakpoints = [];

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }

    /// <summary>How many Newton steps the fit may take. Default 100.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "An iteration budget is one or more.");
            }

            _maximumIterations = value;
        }
    }

    /// <summary>The piecewise exponential's breakpoints, lifelines' <c>breakpoints</c>: positive, finite and ascending.</summary>
    /// <exception cref="ArgumentException">A breakpoint is not positive and finite, or they do not strictly ascend.</exception>
    // CA1819 (properties should not return arrays): the breakpoints are a configuration read once per fit, as
    // KMeansOptions.InitialCentres is.
#pragma warning disable CA1819
    public double[] Breakpoints
#pragma warning restore CA1819
    {
        get => _breakpoints;
        init
        {
            Guard.NotNull(value);
            for (int i = 0; i < value.Length; i++)
            {
                if (!(value[i] > 0.0) || double.IsInfinity(value[i]) || (i > 0 && !(value[i] > value[i - 1])))
                {
                    throw new ArgumentException("Breakpoints are positive, finite and strictly ascending.", nameof(value));
                }
            }

            _breakpoints = [.. value];
        }
    }
}

using Xunit;
using Lodestar.Stats.Internal;

namespace Lodestar.Stats.Tests;

/// <summary>The inversion's fallback, on a tail that leaves Newton no step.</summary>
public sealed class TailInversionTests
{
    // Sf(x) = 1 / (2 (1 + x)), with no density, so every Newton step is infinite and the fallback alone moves.
    private readonly struct StepFreeTail : IUpperTail
    {
        public double Survival(double x) => 0.5 / (1.0 + x);

        public double LogDensity(double x) => double.NegativeInfinity;
    }

    [Theory]
    [InlineData(1e-30, 60.0)]
    [InlineData(1e-300, 1e6)]
    [InlineData(0.5, 0.25)]
    public void The_fallback_alone_reaches_a_root_from_below_one(double seed, double root)
    {
        // Growth by the square root alone never passed 1: from 1e-30 it settled near 1 for a root of 60 (#1690's review).
        double p = 0.5 / (1.0 + root);
        Assert.Equal(root, TailInversion.InvertUpperTail(new StepFreeTail(), p, seed), 1e-12 * root);
    }
}

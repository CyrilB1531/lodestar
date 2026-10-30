namespace Lodestar.Internal;

/// <summary><c>ψ′(x)</c>, the trigamma function, the one copy Lodestar.Stats and Lodestar.Survival share (#1746's Review A).</summary>
internal static class Trigamma
{
    // The recurrence's floor: past it the series' next term, B₁₀/x¹¹, is under 1e-11 of ψ′.
    private const double Floor = 10.0;

    /// <summary><c>ψ′(x)</c> for a positive x, below one too, as Survival's jet passes it 1/λ², by <c>ψ′(x) = ψ′(x + 1) + 1/x²</c> up to Floor, then its asymptotic series.</summary>
    internal static double Of(double x)
    {
        double steps = 0.0;
        while (x < Floor)
        {
            steps += 1.0 / (x * x);
            x += 1.0;
        }
        double w = 1.0 / (x * x);
        return steps + ((1.0 / x) * (1.0 + (0.5 / x) + (w * ((1.0 / 6.0) + (w * ((-1.0 / 30.0) + (w * ((1.0 / 42.0)
            + (w * (-1.0 / 30.0))))))))));
    }
}

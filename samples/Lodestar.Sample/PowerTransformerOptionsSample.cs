using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Which family is fitted, and whether the result is standardised.</summary>
internal static class PowerTransformerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("PowerTransformerOptions (Lodestar.Preprocessing)");

        double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

        // The two families agree on the direction and disagree on the number.
        var boxCox = new PowerTransformerOptions { Method = PowerMethod.BoxCox };
        Console.WriteLine($"  {boxCox.Method} lambda   : {Inv.F4(PowerTransformer.Fit(income, 1, boxCox).Lambdas[0])}");

        // Without the standardisation the powered column spans about a tenth of a unit.
        var raw = new PowerTransformerOptions { Standardize = false };
        Console.WriteLine($"  Standardize {raw.Standardize}: {Inv.List(PowerTransformer.Fit(income, 1, raw).Transform([22.0, 260.0]))}");
        Console.WriteLine();
    }
}

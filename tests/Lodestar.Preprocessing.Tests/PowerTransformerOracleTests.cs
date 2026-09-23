using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_power.json</c>, at the tolerance the objective allows.</summary>
/// <remarks>
/// <c>1e-5</c> rather than the repository's <c>1e-9</c>, and the reason is arithmetic rather than
/// indulgence: the log-likelihood has curvature about 176 at its optimum and a value around 443,
/// which a double carries to roughly <c>3e-11</c>, so the exponent is pinned only to about
/// <c>6e-7</c> — and that moves the transformed values by <c>9.1e-7</c> relative. The corpus
/// generator's docstring carries the derivation, and <c>docs/equivalence.md</c> the row.
/// </remarks>
public sealed class PowerTransformerOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_power.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            var options = new PowerTransformerOptions
            {
                Method = args.GetProperty("method").GetString() == "box-cox"
                    ? PowerMethod.BoxCox
                    : PowerMethod.YeoJohnson,
                Standardize = args.GetProperty("standardize").GetBoolean(),
            };

            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            PowerTransformer fitted = PowerTransformer.Fit(samples, featureCount, options);

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("lambdas")),
                [.. fitted.Lambdas],
                $"{name} lambdas",
                PreprocessingOracleAsserts.PowerTolerance);

            double[] transformed = fitted.Transform(samples);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")),
                transformed,
                name,
                PreprocessingOracleAsserts.PowerTolerance);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("inverse")),
                fitted.InverseTransform(transformed),
                $"{name} inverse",
                PreprocessingOracleAsserts.PowerTolerance);
            replayed++;
        }

        Assert.True(replayed >= 10, $"only {replayed} cases replayed");
    }

    /// <summary>
    /// The exponent is nearer the reference's than the tolerance claims, which is the point of
    /// stating the tolerance from the objective rather than from the implementation: measured
    /// over the corpus, no fitted exponent is further than <c>1e-6</c> from scikit-learn's.
    /// </summary>
    [Fact]
    public void The_fitted_exponents_are_well_inside_the_stated_tolerance()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_power.json");
        double worst = 0.0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            var options = new PowerTransformerOptions
            {
                Method = args.GetProperty("method").GetString() == "box-cox"
                    ? PowerMethod.BoxCox
                    : PowerMethod.YeoJohnson,
                Standardize = args.GetProperty("standardize").GetBoolean(),
            };

            PowerTransformer fitted = PowerTransformer.Fit(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("samples")),
                c.GetProperty("featureCount").GetInt32(),
                options);
            double[] expected = PreprocessingOracleAsserts.Doubles(c.GetProperty("lambdas"));
            for (int i = 0; i < expected.Length; i++)
            {
                worst = Math.Max(worst, Math.Abs(expected[i] - fitted.Lambdas[i]));
            }
        }

        Assert.True(worst <= 1e-6, $"the worst exponent gap is {worst}, past 1e-6.");
    }
}

using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>Both option types are records whose Ω compares by value, and NMF starts where scikit-learn's <c>init=None</c> does (#1232).</summary>
public sealed class DecompositionOptionsTests
{
    [Fact]
    public void Separate_arrays_holding_the_same_svd_omega_are_equal()
    {
        TruncatedSvdOptions left = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };
        TruncatedSvdOptions right = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, right with { RandomMatrix = [1.0, 2.5] });
        Assert.NotEqual(left, right with { Normalizer = PowerIterationNormalizer.Qr });
        Assert.NotEqual(new TruncatedSvdOptions(), new TruncatedSvdOptions { RandomMatrix = [] });
    }

    [Fact]
    public void Separate_arrays_holding_the_same_nmf_omega_are_equal()
    {
        NmfOptions left = new() { RandomMatrix = [1.0, 2.0], Tolerance = double.NaN };
        NmfOptions right = new() { RandomMatrix = [1.0, 2.0], Tolerance = double.NaN };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, right with { RandomMatrix = [1.0] });
        Assert.NotEqual(left, right with { BetaLoss = NmfBetaLoss.KullbackLeibler });
        Assert.NotEqual(left, right with { MaxIterations = 7 });
    }

    [Fact]
    public void Nmf_defaults_to_nndsvda_as_scikit_learns_init_none_resolves()
    {
        Assert.Equal(NmfInitialization.NndSvda, new NmfOptions().Initialization);
    }

    [Fact]
    public void Fitted_arrays_cannot_be_cast_back_and_edited()
    {
        CsrMatrix matrix = new(3, 3, [1.0, 2.0, 3.0, 4.0], [0, 1, 1, 2], [0, 2, 3, 4]);

        TruncatedSvd svd = TruncatedSvd.Fit(matrix, 2);
        Assert.IsNotType<double[]>(svd.Components, exactMatch: false);
        Assert.IsNotType<double[]>(svd.SingularValues, exactMatch: false);
        Assert.IsNotType<double[]>(svd.ExplainedVarianceRatio, exactMatch: false);

        Nmf nmf = Nmf.Fit(matrix, 2);
        Assert.IsNotType<double[]>(nmf.Weights, exactMatch: false);
        Assert.IsNotType<double[]>(nmf.Components, exactMatch: false);

        QrDecomposition qr = QrDecomposition.Householder([1.0, 2.0, 3.0, 4.0], 2, 2);
        Assert.IsNotType<double[]>(qr.Q, exactMatch: false);
        Assert.IsNotType<double[]>(qr.R, exactMatch: false);

        PrincipalComponentVariance variance = PrincipalComponentVariance.Compute([1.0, 2.0, 3.0, 5.0, 4.0, 1.0], 3, 2);
        Assert.IsNotType<double[]>(variance.ExplainedVariance, exactMatch: false);
        Assert.IsNotType<double[]>(variance.CumulativeExplainedVarianceRatio, exactMatch: false);
    }
}

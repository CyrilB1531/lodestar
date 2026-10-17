namespace Lodestar.Stats.Internal;

/// <summary>Refuses an option outside its enumeration before an early <c>NaN</c> could answer for it.</summary>
/// <remarks>
/// Each test refused such an option where its computation read it; an empty or NaN sample, which answers <c>NaN</c> before
/// that point (decision 0010), let <c>(Alternative)99</c> through (#1712's Review A). Called after each member's own
/// refusals -- last where a refusal is met inside the computation -- so an input refused for another reason still is (#1704),
/// but for AndersonDarling.KSample's variant, which main refused before the samples and still does.
/// </remarks>
internal static class Options
{
    internal static void Require(Alternative alternative)
    {
        if (alternative is not (Alternative.TwoSided or Alternative.Less or Alternative.Greater))
        {
            throw new ArgumentOutOfRangeException(nameof(alternative), alternative, null);
        }
    }

    internal static void Require(ExactMethod method)
    {
        if (method is not (ExactMethod.Auto or ExactMethod.Exact or ExactMethod.Asymptotic))
        {
            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }

    internal static void Require(Continuity continuity)
    {
        if (continuity is not (Continuity.Applied or Continuity.None))
        {
            throw new ArgumentOutOfRangeException(nameof(continuity), continuity, null);
        }
    }

    internal static void Require(ZeroMethod zeroMethod)
    {
        if (zeroMethod is not (ZeroMethod.Wilcox or ZeroMethod.Pratt or ZeroMethod.ZSplit))
        {
            throw new ArgumentOutOfRangeException(nameof(zeroMethod), zeroMethod, null);
        }
    }

    internal static void Require(Variance variance)
    {
        if (variance is not (Variance.Equal or Variance.Welch))
        {
            throw new ArgumentOutOfRangeException(nameof(variance), variance, null);
        }
    }

    internal static void Require(NanPolicy nanPolicy)
    {
        if (nanPolicy is not (NanPolicy.Propagate or NanPolicy.Raise or NanPolicy.Omit))
        {
            throw new ArgumentOutOfRangeException(nameof(nanPolicy), nanPolicy, null);
        }
    }

    internal static void Require(Center center)
    {
        if (center is not (Center.Median or Center.Mean or Center.Trimmed))
        {
            throw new ArgumentOutOfRangeException(nameof(center), center, null);
        }
    }

    internal static void Require(KendallVariant variant)
    {
        if (variant is not (KendallVariant.TauB or KendallVariant.TauC))
        {
            throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
        }
    }

    internal static void Require(AndersonKSampleVariant variant)
    {
        if (variant is not (AndersonKSampleVariant.Midrank or AndersonKSampleVariant.Right or AndersonKSampleVariant.Continuous))
        {
            throw new ArgumentOutOfRangeException(nameof(variant), variant, "Not a declared k-sample variant.");
        }
    }

    internal static void Require(ProportionInterval method)
    {
        if (method is not (ProportionInterval.Exact or ProportionInterval.Wilson or ProportionInterval.WilsonCorrected))
        {
            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
}

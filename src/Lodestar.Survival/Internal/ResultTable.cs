namespace Lodestar.Survival.Internal;

/// <summary>The public parameters that a table sized by the caller is blamed on, when <c>TableLength.Of</c> refuses it.</summary>
internal static class ResultTable
{
    /// <summary>The public parameter a table sized by the covariates or parameters is blamed on.</summary>
    public const string FeatureCount = "featureCount";

    /// <summary>The public parameter a table sized by the groups is blamed on.</summary>
    public const string Groups = "groups";
}

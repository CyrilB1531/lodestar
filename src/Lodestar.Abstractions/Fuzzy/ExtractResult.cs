namespace Lodestar.Fuzzy;

/// <summary>A single extraction hit: the matched choice, its score and its index in the input.</summary>
public readonly record struct ExtractResult(string Choice, double Score, int Index);

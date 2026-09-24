namespace Lodestar.Text.Keywords;

/// <summary>How RAKE scores a word before the phrase sums it.</summary>
public enum RakeMetric
{
    /// <summary><c>deg(w) / freq(w)</c>. The paper's, and the reference implementation's default.</summary>
    DegreeToFrequencyRatio,

    /// <summary><c>deg(w)</c>: how many words it shares a candidate with, itself included, counted per occurrence.</summary>
    WordDegree,

    /// <summary><c>freq(w)</c>: how often it occurs at all.</summary>
    WordFrequency,
}

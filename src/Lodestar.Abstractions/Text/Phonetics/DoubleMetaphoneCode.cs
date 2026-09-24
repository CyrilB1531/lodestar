namespace Lodestar.Text.Phonetics;

/// <summary>A word's two Double Metaphone codes.</summary>
/// <param name="Primary">The primary code, empty when the word carries no encodable letter.</param>
/// <param name="Secondary">
/// The alternate code, or empty when the word has no alternate pronunciation. The reference
/// repeats the primary there; empty is the convention <c>jellyfish</c>, <c>metaphone</c> and
/// <c>phonetics</c> share, and <c>docs/decisions/0075</c> takes it for this API.
/// </param>
public readonly record struct DoubleMetaphoneCode(string Primary, string Secondary);

# Lodestar.Fuzzy

Applied fuzzy string matching with rapidfuzz's semantics: `Fuzz.Ratio`,
`PartialRatio`, `TokenSortRatio`, `TokenSetRatio` and `WRatio`; `Process.Extract` and
`ExtractOne` over a list of choices; and blocking deduplication for a list too long to compare
pair by pair. The scores are the ones rapidfuzz returns, on a 0 to 100 scale.

## Install

```bash
dotnet add package Lodestar.Fuzzy
```

## Example

```csharp
using Lodestar.Fuzzy;

string[] choices = ["apple pie", "apple tart", "banana bread", "cherry pie"];

IReadOnlyList<ExtractResult> best = Process.Extract("apple pie", choices, limit: 2);
string first = best[0].Choice;                    // apple pie
double typo = Fuzz.Ratio("apple pie", "appel pie"); // 88.88888888888889
```

## Parity

Replayed against rapidfuzz's `fuzz` and `process` modules.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Text` 0.7.0 or later
- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [migrating from rapidfuzz](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/migrating-from-rapidfuzz.md)
- Reference: [fuzzy/matching](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/fuzzy/matching.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Fuzzy/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Fuzzy/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)

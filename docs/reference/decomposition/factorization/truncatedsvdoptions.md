# TruncatedSvdOptions

What [`TruncatedSvd.Fit`](truncatedsvd-fit.md) is allowed to vary. Every property has an
initialiser, so `new TruncatedSvdOptions()` is scikit-learn's own default configuration and you
set only what you are changing.

<!-- docs-declaration -->

```csharp
public sealed record TruncatedSvdOptions
```

## Properties

| Property | Default | What it does |
| --- | --- | --- |
| `Oversampling` | `10` | Extra columns drawn beyond the rank asked for. More is more accurate and slower; scikit-learn's default is the same 10. |
| `PowerIterations` | `5` | How many times the probe block is pushed through `A` and `Aᵀ`. This is the knob that matters when the singular values decay slowly. |
| `Normalizer` | `Auto` | What happens to the block between the two products — see [`PowerIterationNormalizer`](poweriterationnormalizer.md). |
| `Seed` | `0` | Seeds this package's generator when `RandomMatrix` is null. It reproduces a run of Lodestar, never a run of NumPy. |
| `RandomMatrix` | `null` | Ω itself, row-major and `min(rows, columns) × (componentCount + Oversampling)`: a matrix with fewer rows than columns is factored as its transpose, as `randomized_svd(transpose="auto")` factors it. Given, it replaces the draw entirely. |

**`Seed` and `RandomMatrix` answer two different questions.** `Seed` makes a run repeatable on
your machine; the block it draws comes from a SplitMix64 generator this package owns, because
`System.Random` changed algorithm in .NET 6 and this package ships to runtimes on both sides of
that. `RandomMatrix` makes a run repeatable *across implementations* — hand it the block NumPy
drew and the components come back equal to scikit-learn's entry by entry, which is how this
package's conformance is proved. See
[ADR 0004](../../../decisions/0004-what-is-written-here-and-what-is-delegated.md).

`Oversampling` and `PowerIterations` are both refused when negative, and so is an `Oversampling`
too large to add to the rank asked for — an `int` that wrapped would be a block width nobody
asked for rather than an error anybody could read. `Oversampling = 0` is allowed and means the
probe block is exactly as wide as the rank, which is the fastest and the least accurate the
method gets.

A record since #1232, like every other options type: `with` copies one and changes a property,
and two option sets built from separate arrays holding the same Ω compare equal.

## Members

| member | what it does |
| --- | --- |
| [`TruncatedSvdOptions.Equals`](truncatedsvdoptions-equals.md) | Value equality, Ω element by element. |
| [`TruncatedSvdOptions.GetHashCode`](truncatedsvdoptions-gethashcode.md) | A hash consistent with it. |

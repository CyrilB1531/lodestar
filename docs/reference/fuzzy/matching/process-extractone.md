# Process.ExtractOne

The single best candidate, or nothing.

<!-- docs-declaration -->

```csharp
public static ExtractResult? ExtractOne(string query, IEnumerable<string> choices, Func<string, string, double> scorer = null, double scoreCutoff = 0)
```

**Parameters** — `query` is what to match; a null one matches nothing. `choices` are the candidates,
a null one skipped with its index kept. Both follow rapidfuzz's `None`
([#1233](https://github.com/CyrilB1531/lodestar/issues/1233)). `scorer` defaults to
[`Fuzz.WRatio`](fuzz-wratio.md). `scoreCutoff` is the score a candidate must clear.

**Returns** — `ExtractResult?` — the best candidate, or **`null`** when none clears `scoreCutoff` or
`query` is null.

**Exceptions** — `ArgumentNullException` when `choices` is null, unless `query` is null too, which
answers `null` first, as rapidfuzz does.

**Example** — a match, and a query that clears nothing.

```csharp
using Lodestar.Fuzzy;

string[] choices = ["apple pie", "apple tart", "banana bread", "cherry pie"];

ExtractResult? found = Process.ExtractOne("appel pie", choices);
string choice = found?.Choice;  // => apple pie

ExtractResult? none = Process.ExtractOne("zzz", choices, scoreCutoff: 90);
bool nothingMatched = none is null;  // => True
```

**Remarks** — the nullable return is the point. With `scoreCutoff` at its default of `0` every
query matches *something*, and the best of four unrelated candidates is still returned — which is
how a fuzzy match quietly becomes a wrong answer. Setting a cutoff makes "none of these" an
outcome the type can express, and `null` is that outcome.

Choosing the cutoff is a judgement about the data rather than a default worth shipping: `90`
suits near-identical strings, and far lower suits free text.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Process.Extract`](process-extract.md), [`ExtractResult`](extractresult.md).

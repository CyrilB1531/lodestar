# Lodestar.Abstractions review findings after #1293

**Issues:** [#1294](https://github.com/CyrilB1531/lodestar/issues/1294),
[#1295](https://github.com/CyrilB1531/lodestar/issues/1295),
[#1296](https://github.com/CyrilB1531/lodestar/issues/1296),
[#1297](https://github.com/CyrilB1531/lodestar/issues/1297),
[#1298](https://github.com/CyrilB1531/lodestar/issues/1298),
[#1299](https://github.com/CyrilB1531/lodestar/issues/1299),
[#1300](https://github.com/CyrilB1531/lodestar/issues/1300),
[#1301](https://github.com/CyrilB1531/lodestar/issues/1301),
[#1302](https://github.com/CyrilB1531/lodestar/issues/1302).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The review of `Lodestar.Abstractions` on `main` at `c89b289b`, after #1293, found what that pull
request's NaN-stable hashing missed and five untrue sentences:

1. `SearchResult` hashed its `float Score` raw; #1293 covered only `double` members (#1294).
2. `CsrMatrix.NormalizeRows` wrote `[NaN, 0]` for a row holding an infinity, where
   `sklearn.preprocessing.normalize` refuses a `NaN` or an infinity (#1295).
3. The Rake, TextRank and count-vectorizer options hashed `TokenPattern` through
   `StringComparer.Ordinal.GetHashCode`, which throws on null while `Equals` compares it (#1296).
4. `RecordHashTests` could not fail: net10 hashes every `NaN` alike on its own (#1297).
5. The changelog called two breaking removals forwards (#1298, #1299); `sparse.md` promised sorted
   columns (#1300) and omitted `CreateUnchecked` (#1301), `equivalence.md` the block products
   (#1301), and the `NormalizeRows` page said repeating L1 changes the rows (#1302).

## Decisions

- **A `float` overload of `HashOf`**, the rule the `double` one applies, and `SearchResult`
  overrides `GetHashCode` alone, keeping its generated equality.
- **`HashOfItem` for the pattern**: `EqualityComparer<string>.Default` is ordinal, as the
  `Equals` beside it is, and answers `0` for null.
- **Refused before writing.** `NormalizeRows` scans the stored values first and throws
  `InvalidOperationException`, since the offending value is the instance's own and not an
  argument; the matrix is left as it was. The scan is one pass beside the two the division makes.
- **A canary the host cannot satisfy.** net10 hashes `NaN` to `0x7FF00000` and `0.0` to `0`;
  `HashOf` sends `NaN` to `0`. Each record is held to hashing a `NaN` as it hashes `0.0`, which fails
  on net10 the moment a record stops going through `HashOf`.

## Verification

- `CsrMatrixTests` refuses `NaN`, `+∞` and `−∞` and checks the first row is untouched;
  `RecordHashTests` gains the `float` score and the null patterns, and every record passes the
  canary. Removing `HashOf` from `SearchResult` fails its test on net10.

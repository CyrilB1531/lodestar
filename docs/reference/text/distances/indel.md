# Indel

Edits that may only add or remove, never replace — which is what makes it the measure behind
rapidfuzz's `fuzz.ratio`.

Insertions and deletions only, never substitutions, so a substitution costs two.
`NormalizedSimilarity` × 100 **is** rapidfuzz's `fuzz.ratio`, which is the reason this type is
here rather than folded into `Levenshtein`.

## Members

| Member | What it does |
| --- | --- |
| [`Indel.Distance`](indel-distance.md) | Counts the fewest insertions and deletions that turn one string into the other, with substitution |
| [`Indel.NormalizedDistance`](indel-normalizeddistance.md) | Scales the distance into `[0, 1]` by dividing it by the sum of the two lengths. |
| [`Indel.NormalizedSimilarity`](indel-normalizedsimilarity.md) | `1 - NormalizedDistance`, and — multiplied by 100 — exactly rapidfuzz's `fuzz.ratio`. |

One query against a list pays the bit-parallel table once per call here. [`IndelPattern`](indelpattern.md)
pays it once in total, at the same answers — it is this type's bulk form, and the page says where
it is worth having.

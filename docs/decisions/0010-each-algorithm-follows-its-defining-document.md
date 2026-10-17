---
status: accepted
supersedes: []
amends: ["0005", "0007"]
applies: []
---
# 0010 — Each algorithm follows its defining document

**Status:** accepted · **Date:** 2026-10-17

## Context

[`0005`](0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md) proves each family
against values frozen from its canonical Python library, and
[`0007`](0007-the-deliberate-divergences.md) accepts a divergence from that library only where
reproducing it would reproduce a defect. Read together, they made the library the authority on
behaviour as well as on numbers, its degenerate answers included.

The Review B after [#1702](https://github.com/CyrilB1531/lodestar/pull/1702) showed what that costs.
Aligning `Lodestar.Stats` with scipy one test at a time answered `NaN` for an empty sample in some
tests and refused it in others, and reached a Shapiro-Wilk constant sample that scipy answers `NaN`
for and that Royston's own algorithm, AS R94, refuses as its sixth fault. scipy's answers on
degenerate input are scipy's choices, made test by test; the method is defined elsewhere.

## Decision

**Each algorithm follows the document that defines it — the paper or the published algorithm —
and the Python library is the numeric oracle wherever it implements that document.** Where the
document states a fault, a range or a limit, the code follows it: Shapiro-Wilk refuses fewer than
three values and a range below `1e-19`, and computes past 5000 values, as AS R94 does. Where the
document is silent and the input leaves the statistic undefined — an empty sample, every value
tied, an empty group — the answer is `NaN`, the same whether the input arrived so or was left so by
omission, which keeps `0007`'s reason for treating both paths alike.

This amends `0005`: a corpus frozen from the library still proves the numbers, and no longer
decides a behaviour the defining document decides otherwise. It amends `0007`'s row for
Shapiro-Wilk and Kruskal-Wallis under omission: a sample left degenerate answers `NaN` where the
document is silent, and is refused where the document refuses it. A divergence from the library is
now recorded against the document, in [`../equivalence.md`](../equivalence.md), rather than
justified against the library.

## Consequences

- **`Lodestar.Stats` applies it first.** Kruskal-Wallis on a fully tied pool, the t-tests,
  Mann-Whitney, Kolmogorov-Smirnov and Wilcoxon on an empty sample, and the five k-sample tests on
  an empty group answer `NaN`; Shapiro-Wilk follows AS R94's faults. `equivalence.md` states, row by
  row, each answer that follows the document rather than the library.
- **The other packages follow as their reviews reach them.** A family whose library departs from
  its document keeps the library's corpus for the inputs where the two agree, and states the rest
  in `equivalence.md`.
- **A degenerate behaviour now needs its document read first.** The library's answer is evidence
  of what the document allows, not the rule.

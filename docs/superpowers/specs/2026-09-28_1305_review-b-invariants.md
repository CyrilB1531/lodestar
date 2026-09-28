# Review B as a sweep by invariant

**Issues:** [#1305](https://github.com/CyrilB1531/lodestar/issues/1305).
**Status:** written before the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

Review B is the review of a whole package on `main` after its fixes merge. Run as an open code
review, it returned about nine findings a round on `Lodestar.Abstractions` three rounds in a row
(#1282, #1284–#1292, #1294–#1302). The count did not converge; the severity did. Two causes:

- **An open review is a sample, not an enumeration.** It stops near a fixed budget, and two
  readings of the same code report different things: `NormalizeRows` was read for #1286 and its
  acceptance of `NaN` was reported a round later (#1295).
- **A fix for one instance leaves the rest of its class.** #1285 made the `double` hashes
  `NaN`-stable and missed the `float` one and the null strings (#1294, #1296); four of eighteen
  findings came from the previous round's own pull requests.

## Decisions

Review B runs in two stages.

**1. A sweep per invariant.** Each invariant is a property every member of the package must have,
located by a search over the whole package rather than by reading, and pinned by a test once
fixed. One finding per violation, each graded: *medium or more* when a caller gets a wrong
answer, an exception the API does not promise, or an unbounded allocation; *minor* otherwise.
The generic list, applied to every package:

| invariant | located by |
| --- | --- |
| Every public `GetHashCode` is total on null and hashes every `NaN` alike, for `double`, `double?` and `float` members, through `ValueEquality`. | every `record` and every `GetHashCode` override |
| Every public method refuses `NaN` and `±∞` where its Python reference refuses them. | every public method taking or reading floating-point input |
| Every allocation sized by a product of caller-supplied sizes is bounded by `Array.MaxLength` before it is made. | every `new T[…*…]` and `new T[…, …]` |
| Every public reference-type parameter is null-checked. | every public signature |
| Every sentence of the package's reference pages, README and CHANGELOG is true of the code. | every page, line by line |
| Every public array or mutable collection says truthfully whether the caller may change it. | every public array-typed member |
| Every public member has a reference page. | the exported surface against `docs/reference/` |

A package adds the invariants its domain carries — for `Lodestar.Stats.Regression`, that each
estimator states its consistency and its covariance's assumptions — in the spec of its sweep.

**2. A final open review**, the code-review tool over the package. **Stop rule:** a package leaves
Review B when the sweep and the final review together report nothing of medium severity or more.
Minor findings go to one umbrella issue per package, fixed in a later round.

A fix to a class of defect is itself a sweep: every instance the search finds, not the reported one.

## Rejected

- **Another open round until a round comes back empty**: the count held at nine for three rounds,
  and each round's fixes fed the next.

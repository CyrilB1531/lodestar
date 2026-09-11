---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0088 — CPD does not read the stemmers, and is excluded from that one directory

**Status:** accepted · **Date:** 2026-09-09

## Context

[#307](https://github.com/CyrilB1531/lodestar/issues/307) added Danish, the
second Scandinavian language, and SonarCloud failed the quality gate on
`new_duplicated_lines_density` — 11.0% against a threshold of 3%. Nothing else
on the gate moved: coverage 100%, violations 0, all three ratings at 1.

Part of that was real. `DanishSnowballStemmer` had been written to the shape
`SwedishSnowballStemmer` set, and CPD reported a 30-line block covering the entry
point, the worker declaration and the step dispatch. That is shared *scaffolding*,
so it was factored into `ScandinavianSnowballWorker` the way
`RomanceSnowballWorker` already holds what Spanish, Portuguese and Italian
share. Swedish was rewritten
onto it and its 211-word corpus replayed unchanged. The density fell to 7.7%.

The rest is not real, and a local probe of CPD's own comparison says why. CPD
normalises every literal to a single token before matching, so the longest common
run between the two files is 105 tokens of:

```text
$LIT or $LIT or ... ; private static readonly string [ ] Step1Suffixes = [ $LIT , $LIT , ...
```

That is the valid-s-ending predicate running into the step 1 suffix table — the
two places where Danish and Swedish differ *most*. Danish accepts twenty letters
before a bare `s` and Swedish seventeen; Danish's step 1 lists thirty-one
suffixes and Swedish's thirty-six, and not one suffix is shared. CPD erases
exactly the information that distinguishes them.

It is also not something member ordering can fix, which was measured rather than
assumed. Moving the s-ending predicate away from the table it had landed beside —
to sit after the step that uses it, which is a defensible place for it anyway —
takes the run from 105 tokens to **100**, still exactly at the minimum. The floor
is the table itself: Danish's thirty-one suffixes are ~76 normalised tokens on
their own, so any twenty-four tokens of agreement on either side reach the
threshold, and after `ScandinavianSnowballWorker` the call site that follows every
table (`private void Step1() => StripLongestInR1(Step1Suffixes, IsValidSEnding);`)
is fifteen of them. Factoring the shape out is what *created* that adjacency; the
alternative is to stop sharing the step, which trades a real improvement for a
metric.

The tree already carries the same shape, and much more of it, without its ever
having blocked anything. Measured over the directory with the same comparison:

| pair | longest common run |
| --- | --- |
| Italian ↔ Portuguese | 216 tokens |
| Portuguese ↔ Spanish | 186 tokens |
| Italian ↔ Spanish | 180 tokens |
| **Danish ↔ Swedish** | **105 tokens** |
| every other pair | 62–84 tokens, under the minimum |

Those three Romance pairs are why `PortugueseSnowballStemmer` sits at 7.1%
duplication on `main` and `ItalianSnowballStemmer` at 4.2%: the gate judges new
code only, so they were never asked about. Danish is the mildest case in the
directory and the first to be asked.

[#308](https://github.com/CyrilB1531/lodestar/issues/308) is the corroboration.
Norwegian was written independently, against the same published-description
method, and fails the same condition at 13.2% with 34 duplicated lines. Two
independent lots meeting the same wall is the property of the directory, not of
either lot.

## Decision

**`sonar.cpd.exclusions="src/Lodestar.Text/Stemming/**"`**, set in
`.github/workflows/sonarcloud.yml` beside the exclusions already there.

The directory is the right scope, not the pair of files that tripped it. Every
file in it is a transcription of one published Snowball description into suffix
tables, which is the property that makes CPD unable to read them — a list of the
files that happen to collide today would need editing for Norwegian, and would
be a list nobody could derive from a rule.

This is the repository's first duplication exclusion. It is deliberately not a
lowered threshold and not a project-wide setting: a duplication finding anywhere
else still fails the gate.

## Consequences

- Real duplication between two stemmers stops being reported. The mitigation is
  that the shared shape now has somewhere to live — `SnowballWorkerBase` for what
  every language shares, `RomanceSnowballWorker` and `ScandinavianSnowballWorker`
  for what a family does — so a reviewer asking "should this be in the family
  worker?" is asking a question the layout can answer.
- A future Scandinavian language (Norwegian is the third) inherits
  `ScandinavianSnowballWorker` rather than re-stating its shape, and will not meet
  this gate on the way in.
- If SonarCloud ever stops normalising literals, this exclusion becomes
  unnecessary rather than wrong, and the measurement above is what to re-run
  before removing it.

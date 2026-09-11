---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0094 — Arabic takes `snowballstemmer` as its oracle, and stands outside `SnowballWorkerBase`

**Status:** accepted · **Date:** 2026-09-10

## Context

Two questions had to be settled for Arabic, and
[#312](https://github.com/CyrilB1531/lodestar/issues/312) named both in advance:
that normalisation is the first thing to settle, and that this lot should expect
"to justify whatever it adds to `SnowballWorkerBase`, or to stand apart from it
with a reason".

### §1 The oracle

Every Snowball language here replays a corpus frozen from `nltk`, under the rule
[`0008`](0008-italian-enza-nltk-divergence.md) set: parity with the library a
user migrates from, over the published text.
[`0086`](0086-russian-follows-nltks-table-and-the-descriptions-alphabet.md) and
[`0092`](0092-romanian-keeps-the-cedilla-alphabet-the-description-writes.md)
bounded that rule — follow `nltk` where its *rule tables* differ, the description
where its *representation* leaks.
[`0091`](0091-hungarian-takes-snowballstemmer-as-its-oracle.md) was the first to
leave `nltk` entirely, because its Hungarian omits a letter class.

Arabic falls outside all of it, on a defect neither of those shapes covers:
**`nltk`'s Arabic stemmer is not a pure function.** It keeps `is_verb`,
`is_noun`, `is_defined` and a dozen `*_success` flags on the instance, and the
generator holds one stemmer for a whole word list. Measured over this lot's 148
words, stemming them in sequence against stemming each with a fresh instance
differs on two:

| word | fresh instance | after the words before it |
| --- | --- | --- |
| `كتبوا` | `كتب` | `كتبو` |
| `كتبتم` | `كتب` | `تبتم` |

A corpus frozen from a shared instance therefore encodes **the order of the word
list**, not the algorithm, and nothing downstream could see it: the same order
regenerates the same file, so `Oracles are reproducible` stays green. Worse, no
implementation this package could ship can reproduce it — the other fourteen
stemmers document "Thread-safe", and a stemmer whose answer depends on the words
already stemmed cannot.

`nltk`'s Arabic also carries rules the published description does not, which the
0086/0092 bound would have sent us to `nltk` for — its `Suffix_Noun_Step2b`
removes a `ست` the description gives as `ات` only, and its `Prefix_Step3b` removes
a bare leading `ك` no prefix step lists — while *omitting* two the description
does state: Arabic-Indic digits folded to ASCII, and alef maksura resolved to yeh.

`snowballstemmer` 3.1.1 does all four the way the description reads.

### §2 The worker base

`SnowballWorkerBase` exists for R1 and R2 — the regions, and the suffix searches
qualified by them. The published Arabic algorithm **uses neither**. Its own text
says so, and every guard in it is an explicit character count instead: `len > 3`,
`≥ 4`, `≥ 5`, `> 5`. There is no vowel-driven region to compute and nothing for
`Region`, `InR1`, `InR2`, `LongestSuffixInR1` or `LongestSuffixInR2` to do.

## Decision

**§1 Arabic's corpus is frozen from `snowballstemmer` 3.1.1**, as Hungarian's is.
`docs/equivalence.md`'s Arabic row names that library, and
`tests/oracles/snowball_ar.json`'s metadata says `snowballstemmer`, so which
reference a corpus froze stays readable from the corpus.

**§2 `ArabicSnowballStemmer` does not derive from `SnowballWorkerBase`**, and
adds nothing to it. It is the only stemmer in the package that does not.

## Options

**Follow `nltk` and document the divergence**, the [`0008`](0008-italian-enza-nltk-divergence.md)
and [`0087`](0087-danish-follows-nltk-on-the-apostrophe.md) shape. Rejected, and
not on size this time: it is not implementable. Reproducing a residue left by
previously-stemmed words would mean a stemmer that is not a function of its
argument, which no caller could use concurrently and no reader could interpret.

**Freeze the corpus from `nltk` with a fresh instance per word.** Rejected,
though it was written and measured first. It removes the order-dependence but
keeps the extra rules and the missing normalisation, so the corpus would still
freeze a transcription's accidents as the contract.

**Exclude the affected words from the corpus.** Rejected for the reason
[`0091`](0091-hungarian-takes-snowballstemmer-as-its-oracle.md) gives: a corpus
that pins the part which agrees and stays silent on the part which does not is
the opposite of what a frozen oracle is for.

**Bend `SnowballWorkerBase` to carry length guards as well as regions.**
Rejected. It would add a second, unrelated vocabulary to a class fourteen
languages read for one thing, to serve a single caller that needs none of what is
already there.

## Consequences

- Arabic is the second corpus frozen from `snowballstemmer`, and the reason is
  recorded on `0008`'s row in `docs/decisions/README.md` alongside the others, so
  a reader arriving at 0008 first learns that its rule now has two exceptions and
  what separates them.
- **Two of the length guards in the implementation are fitted to the corpus
  rather than read from the description.** The published text gives its guards per
  step — noun step 1b `> 5`, verb step 2a `≥ 4` — and this implementation merges
  those steps into one ordered table, where a single number has to serve both. The
  `ن` guard sits at 5 and the `ي` guard at 4 because that is what reproduces the
  146 cases; neither is a reading of the prose, and a wider corpus could move
  them. They are named here rather than left to look like transcription.
- `nltk`'s Arabic remains usable for anyone who wants it — this decision is about
  what *this* package guarantees, and a caller comparing against `nltk` on a
  single word in a fresh process will still see the same answers except where the
  four differences above apply.

# The Embeddings findings of the sweep after #1325

**Issues:** [#1331](https://github.com/CyrilB1531/lodestar/issues/1331) to
[#1337](https://github.com/CyrilB1531/lodestar/issues/1337),
[#1339](https://github.com/CyrilB1531/lodestar/issues/1339) to
[#1342](https://github.com/CyrilB1531/lodestar/issues/1342),
[#1344](https://github.com/CyrilB1531/lodestar/issues/1344) (its `VectorMath` half),
[#1347](https://github.com/CyrilB1531/lodestar/issues/1347) to
[#1352](https://github.com/CyrilB1531/lodestar/issues/1352),
[#1355](https://github.com/CyrilB1531/lodestar/issues/1355) and
[#1356](https://github.com/CyrilB1531/lodestar/issues/1356).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The Review B invariant sweep of `Lodestar.Embeddings` on `main` at `eb4d240a` found twenty-one
gaps in the package itself; the satellites' five are their own pull request. They fall in four
groups: files the references refuse and this package read (negative ids, a non-finite score,
`treat_whitespace_as_suffix`, blank or bare-CR `merges.txt` lines, an npy header with an extra
key); behaviour that parted from the reference (a byte-level token outside the byte alphabet lost
its characters, float norms overflowed); sizes allocated before they were bounded; and arguments
refused under another method's parameter, or not at all.

## Decisions

- **Refuse what the reference refuses.** `treat_whitespace_as_suffix` is refused as `byte_fallback`
  is, rather than implemented: it moves the meta symbol after the word, a second segmentation this
  tokenizer does not have.
- **`BpeTokenizer` keeps its id table dense or sparse**, by density: two slots per entry past a
  1,024 allowance, else a dictionary, as `tokenizers`' hash map does.
- **Byte-level decode is per token**, as `tokenizers`' `ByteLevel` decoder is: a token wholly in the
  byte alphabet is mapped back byte by byte, any other is its own UTF-8. `Append` spells such a
  token in the alphabet, so `Finish` stays one pass.
- **`merges.txt` lines end at `\n`**, one `\r` before it dropped, as Rust's `BufRead::lines` does; a
  blank line is refused and `#version` skipped wherever it stands, measured on tokenizers 0.23.2.
- **Float first, `double` where float failed.** `VectorMath.L2Norm` and `Mmr.Select` keep the float
  SIMD sums, and recompute in `double` a sum that overflowed, went NaN from `+∞` and `−∞` products,
  or fell below `1e-24`, where squares may have underflowed. A first version summed everything in
  `double` and made `MmrSelect100` 11.5 times slower (4.3 ms to 49.7 ms).
- **`HasIds` keeps its behaviour and its page changes**: a list of nulls a file declared is saved
  back, and a test pinned that deliberately.
- **`EncodeToIds` keeps its body and its page changes**: an ids-only walk would thread a flag
  through every encoder path for one list the caller discards.
- **A repeated npy header key keeps its last value**, as the dict literal numpy evaluates does.
- **A non-finite score is refused on a piece segmentation can reach**; control, unknown and unused
  pieces never score.
- **`PrecompiledNormalizer.FromCharsMap` copies**; the loaders hand over the blob they just read
  through an internal `FromOwnedCharsMap`, so a load copies nothing more.

## Verification

- `ReviewBFindingsTests` and two loader tests pin each finding; the `Mmr` overflow tests now hold a
  `[2e38f, 2e38f, 0]` vector to rank as its direction does.
- Random differential against tokenizers 0.23.2: 3,000 id sequences over GPT-2's byte-level
  vocabulary with eight added tokens, five outside the byte alphabet, decoded; `main` mismatches
  1,754, this branch none.
- A/B/A against `main` at `8756f2f3` on the developer machine: BPE encode 53.1 / 54.2 / 53.2 ms,
  Unigram 18.3 / 17.5 / 16.8 ms, `MmrSelect100` 4.31 / 4.36 / 4.24 ms (25 KB where it allocated
  21 KB, its norms now `double`), `L2Norm` at 384 28.2 / 28.4 / 27.9 ns.
- Review A found four: a NaN float dot the fallback did not catch, a score check stricter than the
  tokenizer, a null token inside a template list, and a repeated npy key; all four fixed here.

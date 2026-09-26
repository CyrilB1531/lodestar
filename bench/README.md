# Benchmarks

Three complementary tools.

## 1. Intra-C# micro-benchmarks (BenchmarkDotNet)

Rigorous per-method measurement, for optimizing the C# implementation itself:

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Levenshtein*'
```

### The Levenshtein corpora, and which one reaches what

There are two classes, and the difference between their corpora is the whole
point of having both.

`LevenshteinBenchmarks` draws both operands from `"abcdefghijklmnopqrstuvwxyz "`.
That is the near-duplicate matching case, and it is ASCII — so its
`Distance_CodePoint` row decodes into a sequence identical to the UTF-16 one and
measures the decode over a 27-symbol alphabet. It is not a measurement of the
code-point mode. Its `Distance_Utf16_Cjk` row builds the same scattered pair from a
27-symbol CJK alphabet, which leaves Latin-1: past one word that takes the row-major
blocked kernel and its side table, where the ASCII rows take the paired column-major
one, so the two rows move apart when either route does (#718). `Length = 128` is
there for the same reason: it is the first pattern of two words.

`LevenshteinCodePointBenchmarks` is that measurement (#208). Both operands are
drawn from U+1F300..U+1FAFF, so every character is a surrogate pair and the two
readings genuinely differ, which is the case
[decision 0001](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md) points a
caller at. It carries a second parameter the other does not:

- `Distinct = 32` — the pattern fits the 255-symbol dense alphabet at every
  length, so the bit-parallel path applies throughout;
- `Distinct = 512` — the pattern outgrows it as `Length` rises, and the
  implementation falls back to the dynamic program.

Both are run because reporting only the first would publish the fast path's
number as though it were the mode's.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*LevenshteinCodePoint*'
```

`IndelBenchmarks` and `IndelCodePointBenchmarks` split the same way for `Indel`, with one
difference: the code-point `Indel` takes two routes, not one gate. An operand holding no surrogate
decodes to itself, so `IndelBenchmarks`' ASCII `Distance_CodePoint` row measures the check that
proves it and then the UTF-16 kernel. `IndelCodePointBenchmarks` draws every character from
U+1F300..U+1FAFF, 32 distinct, so each operand is renamed into surrogate values the bit-parallel
kernel can compare before it runs (#675). It has no `Distinct` parameter: the renaming's ceiling is 2,048
distinct astral code points, and a pair of 512 cannot reach it.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*IndelCodePoint*'
```

### Where the bit-parallel gate belongs, and why a benchmark cannot say

`MyersGateBenchmarks` and `LcsGateBenchmarks` parameterise the differing middle
directly, so after `Affixes.Trim` the pattern is exactly `Band` long and the
dynamic program runs beside the kernel as the baseline. They answer *how much*
the kernel wins by at a given band.

Each runs that band twice, over two alphabets of 27 symbols: the Latin one every
band used before, and a CJK one above `U+00FF`. Below that boundary the kernels
index their 256-entry equality table directly; above it a pattern took the DP
until #302 and #382 gave it a side table beside the dense one. So `Dp_Cjk` is what
a refusal costs and `Kernel_Cjk` is what the side table costs, on shapes the Latin
rows measure in the same process — `BandedPair` takes the alphabet as a parameter
so that the alphabet is the only thing between the two (#383).

**A CJK row is only a CJK measurement if the band reaches the kernel**, and a
timing cannot tell you it did: both routes return the same number. The gate is 8,
so bands 4 and 6 take the DP on either alphabet by design, and
`WideAlphabetKernelTests` asserts the route for every band at or above it rather
than leaving it to be read off a ratio.

They cannot answer *where to put the gate*, and it is worth being explicit about
why, because the shape invites the mistake: below the gate the dispatch sends
both rows to the DP, so the ratio is 1 and the crossing is invisible exactly
where you want to read it. The kernels are `internal` and the constants private,
so no benchmark reaches around the dispatch either.

What answers it is sweeping the constant and reading the committed corpus end to
end — the metric that is actually reported, on the input that is actually shipped.
Edit `MyersMinPatternLength` (or `BitParallelMinPatternLength`, or
`MyersMinCodePointPatternLength`), rebuild, and run the cross-language harness of
section 3 at each value. `BitParallelLcs.MaxHeldPattern` — the longest pattern for
which the LCS kernel holds its equality table rather than letting `stackalloc` zero
one — is swept the same way, and #301 did:

```bash
dotnet build bench/Lodestar.Text.Benchmarks -c Release
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks --no-build -- compare
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks --no-build -- compare-indel
```

Three things this corpus will not tell you, all of which cost #208 time:

- **Read every bucket, not the one being tuned.** The length-32 bucket keeps
  improving as the gate falls, and the length-8 bucket falls off a cliff below 2.
  A sweep that reports only the bucket it is optimising will happily recommend 1.
- **The crossing depends on the text length, not just the pattern length.** Myers
  costs `setup + O(n)` where the DP costs `O(m·n)`, so they meet at
  `m ≈ 1 + setup/n`. A gate is one number for every `n`, so calibrate it on the
  shortest texts — the regime where being wrong is expensive.
- **The corpus had a hole between 2 and 7, and #409 filled it.** Its scattered
  length-8 bucket trims to a pattern of 0 or 1, in either alphabet — the 10% edit
  rate produces that, not the alphabet — so every conclusion in that range once
  rested on the length-32 bucket alone, whose median pattern is 16. The twenty
  banded buckets have a pattern of exactly their band, 2 through 16, and that is
  what showed the shared gate of 8 to be wrong in three of its four cases.
- **With bands, a gate question needs two runs and not a sweep.** At a gate of 2
  every band takes the kernel and at 17 every band takes the dynamic program, so one
  pair of runs prices both routes over the same pairs and the crossing is where the
  ratio reaches 1. The catch is that the two readings come from separate builds, so
  drift enters the ratio where BenchmarkDotNet's in-process baseline would not —
  about 10% here, which pins a separation of four bands and not a boundary band.
- **Never sum ns/pair across buckets to score a gate value.** The 512 bucket is
  roughly 95% of any such total and no candidate gate can touch it, so the sum
  reports that bucket's run-to-run noise as a result about the constant, and picks a
  different winner than the one bucket that can see the gate.
- **Sweep in both directions.** Each value needs its own build, so the readings are
  taken in sequence and machine drift maps onto the swept axis. Two passes in
  opposite order put that drift on both ends instead; #407's agreed to 5.3%, against
  about 12% for the gate benchmarks' short job.

### Measuring a before against an after, and reading the generated code

Two instruments, both of which failed silently before they worked, in #411.

**Nothing may compile inside the window.** Rebuilding between every measurement leaves
`VBCSCompiler` burning about a quarter of a core into the run that follows: two
executions of *identical* code differed by up to 15.2%, which was the size of the effect
being looked for. Publish each state once into its own directory, `dotnet build-server
shutdown`, then alternate only the runs — which also makes six rounds cheaper than two
were.

```bash
dotnet publish bench/Lodestar.Text.Benchmarks -c Release -o /tmp/state-before
# ... change the source, publish again to /tmp/state-after ...
dotnet build-server shutdown
/tmp/state-before/Lodestar.Text.Benchmarks compare     # alternate, several rounds
/tmp/state-after/Lodestar.Text.Benchmarks compare
```

Read each row against the spread of the runs of one state, not against a remembered
figure: that spread is the row's own noise floor, and on this workstation it is 6% to
25% depending on the bucket.

**Whether a change costs the hot path is a question about generated code**, answered by
diffing the JIT's output rather than by timing a loaded machine (#302, #411):

```bash
DOTNET_JitDisasmSummary=1 DOTNET_TieredCompilation=0 ./driver     # what got compiled
DOTNET_JitDisasm='SubsequenceLengthChars DistanceChars TrySingleWord'   DOTNET_TieredCompilation=0 ./driver > listing.txt
```

Three traps, each of which produced a diff that read as success:

- **Run the built binary, not `dotnet run`** — the launcher swallows the JIT's listing,
  and a diff of two empty captures reports "identical".
- **A method the driver never reaches is never compiled, so never listed.** Run
  `DOTNET_JitDisasmSummary=1` first and check the method you care about is in it. An
  empty capture is a failed measurement; make the script refuse it rather than diff it.
- **The driver's operands must reach the code being compared.** A pair differing in one
  position trims to a pattern of one character and takes the dynamic program, so the
  listing describes a route the change does not touch. Move both ends apart, the way
  `BandedPair` does.

## 2. net10 vs netstandard2.0 — what the broad-reach target costs

`netstandard2.0` is a contract, not a runtime: nothing executes *on* it. What can
be measured is the **netstandard2.0-compiled assembly against the net10-compiled
one, both hosted on .NET 10** — same JIT, same GC, so any difference comes from
the libraries' own conditional code paths.

`Lodestar.NetStandard.Benchmarks` links the *same* benchmark sources as the suite
above (`<Compile Include="../Lodestar.Text.Benchmarks/…" />`, never a copy) and
only swaps which assemblies it references.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks        -- --filter '*VectorMath*' --inProcess
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*VectorMath*'
```

### Measured

Intel i7-4770S, .NET 10.0.10, default job. **Two runs per side, and both are
shown**, interleaved net10 → netstandard2.0 → net10 → netstandard2.0 so that any
drift in machine state lands on both columns rather than on whichever one
occupies the second half of the window.

| Method | Dim | net10 | netstandard2.0 | cost |
| --- | --- | --- | --- | --- |
| `Dot` | 384 | 74.8 / 79.3 ns | 330.3 / 334.0 ns | 4.2×–4.4× |
| `Dot` | 768 | 122.9 / 128.7 ns | 648.5 / 661.1 ns | 5.1×–5.3× |
| `Dot` | 1024 | 173.9 / 177.1 ns | 896.7 / 883.8 ns | 5.0×–5.2× |
| `L2Norm` | 384 | 69.6 / 69.6 ns | 322.9 / 319.1 ns | 4.6× |
| `L2Norm` | 768 | 95.5 / 109.9 ns | 650.1 / 651.3 ns | 5.9×–6.8× |
| `L2Norm` | 1024 | 138.8 / 138.3 ns | 887.4 / 875.5 ns | 6.3× |

That is the `Vector<T>` SIMD path against the scalar fallback, which is the one
place the two builds deliberately differ — the span-based `Vector<T>` constructor
is net-only. Everything else compiles to equivalent IL, so a difference elsewhere
means something changed and is worth investigating. `L2Norm` gains more than
`Dot` because it reads one array instead of two, so the vector path is not
sharing load bandwidth with a second stream; on netstandard2.0 the two methods
cost the same, which is what two equivalent scalar loops should do.

**Read the pairs, not the means.** Every figure above was reported by
BenchmarkDotNet with an interval of about ±1 ns, and the two runs of the same
binary still differ by up to 6% (`Dot` at 384) and once by 15% (`L2Norm` at
768, 95.5 ns then 109.9). The interval describes dispersion inside one process
and says nothing about reproducibility across processes. The ratios are far more
stable than either column, which is the argument for quoting them and not the
absolute numbers.

The machine was not idle: the one-minute load average was 4.8–5.5 on 8 logical
cores at the start of all four runs, against the 1.9–2.3 floor this workstation
reaches when only its desktop client, editor and browser are up. That inflates
both columns and is the reason the pairs disagree as much as they do; it does
not favour either side, since the runs alternate.

That load is not an accident of scheduling, and the earlier figures cannot be
reproduced by re-running these commands: the editor's language servers and the
assistant session that drives the run are themselves part of it. So the table
above is internally comparable — one window, alternating sides, both columns
paying the same tax — and **not** comparable to figures taken on this machine in
a quieter state. Compare ratios across such sets, never absolutes.

### Three things keep this honest

`SetTargetFramework` on the `ProjectReference` is **not sufficient on its own**.
BenchmarkDotNet's default toolchain generates and builds its own project per run,
which re-resolves the reference and restores the net10 build — both suites then
measure the same assemblies while reporting plausible numbers. An earlier version
of this comparison showed a 4% difference for exactly that reason.

So the suite pins the in-process toolchain, removing the generated project, and
asserts what it actually loaded before running anything:

```text
// Lodestar.Text: .NETStandard,Version=v2.0
// Lodestar.Embeddings: .NETStandard,Version=v2.0
```

A mismatch exits non-zero rather than producing numbers. Sanity-check any result
against physics too: a scalar dot product over 1024 floats is latency-bound near
1000 ns, so a result close to the SIMD figure means the isolation broke again.

**And `--inProcess` on the net10 command is the third thing, not decoration.**
The netstandard2.0 project pins `InProcessEmitToolchain` because it has to; the
net10 command would otherwise use the default out-of-process toolchain, and a
ratio taken across those two harnesses mixes the target framework with the
harness that measured it. This section published such ratios until issue #87 —
4.6×, 5.2× and 5.6× — and the flag is what removes the confound.

**What the confound is worth here, measured rather than asserted.** The net10
side was run a third time in the same window, minutes after the four runs above
and on the same loaded machine, with the flag left off:

| Dim | net10, in-process | net10, out-of-process | matched pairing | mixed pairing |
| --- | --- | --- | --- | --- |
| 384 | 74.8 / 79.3 ns | 69.9 ns | 4.2×–4.4× | 4.7×–4.8× |
| 768 | 122.9 / 128.7 ns | 123.0 ns | 5.1×–5.3× | 5.3×–5.4× |
| 1024 | 173.9 / 177.1 ns | 159.6 ns | 5.0×–5.2× | 5.5×–5.6× |

The out-of-process harness reports the *same binary* as up to 10% faster on
`Dot` and 24% faster on `L2Norm` (56.1 ns against 69.6 at 384). Pairing that
column against the in-process netstandard2.0 one — the shape of the old pair of
commands — inflates the gap by up to 0.5× at 384 and 1024, while at 768 the two
pairings agree, which is why the defect was not visible from the numbers alone.

That is the whole of what this control establishes, and it is deliberately not
compared against the figures this section used to publish. Those came off this
same machine, but in a quieter state than a run driven from an editor session
can recreate — the session is part of the load it would have to remove. Two sets
of absolute figures taken under loads that differ by a factor of two support no
inference between them, in either direction. The old ones are withdrawn on the
ground that their two columns came from two harnesses, which is visible in the
commands rather than in the numbers, and not because a later run disagreed with
them.

## 3. Cross-language comparison vs Python

Same committed corpus (`bench/corpus/pairs.json`), same throughput metric
(ns/pair), same auto-scaling best-of-N methodology on both sides — so the numbers
are actually comparable.

```bash
# Python side (rapidfuzz)
.venv-oracles/bin/activate      # built on Python 3.12+, see ../CONTRIBUTING.md
python bench/python/bench_levenshtein.py

# C# side (Lodestar.Text) — matched Stopwatch harness, not BenchmarkDotNet
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare
#   add --codepoint to measure the code-point mode instead of UTF-16

# side-by-side table
python bench/compare.py
python bench/compare.py --format=gfm   # a real markdown table, read natively by nightly_run.md
python bench/compare.py --bands        # the banded buckets instead of the scattered ones
```

The default table is the scattered buckets, which are the claim against rapidfuzz.
`--bands` shows the banded ones, which exist to place the bit-parallel gate and are
not such a claim — twenty band rows in the comparison table would bury the eight
that are (#409).

Results land in `bench/results/` (git-ignored: they are machine-specific and not
authoritative). Every bucket stays inside the BMP, so UTF-16 units and code points
coincide and both sides compute identical distances — and a result is keyed on its
alphabet as well as its length, two buckets answering to each length since #406. `--format=gfm` works on every mode below
the same way; the plain table stays the default because it is the one meant for
a terminal.

### Indel, over the same corpus

`Indel` is `len(a) + len(b) - 2·LCS`, so measuring it measures
`Lcs.SubsequenceLength` — which is also what `fuzz.ratio`, and therefore every
`process.extract`, runs. It gets the same treatment as Levenshtein over the same
buckets, so the two distances can be read side by side (#273):

```bash
python bench/python/bench_indel.py
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-indel
python bench/compare.py indel
```

Both C# harnesses share one timing loop (`CrossLang/PairsHarness.cs`), extracted
rather than copied for the reason `Harness.cs` gives for its own extraction: a
second loop is free to drift from the first while still printing a table that
looks comparable. The same reasoning now covers the other two places the Indel
lot would have copied a Levenshtein one: the Python scripts share
`bench/python/harness.py` and differ only in which `rapidfuzz` distance they hand
it, and the two BenchmarkDotNet classes build their operands through
`ScatteredPair.Build`, seed included — comparing two distances is only meaningful
over identical inputs, and an extracted builder is what makes that true by
construction rather than by inspection.

**What this corpus reaches.** Twenty-eight buckets. Eight are *scattered*, whose
pattern after trimming is an accident of where the mutations fell: four lengths
drawn from 27 Latin symbols, every one ASCII (`U+007A` at most), and four more of
the same lengths from 27 CJK symbols, every one above `U+00FF` (#406). Twenty are
*banded* since #409, whose pattern is exactly the band named — 2 through 16, both
alphabets, 500 pairs each — and they are what can place a gate. Both alphabets carry
4–27 distinct symbols per pattern, so the dense equality table fits at every
length and the bit-parallel kernels are exercised throughout — the failure of #52
and #267, where the new path was never reached at all, does not apply here.

The wide half is what the corpus could not do until #406, and it is what makes a
refusal priceable on the input that actually ships rather than on a synthetic
band. **A wide bucket is only a wide measurement if its pairs reach the kernel**,
which a timing cannot tell you: run `BucketRouteDiagnostics`, which splits the
length-32 bucket of either alphabet on the dispatch's own criterion. It reads 833
of 1 000 CJK pairs on the kernel against 861 Latin.

Both alphabets stay inside the BMP on purpose. UTF-16 units and code points
coincide there, so the C# default and rapidfuzz measure the same quantity;
supplementary characters would break that, an emoji being one code point and two
units. The supplementary case is covered where it can be — the property tests
against the dynamic program, extended for it in #302.

`FuzzBenchmarks.Ratio` also runs this path, on one fixed pair of 43-character
sentences. That is a point, not a curve; `IndelBenchmarks` is the sweep.

`FuzzBenchmarks.PartialRatio` is 43 characters too, under the 64 past which a needle takes
`LongNeedleWindows`' table of one row per word. `PartialRatioLongNeedleBenchmarks` measures that
route at needles of 65, 128 and 512 characters, in two shapes: `Embedded`, the needle's scattered
copy inside a text twice its length, which is what a partial ratio is for, and `EqualLength`, where
every window is an edge window and both orientations are slid (#720).

### Reading the numbers

The comparison is deliberately honest about methodology: the Python side times the
**realistic per-call loop** a user writes. rapidfuzz also exposes batch APIs
(`process.cdist`) that amortise the Python→C boundary; those are faster than the
loop measured here.

The C# side runs bit-parallel kernels too, Myers since
`docs/guides/performance.md` and blocked for patterns past one
word; `docs/guides/performance.md` amends it and retired
the backlog items it left open. The current standing against rapidfuzz is in `docs/guides/performance.md`,
most recently *Blocked Myers, two words at a time (issue #718)*.

## 4. The persistence layer (issue #58)

Three vocabulary loaders and the TF-IDF round trip, on the same three axes as
above, plus one the other sections do not need: **processor time**.

The corpus is generated rather than committed — about 3 MB of vocabulary files:

```bash
python bench/corpus/generate_vocabs.py     # writes bench/corpus/vocabs/, git-ignored
```

All three `bench/corpus/generate_*.py` scripts read `tools/seeded_random.py`, so they need the same
interpreter `.venv-oracles` is built on — **3.12 or later**. Below that they stop with a sentence
naming both versions (`tools/python_floor.py`).

Both language sides read those same files, which is what makes the comparison
mean anything; the bytes are not reproducible across machines and do not need to
be. A harness that cannot find the corpus exits non-zero naming the generator
rather than reporting numbers.

### net10 vs netstandard2.0

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks        -- --filter '*Persistence*' --inProcess
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*Persistence*'
```

`--inProcess` puts the net10 side on the toolchain the netstandard2.0 project
pins, for the reasons section 2 sets out; this tier published a mismatched pair
until issue #88.

Intel i7-4770S, .NET 10.0.10. Loaders parse from memory here, so the figures are
parsing cost rather than disk latency. **Two runs per side, both shown**,
interleaved net10 → netstandard2.0 → net10 → netstandard2.0.

| Operation | net10 | netstandard2.0 | ratio | net10 alloc | ns2.0 alloc |
| --- | --- | --- | --- | --- | --- |
| `VocabTxt` | 6.202 / 5.818 ms | 6.054 / 5.805 ms | 0.98 / 1.00 | 3.62 MB | 3.62 MB |
| `TokenizerJsonWordPiece` | 13.565 / 13.316 ms | 12.920 / 12.886 ms | 0.95 / 0.97 | 4.74 MB | 4.74 MB |
| `TokenizerJsonUnigram` | 15.036 / 15.172 ms | 15.066 / 15.256 ms | 1.00 / 1.01 | 4.64 MB | 4.64 MB |
| `SpieceModel` | 5.711 / 5.686 ms | **10.489 / 10.419 ms** | **1.84 / 1.83** | 3.36 MB | **5.31 MB** |
| `TfidfSave` | 1.872 / 1.848 ms | 1.888 / 1.906 ms | 1.01 / 1.03 | 2.09 MB | 2.09 MB |
| `TfidfLoad` | 5.538 / 5.345 ms | 5.480 / 5.514 ms | 0.99 / 1.03 | 2.86 MB | 2.86 MB |

**This edition is not comparable to the one it replaces.** The corpus is
generated rather than committed, so re-measuring after issue #100 meant
generating it again, and the untouched rows moved with it — `SpieceModel` reads
5.7 ms here against the 4.2 ms published before, on identical code. Every figure
in this section, and in the two below it, comes from one session against one
corpus. What issue #100 was worth is measured against `main` on that same corpus
and reported in the paragraph after next, not by subtracting these numbers from
the previous edition's.

**What the read path change did to these rows.** Measured against `main`
(`cec02e1`) on this corpus, in the same session, net10:

| Operation | before | after | allocated before → after |
| --- | --- | --- | --- |
| `VocabTxt` | 5.898 / 5.683 ms | 6.202 / 5.818 ms | 4.25 → 3.62 MB |
| `TokenizerJsonWordPiece` | 14.378 / 14.156 ms | 13.565 / 13.316 ms | 7.24 → 4.74 MB |
| `TokenizerJsonUnigram` | 15.884 / 16.017 ms | 15.036 / 15.172 ms | 9.64 → 4.64 MB |
| `SpieceModel` | 5.932 / 6.009 ms | 5.711 / 5.686 ms | 4.61 → 3.36 MB |
| `TfidfSave` | 1.860 / 1.863 ms | 1.872 / 1.848 ms | 2.09 → 2.09 MB |
| `TfidfLoad` | 6.410 / 6.476 ms | 5.538 / 5.345 ms | 4.34 → 2.86 MB |

`TfidfSave` is the control and it does not move, on either axis: nothing on the
write path changed. Every loader allocates less, by 15% to 52%, which is the
counted column and does not move between runs — that is the intermediate buffers
disappearing. Time follows allocation everywhere except `VocabTxt`, which reads
3% *slower* both times on both targets. A 224 KB file is small enough that one
sized allocation buys back less than the extra work of getting to it, and 3% is
close enough to this harness's spread that it is reported rather than explained.

One caveat on the `TfidfSave` allocation figure: the benchmark pre-sizes its
destination `MemoryStream` to the artifact's exact final length, which a caller
writing to `new MemoryStream()` cannot do. That removes the buffer-doubling
garbage a realistic save would pay, so the number is friendlier than the typical
case. It is deliberate — the question on this axis is what the two *targets* cost
each other, not what a caller pays — but it is not a general "cost of Save".

Four rows are noise, which is what equivalent IL should produce, and two runs per
side say so more convincingly than one. Those four scatter between 0.98× and
1.03× and **change sign between rounds** — `VocabTxt` reads 0.98× then 1.00×,
`TfidfLoad` 0.99× then 1.03×. A single run per side cannot distinguish that from
a small consistent penalty, which is what this table used to report.

`TokenizerJsonWordPiece` is the one row that leans without changing sign: 0.95×
and 0.97×, the netstandard2.0 build faster both times by 3–5%. It leaned the same
way before issue #100 (0.95× and 0.98× on `main`, same corpus, same session), so
it is not something this work introduced, and 3–5% on a row whose two targets run
identical IL is small enough to be left as an open observation rather than
explained away.

`SpieceModel` is the exception and it is real: netstandard2.0 allocates twice per
piece where net10 allocates nothing, once for the `byte[4]` scratch buffer in
`ProtobufReader.ReadFloat` and once for the array copy in `DecodeUtf8` that
`netstandard2.0` needs because it has no span overload. Across 29 861 pieces that
is the whole 1.95 MB difference — unchanged by issue #100, which took the same
1.25 MB off both targets and left the gap between them exactly where it was. It
costs 1.83×–1.84× the time. The allocation column is counted rather than
sampled and does not move between runs at all.

**What the toolchain mismatch was worth here.** Measured in the session that
produced the previous edition of this table, and not re-measured since — the
finding is about the harness rather than about the library, and neither has moved
in that respect. Running the net10 side out-of-process in the same window — the
shape of the command pair this section used to publish — made the same binary
read up to 8% faster (`TfidfLoad` 4.625 ms against 4.920 / 5.047 in-process;
`VocabTxt` 4.092 against 4.251 / 4.308). Pairing that column against the
in-process netstandard2.0 one gave `TfidfLoad` 1.10×–1.11× and `VocabTxt`
1.03×–1.06×, where the matched pairing gave 1.01×–1.04× and 0.99×–1.01×. The
mismatch is the same size as the differences the noise rows are read for, which
is why nothing can be concluded from them under it. It is far too small to touch
`SpieceModel`.

**Conditions.** The one-minute load average was 1.4–4.1 on 8 logical cores across
the four runs above, and 1.8–3.2 across the four `main` runs they are compared
against; the editor's language servers and the session driving them are part of
that and cannot be excluded from inside it. Both columns pay it equally and the
runs alternate, so the table is internally comparable — and the before/after
table is comparable to it, having been taken in the same session on the same
corpus. Neither is comparable to the previous edition, for the reason given
above it.

### vs Python

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-persistence
python bench/python/bench_persistence.py
python bench/compare.py persistence
```

Lodestar on .NET 10.0.10 against Python 3.12.3 with `tokenizers` 0.23.1,
`sentencepiece` 0.2.2 and `scikit-learn` 1.9.0. Ratios above 1 mean Lodestar is
faster.

| Operation | Lodestar | Python | wall | Lodestar cpu | Python cpu | **cpu** |
| --- | --- | --- | --- | --- | --- | --- |
| `spiece_model` | 6.163 ms | 33.475 ms | 5.43× | 6.462 ms | 33.473 ms | **5.18×** |
| `tokenizer_json_unigram` | 15.623 ms | 53.566 ms | 3.43× | 16.126 ms | 53.559 ms | **3.32×** |
| `vocab_txt` | 6.354 ms | 11.632 ms | 1.83× | 6.699 ms | 11.631 ms | **1.74×** |
| `tokenizer_json_wordpiece` | 13.328 ms | 18.494 ms | 1.39× | 13.754 ms | 18.490 ms | **1.34×** |
| `tfidf_save` | 1.925 ms | 3.232 ms | 1.68× | 1.970 ms | 3.232 ms | **1.64×** |
| `tfidf_load` | 5.525 ms | 4.205 ms | 0.76× | 5.821 ms | 4.205 ms | **0.72×** |

Both sides were run back to back on an otherwise idle machine, and the pair above
comes from one such run rather than the best figure of each operation across
several — picking per-row winners from different runs would flatter whichever
side happened to be measured last. Run-to-run spread on this harness is a few
percent, so treat differences smaller than that as noise; the loader ratios are
far larger than it.

Every ratio here is smaller than the previous edition's, and none of that is a
regression: this is the regenerated corpus of the section above, on which the
Python side is uniformly faster than it was on the old one (`vocab_txt` 11.632 ms
against 16.860). The Lodestar column moved in the other direction — the same
harness on `main`, on this corpus, in this session, read `vocab_txt` 6.799 ms,
`tokenizer_json_wordpiece` 14.269, `tfidf_load` 6.277 and `tfidf_save` 2.032, so
every row above except `tokenizer_json_unigram` is faster after issue #100 than
before it, `tfidf_load` by 12%.

### Why processor time is reported too

Elapsed time alone flatters this runtime.NET's background collector does its
work on other threads, so an allocation-heavy operation finishes in less elapsed
time than it costs: every Lodestar row above burns 1.02–1.07 processor-seconds per
elapsed second, while CPython is strictly single-threaded and measures 1.00 on
every row. That gap is narrower than the 1.12–1.21 the previous edition reported,
and it narrowed for the reason issue #100 exists: an operation that allocates a
third less gives the background collector a third less to do.

`tfidf_load` is the one row Python still wins, and it now wins it on both columns
rather than only on processor time — 0.76× wall, 0.72× cpu. Issue #100 moved that
row toward parity rather than away from it (0.67× wall on `main`, same corpus,
same session); what is left is a corpus on which scikit-learn's loader does
better than on the previous one, plus the background-collection residue, which is
a property of the host runtime rather than something a library chooses — an
application that wants it gone sets `ConcurrentGarbageCollection=false` in its own
runtimeconfig.

### Reading the numbers

The two sides do not do the same amount of work on the loader rows, and the
comparison says so rather than hiding it. `tokenizers` and `sentencepiece` build
a whole tokenizer: the normalizer and pre-tokenizer graph, and the Rust or C++
matcher they will encode with. Lodestar's loaders build a validated dictionary and
stop — the guides tell readers to construct a tokenizer from it as a second step.
A margin in Lodestar's favour therefore reflects, in part, work it does not do.

Both are also native code behind a thin binding, not interpreted Python.

The `tfidf_save` / `tfidf_load` pair is the one comparison that is close to like
for like, and it is the one that moved most. It started at 0.60× and 0.44× — both
losses — and reached the table above through six changes, each of which was kept
only because it measured: shortest-round-trippable doubles, the idf vector as
base64, the relaxed JSON encoder, folding the ordering check into the read loop,
sizing the vocabulary buffer from the declared count, and — issue #100 — reading
the payload into one buffer sized before it is filled while decoding the base64
straight into the array that keeps it. Two further changes
were measured and **discarded** for showing no gain: disabling writer validation,
and an earlier version of that last buffer change, which paid nothing until the
idf vector stopped dominating the profile. The reasoning is in
[`docs/decisions/0001`](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md).

## 5. Classification metrics (issue #61)

`ConfusionMatrix`, `Accuracy`, `Precision`/`Recall`/`F1`, `ClassificationReport`
and `RocAuc`, against scikit-learn's equivalents, on the same three axes as
persistence — wall time, processor time, and net10 vs netstandard2.0 — plus a
fourth this branch adds: **processor time is the merge gate**, not a footnote.
Every row in the cross-language table below must be ≥ 1×, or the branch does
not merge.

Issue #93 later added `BalancedAccuracy`, `MatthewsCorrelation` and `CohenKappa`
to the same cross-language harness, taking it from six operations to **nine**.
They share this section's corpus, harnesses, methodology and gate, so the prose
below covers them; their 18 measured rows were produced in a separate window with
its own load average and are published once, in
[`docs/guides/performance.md`](../src/Lodestar.Metrics/performance.md#balanced-accuracy-matthews-correlation-cohens-kappa-issue-93),
rather than duplicated into the table here.

The corpus is generated rather than committed, like `bench/corpus/vocabs/` —
six JSON files, about 54 MB total:

```bash
python bench/corpus/generate_metrics.py     # writes bench/corpus/metrics/, git-ignored
```

Six shapes — (1 000, 2), (1 000, 10), (100 000, 2), (100 000, 10),
(1 000 000, 2), (1 000 000, 10) samples × classes — each with `y_true`,
`y_pred`, a sample-weight column (generated but unused by the nine benchmarked
operations, on both sides), and scores for the ROC-AUC rows. The 10-class score
matrix stops at 100 000 rows: a million rows by ten classes is 200 MB of JSON,
which would measure the parser rather than the metric.

### Intra-C#, and net10 vs netstandard2.0

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks        -- --filter '*MetricsBenchmarks*' --inProcess
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*MetricsBenchmarks*'
```

Default job on both sides. The full matrix — 3 sizes × 2 class counts ×
`MetricsBenchmarks`' 5 methods = 30 benchmarks — takes about ten minutes per run.
This tier is not the nine-operation cross-language set: `MetricsBenchmarks` times
`Matrix`, `MatrixWeighted`, `AccuracyScore`, `F1Macro` and `Report` only, and the
three issue-#93 metrics have no BenchmarkDotNet method of their own.

The four regression rows of the cross-language set — `mse`, `mae`, `median_ae` and `r2` —
have theirs in `RegressionMetricsBenchmarks`, at n = 100 000 and 1 000 000 over the corpus's
own distribution generated in-process, so it needs no corpus file. Both harnesses link it,
and it takes the same pair of commands with `'*RegressionMetricsBenchmarks*'` as the filter.

**`--inProcess` on the first command is not decoration.** Without it the two
commands do not measure the same way: `Lodestar.NetStandard.Benchmarks` pins
`InProcessEmitToolchain` (its `Program.cs` needs it, or BenchmarkDotNet's
generated project re-resolves the `ProjectReference` and silently restores the
net10.0 build), while the first command would use the default out-of-process
toolchain. Any figure compared across those two harnesses mixes the target
framework with the harness that measured it. This tier was published that way
before, and the flag is what removes the confound. Sections 2 and 4 above and the
batched-embedding comparison in `docs/guides/performance.md` carried the same
defect and were re-measured with the flag in place (issues #87 and #88). Every
net10-versus-netstandard2.0 figure in this repository now comes from a pair of
commands that share a toolchain, and from two runs per side rather than one.

Same isolation assertion as those sections: the netstandard2.0 run proves it
loaded the netstandard2.0 build before any number from it is trusted.

```text
// Lodestar.Metrics: .NETStandard,Version=v2.0
```

**Every figure below is two runs, not one, and the two are shown.** The runs
were interleaved (net10, netstandard2.0, net10, netstandard2.0) so that any
drift in machine load spreads across both targets instead of landing on
whichever one occupies the second half of the window. Intel i7-4770S,
.NET 10.0.10. `Matrix`, at all six shapes:

| Samples | Classes | net10 (run 1 / run 2) | netstandard2.0 (run 1 / run 2) | ratio |
| ---: | ---: | ---: | ---: | ---: |
| 1 000 | 2 | 7.167 / 7.792 µs | 7.090 / 7.003 µs | 0.90–0.99× |
| 1 000 | 10 | 7.010 / 8.620 µs | 6.951 / 7.017 µs | 0.81–0.99× |
| 100 000 | 2 | 805.1 / 800.9 µs | 795.4 / 799.2 µs | 0.99–1.00× |
| 100 000 | 10 | 916.5 / 899.5 µs | 892.1 / 894.4 µs | 0.97–0.99× |
| 1 000 000 | 2 | 8.396 / 8.181 ms | 8.158 / 8.357 ms | 0.97–1.02× |
| 1 000 000 | 10 | 9.421 / 9.139 ms | 9.118 / 9.145 ms | 0.97–1.00× |

**At n=100 000 and n=1 000 000 the two targets are at parity**, and now with
something behind the claim: all 40 measurements there — 5 methods × 4 shapes ×
2 pairings — land between 0.97× and 1.05×, while each target's own spread
between its two runs stays inside 1.05× (net10) and 1.02× (netstandard2.0).
The difference between the targets is the same size as the noise, which is the
honest way to say parity.

**At n=1 000 the ratio column is not measuring the target framework.** The
net10 side moves by up to **2.64×** between two runs of the same binary over
the same corpus — `AccuracyScore` at k=10 reads 836 ns in one run and 2 212 ns
in the next — while netstandard2.0 stays inside 1.02× across every method. The
inflation is cleanly inverse to the work per operation: `AccuracyScore` (~0.84 µs
per op) moves 2.64×, `Matrix`, `MatrixWeighted` and `F1Macro` (~7–8 µs) move
1.1×–1.3×, and `Report` (~10–16 µs) moves by 1–2%. That is the shape of a fixed
per-invocation overhead appearing in one process and not another, not of a
difference in the metric code — the same computation is happening either way.

The consequence is worth stating plainly, because this section got it wrong
before: **BenchmarkDotNet's ± margin describes dispersion *within* one process
and says nothing about reproducibility *across* processes.** An earlier version
published `AccuracyScore` at n=1 000, k=10 reading 2.59× and explained it as
the short job's noise floor — implying a longer job would settle it. A longer
job did not. The 2.59× vanished, and k=2 came back at 0.64× with a ±0.3%
margin on the net10 side, which is to say a tight interval around a figure
that a re-run then contradicted outright (1 331 ns, then 833 ns). A
`MatrixWeighted` gap of 1.06×–1.18×, apparently systematic across all six
shapes, evaporated the same way once the toolchain confound was lifted and the
runs repeated. Anything at n=1 000 in this tier needs repeated processes
before it means anything.

One small-n row *is* stable on both sides, and it says something real:
`Report` runs **1.06×–1.11× slower on netstandard2.0 at n=1 000**, consistently
across both pairings and both class counts, fading to 1.00×–1.05× at the larger
sizes. A fixed per-call cost the netstandard2.0 build pays and net10 does not,
drowned once O(samples) work dominates.

This is still **not** the `VectorMath.Dot` story from section 2, where the gap
is 4.2×–5.3×. `Lodestar.Metrics` has no `Vector<T>` SIMD path, and every
benchmark here is a scalar loop over `int[]`/`double[]`. The one piece of
target-conditional code in its dependencies is `Lodestar.Internal.Guard`, which
picks `ArgumentNullException.ThrowIfNull` on net10 and a hand-written check on
netstandard2.0 — a null test outside every loop, which cannot produce a
per-sample difference and does not.

### vs Python

```bash
.venv-oracles/bin/activate && python bench/python/bench_metrics.py
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-metrics
python bench/compare.py metrics
```

Lodestar on .NET 10.0.10 against scikit-learn 1.9.0 / NumPy 2.5.1 on Python
3.12.3. Ratios above 1 mean Lodestar is faster. The 29 measured rows are
published in
[`docs/guides/performance.md`](../src/Lodestar.Metrics/performance.md#classification-metrics-issue-61--vs-scikit-learn),
not duplicated here.

**Merge gate: 29/29 rows at or above 1× on processor time.** Twenty-nine rows,
not twenty-nine operations: six operations over six shapes, less the seven
shape/operation pairs the two ROC-AUC rows do not cover. The three issue-#93
operations add 18 more rows, all of them ≥ 16.5× — published in
[`docs/guides/performance.md`](../src/Lodestar.Metrics/performance.md#balanced-accuracy-matthews-correlation-cohens-kappa-issue-93),
measured in their own window, and not folded into the rows above because they do
not share its load conditions. The
narrowest margin here is 2.74× (`roc_auc_ovr_macro` at n=100 000, k=10). The design
brief named `roc_auc_binary` at a million samples — where the cost is a sort —
as the likely candidate for falling below 1×, with a radix pass over the
`double` bit patterns as the fallback if it did. It came in at 3.81×; no row
needed that change on this branch.

### Measurement conditions

Both files were produced back to back, Python first, on a machine left to
settle first — an earlier pairing was discarded outright for having started
while the five- and fifteen-minute averages were still shedding the tail of a
preceding test suite. The pair in the table above:

| Side | Started | Written | Load at start (1 / 5 / 15 min) |
| --- | --- | --- | --- |
| Python (`python-metrics.json`) | 19:09:25 | 19:12:30 | 1.52 / 3.71 / 3.94 |
| C# (`csharp-metrics.json`) | 19:13:19 | 19:16:41 | 4.20 / 4.10 / 4.05 |

The machine carries a permanent 30–40% background load from the desktop
client, an editor and a browser; a one-minute average of 1.9–2.3 is this
workstation's floor, and the Python side started below it.

**The C# side started at 4.20, and that is its predecessor's own wake** — 49
seconds after the Python run finished saturating a core. The second side of any
back-to-back pair pays this; the alternative, waiting for the tail to decay,
buys a quieter machine at the cost of the two sides no longer being back to
back. The bias it introduces runs *against* Lodestar — the C# figures are the
ones measured on the busier machine — so every ratio in the table above, and
the merge gate that reads them, is conservative rather than flattering.

Memory was never the constraint these runs looked like they might have: the
working set at n=1 000 000 is about 16 MB, and the machine had 22 GB free with
3 GB in swap. CPU contention is the only thing worth waiting out here.

### Why processor time barely differs from wall time here

Unlike the persistence comparison in section 4 — where Lodestar burns 1.12–1.21
processor-seconds per elapsed second, so the wall and cpu columns diverge and
`tfidf_load` even flips winner between them — the two columns above agree to
within about 1% on every row (up to 3.4% on the single heaviest row,
`roc_auc_ovr_macro` at n=100 000). These operations allocate little enough
(a few kilobytes at most; see `MetricsBenchmarks`'s `[MemoryDiagnoser]` output)
that .NET's background collector never gets involved, so there is no gap
between the two columns for the cpu one to correct.

### Reading the numbers

The rows at n=1 000 (70×–620×) are not telling you Lodestar is two orders of
magnitude faster at the arithmetic — a confusion matrix over 1 000 samples is
sub-microsecond work either way. They are measuring CPython's per-call
interpreter overhead, which the auto-scaling loop cannot amortise away because
each `skm.*` call re-enters the interpreter regardless of how little work is
inside it. The rows that carry the argument about the underlying computation
are the ones at n=100 000 and n=1 000 000, where the ratios settle to a still
decisive but far more modest 2.7×–43×.

None of the nine operations passes `sample_weight` on either side — the corpus
carries that column, but neither the brief's own six calls nor the three issue-#93
ones use it, so this comparison does not exercise `ConfusionMatrix`'s weighted
path at all; see `MetricsBenchmarks.MatrixWeighted` in the intra-C# tier above for
that.

`precision_recall_f1_macro` is the one operation where the two sides do not do
quite the same amount of work, though both compute the same three numbers over
the same matrix. scikit-learn's `precision_recall_fscore_support` builds the
per-class true-positive/predicted/support sums once and reads precision,
recall and F1 off that single pass. The Lodestar side builds the confusion
matrix once too, but `Precision.Score`/`Recall.Score`/`F1.Score` each walk
those sums again independently — three redundant `O(classes)` passes instead
of Python's one. At 2 or 10 classes that redundancy is a handful of additions,
several orders of magnitude below the `O(samples)` matrix construction both
sides pay first, so it is invisible in the ratios above; it is called out here
because it is real, not because it matters at this scale. `classification_report`
carries the same pattern more deeply (each of its per-class arrays, and each of
its macro/weighted averages, re-derives those sums again) for the same reason
and to the same effect: negligible at these class counts.

Everything else — `confusion_matrix`, `accuracy`, `roc_auc_binary`,
`roc_auc_ovr_macro` — is like for like: same algorithm (`BinaryRoc` mirrors
scikit-learn's `_binary_clf_curve` sort-and-accumulate exactly, and
`MultiClassRoc`'s one-vs-rest reduction is scikit-learn's own), same data,
parsed once outside the timed loop on both sides so neither pays JSON-parsing
cost inside the measurement.

## 6. Multiclass ROC-AUC, sequential against parallel (issue #86)

C# against C#: the same operation at one, two, four and eight workers. Inputs are
generated in-process from a fixed seed — there is no Python side here, so there is
no shared corpus to keep in step.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- roc-parallel
```

The axis is **elapsed time**. Processor time rises with the worker count, which is
the point of spending cores rather than a fault in the measurement, and both are
reported side by side.

This is its own section rather than a subsection of section 5 for a reason beyond
tidiness: its `dop=1` figures look pairable with section 5's `roc_auc_ovr_macro`
row and are not. Different input, different machine load, and a sequential path
that was rewritten for this issue.

## 7. Persisting an embedding index (issue #62)

`EmbeddingIndex.Save` and `Load` on 10 000 vectors of 384 dimensions — 15 MB of
floats, the shape a sentence-transformer corpus actually has. The array is
generated from a fixed xorshift32 seed on both sides rather than committed: what
a float block costs to write depends on how many floats it holds, not on what
they are, and both sides reproduce the same shape from the same arithmetic
without a fixture. In Python this is a 10 000 × 384 loop of plain-int arithmetic
(`numpy.uint32`'s shifts raise on the overflow the algorithm depends on); it took
2.35 s measured once outside the timed loop, which is fine to pay a single time
at process start and did not need caching.

Measuring this shape is what found a bug: loading it used to need an explicit
`ArtifactLoadOptions { MaxArrayLength = 4_000_000 }`, because the reader applied
that 1 000 000-element default — sized for vocabularies, not vector blocks — to
the decoded float count, refusing 10 000 × 384 = 3 840 000 floats and, with it,
any index past 2 604 vectors of this dimension. It is fixed: the vector block is
bounded by `MaxTotalBytes` instead, which caps the whole payload in bytes before
parsing begins rather than the decoded element count after. `EmbeddingIndexLoad`
and `embedding_index_load` below call `Load` with the library's own defaults, and
need no options to do it.

### net10 vs netstandard2.0

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks        -- --filter '*EmbeddingIndex*' --inProcess
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*EmbeddingIndex*'
```

Intel i7-4770S, .NET 10.0.10. **Two runs per side, both shown**, interleaved
net10 → netstandard2.0 → net10 → netstandard2.0, for the same reason section 4
gives: one run per side cannot tell a small consistent penalty from noise.

| Operation | net10 | netstandard2.0 | ratio | net10 alloc | ns2.0 alloc |
| --- | --- | --- | --- | --- | --- |
| `EmbeddingIndexSave` | 13.75 / 13.57 ms | 14.83 / 15.24 ms | **1.08 / 1.12** | 54.29 MB | 54.29 MB |
| `EmbeddingIndexLoad` | 12.96 / 12.68 ms | 14.36 / 14.41 ms | **1.11 / 1.14** | 35.35 MB | 35.35 MB |

**The two targets now diverge, and they did not before.** This table used to read
0.99×–1.01× on both rows, and the previous edition said in as many words that
there was no `SpieceModel`-shaped story here. There is one now, and issue #100
put it there. Both operations scan the whole vector block for non-finite
components — `Load` before handing the index back, `Save` before writing a file
it could not honestly write — and that scan is 3.84 million floats. It is now
vectorized through `Vector<float>`, which the netstandard2.0 build does not
compile: `Vector.IsHardwareAccelerated` and the span constructor sit behind
`#if NET5_0_OR_GREATER`, the same guard `Pooling.cs` and `VectorMath.cs` already
use. netstandard2.0 keeps the scalar loop and pays for it.

The size of the gap says the same thing twice: 1.47 ms on `Save` and 1.45 ms on
`Load`, on two operations that share nothing else. That is the scan, priced on
its own — and it matches the 18% of the load figure the scan measured at before
it was vectorized, which is what decided it was worth vectorizing at all.

Everything else about the two targets is still identical.
`Base64Numbers.WriteSingles` and `ReadSingles` use `MemoryMarshal.Cast`/`AsBytes`
on both, on purpose: `BitConverter.SingleToInt32Bits` does not exist on
netstandard2.0, so the encoding path never forks the way `ProtobufReader` does.
Allocation is identical down to the byte and does not move between runs, which
section 4 already established is what the counted (not sampled) column does.

**What the read path change was worth here.** Against `main` (`cec02e1`) in the
same session: `EmbeddingIndexLoad` 36.894 / 37.485 ms → 12.948 / 12.921 on net10
and 34.437 / 34.341 → 14.36 / 14.41 on netstandard2.0, with allocation falling
from 90 MB to 35.35 MB on both. That is **2.88× faster on net10 and 2.39× on
netstandard2.0**, for an artifact whose bytes did not change. `EmbeddingIndexSave`
falls too, 16.96 ms to 13.57 on net10, which is the vectorized scan rather than
the read path — `Save` runs the same check. Its netstandard2.0 side reads 16.88 →
15.04 ms, an 11% improvement this work does not explain: nothing on that target's
write path changed, and it is recorded here as measured rather than attributed.

**Conditions.** The one-minute load average ran 1.6–1.9 across these four runs and
1.4–3.2 across the four `main` runs they are compared against, in the same session
as the cross-language pair below — see that section's measurement-conditions note.
All of section 4, section 7 and the cross-language pair were taken in this one
session, on one regenerated corpus, and are comparable to each other; none of them
is comparable to the editions they replace.

### vs numpy — what the format choice costs

`numpy.save` writes a short header followed by the raw little-endian block. That
is precisely what a dedicated binary format for this artifact would have
produced, so this comparison measures the decision recorded in
[0001](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md) rather than illustrating it.
`faiss` was deliberately not added to make this point a second way: on a flat
index it also writes the same raw block `.npy` does, so pulling it in as a
dependency would cost a pinned package to measure the same floor twice.

```bash
python bench/python/bench_persistence.py
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-persistence
python bench/compare.py persistence
```

Lodestar on .NET 10.0.10 against numpy 2.5.1 on Python 3.12.3. Ratios above 1 mean
Lodestar is faster.

| Operation | Lodestar | numpy | wall | Lodestar cpu | numpy cpu | **cpu** |
| --- | --- | --- | --- | --- | --- | --- |
| `embedding_index_save` | 12.419 ms | 15.084 ms | 1.21× | 13.349 ms | 15.083 ms | **1.13×** |
| `embedding_index_load` | 12.129 ms | 2.492 ms | 0.21× | 13.897 ms | 2.492 ms | **0.18×** |

**Read that `0.21×` as a format, not as a speed.** It puts our JSON artifact — a document to
scan and validate — against numpy's raw block, so it prices decision 0001 exactly as this
section says it does, and says nothing about how fast the two languages ingest the same bytes.
`embedding_index_ingest_npy` is the row that does: **both sides read a `.npy` and return
something searchable**, `np.load` against `NpyFile.Read` plus
[`EmbeddingIndex.FromOwnedBlock`](../docs/reference/embeddings/search/embeddingindex-fromownedblock.md).
It called `FromBlock` until #466: `np.load` returns the array it has just filled and copies it no
further, so copying the block into the index charged this side a copy numpy never pays, and priced
a route the caller need not take. Adopting is the like-for-like chain, and what
[`NpyBlock.OwnedArray`](../docs/reference/embeddings/persistence/npyblock.md) exists to allow.
For a flat cosine index the matrix *is* the index, so `np.load` alone is the honest counterpart,
and neither side normalizes — which is what a block written by an embedding pipeline already is.
It was added with the bulk ingest (#474); before that there was nothing on the C# side to pair it
with, because `Add` was the only way in.

**Its first runner reading is 0.21–0.23× wall, 0.19× cpu across three rounds** — numpy four to
five times faster, on the same 15 360 128 bytes. Taking the format advantage away made the gap
*wider* than `embedding_index_load`'s 0.24–0.27×, because that row was letting numpy be compared
against 5 MB less work.

**#466 took the copies between the stream and the index out and the row inverted**, from 0.19× of
numpy's cpu to **1.00–1.13×** and from 0.21–0.23× of its wall to 1.21–1.25×. cpu is the column this
harness trusts, so the honest reading is *parity to slightly ahead*, not the wall figure. How many
copies there were, and which of them paid, is docs/guides/performance.md's subject rather than this section's.
The reading above stays as measured.
Two dispatches separated the causes: reading the payload straight into the `float[]` moved nothing,
and adopting the array rather than copying it into the index moved all of it — by more than the
copy it removed, because the copy came with a second 15.36 MB allocation.
`docs/guides/performance.md`
has the shape the reader took; [#480](https://github.com/CyrilB1531/lodestar/issues/480) carries
the half that is still unexplained.

> **#323 changed the save path after this window.** The row above stays as measured;
> what it cannot show is that its `1.13×` inverts to `0.27×` on a newer machine,
> because `numpy.save` is bandwidth-bound where this artifact's base64 encoding is
> not. The before/after, and why the ratio is a property of the machine as much as
> of the code, are in [`docs/guides/performance.md`](../docs/guides/performance.md).

Against `main` in the same session, on the same numpy figures:
`embedding_index_save` read 14.164 ms (0.94× wall, 1.06× cpu) and
`embedding_index_load` 32.460 ms (0.08× wall, 0.07× cpu).

| | Lodestar artifact | `.npy` |
| --- | --- | --- |
| bytes on disk | 20 589 007 | 15 360 128 |

That row is the one thing in this section issue #100 did not move, and not moving
it was the point: the same index saved by the build before this work and by the
build after it produces the same file, verified byte for byte by hash, and each
build reads the other's.

Neither harness command above prints a byte count — `Harness.Measure` only
records `ms_per_op` and `cpu_ms_per_op`. The Lodestar figure is `stream.Length`
after building the same index through `BuildIndex()` and calling `Save`; the
`.npy` figure is `len(buffer.getvalue())` after `np.save` on the same
`build_vectors()` array — the same two calls each `embedding_index_save` row
above times, with the byte count kept instead of discarded.

That is a 1.34× size ratio. Base64 alone accounts for 1.333× of it (the vector
block is 15 360 000 bytes of floats, 20 480 000 as base64 text); the remaining
0.5% is the `dimension`/`normalize`/`count` fields and the 10 000 quoted `doc-N`
ids, none of which `.npy`'s header carries.

### Reading the numbers

The size ratio is what the design predicted: about 4/3, plus a fraction of a
percent for the fields a raw block does not need. It has not moved and cannot:
this is the same artifact, byte for byte, as before issue #100.

`EmbeddingIndexSave` now reads 1.21× wall and 1.13× cpu — faster than
`numpy.save` rather than 1% behind it, which is not the read path but the
non-finite scan `Save` shares with `Load` becoming a vector pass.

`EmbeddingIndexLoad` is still the row that costs. It is 4.87× slower than
`numpy.load` on wall time and 5.58× on cpu, against 13.0× and 13.7× on `main` in
this same session — a real change, and still not parity. The gap is no longer made of copies.
The read path now reads the payload into one buffer sized from the stream's own
length before it is filled, decodes the base64 straight into the `float[]` that
keeps it, and scans that array for non-finite components by vector: three passes
where it used to take five, and 35.35 MB allocated to move 15 MB of floats where
it used to take 90 MB. What remains is the format's own floor — 20 MB of base64
text has to be read and decoded where `.npy` reads 15 MB and casts it — plus a
scan `numpy.load` does not perform at all, because it does not promise what this
artifact promises about its contents. Closing the rest of that gap means not
materialising the payload, which `MaxTotalBytes` currently forbids by design; that
is a different decision from this one and has not been taken.

None of this was anticipated by size alone, and the honest reading is not that
the design was wrong to choose JSON: ADR 0001 weighed one format against two and
a fixed 33% against a decode that reads the whole payload into memory first, and
said so plainly. What the 33% figure did not say, because nothing in that
decision measured it, is what the *implementation* of that buffered decode would
cost — an order of magnitude on `Load`, most of it in copies the format never
required. Issue #100 measured that, removed it, and re-measured: the size cost
landed on prediction and stayed there, the time cost did not and has now come
down by 2.9× without the format moving. The residue above is the format's, and
this section will keep reporting it.

The figures carry their own error bars: both cross-language figures are the best
of five repeated measurements each, not a single sample, and the BenchmarkDotNet
figures above spread no more than 2.8% from their own minimum within a target —
nowhere near either the 2.9× the change is worth or the 4.9× that is left.

**Measurement conditions.** Both sides were run back to back, Python first, in
the same session as the BenchmarkDotNet runs above — this machine carries a
permanent background load from an editor, a browser and the assistant session
driving the benchmark itself, and there was no quieter window to wait for
without breaking the back-to-back pairing the comparison depends on:

| Side | Started | Written | Load at start (1 / 5 / 15 min) |
| --- | --- | --- | --- |
| Python (`python-persistence.json`) | 17:58:14 | 17:59:14 | 1.59 / 1.82 / 3.77 |
| C# (`csharp-persistence.json`) | 17:59:14 | 18:00:24 | 1.74 / 1.81 / 3.64 |
| C# on `main`, for the before/after | 18:16:38 | 18:17:44 | 1.44 / 1.91 / 2.62 |

The two starts are close on all three windows — within 0.15 on the one-minute
average, lower on five and fifteen minutes for the side measured second — so,
unlike section 5's pair, there is no direction here for the load itself to have
biased the ratio: neither side woke the other into a spike the way section 5's
C# side inherited from Python's. The C# side started the moment Python's file was
written, with no gap for this session's own overhead to open. The `main` row was
taken 16 minutes later at a comparable load, which is what makes the before/after
in this section and in section 4 a comparison rather than two tables.

### The save rows are no longer all in memory

`embedding_index_save_file` is the write counterpart of `embedding_index_load_file`,
which #336 added because the file path is the one a caller takes — every other save
row here writes to a `MemoryStream`. It uses a path of its own, so neither direction
measures a file the other just touched, and neither side flushes to the device. It
priced pre-sizing the file, which
`docs/guides/performance.md`
refused, and it outlives that question.

### Every load row here is measured on a warmed heap

`compare-persistence` runs `embedding_index_save` before `embedding_index_load`,
in one process, and the save allocates tens of megabytes. That leaves the
large-object heap grown and its **pages already committed** for the load that
follows — so the load does not pay the page commits #324 identified as the
irreducible part of its allocation phase. It is not an artefact of the ordering
being wrong; it is what any harness measuring both directions of the same format
will do unless it is built not to.

Step 1 made it visible rather than created it. Removing the save's 20 MB buffer
withdrew the subsidy, and `embedding_index_load` slowed by 1.22× on the container
and by 1.10–1.17× on the nightly runner once the runners' own difference is netted
out — with **the allocation identical to three digits on both sides**, which is
what says the load is doing the same work and paying for more of it.

So read every `embedding_index_load*` row on this page as **flattered by 8.1% — the
printed figure times 1.088 is what a load costs unsubsidised** — and do not use one
as the control for a change to the save path. Section 8's
`block_copy_floor` is what a control looks like here: a `memcpy` that allocates
nothing and therefore cannot be subsidised by anything.

**That 8.1% is measured, and it replaces an inference of "on the order of 20%" that
stood here until [#433](https://github.com/CyrilB1531/lodestar/issues/433) ran. The
two do not agree: the inference overstated the subsidy by about 2.5×.** It was read
off a band of before-and-after figures spanning two changes and two machines, which
is a wide enough thing to read a number off that it should not have been quoted to a
digit. Section 9 has the measurement and the conditions.

**The harness is not split into two processes, and that is #433's answer.** Loading in
a process that has never saved is the obvious fix and it was refused on the number.
8.1% moves the published `embedding_index_load` row from 0.25× to 0.23× — from 4.0×
behind `numpy.load` to 4.3×. Nobody reads those two differently. Against that, a split
costs a process launch per row and, worse, the back-to-back pairing that the
measurement-conditions section above argues is the only reason the C# and Python rows
are comparable: two processes started minutes apart are two machines' worth of drift
inside what is supposed to be one window. **Stating the subsidy is worth more than
removing it**, so it is stated here, in section 9, and in the performance guide, and
`heap-warmth` stays committed so the next person to doubt it can re-run it rather than
re-argue it.

## 8. Where a save's time goes (issue #429, step 0)

Not a comparison: a **profile of one operation against itself**. Five phases over
the same 10 000 × 384 index as section 7, each a strict subset of the one above
it, so the shares are readable without a second harness to reconcile.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- save-phases
```

| phase | what it does |
| --- | --- |
| `save_total` | `EmbeddingIndex.Save` end to end |
| `write_base64_property` | the vector block alone, through `Utf8JsonWriter.WriteBase64String` |
| `write_base64_chunked` | the same block in 240 KB slices into one rented buffer |
| `base64_encode` | `Base64.EncodeToUtf8` on the block and nothing else |
| `block_copy_floor` | a `memcpy` of the same 15.36 MB, encoding nothing |

The last row is what makes the table decide anything. An encode that costs no more
than moving the same bytes is bandwidth-bound, and nothing parallelises past a
bandwidth it is already at — which is how a proposal to thread the base64 was
refused rather than tried.

**The phases run round-robin, one round each, not one phase to completion.** This
is the whole design and it is not tidiness. A first cut ran each phase's nine runs
back to back, so a collection storm landed inside one phase's window and the
harness reported `write_base64_property` at **136.7% of `save_total`** — impossible
for a strict subset of the same work, and obvious only because the subset relation
gives the table something to contradict. Interleaving spreads that cost across
every phase instead of concentrating it in whichever one was unlucky.

Medians of nine runs after three warm-ups, with the full spread printed under the
table: on a shared machine the floor is the honest half of a row, and a phase that
allocates 20 MB per call announces itself by varying, not by being slow on average.

## 9. Is a load measured on a warmed heap (issue #433)

Not a comparison and not a profile: **one operation, two processes.**

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- heap-warmth prepare
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- heap-warmth cold
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- heap-warmth warm

python bench/python/bench_heap_warmth.py prepare
python bench/python/bench_heap_warmth.py cold
python bench/python/bench_heap_warmth.py warm
```

**Both languages, or neither.** The finding is a ratio per language and the two ratios
are then compared, so taking them on two machines compares the machines as much as the
languages. `Benchmark (on demand)` (section 10) runs all four states inside one round for
that reason, and the Python side mirrors the C# structure rather than its code — three
subcommands, the warming saves before the loop, the stream built outside the timer.

`compare-persistence` measures every save, then every load, in one process — so by
the time a load is timed the large-object heap has already been grown and its pages
committed by the saves. The question is what that is worth, and **it cannot be two
rows of one run**: a save warms the heap for everything after it, and nothing inside
a process honestly undoes that. Hence three subcommands.

`prepare` writes the artifact once. **`cold` then reads those bytes having built and
saved nothing** — that is what makes it cold, and it is why the artifact comes off
disk rather than from a save. `warm` does its saves **first** and then loads, which
is the order the harness uses; a save between two loads would add garbage competing
with the load instead of leaving a heap behind it.

Both states report **allocation per load**, and they must agree. If they do not, the
two are running different workloads and no timing comparison between them is valid —
the claim under test is that the load does identical work and pays for more of it.

**The result.** Nine alternating rounds on one hosted runner, all four states inside
each round, `Benchmark (on demand)` run 3:

| | cold | warm | warm/cold |
| --- | ---: | ---: | ---: |
| `EmbeddingIndex.Load` | 18.101 ms | 16.559 ms | **0.919** |
| `np.load` | 1.382 ms | 1.316 ms | **1.001** |

Medians of the nine round medians; the `warm/cold` column is the median of the nine
**paired** ratios, which is the statistic the alternation is for — pairing an unpaired
cold median against an unpaired warm one puts numpy at 1.05× and is an artefact.

**We have the subsidy: warm is faster in 9 rounds of 9** (sign test p = 0.004),
by 8.1%. **numpy does not have it: 4 of 9**, median ratio 1.001 — a coin toss.

**The timing is the weaker half of the evidence.** Every round, on both sides of the
C# comparison: allocation 37 069 648 bytes cold against 37 069 848 warm — 200 bytes
apart in 37 MB — and collections **4/4/4 cold against 3/3/3 warm**. Same work, same
allocation, one fewer garbage collection. That is the mechanism, and it is discrete
and reproducible where a millisecond is neither.

**On a shared machine none of this reproduces.** The same instrument on the container
put warm faster, level, then slower over three rounds with one state's median swinging
4× between them. That is why section 10 exists.

## 10. Running a diagnostic on a second machine (issue #461)

`roc-parallel` (section 6), `save-phases` (section 8), `heap-warmth` (section 9),
`pool-cost` (section 11), `ingest-phases` (section 12), `sidecar` (section 13) and
`tensor-primitives` (section 14) are
C#-only subcommands rather than `[Benchmark]` classes, so no benchmark or harness in
`bench-map.json` selects them and the nightly never runs one. That is deliberate — they
answer a question a lot asks once, not a regression worth watching every night.

`bench-map.json` does name them, in its own `diagnostics` list, and `tools/check_bench_map.py`
checks that list against what `Program.cs` dispatches. Exempt from the nightly is not exempt
from existing: a subcommand nothing names cannot be told apart from a harness somebody forgot
to map, which is the finding that file exists to raise.

The cost of that was that they ran only on a contributor's own machine, and for
some of them the machine is the finding: `save-phases` measures shares inside one
window and those transfer, but a subcommand comparing two processes has absolutes
that do not. `docs/guides/performance.md`
withdrew a 1.61× taken on a shared container for exactly that reason.

**`Benchmark (on demand)`** closes it: dispatch
`.github/workflows/bench-ondemand.yml` against any branch, name the subcommand,
and read the job summary. The subcommand is free text rather than a list, because
the list would be a promise about a ref the workflow has not checked out yet — a
branch adding one runs it the day it exists, without editing the workflow.

It **prints and does not publish** — nothing there writes a page, opens a pull
request or touches the wiki. A number becomes a published figure when a person
reads it and decides, which is what `performance.md`'s name-the-machine rule is
for. The workflow records `uptime`, the core count and the CPU model beside every
run so that decision has what it needs.

## 11. What renting the payload costs against allocating it (issue #470)

One primitive, two rows, interleaved in one process:

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- pool-cost
```

Allocating a 20 589 007-byte buffer against renting and returning one, both touching every page.
**Against an uninitialized allocation**, because `Buffers.AllocateUninitialized` is what the read
path uses — comparing a zeroed `new byte[]` would charge the pool's rival for a memset the code
does not do and inflate the saving by the whole zeroing.

On a hosted runner: allocate **1.783 ms** median against rent's **0.042**, so 42× and 1.74 ms a
load. The allocation's own minimum is 0.071 ms, as cheap as the rent — **what costs is the
large-object collection it provokes**, not the allocation. `docs/guides/performance.md`
is what that decided, amending 0053, which had refused pooling without ever timing it.

## 12. Where the .npy ingest's time goes (issue #480)

Ten phases, interleaved, each with the collections it provoked beside its milliseconds:

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- ingest-phases
```

Issue #466 removed a whole copy of the 15.36 MB block from `NpyFile.Read(Stream)` and the
published row did not move; removing the copy into the index moved it by more than a copy is worth. Both
readings came from subtracting whole rows, which cannot say where the time went. This takes the
ingest apart instead: the staged read, the zero-copy overload beside it, the `float[]` allocation
cold and reused, the payload read into an array that already exists, the header alone, `FromBlock`
against `FromOwnedBlock`, and a bare `memcpy` of the block as the floor every share is read
against.

**`parse_header_only` is the header through the memory overload**, not through the stream reader
whose parse the ingest actually pays, so what it prices is the parse plus that overload's own
floor. It lands on top of `read_memory_view` — 0.005–0.006 ms against 0.005–0.008 — and that is
the result rather than an accident of the phase: on a block this size the header is below the
table's resolution either way.

**`from_owned_adopt` never touches the array it allocates**, so by this file's own page-touching
argument it is charged for reserving the pages and not for committing them — which is exactly why
the row reads in hundredths of a millisecond. The reading is asymmetric on purpose:
`from_block_copy` − `from_owned_adopt` is a copy *plus* the destination's first touch, not the
copy alone. The surcharge is small on this table — `from_block_copy` 0.964–1.089 ms against
`block_copy_floor`'s 0.936–0.976 into a warm target — and touching the array would change what
the row measures rather than clean the subtraction up.

**`ingest_total` and `ingest_total_last` are one measurement at two positions.** They call the
same method, first in the round and last, so any difference between them is position and nothing
else. That is the single variable `docs/guides/performance.md`
left open: its gap survived the first table, and it named a phase-reordering run as what would
settle it. The `gen` columns are the half to read first — a collection provoked by one ingest is
paid by whichever phase runs next, not by the one that allocated.

**Two independent subtractions have to agree, and that is the point of the mode.**
`read_stream_owned - stream_copy_floor` is what the read's allocation costs inside the read;
`allocate_cold - allocate_reused` prices the same allocation on its own. If they disagree, the
allocation is not what the difference between those rows is made of.

The gen columns are collections **summed over the nine runs**, not per run: a block this size
provokes at most one gen2 per run, and a column of zeroes and ones says less than a total.
`docs/guides/performance.md`
is why they are there at all — on the artifact buffer the time and the collection count told
different stories, and only the second one explained the first.

Nothing here is published. It prints a table and writes no page: what it measures becomes a figure
when a person reads it on a named machine and decides, which is [section 10](#10-running-a-diagnostic-on-a-second-machine-issue-461)'s
rule and not this mode's exception.

## 13. What a binary sidecar would buy (issue #436)

Six rows, interleaved one round each:

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- sidecar
```

The artifact against a `.npy` block plus the head a sidecar would still have to write, and six
timings: the artifact load, the block read, a **floor** — the read plus one copy into a backing
store, which is what a bulk ingest would do — the rebuild through `Add` that was the only route
before issue #474, and the two rows that lot added:

- `ingest copy` is the sidecar route as it will exist: the block read, then
  `EmbeddingIndex.FromBlock`. It is the row #474's gate is read off — landing with `sidecar floor`
  is the finding, and landing near `rebuild index` instead is the refusal.
- `ingest only` is the ingest alone, on a block already in hand, so a later regression in the read
  or in the ingest can be attributed to one of them rather than to their sum.

The floor is a bound in the sense section 8's `block_copy_floor` is one: a route measured so the
method that would take it can be judged against it.

**The floor and the ingest do not allocate the same way, and the asymmetry runs in the ingest's
favour.** `sidecar floor` takes its backing store from `new float[...]`, which the CLR zero-fills,
while `EmbeddingIndex.FromBlock` allocates through the library's own uninitialized allocation and
skips the zeroing. Both are one copy, and not the same kind of one: the ingest is ahead by one
memset of the block, about 15 MB at this corpus. It is stated rather than equalised, because the
uninitialized allocation is internal to the library and this project consumes the published
packages, and because re-cutting `sidecar floor` would invalidate the 5.847 ms
[ADR 0001](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
published, which is the bar this lot is judged against. The bias runs in the ingest's favour, so a
slow `ingest copy` is not an artefact of it: landing near `rebuild index` remains the refusal it
looks like. What it does mean is that `ingest copy` coming in *below* the floor must not be read as
beating it by the whole margin — part of that gap is the memset the floor pays and the ingest does
not.

There is no row for `FromOwnedBlock`. With `AlreadyNormalized` it assigns four fields and is
constant time whatever the block's size, so its ceiling is the `read npy block` row and a row of
its own would publish noise.

On a hosted runner: the sidecar is **1.331× smaller** and its floor is **2.02× faster** than the
artifact load, while the rebuild route is **0.66×** — slower than what it would replace.
[ADR 0001](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
takes the sidecar and makes the bulk ingest its precondition.

With that ingest built (#474), the same runner puts `load / ingest` at **1.45–1.62×** across three
rounds where `load / rebuild` is 0.62–0.65× — the route stops being slower than the artifact. It
does not reach the floor: `ingest / floor` is 1.13–1.26×, and the floor is the flattered side of
that comparison per the paragraph above. This section says only how to take the figures.

**Not on a container.** There the same rows put the floor at 0.73×, the opposite conclusion, with
the floor row spread over 12–43 ms against the runner's 4.0–8.5.

## 14. Our kNN kernel against `TensorPrimitives` (issue #437, V6)

Seven rows, interleaved, agreement checked before any of them is timed:

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- tensor-primitives
```

Issue #427's open-risks table carries *"`TensorPrimitives` makes the kNN redundant"*, and V6 of
[#437](https://github.com/CyrilB1531/lodestar/issues/437) is the only verification there that wants
a measurement rather than a reading. This is it.

**The access pattern is the whole question.** `EmbeddingIndex` normalizes on insertion, so `Search`
is a dot product over a contiguous block rather than a cosine — comparing our dot against
`TensorPrimitives.CosineSimilarity` would charge the BCL for two norms we never compute. Both
shapes are measured for that reason, and a third pair sweeps the block in **one** call rather than
10 000 calls of 384 floats, because that is what `TensorPrimitives` is designed for and the kNN
rows do not show it.

Every row calls the shipped [`VectorMath.Dot`](../docs/reference/embeddings/search/vectormath-dot.md)
rather than a copy: a kernel reproduced for a benchmark is a kernel nobody ships, and the throwaway
probe that preceded this mode had rewritten the horizontal sum.

**Agreement runs first and prints before the table.** A speed comparison between two routes that
disagree is meaningless, so it is a precondition here rather than a footnote beside the result.

The package is referenced by `bench/` and by nothing under `src/`. V6 is a question about an
incumbent, and referencing it to ask would be answering it.

The first run's answer is [decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md):
`TensorPrimitives` is 1.09–1.23× faster on the shape `Search` runs, and the kNN is still not
redundant, because the dot is only about half a query. **The container inverted every one of those
ratios** — it reported ours 3.7× faster on the dot — which is section 10's rule holding rather than
an aside about this mode.

**The vector width decides the magnitude on AVX-512 hardware** ([#754](https://github.com/CyrilB1531/lodestar/issues/754)).
The diagnostic prints `Vector512.IsHardwareAccelerated`; compare it with the 512-bit path on and off
through the .NET 10 knobs, `DOTNET_PreferredVectorBitWidth=256` or `DOTNET_EnableAVX512=0`.
`DOTNET_EnableAVX512F=0` is ignored by .NET 10 and leaves the path on.

## 15. Against the .NET incumbents (issue #438)

Every section above answers *"should you leave Python"*. `LevenshteinIncumbentBenchmarks` and
`FuzzIncumbentBenchmarks` answer the question a .NET reader asks first — *"why this over the .NET
library that already exists"* — which
[#438](https://github.com/CyrilB1531/lodestar/issues/438) requires of every package and which
nothing here measured until they landed.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- \
  --filter '*IncumbentBenchmarks*' --job short
```

Incumbents are referenced by `bench/` and by nothing under `src/`, pinned exactly in
`Lodestar.Text.Benchmarks.csproj`:

| class | baseline | incumbent |
| --- | --- | --- |
| `LevenshteinIncumbentBenchmarks` | `Levenshtein.Distance` | Fastenshtein 1.0.12, Quickenshtein 1.5.1, F23.StringSimilarity 7.0.1 |
| `FuzzIncumbentBenchmarks` | `Fuzz.*` | Raffinert.FuzzySharp 6.0.0 |
| `TokenizerIncumbentBenchmarks` | `WordPieceTokenizer`, `SentencePieceTokenizer`, `BpeTokenizer` | Microsoft.ML.Tokenizers 2.0.0 |
| `VectorizerIncumbentBenchmarks` | `TfidfVectorizer` | ML.NET 5.0.0 `FeaturizeText` |
| `MetricsIncumbentBenchmarks` | `Lodestar.Metrics` | ML.NET 5.0.0 binary evaluator |

**The values agree before the clocks run.** A speed table over two functions that return different
answers means nothing. Checked over `kitten`/`sitting`, `flaw`/`lawn`, the sentence pair the fuzzy
class uses, an empty operand and an identical pair: all four Levenshtein implementations return the
same distance on all five, and Lodestar and FuzzySharp return the same ratio on all four operations
to the last digit of the double. The tokenizers were checked the same way, over 200 documents of
the corpus: identical ids, both models, once `Microsoft.ML.Tokenizers` is told not to prepend a
beginning-of-sentence piece — which is the sort of thing an unchecked table would have silently
measured. The byte-level BPE row, added for [#673](https://github.com/CyrilB1531/lodestar/issues/673),
was checked over all 5 000 documents: identical ids against `CodeGenTokenizer`, the incumbent's GPT-2
byte-level BPE, once its vocabulary carries the `<|endoftext|>` its `Create` insists on, appended past
the last id where no encode reaches it. Section 14's rule, applied here as a precondition rather than a footnote.

Five shapes, for five reasons:

- **Levenshtein** takes `Length` at 8, 64 and 512, because the bit-parallel path is what the long
  row is for and a single length would hide it. `Levenshtein.Distance`'s default overload is the
  baseline: all four libraries compare UTF-16 code units, so `TextElement.CodePoint` would be
  measuring something none of the incumbents offers.
- **The fuzzy ratios** take the *operation* as the parameter — `Ratio`, `PartialRatio`,
  `TokenSetRatio`, `WRatio` — rather than writing eight methods. With one baseline for the whole
  class BenchmarkDotNet compares `PartialRatio` against `Ratio` instead of against its counterpart,
  which is not the question; as a parameter, each pair gets its own baseline row.
- **The tokenizers** take the model as the parameter for that same reason, and encode all 5 000
  documents of `bench/corpus/vocabs/documents.json` per operation, so the number is encoding cost
  rather than model loading — both tokenizers are built once in `[GlobalSetup]`, as
  `BpeBenchmarks` does.
- **The vectorizers** are the one pair that is *not* like-for-like, and the next section is what
  that costs the reading.
- **The metrics** take the *request* as a parameter, because that is where the two libraries
  differ: `Bundle` asks both sides for the six numbers ML.NET's binary evaluator returns, and
  `AccuracyAlone` asks for one. ML.NET runs the same `EvaluateNonCalibrated` call in both rows,
  deliberately — it has no call that returns one metric, so accuracy alone costs a caller the
  bundle. That is the measurement, not an unfair setup.

### `FeaturizeText` is not our vectorizer, and the ratio alone would lie

The other two classes compare functions that return the same answer. This one does not, and #438
says so itself: `FeaturizeText` is `IDataView`-coupled and does a different job, so *"a row
treating them as equals would contradict our own argument."*

Measured on the same corpus rather than asserted:

| documents | Lodestar `TfidfVectorizer` | ML.NET `FeaturizeText` |
| ---: | --- | --- |
| 200 | 200 × 7 018 sparse, 7 996 stored values | 200 × 31 112 dense, 70 307 non-zero, 6 222 400 floats materialized |
| 1 000 | 1 000 × 21 867 sparse, 39 974 stored values | 1 000 × 81 384 dense, 351 217 non-zero, 81 384 000 floats materialized |

`FeaturizeText` adds character n-grams to the word n-grams and L2-normalizes, so its feature space
is roughly four times wider and it produces about **8.8× more non-zero features**. It is doing more
work, and a wall-clock ratio that ignores that is not a claim anyone should believe.

Divide it out and the honest number appears. At 1 000 documents the container run measures 45.6 ms
against 445.0 ms — 9.75× — for 39 974 and 351 217 non-zero features respectively: **1.14 µs and
1.27 µs per feature produced, within about 11 % of each other.**

So the advantage is not that the arithmetic is faster. It is that a sparse matrix stores 39 974
values where the dense pipeline materializes 81 million floats for the same thousand documents, and
that ours needs no `IDataView`, no schema and no pipeline object to hand back a `CsrMatrix`. That
is the argument the README makes, and it is the one this measurement supports — no more.

**Lucene.NET is deliberately not here.** #438 names it "where it overlaps", and on the vectorizer
it does not: its TF-IDF exists inside an index, reached through an `IndexSearcher`, so comparing a
library call to an indexing engine would repeat exactly the category error this section exists to
avoid. Its analysis chain does overlap our tokenizers, which is a different measurement and a
different lot.

### `Score` is a margin, not a probability — and an unchecked harness measures nothing

The metrics pair agrees exactly once its harness is right: identical accuracy, identical confusion
matrix, AUC within 5e-9 (ML.NET takes the score as a `float`). Getting there took a correction
worth recording, because nothing about it fails loudly.

ML.NET's binary evaluator thresholds the **`Score` column at zero** and ignores `PredictedLabel`.
Fed probabilities in `[0, 1]` — the obvious thing to put in a column named `Score` — it classifies
every row positive:

```text
PREDICTED || positive | negative | Recall
 positive ||   10,007 |        0 | 1.0000
 negative ||    9,993 |        0 | 0.0000
```

Accuracy 0.5003 against our 0.7496, and a benchmark comparing a real classifier to a degenerate one
would have run happily. AUC agreed throughout, being rank-based, so the one metric a careless check
looks at was the one that could not catch it. Passing the margin (`score - 0.5`) makes both sides
agree to the digit.

Same shape as the beginning-of-sentence piece in the tokenizer pair: a convention on one side only,
silent, and fatal to the comparison.

### Coverage is the other half of the metrics answer

Issue #438 expects this package to win on coverage rather than speed, and says the table should state
which of the two it measures. Counted by reflection over both assemblies rather than asserted:

| | types | metric entry points |
| --- | ---: | ---: |
| `Lodestar.Metrics` | 54 static metric classes | 81 distinct public method names |
| ML.NET | 6 result types (`BinaryClassificationMetrics`, `CalibratedBinaryClassificationMetrics`, `MulticlassClassificationMetrics`, `RegressionMetrics`, `ClusteringMetrics`, `RankingMetrics`) | 28 distinct properties |

The `AccuracyAlone` row is that difference made measurable rather than argued.

**Where the numbers may be published.** Not from a container. Section 10's rule holds here with no
exception — `docs/guides/performance.md`
withdrew a 1.61× taken on a shared container, and section 14 records the container *inverting* every
`TensorPrimitives` ratio. A container run of these two classes is a smoke test that the harness
works, and nothing else. [`docs/guides/performance.md`](../docs/guides/performance.md) takes them from a named machine, in
the per-package comparison each class belongs to (#679). The nightly publishes their ratios to
`docs/guides/nightly_run.md` on its own, and `docs/guides/benchmark_latest.md` carries each class's last
reading forward until a change selects it again, dated by the night it was measured rather than the
night the page was written, since all five classes are in
`bench-map.json` and are selected by any change under `src/Lodestar.Fuzzy/`,
`src/Lodestar.Text/Distances/`, `src/Lodestar.Text/Vectorization/`,
`src/Lodestar.Embeddings/Tokenization/` or `src/Lodestar.Metrics/`.

**The nightly remembers its ratios.** Every `Ratio` it publishes is appended to
`bench/nightly/ratios.csv`, and `tools/nightly_series.py` reports at the end of the page each
ratio that stepped past its own noise or drifted over ten days (`docs/guides/nightly_run.md`).
That is how measuring a class tells anyone it moved, where before a movement was rendered and
overwritten the next night (#672). A class with no `[Benchmark(Baseline = true)]` has no ratio,
so it has no memory either.

### `Lodestar.Conformal` has no incumbent, and that is the measurement

[#438](https://github.com/CyrilB1531/lodestar/issues/438) attaches "one benchmark against a named
.NET incumbent" to the package rule, so a package with no benchmark owes an explanation rather than
a blank row. `Lodestar.Conformal` has none because there is nothing to name: the survey behind
[#441](https://github.com/CyrilB1531/lodestar/issues/441) found no C# implementation of conformal
prediction at all — not an abandoned one, not a partial one. Against Python, MAPIE is the reference
the oracle corpus already replays; a wall-clock row against it would price `numpy.sort` on a few
dozen doubles.

That is also what the work is. `SplitConformal.Quantile` sorts the calibration scores and indexes
one of them; the calibration set is small by construction — a few dozen to a few thousand points,
sorted once and reused for every prediction — so a benchmark here would report `Array.Sort` under a
name suggesting it measured conformal prediction.

Two things would change that and neither has happened: a second .NET implementation appearing, or a
**normalised** conformity score landing here. The second is the one to watch — dividing each
residual by a per-sample estimate of the local spread means a second model's inference inside the
calibration loop, which is a cost that is not obvious by inspection and would earn its own class.

## 16. `Lodestar.Decomposition` against ML.NET's PCA (issue #438, #440)

**Not a like-for-like comparison, and the numbers below (once there are numbers) must not be read
as one.** ML.NET's `ProjectToPrincipalComponents` computes **centred** PCA at a **fixed rank of
20**, over an `IDataView`. `TruncatedSvd` computes **uncentred** truncated SVD, at a rank the
caller names, over a `CsrMatrix`; `Nmf` factors a **non-negative** basis neither of the other two
produces. Three different decompositions cannot be checked for agreement, so what is checked
instead is that each side reconstructs its own input to its own stated error — the shape #438's
harness already uses for `FeaturizeText` (section 15), applied here across three algorithms
instead of two.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Decomposition*'
```

`DecompositionBenchmarks` builds one fixed corpus in `[GlobalSetup]` — a 2 000 × 500 sparse
term-document matrix at 2% density, seeded so two runs measure the same matrix — plus the dense
`float[][]` twin `ProjectToPrincipalComponents` needs, since it has no `CsrMatrix` overload. Three
rows, all at rank 20:

| class | method | what it fits |
| --- | --- | --- |
| `TruncatedSvd` | `TruncatedSvd_Rank20` | uncentred truncated SVD, over the sparse matrix |
| `Nmf` | `Nmf_Rank20` | non-negative factorization, capped at 50 iterations |
| ML.NET 5.0.0 | `MlNet_ProjectToPrincipalComponents_Rank20` | centred PCA, over the dense twin |

Each `[Benchmark]` method calls its package's own `Fit` — the ML.NET pipeline is built and fit
inside the measured region too, not in `[GlobalSetup]`, so all three rows answer the same
question: the cost of fitting, not the cost of using an already-fitted model. For PCA that is
where the work is regardless — `Fit` computes the components, and the `Transform` the row also
runs is a cheap dense projection over an already-fitted model.

**No numbers are published from a container.** Per
`docs/guides/performance.md`, a
run on a shared cloud container is not the machine `docs/guides/performance.md` reports — the same
row there has read 3× slower on one. The three rows taken on a named machine are in
[`docs/guides/performance.md`](../src/Lodestar.Decomposition/performance.md#truncated-svd-and-nmf-against-mlnet-500s-projecttoprincipalcomponents)
(#679).

## 17. BK-tree vs a length-filtered scan (issue #526)

`BkTreeBenchmarks` measures `BkTree.WithinDistance` against the baseline a caller writes instead
of an index: a linear scan that skips any word whose *length* already puts it out of range, then
calls `Levenshtein.Distance` on what survives. Both sides answer the identical question —
"everything within edit distance `k`" — over the same corpus, so this is like-for-like without the
caveats sections 15 and 16 need.

The corpus is generated rather than committed, like the other two:

```bash
python3 bench/corpus/generate_dictionary.py   # writes bench/corpus/dictionary.json, git-ignored
```

20 000 words in two shapes: `uniform` (independent random 4–10 letter strings) and `clustered`
(2 500 roots, each generated word one or two mutations from a root) — the shape a natural
dictionary has, where near neighbours are common rather than incidental.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BkTree*'
```

`[Params]` sweeps `Radius` (1 to 4) and `Shape` (`uniform`, `clustered`) against
`LengthFilteredScan` as the baseline — 8 pairs, 16 benchmarks, default job. 200 queries drawn from
the corpus itself: looking up a word already in the dictionary is the case a spelling corrector
hits every keystroke, and the densest neighbourhood the tree has to work through.

Numbers are published in
[`docs/guides/performance.md`](../src/Lodestar.Text/performance.md#bk-tree-vs-a-length-filtered-scan-issue-526),
and the reader-facing take-away is in
[`docs/guides/dictionary-lookup.md`](../docs/guides/dictionary-lookup.md#where-the-tree-stops-paying) — this section
documents how to measure, not what was measured.

## 18. `Lodestar.Stats` against `Accord.Statistics` (issue #442)

`Lodestar.Conformal`'s spec recorded "no .NET incumbent" and that was true there. It is not true
for hypothesis tests: `Accord.Statistics` 3.8.0 is archived (November 2020, last published October
2017) but still installable, and carried this exact surface before this package existed. Section 15
already sets the pattern — a second implementation is a second opinion — and this section applies
it a third time, in its own project rather than inside `Lodestar.Text.Benchmarks`, since nothing
`Accord.Statistics` needs has anything to do with what that project already references.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*'
```

### Resolving `Accord`'s names against the restored package, not the plan

The design spec guessed at `Accord`'s spellings from a 2017-era release. All three guesses turned
out right, checked by loading the restored `netstandard2.0` asset of `Accord.Statistics.dll`
through reflection and listing `Accord.Statistics.Testing`'s public types rather than trusting the
guess:

| family | `Lodestar.Stats` | `Accord.Statistics.Testing` |
| --- | --- | --- |
| independent-samples t-test | `TTest.Independent` | `TwoSampleTTest(double[], double[], assumeEqualVariances: false)` |
| Mann-Whitney U | `MannWhitney.Test` | `MannWhitneyWilcoxonTest(double[], double[])` |
| chi-square test of independence | `ChiSquare.Contingency` | `new ChiSquareTest(new Accord.Statistics.Analysis.ConfusionMatrix(a, b, c, d), yatesCorrection: …)` |

No pair was dropped — `Accord` carries a counterpart for every one of the three families #442
names, so nothing here records an absence.

**`Accord.Statistics` 3.8.0 needed no NU1701 suppression.** The brief for this task expected one,
reasoning from the package's `netstandard1.4` primary target. Measured instead: the `.nupkg` also
ships a `lib/netstandard2.0` asset — inspected directly in the local NuGet cache — which is what a
`net10.0` project restores against, confirmed by `dotnet restore` and `dotnet build` on
`Lodestar.Stats.Benchmarks.csproj` producing no NU1701 or compatibility warning to suppress.
`Accord.Math` and `Accord` (the two transitive dependencies) resolve the same way.

**The chi-square comparison needs `yatesCorrection: true` to compare the same statistic.**
`ChiSquare.Contingency`'s own default is `Continuity.Applied`, which is Yates's correction on a 2x2
table ([`src/Lodestar.Stats/ChiSquare.cs`](../src/Lodestar.Stats/ChiSquare.cs)); `ChiSquareTest`'s
own default is `yatesCorrection: false`. Left at their defaults the two rows would time two
different statistics under one name — checked once by running both settings on the corpus case
below, where `false` reproduces the *uncorrected* scipy figure and `true` the corrected one to the
same precision as every other pair here.

### The three families agree with `scipy`, not just with each other

`Lodestar.Stats`' oracle discipline (`docs/equivalence.md`, `CLAUDE.md`'s "Conformance is proven by
frozen oracles" section) means every one of its own numbers already traces to `scipy`. What this
task adds is a second, independently-implemented set of numbers on the *same* inputs, and the check
that all three agree everywhere they were compared — the benchmark's own random samples, plus one
frozen corpus case per family from `tests/oracles/`:

| case (from `tests/oracles/stats_*.json`) | `scipy` | `Lodestar.Stats` | `Accord` |
| --- | --- | --- | --- |
| `ttest_ind(a=[1,4,7,9], b=[2,3,8,12,15], equal_var=False)` | p = 0.3998549375410838 | p = 0.39985493754108214 | p = 0.39985493754108359 |
| `mannwhitneyu(a=[1,4,7,9], b=[2,3,8,12,15], method="auto")` | p = 0.5555555555555556 | p = 0.55555555555555558 | p = 0.55555555555555558 |
| `chi2_contingency([[10,20],[30,40]], correction=True)` | p = 0.5040358664525046 | p = 0.50403586645250453 | p = 0.50403586645250464 (`yatesCorrection: true`) |

Every row agrees to 1e-9 or tighter — the differences shown are the last one or two digits of a
`double`, the same order of noise `docs/equivalence.md`'s own `1e-9` tolerance exists to absorb, not
a disagreement. **No case was found, in either the random samples the benchmark itself draws or
these three frozen corpus cases, where `Accord` and `scipy` (and therefore `Lodestar.Stats`) part
ways.** That is a reportable result in its own right: it means this package's oracle-driven
implementation and the one incumbent both converge on the textbook formulas for these three
families, at every input shape checked. Should a future corpus case turn up a real disagreement,
`scipy` remains the reference this package follows (`docs/equivalence.md`); `bench/README.md` is
where that would be recorded, and the corpus itself would not change.

### Configuration

`[Params(100, 10_000)]` on `SampleSize` sweeps both operands' length together, so the two rows per
method are the same shapes `MannWhitney`'s own guard cares about: 100 is small enough for
`ExactMethod.Auto` to still be a live question, and `10_000 * 10_000` is far past
`MannWhitney.MaxExactProduct = 20_000`, so that row is where both implementations are forced onto
the asymptotic path by design — the comparison worth taking at that size, not an artefact of one
side falling back and the other not (see
[`src/Lodestar.Stats/MannWhitney.cs`](../src/Lodestar.Stats/MannWhitney.cs)). `[GlobalSetup]` draws
`_a` and `_b` from a fixed seed (442) so every run measures the same inputs, and builds the fixed
2×2 `_table` the two chi-square rows share.

`--job short` — `ShortRun(LaunchCount=1, WarmupCount=3, IterationCount=3)` — is what was actually
run, against `[MemoryDiagnoser]`. Section 15 uses the same override for the same reason: a full run
(`--filter '*'` with no `--job` override, BenchmarkDotNet's default job of up to 15 iterations) was
not taken here. `docs/guides/performance.md` carries the resulting numbers, the machine, and the
window, per the rule for where a fact belongs (`CLAUDE.md`'s "Where a fact
belongs" table).

### The tails underneath: `DistributionTailBenchmarks`

`ChiSquare.Contingency`'s row above is mostly the chi-squared tail, which no whole-test row can
separate out. `DistributionTailBenchmarks`, in the same project, times one published tail call per
row and has no incumbent: `Distributions.ChiSquaredSf` at one, three, four, a hundred and a
fractional 2.5 degrees of freedom (arguments from `tests/oracles/stats_distributions.json`, plus
4.41 for the two without a corpus case), and `Distributions.NormalQuantile`, which inverts the
normal tail. The
fractional row is the control: no closed form covers it, so it moves only when the iteration does.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- \
  --filter '*StatsBenchmarks.*ChiSquare*' '*DistributionTailBenchmarks*'
```

`RankTestBenchmarks` is the rank tests alone, past the sizes `StatsBenchmarks` reaches and with no
incumbent: `KruskalWallis.Test` over three groups, `Wilcoxon.Paired` and `MannWhitney.Test`, each
sample `SampleSize` values at 10,000 and 100,000, `Ties` rounding them to hundredths so most share
a rank. It exists to watch the merged rankings of #711 and #719; Mann-Whitney is the row that did not
change the second time.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*RankTestBenchmarks*'
```

Numbers are published in
[`docs/guides/performance.md`](../src/Lodestar.Stats/performance.md#lodestarstats-against-accordstatistics-issue-442)
— this section documents how to measure, not what was measured.

## 19. `Lodestar.Stats.Regression` against `Accord.Statistics` (issue #566)

[#566](https://github.com/CyrilB1531/lodestar/issues/566) expected this section to say there is no
.NET incumbent for regression inference. **The reading it also asked for says otherwise**, and the
measurement replaces the explanation:
[decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) loaded both
candidates through a `MetadataLoadContext` and found `Accord.Statistics` 3.8.0 exporting the whole
summary table — `MultipleLinearRegressionAnalysis` with `StandardErrors`, `Confidences`, `FTest`,
`RSquareAdjusted`, and a `Coefficients` collection whose row carries `TTest` and its interval.

`MathNet.Numerics` 5.0.0 is not a candidate here and the reason is worth stating, because it is not
"it has no regression". It has four families of them, and every one returns coefficients. Its only
coefficient standard errors live in `Optimization.NonlinearMinimizationResult`, for the non-linear
minimisers, unreachable from `LinearRegression` — so there is no MathNet call that produces the
thing this row prices.

Section 18 already resolves `Accord.Statistics` in this project, so the plumbing is unchanged: the
package is archived and LGPL-2.1, which bars it from `src/` under
[decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) and does
not bar it from a benchmark project that ships nothing.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*OlsBenchmarks*' --job short
```

### What the pair does and does not compare

Both rows compute a whole table, and **the two tables are not the same table.** Accord exports no
variance inflation factor anywhere in its 4 796 members, so `Learn` performs one solve. `Fit`
performs five at four regressors — the model, plus one auxiliary regression per regressor — because
its table carries the VIFs. Neither library offers a cheaper shape, so this is the only comparison
available; there is no coefficients-only row either, since adding one would have measured `Fit`
twice under two names.

That asymmetry is what the numbers show, and it inverts with size: this package wins at 100 rows
and loses at 10 000, which is exactly what doing five factorisations instead of one predicts once
the `O(mn²)` term overtakes the fixed per-call overhead. Read the ratio next to what each side
returns, not on its own.

The shapes differ on purpose and the difference is part of the cost. `Lodestar.Stats.Regression`
takes a row-major `ReadOnlySpan<double>` — one allocation the caller already has — where Accord
takes `double[][]`, one array per row. At ten thousand rows that is ten thousand allocations before
any arithmetic happens, which `[MemoryDiagnoser]` reports rather than hides.

Four regressors throughout, on a seeded design (`Random(566)`), at 100 and 10 000 rows: a hundred
rows is where a table is actually read, and ten thousand is where the QR's `O(mn²)` starts to show
against the per-row allocation.

Numbers are published in
[`docs/guides/performance.md`](../src/Lodestar.Stats.Regression/performance.md#lodestarstatsregression-against-accordstatistics-issue-566)
— this section documents how to measure, not what was measured.

## 20. `Lodestar.Survival` against nothing, deliberately (issue #569)

**There is no incumbent, and that absence is the section.** A NuGet capability search on
2026-09-09 returned **0 packages** for `survival analysis` and **0** for `kaplan meier`; the
searches are recorded in
[decision 0002](../docs/decisions/0002-provenance-and-the-allowed-references.md)
with their queries and counts. The #427 protocol reads an incumbent's exported surface through a
`MetadataLoadContext` rather than its README — and where there is no assembly to load, the protocol
is discharged by recording the searches instead of by pretending to run it.

`scikit-survival` is the nearest reference in any language and is **refused**, not unavailable: its
licence is GPL-3.0-or-later, which [decision 0002](../docs/decisions/0002-provenance-and-the-allowed-references.md)
excludes outright. `lifelines` (MIT) is the oracle the corpora are frozen from, in
`tools/generate_oracles.py`, and it is a Python library — not a .NET package this could race.

So `SurvivalBenchmarks` measures the three estimators against **each other and against input
shape**, which is the only comparison available:

```bash
dotnet run -c Release --project bench/Lodestar.Survival.Benchmarks -- --filter '*SurvivalBenchmarks*' --job short
```

### Why ties are a parameter and not an accident

`SampleSize` alone does not move these estimators the way it looks like it should. Both curves walk
a **step table**, one entry per distinct duration, so a sample of a hundred thousand subjects with
four thousand distinct durations does less per-step work than its size suggests and more per step.
`DistinctPercent` is therefore a parameter beside it — 100 for every duration distinct, 4 for heavy
ties — and it is what separates the two curves:

- Kaplan-Meier multiplies **once per step**, so a tied sample costs it less.
- Nelson-Aalen's tie correction sums `1 / (n - i)` **once per event**, so a tied sample costs it the
  same as an untied one of the same size. The two therefore converge as ties thin out and part as
  they thicken, and `KaplanMeierEstimate` is the baseline so the ratio reads directly.

`LogRankTest` is given the sample split in half, which is what a two-arm trial is. It walks both
arms once per distinct **event** time — censoring-only times carry no information and are skipped —
so its cost tracks the number of events rather than the number of subjects.

A third of the subjects are censored throughout, on a seeded corpus (`Random(569)`), which is an
ordinary trial's shape rather than a best case.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md) —
this section documents how to measure, not what was measured.

## 21. BM25 against LuceneSharp (issue #573)

The #427 protocol reads an incumbent's exported surface through a `MetadataLoadContext` rather
than its README, and here it settles which Lucene. **`Lucene.Net` has never shipped a stable
release** — `4.8.0-beta00018`, published 2026-06-22, is still the newest, twelve years after
Lucene 4.8.0 shipped in Java, and it carries no `net10.0` asset.
**`LuceneSharp.Core` 26.8.4415** (Apache-2.0, `curiosity-ai/lucene-sharp`) is the .NET 10 port,
and loading it names `BM25Similarity` as the default scorer — the same Okapi formula this
package computes, so the two rows price one algorithm rather than two.

Its API is not Lucene.Net's, and three differences cost a session: `ByteBuffersDirectory` is the
in-memory store, `IndexSearcher.SetSimilarity` is a method rather than a property, and
`BooleanQuery` exposes no public constructor. A single `TermQuery` sidesteps the last of those
and is the same shape of work on both sides — what is being priced is the index, not the query
language.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Bm25Benchmarks*' --job short
```

### Two phases, because one ratio would lie

**These are not like-for-like, and that is the measurement.** Lucene needs an index — a directory,
a writer, an analyzer, a commit — where `Bm25Index` scores a `CsrMatrix` a caller already built to
vectorize with. One ratio would hide one of the two costs, so both are rows.

This section first predicted which way each row would lean: the query favouring this package, the
build favouring Lucene. **The measurement refuted the first half**
([#677](https://github.com/CyrilB1531/lodestar/issues/677)). Lucene answered the query 7.7× faster at
1,000 documents and 77.6× at 20,000, because `Bm25Index.Top` sorted every document to keep ten. Since
[#751](https://github.com/CyrilB1531/lodestar/issues/751) it keeps a bounded heap instead, and the query
row reads 1.6× in this package's favour at 1,000 documents and 1.4× in Lucene's at 20,000. The two rows
stand; neither reading is a prediction any more.

- `LodestarQuery` / `LuceneQuery` — one query against a structure already standing.
- `LodestarFromText` / `LuceneFromText` — text in, ranking out, index included.

The reading is neither row alone. From text, Lucene is ahead. For a caller who already holds the
matrix, the index is cheaper to build than Lucene's, and whether each query is dearer depends on the
corpus's size; `performance.md` has where the two cross.

Lucene also answers a different question — a real query language, an index on disk, and the
Block-Max WAND top-k this package does not have. [#440](https://github.com/CyrilB1531/lodestar/issues/440)
lot 4 closed on exactly that point, and nothing here reopens it.

A seeded corpus (`Random(573)`) of 500 vocabulary terms and 40 tokens per document, at 1 000 and
20 000 documents: a thousand is where a matrix is rebuilt per request, twenty thousand is where
the index starts to pay for itself.

Numbers are published in [`docs/guides/performance.md`](../src/Lodestar.Text/performance.md#bm25-against-lucenesharp-issue-677) —
this section documents how to measure, not what was measured.

## 22. `Lodestar.Stats` and `Lodestar.Stats.Regression` against scipy and statsmodels (issue #595)

Sections 18 and 19 measure both packages against `Accord.Statistics`, which answers whether this
is the better .NET choice. This one answers the question `CLAUDE.md`'s thesis actually makes —
whether it replaces the Python script that exists today — and it is the fifth and sixth
cross-language harness, alongside `levenshtein`, `indel`, `metrics` and `persistence`.

Two harnesses over one corpus. `stats` is the hypothesis tests against `scipy.stats` — the first
three, and since #1162 Fligner-Killeen and the k-sample Anderson-Darling test on the corpus's two
samples, Spearman's matrix over its design and the point-biserial correlation of the second sample
against the first's sign;
`ols` is the summary table against `statsmodels`. They are mapped separately in
`bench/bench-map.json` so a change to one package does not re-run the other.

```bash
python bench/corpus/generate_stats.py          # ~15 MB, three sizes, git-ignored
python bench/python/bench_stats.py             # writes python-stats.json and python-ols.json
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-stats
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-ols
python bench/compare.py stats
python bench/compare.py ols
```

`--sizes 1000,10000` on either subcommand skips the rest; the merge gate in `bench/compare.py`
refuses a filtered run, the way `compare-metrics` already does.

Three things about the corpus are deliberate, and each was a correction to a first draft:

- **The contingency table's shape grows with the sample size** (4×5, 12×15, 40×50). Chi-square
  costs what the table's shape costs, not what its counts hold, so a fixed table measured the
  same work three times and reported it as three rows — at an implausible 668×.
- **The two samples have unequal variances.** Welch's test is what is being measured, and a
  pair Student's would answer identically would not exercise it.
- **The design is written once, row-major and without a constant column.** Each side adds its
  own intercept — `sm.add_constant` on one, `Fit`'s own leading column on the other — rather
  than the generator writing two shapes.

**`statsmodels` splits the VIF out of `.fit()`, and Lodestar does not.** `OrdinaryLeastSquares.Fit`
computes the coefficients, their errors, the tails, the intervals *and* the VIFs in one call
whether or not a caller reads them. `bench_stats.py` therefore measures `ols_summary_*` and
`ols_vif_*` separately and `bench/compare.py` sums the two before dividing, so the ratio prices
one table against one table. Splitting the C# side to match would mean fitting twice.

**Read the `cpu` column here, not just `wall`.** The note the other harnesses carry — that
elapsed time hides .NET's background GC threads while CPython is single-threaded — is only half
true on this corpus: `statsmodels` reaches LAPACK through numpy, which is threaded. At 100 000
rows its OLS rows measure roughly ten times more processor time than elapsed. Reporting the wall
clock alone would credit Python with work it spread across cores.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md)
— this section documents how to measure, not what was measured.

## 23. The sketches against the exact measure (issue #602)

`Lodestar.Text.Similarity`'s five exact measures compare **two** inputs. A corpus of a million
documents holds half a trillion pairs, and no exact measure survives that — which is the whole
reason `MinHash`, `SimHash` and `LshIndex` exist. This section prices that claim rather than
asserting it.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*SimilaritySketch*' --job short
```

**These do not compute the same thing, and that is the measurement.** `ExactPairwise` answers
every pair correctly at a quadratic number of set comparisons; `SketchThenVerify` answers most
pairs at one signature per document plus a banded lookup. Reporting only the query would hide the
signatures, and reporting only the signatures would hide what they buy on the millionth pair — so
both are rows, and `SignaturesOnly` and `FingerprintsOnly` sit beside them to show where the
sketch's own time goes.

The corpus is seeded (`Random(602)`), 24 tokens per document over a vocabulary of 2 000, at 500
and 2 000 documents — the smaller is where an exact scan is still reasonable and the larger is
where it stops being. `Permutations` is a parameter at 64 and 128 because a signature's length is
also its resolution: the estimate can only take `1 / Permutations` steps, so the two axes are
chosen together rather than separately.

**`SketchThenVerify` and `ExactPairwise` do not return the same count**, and a run where they did
would mean the corpus was too easy to be interesting. The gap is the false negatives the banding
accepts, and [`LshBanding.CollisionProbability`](../docs/reference/text/similarity/lshbanding-collisionprobability.md)
predicts it before the run.

**No incumbent row.** `MinHashSharp` 1.1.1 is the .NET incumbent and it ships `net6.0` only, so a
`net10.0` benchmark could price it while the shipped library cannot use it —
[#602](https://github.com/CyrilB1531/lodestar/issues/602) opened on exactly that asymmetry.
Benchmarking a package this repository refuses on framework coverage would measure something no
caller of `Lodestar.Text` can reach on one of its two targets.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md) —
this section documents how to measure, not what was measured.

## 24. Myers on the accelerator, against a bit-parallel CPU path (issue #444, kernel 3)

**This section was written expecting to report a kernel that does not ship, and the measurement
said otherwise — by two orders of magnitude.** The reasoning was that bench/README.md's GPU gate prices a
kernel against this repository's own path, that the path here is `Levenshtein.Distance`, and that
Myers is bit-parallel on both sides: one machine word per dynamic-programming row, tens of
nanoseconds for a short pair, against an accelerator amortising a renaming, two transfers and a
launch.

Every step of that is true and the conclusion was still wrong. What it missed is that
**the baseline is one thread** and the kernel is tens of thousands, over a workload with no
dependency between pairs. The measured gain is 28× to 146×
([`docs/guides/performance.md`](../docs/guides/performance.md) has the table), and the honest
caveat travels with it: a `Parallel.For` over the CPU path would close much of that gap, and
bench/README.md's GPU gate's baseline does not ask for one. A reader comparing against a parallel CPU
implementation should expect a smaller number.

```bash
dotnet run -c Release --project bench/Lodestar.Gpu.Benchmarks -- --filter '*BitParallel*' --job short
```

The parallelism is **across pairs, not inside one**. There is nothing left to widen within a
comparison, so the kernel runs one whole distance per thread in registers and wins — if it wins —
only by running many at once.

Three rows, as the other two kernels have:

- `CpuBaseline` — one `Levenshtein.Distance` call per string. The gate's baseline.
- `GpuResident` — the gate row, with the batch already renamed and resident. The equality table
  upload and the distance read-back are inside the measurement.
- `GpuFromHost` — not the gate. It prices the renaming a resident batch avoids, and on **this**
  kernel that is the half likely to decide the answer: renaming is a pass over every character,
  on the host, in the language the baseline is already written in.

A seeded corpus (`Random(4444)`) over a 26-letter alphabet, a 24-character pattern, at 10 000 and
200 000 strings by 32 and 256 characters. Text length is a parameter because Myers costs one word
per character of *text* regardless of pattern length, so it is the axis that moves both sides
together — and the one where the accelerator's fixed costs are amortised or are not.

**A failed gate is published rather than deleted.** A kernel that reaches 2× is a row in
[`docs/guides/performance.md`](../docs/guides/performance.md) and a kernel that does not ship,
which tells the next reader more than an absence would.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md) —
this section documents how to measure, not what was measured.

## 25. What residency buys across two operations (issue #444, kernel 4)

the GPU gate deferred the chainable device-resident types until three kernels existed, on the
ground that **chainability is a claim about two operations sharing a residency** and cannot be
measured with one. Three exist, so here it is priced.

```bash
dotnet run -c Release --project bench/Lodestar.Gpu.Benchmarks -- --filter '*ChainedProduct*' --job short
```

**`RoundTripped` is the baseline, not `CpuBaseline`, and that is deliberate.** The question this
section asks is not whether an accelerator beats a processor — section 24's kernel already asks
that one — it is what a caller loses by letting an intermediate cross the bus. Putting the
round-tripped version in the denominator makes the ratio read as *what residency is worth*
directly, with no subtraction.

| row | what it does |
| --- | --- |
| `RoundTripped` | two sparse-dense products, with a download and a re-upload between them |
| `Chained` | the same two, the intermediate never leaving the accelerator |
| `CpuBaseline` | `CsrMatrix.Multiply` composed twice, for the absolute scale the ratio sits on |

`CpuBaseline` is there so the ratio cannot be read in a vacuum. A chain twice as fast as a round
trip is worth nothing if both are slower than the CPU path, and that is a reading only the third
row makes possible.

The intermediate is what the parameters are chosen to size: a 4 000-column inner dimension by
`Width` ∈ {32, 128} puts 128 000 to 512 000 doubles — one to four megabytes — on the bus per step
that is not chained, and `Rows` ∈ {2 000, 20 000} moves the work without moving that transfer.

**This section first predicted the gap would be roughly flat in `Rows`, and the measurement
contradicts it: the gap shrinks, from 2.23× to 1.24× at `Width` 32.** The premise was right and
the conclusion did not follow from it. A fixed transfer against work that grows with `Rows` makes
the transfer a smaller *share* of the total, so the ratio must fall — flat was never what the
arithmetic predicted. The prediction in `Width` does hold: 2.23× to 2.94× at 2 000 rows, as a
transfer growing with the operand's width should.

What that means for a caller is the useful half: **residency is worth most where the work is
smallest**, which is the opposite of the intuition that a bigger job justifies more machinery.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md) —
this section documents how to measure, not what was measured.

## 26. MinHash signatures, and the half that turned out to dominate (issue #444, kernel 5)

`Lodestar.Text.Similarity.MinHash` does two things per document: hash each token, then minimise
each hash through every permutation. Only the second is parallel, so that is the half the kernel
took — and the split is why this section has two rows rather than one.

```bash
dotnet run -c Release --project bench/Lodestar.Gpu.Benchmarks -- --filter '*MinHashSignatures*' --job short
```

| row | what it measures |
| --- | --- |
| `CpuBaseline` | `MinHash.Signature` per document: hash and minimise in one pass |
| `GpuResident` | the minimisation alone, over hashes the host already holds |
| `GpuWithHashing` | the host's hashing pass *and* the upload *and* the minimisation |

**Read `GpuWithHashing`. It is what a caller starting from tokens pays, and it is the row that
matters.** Measured: the minimisation is **34× to 66×** faster on the accelerator, and end to end a
caller sees **1.27× to 1.57×** — because hashing is most of the work and it stays on the host. The
kernel clears bench/README.md's GPU gate's 5–10× gate on the part it took and **misses it on the part a caller
experiences.**

That is not a disappointing result, it is a located one. The obvious next move is to hash on the
accelerator too, and it is a different kernel rather than an extension of this one: parity requires
the first four bytes of SHA-1 little-endian, which is what `datasketch` exports as `sha1_hash32`, and
a SHA-1 implementation that agrees with it bit for bit is its own piece of work. Until that exists,
this kernel is worth using by a caller who **already holds hashes** — one who sketches the same
corpus under several permutation sets, for instance, where the hashing is paid once and the
minimisation many times.

A seeded corpus (`Random(4446)`), 24 tokens per document over a vocabulary of 5 000, at 5 000 and
50 000 documents by 64 and 128 permutations. Permutation count is a parameter because it is also the
estimate's resolution — the two are chosen together, never separately — and because it is the axis
the kernel's own work scales on while the hashing does not.

Numbers are published in [`docs/guides/performance.md`](../docs/guides/performance.md) —
this section documents how to measure, not what was measured.

## 27. `Lodestar.Stats.Regression`'s generalized linear model against `Accord.Statistics` (issue #616)

[Decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
kept the GLM inside `Lodestar.Stats.Regression` rather than a new package, on the same reading
[decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) gave the
OLS half. Section 19 already resolves `Accord.Statistics` in this project for that half; the GLM
reaches the same incumbent through a different corner of its surface —
`GeneralizedLinearRegression` fitted by `IterativeReweightedLeastSquares`, the constructor-and-`Run`
pair rather than the `LogisticRegression`-typed generic overload, because a `LogitLinkFunction`
passed to the untyped constructor is what lets one benchmark class drive either family through the
same shape.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*GlmBenchmarks*' --job short
```

### What the pair does and does not compare

Both rows fit a `Binomial` model — a logit link — to convergence and read the first regressor's
Wald p-value off it: `GeneralizedLinearModel.Fit(...).PValues[1]` against
`regression.GetWaldTest(1).PValue`. Both are given the same 100-iteration, `1e-8` budget
`GlmOptions`'s own defaults use, so neither side is favoured by a looser convergence criterion.

**`Accord.Statistics` 3.8.0's own recommended replacement for the API this benchmark drives,
`IterativeReweightedLeastSquares.Learn(x, y)`, does not terminate within any bound this project
could measure on the corpus below** — observed directly by running it, not assumed from its
signature. The constructor-and-`Run` pair this benchmark uses instead accepts an explicit
iteration budget and is what `Accord.Statistics`' own `LogisticRegression`-typed generic overload
does not expose for an arbitrary link function, which is why the untyped
`GeneralizedLinearRegression` is the one benchmarked rather than the typed one section 19 loads for
its own comparison — `IterativeReweightedLeastSquares.Learn` on the typed class does terminate, but
only fits the logit link.

The shapes differ on purpose, the same way section 19 records for the OLS pair:
`Lodestar.Stats.Regression` takes a row-major `ReadOnlySpan<double>`, where Accord takes
`double[][]`, one array per row — an allocation the design generation pays once per row that
`[MemoryDiagnoser]` reports rather than hides.

A seeded logistic design (`Random(616)`), at 200 and 2 000 rows by 1 and 3 regressors — enough
rows for the table in section 19's own reading to be worth reading, and few enough regressors that
neither side pays for a design this package's own test corpus does not also exercise.

### Agreement, and why the timed budget is not the agreement budget

The two stop on different quantities. `GeneralizedLinearModel.Fit` stops when the absolute change in
deviance falls to its `Tolerance`, which is statsmodels' `atol` with `rtol` at zero. `Accord`'s `Run`
returns the largest relative change in the coefficients, and the loop above stops on that. At the
shared `1e-8` the coefficients agree within `1.4e-10` relative on all four designs. The standard
errors and p-values do not: each side takes its covariance from the IRLS weights of its own last
iterate. So agreement is checked once, outside `BenchmarkDotNet`, with this package at `Tolerance =
1e-13` and `Accord` looping to `1e-15`, where every coefficient, standard error and p-value agrees
within `1e-9`. The timed rows keep the shared `1e-8`, so neither side is handed a looser budget.

The numbers, on a named machine and with the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Stats.Regression/performance.md#lodestarstatsregressions-generalized-linear-model-against-accordstatistics-issue-678).

`GlmPoissonBenchmarks` fits the Poisson family alone, one regressor over 2,000 rows, at a mean count
of 5, 50,000 and 5,000,000: the one axis the log-likelihood's `log(y!)` sees, on its table below 256,
on Stirling's series above it, and past the million the fit refused until #665. It has no incumbent.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*GlmPoissonBenchmarks*'
```

## 28. `Lodestar.Stats.TimeSeries`'s serial-correlation diagnostics against `Cortex.TimeSeries` (issue #617)

The autocorrelation function, the partial autocorrelation function and the Ljung-Box test ship in
`Lodestar.Stats.TimeSeries`, which
[decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
made a package of its own when the stationarity tests needed `Lodestar.Stats.Regression`; the
benchmark still lives in `bench/Lodestar.Stats.Benchmarks`. `Cortex.TimeSeries` 1.1.0 is the one .NET
library carrying the same three functions — `Cortex.TimeSeries.Diagnostics.AutocorrelationTests`'s
`ACF`, `PACF` and `LjungBox` — through its own `Cortex.ML` dependency, which is exactly the edge
[decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) bars from
`src/`: it goes in this project only, and `tools/check_nuspec_dependencies.py` is what fails the
build if that boundary is ever confused.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*SerialCorrelationBenchmarks*' --job short
```

### What the six-way pair compares, and what it does not

`Cortex.TimeSeries`'s own `ACF`/`PACF` return the bare sequence with no confidence band, and its
`LjungBox` returns a single statistic and p-value at the requested lag rather than a list cumulated
lag by lag. Each pair here is timed on the capability the two sides share — the sequence itself, or
the one-lag statistic — not on the band or the cumulative list only this package computes; a wider
comparison would be timing this package against nothing, since `Cortex.TimeSeries` has nothing on
the other side of it. Three pairs, six benchmarks: `LodestarAutocorrelation`/`CortexAutocorrelation`,
`LodestarPartialAutocorrelation`/`CortexPartialAutocorrelation`, and
`LodestarLjungBox`/`CortexLjungBox`, each read at the last of 20 lags on a seeded AR(1)-like series
(`Random(617)`, `value = 0.6 * value + random.NextDouble()`) at 200 and 2,000 points.

### Two of the three families agree closely; the partial one does not

Run once directly (not through `BenchmarkDotNet`, to separate correctness from timing) on the same
seeded series this benchmark generates:

| function | `Lodestar.Stats.TimeSeries`, n=200 | `Cortex.TimeSeries`, n=200 | `Lodestar.Stats.TimeSeries`, n=2,000 | `Cortex.TimeSeries`, n=2,000 |
| --- | --- | --- | --- | --- |
| ACF (lag 20) | -0.070256300056411 | -0.070256300056411 | 0.073705585471248 | 0.073705585471248 |
| PACF (lag 20) | -0.074734834234328 | -0.066452471794550 | 0.025064907419327 | 0.024797721466792 |
| Ljung-Box Q (lag 20) | 217.742254206699 | 217.742254206699 | 1138.997880702452 | 1138.997880702452 |

ACF and Ljung-Box agree to noise-level precision — the same shared reason section 18 already
records for the hypothesis tests, a second implementation converging on the same textbook
arithmetic. **The partial autocorrelation does not**, and the gap is not close to floating-point
noise at either size. [`SerialCorrelation.PartialAutocorrelation`](../docs/reference/stats-timeseries/correlation/serialcorrelation-partialautocorrelation.md)
documents that `ywadjusted` (also spelled `yw`, `ywa`, `yw_adjusted`) is the reference's own default
and the only method shipped, where the reference offers seven other estimators across four
families (`docs/equivalence.md`'s serial-correlation section) — `Cortex.TimeSeries`'s own
`PACF` does not publish which of those it solves, and this gap is consistent with it being a
different one. This is a correctness note for a reader comparing the two libraries' numbers
directly, not a defect: the oracle this package answers to is `statsmodels`, checked on the frozen
corpus in `tests/oracles/stats_timeseries.json`, and `Cortex.TimeSeries` was not checked against
that corpus.

### Configuration

`[Params(200, 2_000)]` on `SampleSize`, the same two sizes section 18 and section 27 sweep.
`LagCount` is fixed at 20 rather than parameterised — large enough that the Levinson-Durbin
recursion inside `PartialAutocorrelation` does real work at both sizes, and well under the
`n/2` ceiling `PartialAutocorrelation` enforces at `n = 200`. `[GlobalSetup]` builds an
AR(1)-like series from a fixed seed (617, this issue's own number) so every benchmark measures the
same input, and so the correctness table above is reproducible from the same seed.

The numbers, on a named machine and with the default job rather than `ShortRun`, are in
[`docs/guides/performance.md`](../src/Lodestar.Stats.TimeSeries/performance.md#lodestarstats-serial-correlation-diagnostics-against-cortextimeseries-issue-617).
They were taken from a checkout with sibling worktrees carrying the same benchmark project, which
`BenchmarkDotNet` 0.14.0 accepted.

## 29. The four robust covariances, against the ordinary one (issue #705)

[Decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
gave `OrdinaryLeastSquares` the `Hc0` to `Hc3` heteroskedasticity-consistent covariances.
`RobustCovarianceBenchmarks` prices them against the ordinary covariance on the same fit.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*RobustCovarianceBenchmarks*'
```

### Why it is its own class

It is not a parameter on section 19's `OlsBenchmarks`. `Accord.Statistics` has no robust mode, so
five covariance values there would multiply an Accord row that cannot move. This class has no
incumbent; `Nonrobust` is the baseline, and every row is the same `OrdinaryLeastSquares.Fit` call
with a different `OlsOptions.CovarianceType`.

### What the rows mean

A robust covariance adds a pass over the design building the filling `Xᵀ Ω X`, which is
`O(n·k²)`, two `k×k` products for the sandwich, and a `k×k` solve for the Wald statistic. `Hc2` and
`Hc3` also need the leverages, an `O(n·k)` pass over `Q`. It also moves the coefficient tests
from Student's t to the normal, so a robust row runs different tail functions from the baseline.
At small sizes that second difference is larger than the first, and
[`docs/guides/performance.md`](../src/Lodestar.Stats.Regression/performance.md#hac-and-cluster-robust-covariances-against-statsmodels-issue-775)
carries what each covariance costs beside the ordinary fit.

### Configuration

`[Params(100, 10_000)]` on `SampleSize`, the same two sizes section 19 sweeps, and the five
`CovarianceType` values. Four regressors and the seeded design `OlsBenchmarks` builds, so the
`Nonrobust` row reads beside section 19's. Each benchmark returns the first regressor's p-value.

Run it with the default job. `ShortRun` put `Nonrobust` at 10,000 rows at 1,727 μs ± 1,027 μs, an
error bar wider than the effect being measured.

## 30. The variance principal components explain, against NumFlat (issue #701)

[Decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
put `PrincipalComponentVariance` in `Lodestar.Decomposition`.
[Decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
read two PCA incumbents and found one reporting the same number: NumFlat 1.3.4, whose
`PrincipalComponentAnalysis.EigenValues` ships `net8.0` only. It is MIT-licensed and referenced by
`Lodestar.Text.Benchmarks` alone. ML.NET's `ProjectToPrincipalComponents` exposes no eigenvalue,
so it has no row here.
[Decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
found two more: Meta.Numerics 4.2.0 (MS-PL, `netstandard2.0`), measured in section 50, and the
commercial Numerics.NET, which is not measured under a trial licence.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*PrincipalComponentVarianceBenchmarks*'
```

### What the pair does and does not compare

`Lodestar_ExplainedVariance` calls `PrincipalComponentVariance.Compute` and reads the first
explained variance. `NumFlat_Pca` constructs `PrincipalComponentAnalysis` and reads its first
eigenvalue. **The asymmetry favours NumFlat's row**: its constructor also computes the
eigenvectors and the mean, which this package does not. Its input is a `Vec<double>[]`, one per
row, built once in `[GlobalSetup]`, so neither side pays for the other's layout inside the
measured call.

The two agree before they are timed. Run once outside `BenchmarkDotNet` on the same seeded blocks,
the largest difference anywhere in the spectrum was `1.1e-14` of the first eigenvalue.

### Configuration

`[Params]` on `Shape`: 200 × 10, 2,000 × 10, 2,000 × 50, and 100 × 200. The last is wide, so this
package solves the 100 × 100 Gram matrix of the rows rather than the 200 × 200 one of the columns.
Each column of the `Random(701)` block is scaled by its index, so the spectrum is spread rather
than flat.

The numbers, on a named machine and with the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Decomposition/performance.md#the-variance-principal-components-explain-against-numflat-issue-701).

## 31. The two published quantiles, and what they cost their callers (issue #709)

`docs/guides/performance.md`
replaced the bisection behind `Distributions.NormalQuantile` and `Distributions.StudentQuantile` with
a safeguarded Newton inversion. `QuantileBenchmarks` times one call of each, so the cost is read
directly rather than subtracted out of a larger benchmark.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*QuantileBenchmarks*'
```

### What the rows mean

`NormalQuantile` and `StudentQuantile` are the arguments a caller actually passes: `0.975`, and
`df = 95` for Student, the residual degrees of freedom of the 100-row fit in `OlsBenchmarks`.
`NormalQuantileFarTail` (`p = 1e-300`) and `StudentQuantileCauchyFarTail` (`p = 1e-12`, `df = 1`)
are where an iterative inverse is most likely to spend steps. None allocates.

**A quantile's cost follows its tail's.** Each call evaluates `Normal.Sf` or `StudentSf` one to four
times, so a change to the incomplete gamma or beta underneath moves these rows as well — the tail
is the thing to measure when they move.

### Measuring the callers with it

The change is only visible in a caller that takes a quantile, so a before/after run adds
`SerialCorrelationBenchmarks`' two autocorrelation pairs and `OlsBenchmarks.Lodestar_Ols`:

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- \
  --filter '*QuantileBenchmarks*' '*SerialCorrelationBenchmarks.*Autocorrelation*' '*OlsBenchmarks.Lodestar_Ols*'
```

## 32. What a Cox fit costs (issue #684)

[Decision 0003](../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
added `CoxProportionalHazards.Fit` to `Lodestar.Survival`. There is no incumbent to race, for the
reason section 20's `SurvivalBenchmarks` records:
[decision 0002](../docs/decisions/0002-provenance-and-the-allowed-references.md)
found no .NET survival package. `CoxBenchmarks` measures the shape of the cost instead.

```bash
dotnet run -c Release --project bench/Lodestar.Survival.Benchmarks -- --filter '*CoxBenchmarks*'
```

### What the rows mean

Each row is one whole fit: Newton-Raphson to convergence, the null log-likelihood, the table and
the concordance. Two terms compete.

- **The likelihood pass** is linear in the sample and quadratic in the covariates, once per
  iteration. It accumulates the risk set's second moments, a `p × p` sum per subject.
- **Harrell's concordance** is `n log n` in the sample and independent of the covariates. It walks
  the subjects in time order against a Fenwick tree of earlier events.

`[Params(1_000, 10_000)]` on `SampleSize` and `[Params(2, 8)]` on `Covariates` separate them. The
covariate axis moves the first term only. **A row that stops moving with the covariate count
means the concordance dominates again.** That is how the first draft's pairwise concordance showed
itself: 270 ms at 10,000 subjects for 2 covariates and for 8.

### Configuration

A seeded design (`Random(684)`) with covariates uniform on `[-1, 1]`, alternating log hazard ratios
of `+0.5` and `-0.5`, exponential durations rounded up to whole months so ties are ordinary, and
roughly a third censored, as `SurvivalBenchmarks` has it.

## 33. What BPE's piece cache buys a long-lived tokenizer (issue #743)

`BpeTokenizer` caches each piece's merged ids, as HuggingFace `tokenizers`' `BPE` does: up to 10,000
pieces shorter than 256 characters, never evicted. `BpeWordCacheBenchmarks` measures what that buys.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BpeWordCacheBenchmarks*'
```

### Why not `BpeBenchmarks`

Its corpus is random letter strings, 34,274 distinct words, so a cache filled with the first
10,000 is rarely hit by the rest. A fresh tokenizer over it gains nothing. `BpeBenchmarks` and the
`ByteLevelBpe` row of `TokenizerIncumbentBenchmarks` still read faster with the cache, and **that
gain is an artefact of repetition**: both build their tokenizer once, so from the second iteration
on, the cache holds the very words each iteration reads.

### The configuration that does not time its own warm-up

GPT-2's vendored vocabulary (`tests/oracles/gpt2_vocab.json`, `gpt2_merges.txt`) over the paragraphs
of decisions 0001 to 0100, split at blank lines. Decisions are immutable, so the text does not drift.
The first half warms the cache, the second half is timed.

The tokenizer is rebuilt and warmed in `[IterationSetup]`, before every iteration. The first half
does not fill the 10,000 entries, so a tokenizer warmed once would take the timed half's own pieces
into its cache on the first iteration and time a cache filled by the text it reads from then on.
`[IterationSetup]` sets one invocation per iteration, and at about 7 ms an encode BenchmarkDotNet warns
that the iteration is short. The warning stands: the measured standard deviation is what says whether
the row can be read, and `performance.md` gives it.

## 34. k-means against NumFlat and Meta.Numerics (issue #681)

`Lodestar.Cluster` ships `KMeans` alone. NumFlat 1.3.4 (MIT, `net8.0` only) ships k-means, k-medoids,
DBSCAN and two Gaussian mixtures; Meta.Numerics 4.2.0 (MS-PL, `netstandard2.0`) ships k-means as
`Multivariate.MeansClustering`. Both are referenced by `Lodestar.Text.Benchmarks` alone.
[Decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md) has the
reading and what the package does next.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*KMeans*Incumbent*'
```

### Two classes, because only one pair is like-for-like

**`KMeansLloydIncumbentBenchmarks` times Lloyd's iterations alone.** NumFlat's `KMeans.Update` runs
one iteration from given centroids, so `NumFlat_Lloyd` starts from the same centres as
`Lodestar_Lloyd` — the first `k` rows, one per blob — and runs exactly as many iterations as
`KMeans.Fit` did with `Tolerance = 0`, which stops when the labels settle. Neither side's
initialisation is inside the measured call. Meta.Numerics has no row: `MeansClustering` takes neither
starting centres nor a seed.

**`KMeansFitIncumbentBenchmarks` times what a caller pays**, k-means++ included, under each library's
own stopping rule: `KMeans.Fit` with `Seed = 681`, NumFlat's `KMeans` constructor with
`TryCount = 1` (its default is three; this package and scikit-learn's default start once), and
`MeansClustering`. **The rule is part of the price and is not the same rule.** This package stops on
scikit-learn's tolerance, scaled by the mean feature variance, and on these blobs that is one or two
iterations; neither incumbent exports how many it ran.

### The blocks, and the agreement checked before timing

`ClusterBlobs` draws `k` centres uniformly from `[-spread, spread]` per feature and unit-variance
Gaussian noise around them, row `i` from blob `i mod k`, seeded with 681. The Lloyd class uses
`spread = 4`, so the blobs overlap and the loop runs 101, 4 and 8 iterations on the three shapes;
the fit class uses `spread = 1000`, so any k-means++ draw finds the same partition.

`GlobalSetup` refuses to time anything unless the centres agree at `1e-9` — in order for the Lloyd
pair, matched to the nearest unused centre for the fit rows. Run once outside `BenchmarkDotNet`, the
largest difference on every shape and every pair was **exactly zero**: the same partition gives the
same means, summed in the same order.

The numbers, on a named machine and with the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Cluster/performance.md#k-means-against-numflat-and-metanumerics-issue-681).

## 35. Stationarity and seasonal decomposition against `Cortex.TimeSeries` (issue #671)

`Lodestar.Stats.TimeSeries`' augmented Dickey-Fuller test, KPSS and seasonal decomposition, against
`Cortex.TimeSeries` 1.1.0's `StationarityTests.AugmentedDickeyFuller`, `StationarityTests.KPSS` and
`SeasonalDecompose.Decompose` — the one free .NET package carrying all three, referenced by this project
only, as section 28 explains.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*StationarityBenchmarks*'
```

### What agrees, checked before timing

Run once outside `BenchmarkDotNet` against `statsmodels` 0.15.0 on a seeded 200-point AR(1) and a
96-point monthly series:

| function | agrees with `statsmodels`? | how |
| --- | --- | --- |
| ADF statistic, fixed lag 1 to 8 | yes, to `1e-14` | the same regression |
| ADF p-value | **no** | Cortex returns `0.01` where `statsmodels` gives `1.1e-9` to `9.5e-4`: a clamp, not MacKinnon's surface |
| ADF at `maxLags = 0` | **no** | Cortex uses 5 lags, where `statsmodels` fits none |
| KPSS, level | yes, statistic and p-value, exactly | Cortex's window is `floor(12·(n/100)^¼)`, 14 at n = 200, where `statsmodels`' legacy rule takes the ceiling, 15 |
| seasonal decomposition, additive and multiplicative | yes, to `1.1e-14`, `NaN` positions included | the same moving average |

So each pair is timed on the configuration where both return the same statistic: ADF at a fixed lag of
4, KPSS at the window Cortex chooses (fixed on this side in `[GlobalSetup]`), and the additive
decomposition at period 12. `GlobalSetup` refuses to time anything whose statistics differ by more than
`1e-9`.

### What the ADF pair does not compare

Both sides fit the same regression and read the same t statistic. This package's fit is
`Lodestar.Stats.Regression`'s Householder least squares reduced to the t statistics and the residual
sum of squares — `OrdinaryLeastSquares.Estimate`, not `OrdinaryLeastSquares.Fit`, whose
inference table and VIFs made the first measurement 2.5× to 2.8× slower than Cortex's. What stays
incomparable is the p-value: Cortex's is clamped at `0.01`.

### Configuration

`[Params(200, 2_000)]` on `SampleSize`, as section 28. `Random(671)`, an AR(1) at 0.5 on uniform
shocks, and a seasonal series built from it with period 12. The numbers, on a named machine and with
the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Stats.TimeSeries/performance.md#stationarity-and-seasonal-decomposition-against-cortextimeseries-issue-671).

## 36. Weighted least squares against Math.NET Numerics, and the least-squares pipeline under it (issue #782)

`WeightedLeastSquares.Fit` against `WeightedRegression.Weighted` from Math.NET Numerics 5.0.0 — MIT, maintained, and
the one numerics library `src/` already reaches through `Lodestar.Extensions.MathNet` — and `OrdinaryLeastSquares.Fit`
against Math.NET's `MultipleRegression.QR`, beside the Accord row section 18 already carries.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*WeightedLeastSquaresBenchmarks*' '*OlsBenchmarks*'
```

### What the rows mean

Math.NET returns the coefficient vector and nothing else; Lodestar's row is the whole `OlsSummary` table. `[GlobalSetup]`
refuses to time either side if their slopes differ by more than `1e-9` relative, NaN included, so a faster row cannot be
a different answer. The weighted design is four uniform regressors whose noise grows with the signal, weighted by the
inverse of that variance.

### Why the pipeline moved in a benchmark pull request

The first measurement, of the code `#774` shipped, had Lodestar 3.0× to 5.2× slower than Math.NET. Profiled, a
third of a weighted fit was forming Q explicitly for a solve that reads only R, and a quarter a second QR, again with Q
formed, for the variance inflation factors. `docs/guides/performance.md` has the measurements of each change; the
pipeline they describe is shared by `OrdinaryLeastSquares`, `WeightedLeastSquares` and the IRLS loop of
`GeneralizedLinearModel`, so `RobustCovarianceBenchmarks`, `GlmBenchmarks` and `GlmPoissonBenchmarks` are re-run beside it.

### Configuration

`[Params(200, 2_000, 20_000, 200_000)]` for WLS and `OlsBenchmarks`' own `[Params(100, 10_000)]`. `Random(768)` and
`Random(566)`. The numbers, on a named machine and with the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Stats.Regression/performance.md#the-least-squares-pipeline-against-mathnet-numerics-issue-782).

## 37. The negative binomial GLM against `statsmodels` (issue #781)

No free .NET library fits a negative binomial GLM at parity: Accord.Statistics' `GeneralizedLinearRegression` weights
its IRLS by the link's derivative alone, which is the right weight only for a canonical link, and the negative
binomial's log link is not one. The commercial libraries that do are not measured under a trial licence. So the
incumbent is `statsmodels` 0.15.0, through the cross-language harness section 21 describes, over the same corpus:
`generate_stats.py` adds a count response drawn as a gamma-mixed Poisson with `α = 1`, on a stream of its own so the
existing draws do not move.

```bash
python bench/corpus/generate_stats.py
python bench/python/bench_stats.py             # also writes python-glm.json
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-glm
python bench/compare.py glm
```

### What agrees, checked before timing

Both sides fit `GLM(counts, add_constant(design), family=NegativeBinomial(alpha=1)).fit()` and its summary quantities.
Run once outside the harness, the coefficients agree to `3.8e-15`, `1.1e-14` and `1.3e-14` relative at 1 000, 10 000
and 100 000 rows, with the same iteration counts (7, 6, 6). As section 21 says, read the `cpu` column: `statsmodels`
reaches LAPACK through numpy, which is threaded.

## 38. Generalized least squares against Math.NET Numerics (issue #771)

`GeneralizedLeastSquares.Fit` against the GLS a Math.NET Numerics 5.0.0 user writes, since Math.NET exports
none: `Matrix.Cholesky()` of the error covariance, `Solve` for `Σ⁻¹X` and `Σ⁻¹y`, and the normal equations
`(XᵀΣ⁻¹X)⁻¹XᵀΣ⁻¹y`. Math.NET is MIT, maintained, and the one numerics library `src/` already reaches through
`Lodestar.Extensions.MathNet`; the benchmark project references it directly.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*GlsBenchmarks*'
```

### What the rows mean

Lodestar's row is the whole `OlsSummary` table; Math.NET's is the coefficient vector alone, which is all it
offers. `[GlobalSetup]` refuses to time either side if their slopes differ by more than `1e-9` relative, so a
faster row cannot be a different answer. `Numerics.NET` and Extreme Optimization export GLS and are commercial
(decision 0004); they are not measured here.

### Configuration

`[Params(50, 200, 500, 1_000)]` on `SampleSize`, which is also the covariance's order, so the cost grows with
its cube. `Random(771)`, four uniform regressors, AR(1) errors at 0.6 and the matching covariance
`0.6^|i−j|`. The numbers, on a named machine and with the default job, are in
[`docs/guides/performance.md`](../src/Lodestar.Stats.Regression/performance.md#generalized-least-squares-against-mathnet-numerics-issue-771).

## 39. The Gamma GLM against `statsmodels` (issue #770)

The same harness as section 37, the same corpus and command: `generate_stats.py` adds a positive response, a shape-4
Gamma draw around a log-linear mean on a stream of its own, and `compare-glm` fits it through `GlmFamily.Gamma` with
`GlmLink.Log` beside the negative binomial row. The log link, not the default inverse, because nothing keeps an inverse-link
mean positive on a random design, and a fit this refuses is not a timing. No free .NET library fits the Gamma family:
Accord's `GeneralizedLinearRegression` returned NaN coefficients on the inverse link, and the commercial libraries are not
timed under a trial licence.

### What agrees, checked before timing

Run once outside the harness, the coefficients agree to `2.2e-15`, `1.7e-15` and `1.8e-14` relative at 1 000, 10 000
and 100 000 rows, the Pearson scale to `2.2e-14` or better, with the same iteration counts (10, 8, 8).

### The families already shipped, re-run beside it

`GlmFamily.Gamma` threads the link and the scale through the IRLS loop every family shares, so `GlmBenchmarks` and
`GlmPoissonBenchmarks` are re-run against `main`; `docs/guides/performance.md` has both, and the regression the first run
found.

## 40. HAC and cluster-robust covariances against `statsmodels` (issue #775)

`compare-ols` gains two rows over the same corpus as `ols_summary_*`: `ols_hac_*` fits `CovarianceType.Hac` with four
lags, and `ols_cluster_*` fits `CovarianceType.Cluster` over clusters of 20 consecutive rows. Both sides derive the lags
and the labels from the row index (`HAC_LAGS` and `CLUSTER_SIZE` in `bench_stats.py`, their C# twins in
`StatsCrossLang`), so the corpus does not change. statsmodels leaves the VIFs outside `.fit()`, so its two rows price
them inside the same call. `bench/compare.py`'s fold only maps `ols_vif_*` into `ols_summary_*`. No free .NET library
computes either covariance, so there is no .NET row.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-ols
python3 bench/python/bench_stats.py
python3 bench/compare.py ols
```

`HacClusterBenchmarks` prices the allocations the harness does not see: the ordinary fit, HC0, HAC and cluster over 100
and 10 000 rows, same seed and shape as `OlsBenchmarks`. It is a class of its own rather than two more values on
`RobustCovarianceBenchmarks`, so that class's source is the same on `main` and on a branch, and an A/B/A of it prices
the shared sandwich alone.

### What agrees, checked before timing

Run once outside the harness on the benchmark corpus, every standard error agrees with statsmodels to `3e-13` relative
or better at 1 000, 10 000 and 100 000 rows, under both covariances. `HacClusterBenchmarks`' setup refuses to time a HAC
with no lags that is not HC0 to `1e-12`, a comparison written to fail on `NaN`.

**Read `fvalue` before `f_pvalue`.** On a cluster fit, statsmodels 0.15.0's `f_pvalue` read first uses `n - k`
denominator degrees of freedom, where `fvalue` caches the `G - 1` p-value of its own `f_test`. `bench_stats.py` reads
them in that order, as the oracle generator does.

## 41. GLM offsets and exposure against `statsmodels` (issue #787)

`compare-glm` gains `glm_poisson_exposure_*`, a Poisson fit of the corpus's counts with an exposure. Both sides
derive the exposure from the row index as `1 + row % 3` (`EXPOSURE_CYCLE` in `bench_stats.py`, `ExposureCycle` in
`StatsCrossLang`). A cycle of four put the 100,000-row fit's last deviance change at 1.2e-8 against the 1e-8
tolerance. At that point rounding decides the iteration count, and the two sides would time 5 and 6 iterations.
Accord's GLM takes no offset (its `Offsets` belong to `ProportionalHazards`), so there is no .NET row.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-glm
python3 bench/python/bench_stats.py
python3 bench/compare.py glm
```

`GlmOffsetBenchmarks` prices a Poisson fit with and without an exposure, with its allocations. It is a class of its
own so `GlmPoissonBenchmarks` keeps its `main` source, and an A/B/A of that class and `GlmBenchmarks` prices the IRLS
loop every fit shares.

### What agrees, checked before timing

Run once outside the harness on the benchmark corpus, the standard errors agree with `statsmodels` to `3.7e-15` and
the null deviance to `2.0e-13`, with 5 iterations on both sides at 1,000, 10,000 and 100,000 rows.
`GlmOffsetBenchmarks`' setup refuses to time an exposure of ones whose slope is not the unexposed fit's to `1e-12`.

## 42. The multinomial logit against Accord and `statsmodels` (issue #788)

`MultinomialLogitBenchmarks` races `MultinomialLogit.Fit` against Accord.Statistics 3.8.0's
`MultinomialLogisticRegression`, learned by `LowerBoundNewtonRaphson`: 200 and 2,000 rows, three regressors,
three categories. Accord is LGPL-2.1 and archived, so it is raced rather than delegated to (decision 0004).

`compare-glm` gains `mnlogit_*` over the stats corpus. Both sides take the category as the count response modulo three
(`MNLOGIT_CATEGORIES` in `bench_stats.py`, `MultinomialCategories` in `StatsCrossLang`), and each prices the whole
table. The null log-likelihood inside that table is a Nelder–Mead and BFGS refit in `statsmodels` and a closed form
here (decision 0004). That difference in work is part of what the row measures, because it is part of what each
library computes to report the table.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*MultinomialLogitBenchmarks*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-glm
python3 bench/python/bench_stats.py
python3 bench/compare.py glm
```

### What agrees, checked before timing

- **Against `statsmodels`:** run once outside the harness on the benchmark corpus, the standard errors agree to
  `1.8e-13` or better at 1,000, 10,000 and 100,000 rows, with 5 Newton iterations on both sides.
- **Against Accord:** its tolerance is set to `1e-10`, where its coefficients agree with this fit's to `8.4e-10`, and
  the class's setup refuses to time a gap above `1e-8`. **Accord's standard errors are not compared**: they read the
  lower-bound Hessian its algorithm iterates on, and measured 33% to 43% from the ones this fit and `statsmodels`
  report.

## 43. The vector autoregression against `statsmodels` (issue #786)

A harness of its own, `compare-var`, over the same corpus as `compare-ols`: both sides read the design's first two
columns as a two-variable series and fit a VAR(2) — `VAR_VARIABLES` and `VAR_LAGS` in `bench_stats.py`, their C# twins
in `StatsCrossLang`. Each row prices the whole table: the coefficients and their errors, both residual covariances,
the log-likelihood and the four criteria. No .NET library estimates a VAR
([decision 0004](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0004-what-is-written-here-and-what-is-delegated.md),
re-read for this lot over eight packages), so there is no .NET row.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-var
python3 bench/python/bench_stats.py
python3 bench/compare.py var
```

`VectorAutoregressionBenchmarks` prices the allocations the harness does not see, over both axes that move the work:
500 and 5,000 observations, two and five variables, one and four lags. The design is `1 + K·p` columns wide and each
equation is its own least squares, so the two parameters multiply.

### What agrees, checked before timing

Run once outside the harness on the benchmark corpus, the standard errors agree with `statsmodels` to `7.5e-15` and
the Akaike criterion to `1.6e-15`, at 1,000, 10,000 and 100,000 rows.

## 44. The splitters against ML.NET and `scikit-learn` (issue #762)

`SplitterIncumbentBenchmarks` races `Splitters` against ML.NET 5.0.0's `CrossValidationSplit` and
`TrainTestSplit`, the only splitters in .NET, at 10,000 and 100,000 rows and five folds.

**The two sides cannot be made to agree, and that is the finding rather than a caveat.** ML.NET
splits an `IDataView` by hashing a generated sampling key, so its folds are not reproducible from
outside and it never stratifies at all
([dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396), open since
2019). There is no agreement check before timing here because there is nothing to check: what these
rows price is the same intent, not the same answer. Agreement is proven elsewhere and exactly —
`tests/oracles/preprocessing_splitters.json` replays these folds index for index against
`scikit-learn`.

**ML.NET's split is lazy, so it has to be measured twice.** `CrossValidationSplit` returns data views
that filter rows when they are enumerated, so the call alone measures the construction of ten
wrappers — flat in the row count, which is the tell. The `_Read` rows call it and then read which
rows each fold holds, which is what a caller needs before fitting anything. The `IDataView` is built
in `GlobalSetup` and excluded from every measurement, so ML.NET is not charged for its own entry
cost here.

`compare-splitters` puts the same three splitters against `scikit-learn` 1.9.0 at 10,000, 100,000 and
1,000,000 rows. No corpus file: a splitter's whole input is a row count and a label per row, so both
sides build the labels from the row index by the same rule — three classes at 60 / 30 / 10, skewed
rather than balanced because that is where a stratified splitter does work a plain one does not.
`scikit-learn`'s `split()` is a generator and is drained into a list, since yielding a fold and
building one are not the same work; its `X` is a dummy column allocated outside the timed region,
because it takes the matrix only to read `len(X)` from it.

Issue #1157 added eight operations to both sides: the seeded `KFold`, `StratifiedKFold` and
train/test split, the stratified train/test split, `GroupKFold`, `StratifiedGroupKFold`,
`TimeSeriesSplit` and `RepeatedKFold` at three repeats. Every seed is 1157 on both sides, and groups
are runs of 100 rows (`arange(n) // 100`), so the group count grows with the data: 100, 1,000 and
10,000. Agreement is again the corpus's job, `tests/oracles/preprocessing_splitters_seeded.json`,
not the harness's.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*SplitterIncumbent*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-splitters
python3 bench/python/bench_splitters.py
python3 bench/compare.py splitters
```

### What moved while this was measured

`TrainTest` sorted both index halves and filled an order array even when the read order was the
identity, where the halves come out ascending on their own. Two steps, each measured at a million
rows: dropping the sort on an identity read took **6.1 ms to 2.5 ms**, and writing the two halves
straight out — no order array, nothing to sort — took it to **0.5 ms**. A/B/A on the first step put
the sort back and measured **6.1 ms** again, which is what says the difference is the sort rather
than the machine: the 1,000,000-row rows carry about ±20% run-to-run spread here, enough to invent a
regression out of one reading, and `KFold` at a million rows read 10.0 ms once and 12.0 to 12.4 ms in
every run after, on code neither step touched.

## 45. The scalers against ML.NET's normalizers (issue #763)

`ScalerIncumbentBenchmarks` races [`MinMaxScaler`](../src/Lodestar.Preprocessing/MinMaxScaler.cs),
`MaxAbsScaler` and `RobustScaler` against ML.NET 5.0.0's `NormalizeMinMax` and
`NormalizeRobustScaling`, at 1,000 and 20,000 rows of ten features.

**`fixZero: false` is passed, and it matters.** ML.NET's `NormalizeMinMax` defaults to `fixZero:
true`, which scales around zero onto `[−1, 1]` rather than mapping the fitted minimum to the bottom
of a range — nearer this package's `MaxAbsScaler` than its `MinMaxScaler`, under the other one's
name. With `fixZero: false` the two compute the same transform, which `GlobalSetup` checks row by
row to `1e-6` (the tolerance is single precision because that is what ML.NET's pipeline carries)
before anything is timed.

**The estimator is lazy**, like the splitter of section 44, so it appears twice: `Fit` alone, and
`Fit` followed by reading the scaled values back — the only one of the two a caller can use.

**The job is pinned**, which no other class here does. These rows span 65 µs to 15 ms and each
allocates a fresh matrix; left to size itself the engine put 4,096 invocations in an iteration, each
warmup slower than the last as the collector fell behind, and `RunStrategy.Monitoring` reported a
standard deviation above half the mean. One invocation per iteration with a real warmup is what
measures this shape, and the published numbers hold to a few percent.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*ScalerIncumbent*'
```

### What the shapes say

`RobustScaler` costs about nine times `MinMaxScaler` and allocates twice as much, because a
percentile has to order the column: it sorts each feature once, where the other two take a single
pass. That is the price of the statistic rather than of this implementation — `numpy.percentile`
partitions instead of sorting, which is the same asymptotic work with smaller constants, and is the
obvious place to look if that row ever needs to be cheaper.

## 46. The encoders and the imputer against ML.NET (issue #764)

`EncoderIncumbentBenchmarks` races [`Encoders`](../src/Lodestar.Preprocessing/Encoders.cs) and
`SimpleImputer` against ML.NET 5.0.0's `OneHotEncoding` and `ReplaceMissingValues`, at 1,000 and
20,000 rows over twenty categories. The job is pinned as section 45's is, and for the same reason.

### The shape that had to be corrected before any number was published

**ML.NET's `OneHotEncoding` takes one named column**, where this package's encoders take a row-major
matrix of however many features. The first run of this class encoded **four** features here against
**one** there, which is four times the work for the same row: it reported this package 2.8× slower at
20,000 rows. Like for like — one categorical column on both sides — it is **1.47× faster**, and the
class now says so where the constants are declared.

`ReplaceMissingValues` does take a vector column, so the imputer rows compare four features against
four and need no such care.

**The estimator is lazy**, as section 45 found for the normalizers: `Fit` alone builds the mapping
and the work happens when the values are read. Both are reported, and the read is the one a caller
can use.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*EncoderIncumbent*'
```

### What the shapes say

`Encoders.Ordinal` costs about three quarters of `Encoders.OneHot` and allocates a **tenth** of it:
one column out against one per category. That ratio is the argument for the ordinal encoding and
against it at once — cheaper, and it hands a model numbers whose order means something the data did
not say, which
[`docs/reference/preprocessing/encoding/ordinalencoder.md`](../docs/reference/preprocessing/encoding/ordinalencoder.md)
states where a caller will read it.

## 47. Fitting over batches, and over a sparse matrix (issue #765)

`PartialFitBenchmarks` prices the two paths #765 adds against the whole-matrix fit they stand in
for: ten features, 10,000 and 100,000 rows, cut into 10 and 100 batches; and a `CsrMatrix` with one
value in ten stored, against the same data densely.

**No foreign incumbent.** ML.NET's normalizers have no incremental entry point, and its sparse
support lives inside the pipeline rather than in a matrix a caller holds. These rows compare this
package against itself, which is what the question actually is: whether the batched and sparse paths
cost what they should.

### What the first run found, before the numbers meant anything

The batched fit measured **twice as fast** as the whole one, which no arithmetic justifies — both
visit every value. The cause was in this package, not in the benchmark: `StandardScaler.Fit` sums
with Neumaier compensation and the incremental update did not, following the reference's plainer
form. **Two paths of one class disagreeing about how careful they are is worse than either choice**,
so the incremental path now compensates too; it costs about half the gap, and the ratio it leaves is
one a reader can trust.

The corpus passed under both versions — the difference lives below the 1e-9 it compares at — so
reading the benchmark is what caught it.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*PartialFitBenchmarks*'
```

## 48. DBSCAN against NumFlat and `Dbscan` (issue #759)

Two classes, because the two incumbents do not reach the same data.
`DbscanIncumbentBenchmarks` is planar — 5,000 and 20,000 points, where all three can compete —
and `DbscanDimensionBenchmarks` is the same measurement at eight and sixteen features, where
`Dbscan` 3.0.0 has no entry point at all: its `Point` carries `X` and `Y` and nothing else, read
from the artefact and not from its README. Both share `DbscanBlock`, so neither restates the
fixture the other uses.

**The radius is measured per shape, not chosen once.** The blobs are unit-variance Gaussian, so the
distance to a fifth neighbour grows with the dimension: 1 recovers every blob at two features,
finds nothing at eight until 2, and needs 4 at sixteen. A radius that split a blob or merged two
would time a different problem on each side of the table.

### What the agreement check found

Section 15's rule is that both sides are checked to return the same answer before either is timed.
Here the check is on the *partition* rather than on the labels, because cluster numbers are
arbitrary across libraries and the partition is not — and it refused a shape:

**NumFlat 1.3.4 loses a border sample that the ascending scan reaches before any cluster exists.**
The minimal case is nine points in one dimension, and it was already in this repository's own
corpus as `the same border point, listed first`:

| | labels |
| --- | --- |
| points, `eps=1.0`, `min_samples=4` | `1.3, 0.0, 0.1, 0.2, 0.3, 2.3, 2.4, 2.5, 2.6` |
| scikit-learn 1.9.0 | `0, 0, 0, 0, 0, 1, 1, 1, 1` |
| `Lodestar.Cluster` | `0, 0, 0, 0, 0, 1, 1, 1, 1` |
| NumFlat 1.3.4 | `-1, 0, 0, 0, 0, 1, 1, 1, 1` |

The point at `1.3` has three neighbours and needs four, so it is not a core sample. Listed first,
the scan reaches it before any cluster has been grown and NumFlat marks it noise permanently; the
original algorithm, and scikit-learn, let a later expansion take it as a border sample. The other
three orderings of the same nine points agree, because there the expansion claims it before the
scan arrives — which is why one ordering would have hidden this and four did not.

At scale the same defect showed as 1,017 differing rows of 5,000 at sixteen features and a radius
of 3, where 69% of the matrix is noise and border samples are everywhere; this package matched
scikit-learn on every one. The obvious alternative explanation was tested and refused: against
NumFlat's `minPoints=5`, this package's `minimumSamples=5` disagrees on 1,017 rows where `4` gives
1,276 and `6` gives 1,092 — the minimum sits at equal parameters, so the two count a neighbourhood
the same way.

Each shape in the table below is therefore measured at a radius that recovers its blobs cleanly,
where all three libraries return the same partition and the check passes.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Dbscan*Benchmarks*'
```

## 49. Agglomerative clustering against `Aglomera` (issue #760)

`AgglomerativeIncumbentBenchmarks` builds a whole merge tree, this package against `Aglomera` 1.1.1,
at 500 and 1,500 samples drawn from five Gaussian blobs in four dimensions, under each of the four
linkages. `Aglomera` is MIT by `licenseUrl` — the package carries no licence field of its own — and
`netstandard1.3`, and it has not been released since 2020.

### What was measured before anything was written

The issue asked for `Aglomera` to be compared with scikit-learn **before any code**, with delegating
to it as the explicit alternative. Five fixtures under four linkages, trees compared as sets of rows
in merge order:

| fixture | single | complete | average | ward |
| --- | --- | --- | --- | --- |
| three blobs, 30 points, 3-D, no ties | same | same | same | same |
| a line of five, one tie | reversed tie | reversed | reversed | reversed |
| duplicate rows | reversed | reversed | reversed | reversed |
| an evenly spaced line, every gap tied | **different tree** | reversed | reversed | reversed |
| a unit square, every side tied | **different tree** | **different tree** | **different tree** | **different tree** |

**Exact where no two merge heights tie; divergent under every linkage where they do** — it breaks
ties toward the higher index where the reference breaks them toward the lower, and on a square a
different first merge is a different tree. **Its Ward height is also on another scale**: the rise in
the sum of squares, `d²/2` where the reference reports `d`. So delegation does not give parity, and
this benchmark runs on Gaussian blobs, where no heights tie and the two agree.

### The agreement check

Node ids are arbitrary across libraries, so `AgglomerativeAgreement` compares what does not depend
on them: every merge height in merge order — Ward's after undoing the `d²/2` — and the partition
at the cut, numbered by first appearance on both sides. A dry run exercised it on all eight
combinations of size and linkage before any timing was taken.

### What the reference taught about its own ties

Parity with ties meant reproducing the reference's floating-point order and not only its algebra.
scipy updates Ward by multiplying through by `t = 1/(nx + ny + ni)`; the equal form that divides
once at the end gave **a different tree on 16 of 400 random integer datasets**, and a set of
hand-built tie fixtures passed under it. That is why `cluster_agglomerative.json` carries random
integer data as well as the fixtures above.

### The numbers

`--job short` on the machine `docs/guides/performance.md` names, with nothing else running. **A first
run was discarded**: the oracle generator ran beside it for a minute or two, which `.dotnet-guarded`
does not prevent because it is not `dotnet`, and its error intervals reached 92% of the mean. The
rerun below has a standard deviation within 3.5% of the mean on every row.

| samples | linkage | Lodestar | `Aglomera` 1.1.1 | ratio | allocated, Lodestar | allocated, `Aglomera` |
| ---: | --- | ---: | ---: | ---: | ---: | ---: |
| 500 | ward | 2.03 ms | 48.91 ms | **24.14** | 1.04 MB | 64.19 MB |
| 500 | complete | 1.98 ms | 61.70 ms | **31.21** | 1.04 MB | 83.11 MB |
| 500 | average | 1.92 ms | 56.28 ms | **29.37** | 1.04 MB | 59.88 MB |
| 500 | single | 0.45 ms | 87.45 ms | **194.69** | 0.06 MB | 81.29 MB |
| 1,500 | ward | 17.44 ms | 1,110.66 ms | **63.70** | 8.97 MB | 573.62 MB |
| 1,500 | complete | 16.98 ms | 1,209.18 ms | **71.23** | 8.97 MB | 747.36 MB |
| 1,500 | average | 17.28 ms | 1,224.95 ms | **70.91** | 8.97 MB | 537.15 MB |
| 1,500 | single | 3.53 ms | 2,081.80 ms | **589.97** | 0.18 MB | 732.69 MB |

**The gap widens with the sample count**: 24× to 31× at 500 becomes 64× to 71× at 1,500 for the three
nearest-neighbour-chain linkages, which is quadratic time against `Aglomera`'s cubic growth.
**Single linkage is the extreme on both axes**: its spanning tree computes each distance when it
needs one and holds no matrix, 185 KB at 1,500 samples where `Aglomera` allocates 733 MB.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*AgglomerativeIncumbent*'
```

## 50. Meta.Numerics against `Lodestar.Stats` and `PrincipalComponentVariance` (issue #756)

`MetaNumericsStatsBenchmarks` races eight test families against `Meta.Numerics` 4.2.0, and
`MetaNumericsPcaBenchmarks` races the explained variance. Meta.Numerics is **MS-PL, maintained, and
ships `netstandard2.0`**, where `Lodestar.Stats`' only other incumbent is archived and
`PrincipalComponentVariance`'s only other one is `net8.0`-only — so below `net8.0` this is the only
second opinion there is, which decision 0004 read and this measures.

### The pairing that would otherwise time two different statistics

`Univariate.StudentTTest(a, b)` is the **pooled-variance** two-sample t, so the race passes
`Variance.Equal` rather than `TTest.Independent`'s Welch default — the same care `StatsBenchmarks`
takes with Accord's Yates correction.

### What agrees, checked before timing

`GlobalSetup` runs every pair once outside the timed region. **A statistic that disagrees stops the
run**: it means the two libraries are computing different quantities, and a row would say nothing. A
**p-value that disagrees is printed instead**, because the libraries deliberately differ there. At
n=100 the run records six:

| pair | this package | Meta.Numerics | why |
| --- | ---: | ---: | --- |
| chi-square statistic | 7.91919 | 9.09091 | **Yates**: `ChiSquare.Contingency` applies the continuity correction by default and `PearsonChiSquaredTest` does not |
| chi-square p | 0.00489131 | 0.00256883 | the same correction, read through the distribution |
| Mann-Whitney p | 0.0113229 | 0.0112835 | continuity correction; **the statistic agrees exactly** |
| Kolmogorov-Smirnov p | 0.111195 | 0.111133 | **exact against asymptotic** — `ExactMethod.Auto` takes the exact branch at 100×100; the statistic agrees |
| Wilcoxon statistic | 1835.5 | 2543 | a different signed-rank convention, not a different answer |
| Wilcoxon p | 0.879671 | 0.950651 | follows from the statistic above |

The Student t, Kruskal-Wallis, one-way ANOVA and Kolmogorov-Smirnov **statistics** agree to `1e-9`
relative, and the Fisher exact p-values agree exactly.

### The wide matrix Meta.Numerics refuses

`Multivariate.PrincipalComponentAnalysis` raises `InsufficientDataException` on a matrix with more
columns than rows — 100 rows by 200 features, which `PrincipalComponentVariance` and NumFlat both
answer. That is why the Meta.Numerics PCA row is its own class over the three shapes it accepts,
rather than a third row in `PrincipalComponentVarianceBenchmarks`. It also takes the **columns**
where the other two take the rows; the transposition is done in `GlobalSetup` so the row is not
charged for it.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*MetaNumericsStats*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*MetaNumericsPca*'
```

### What the comparison changed in this package

Meta.Numerics was ahead on two of the ten rows and behind on the rest, which is the shape of a cost
here rather than of a difference in what the two compute. Both were fixed inside this lot, A/B/A
under the machine lock:

| what changed | A | B | A again |
| --- | ---: | ---: | ---: |
| `FisherExact.Test`, the 2×2 table above | 7,420 ns | **256 ns** | 7,487 ns |
| `KolmogorovSmirnov.TwoSample`, n = m = 100 | 29,152 ns | **1,544 ns** | 29,020 ns |

**Fisher** took nine log-gammas per candidate table and recomputed the loop-invariant denominator
every iteration; it now walks the hypergeometric recurrence from the mode — one exponential, then
multiplications — at zero allocation. **Kolmogorov-Smirnov** built the `(n+1)×(m+1)` table even for
two samples of the same size, where Hodges' closed form answers in O(n); allocation fell from
86,512 B to 1,610 B.

**No default moved and nothing became an approximation.** `ExactMethod.Auto` still chooses what it
chose before — measured against `scipy` 1.18.1, whose own `method="auto"` takes the exact branch at
n = 100, 1,000 and 10,000, so a default that switched to asymptotic would have walked away from the
parity this package exists for. `stats_ks.json` gains an equal-size pair of 100 and a
100-against-99 pair, the second of which must take the table rather than the closed form; both
replay at 1e-9.

## 51. BERT's basic tokenization, over three flavours of text (issue #1048)

`BertNormalizerBenchmarks` measures the `vocab.txt` route — `BertNormalizer` then
`BertPreTokenizer` ahead of WordPiece, which is what [`0005`](../docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)
turned on — over the 5,000 corpus documents, with `vocab_30k.txt` loaded twice, cased and uncased.

**Three flavours, because the normalizer has three paths and the corpus only exercises one.** The
documents as generated are lowercase ASCII, which the normalizer does not change; `Accented`
replaces every `e` with `é`, which is what makes an uncased model decompose and strip; `Cjk`
replaces every `a` with `中`, which is what makes it pad. Derived in `[GlobalSetup]` from the same
documents rather than generated as a corpus of their own, so the three rows differ in one character
class and nothing else.

**No foreign incumbent.** `Microsoft.ML.Tokenizers`' WordPiece has no `BertNormalizer` in front of
it — section 15 times that pairing, which is the comparable one. These six rows compare this
package against itself, which is the question #1048 asked: whether the normalizer costs what it
should on the case it was written for.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BertNormalizerBenchmarks*'
```

## 52. The code-point scorers' alphabet, on repetitive and near-unique astral text (issues #1056, #1057)

`FuzzCodePointBenchmarks` times the `TextElement.CodePoint` scorers on the four shapes the delta
review measured: two 10,000-emoji strings holding eight distinct code points, 2,000 emoji words,
10,000 characters of French prose plus one emoji, and 10,000 distinct astral scalars per side. A
fifth row scores `WRatio` against an empty operand. Every row builds its text in `[GlobalSetup]`, so
the class needs no corpus.

**No foreign incumbent.** No .NET library scores rapidfuzz's code-point mode, and the question is
this package against its own previous revision: whether the map is sized by the code points it
holds. **The allocation column is the result**; the time moves little, because the alphabet is a
small share of an Indel over 10,000 code points.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*FuzzCodePointBenchmarks*'
```

## 53. The three correlation tests against `Meta.Numerics` (issue #1120)

`CorrelationBenchmarks` times [`Pearson.Test`](../docs/reference/stats/tests/pearson-test.md),
[`Spearman.Test`](../docs/reference/stats/tests/spearman-test.md) and
[`KendallTau.Test`](../docs/reference/stats/tests/kendalltau-test.md) against
`Meta.Numerics` 4.2.0's `Bivariate.PearsonRTest`, `SpearmanRhoTest` and `KendallTauTest`, at 100
and 10,000 pairs. Section 18 already resolves `Meta.Numerics` in this project; the three names
were read out of the restored `netstandard2.0` asset by reflection rather than guessed, the same
protocol section 18 set, and all three take two `IReadOnlyList<double>`.

**The corpus is untied on purpose, and that is not a convenience.** `[GlobalSetup]` draws the
predictor from a continuous generator and the response as `2x + 0.3u`, so nothing is tied and no
corpus file is needed. Measured on an eight-point tied fixture, the two libraries do not compute
the same quantity once anything is tied:

| tied fixture | `Lodestar.Stats` | `Meta.Numerics` | why |
| --- | ---: | ---: | --- |
| rho | 0.9193 | 1 | it averages no tied rank |
| tau | 0.8573 (tau-b) | 0.5 | it computes tau-a, with no tie correction |

Timing those under one name would compare two different numbers, which is what section 18's
`MetaNumericsAgreement` exists to prevent. Untied, the three statistics agree to `1e-9` and so do
the p-values: the agreement check recorded **0 differences at both sizes**, where section 18's
eight families recorded several. `Setup` throws rather than times if that ever stops holding.

A seventh row, `Lodestar_KendallTau_Asymptotic`, pins `ExactMethod.Asymptotic` so the row times
the concordance count alone — `ExactMethod.Auto` builds the exact table at a hundred pairs and
not at ten thousand, which would be two different amounts of work under one name.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*CorrelationBenchmarks*'
```

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 54. The variance, proportion and fit tests against `Accord.Statistics` (issue #1121)

`VarianceAndFitBenchmarks` times [`Levene.Test`](../docs/reference/stats/tests/levene-test.md),
[`Bartlett.Test`](../docs/reference/stats/tests/bartlett-test.md),
[`Binomial.Test`](../docs/reference/stats/tests/binomial-test.md) and
[`AndersonDarling.Test`](../docs/reference/stats/tests/andersondarling-test.md) against
`Accord.Statistics.Testing` 3.8.0, at 100 and 10,000 values. Section 18 already resolves `Accord`
in this project; the four names and their constructor shapes were read out of the restored
`netstandard2.0` asset by reflection rather than guessed, the same protocol:

| family | `Accord.Statistics.Testing` |
| --- | --- |
| Levene | `new LeveneTest(double[][] samples, median: true)` |
| Bartlett | `new BartlettTest(double[][] samples)` |
| binomial | `new BinomialTest(int successes, int trials, double p, OneSampleHypothesis)` |
| Anderson-Darling | `new AndersonDarlingTest(double[] sample, IUnivariateDistribution<double>)` |

**Friedman has no row, because it has no incumbent.** Neither `Accord.Statistics` — 33 public
`*Test` types, none of them Friedman's — nor `Meta.Numerics` carries it, so there is no pair to
time and its comparison belongs to the cross-language harness rather than here.

**`Accord`'s Anderson-Darling is not quite the same test, and is made comparable on purpose.** It
takes the hypothesised distribution as an argument, where `scipy.stats.anderson` estimates the
mean and the spread from the sample; handing it a normal fitted to that same sample makes the two
statistics the same quantity, which `MetaNumericsAgreement` checks before anything is timed. They
agree.

**And at ten thousand values it refuses.** `AndersonDarlingTest`'s constructor converts the
statistic to a p-value eagerly, and that conversion throws
`InvalidOperationException: CCDF computation generated NaN values` from inside its own
distribution. The first run of this class lost all nine of its 10,000-value rows to it, because
the call sat in `[GlobalSetup]`. The check now catches the refusal and records it, so the other
three pairs stay measurable and the `Accord_AndersonDarling` row reports `NA` — which is the
finding, not a gap: Anderson-Darling is the normality test one keeps *because* it holds up on
large samples, where Shapiro-Wilk's own reference stops at 5,000.

**One recorded disagreement, on the binomial p-value.** At `k = 33`, `n = 100`, `p = 0.5` this
package and scipy both answer `0.00087372` — exactly twice the lower tail, the null being
symmetric — and `Accord` answers `0.000641249`, which is neither that nor the sum of the outcomes
no more likely than the observed one. What it computes is not stated in the package. The pair is
timed anyway, both sides computing an exact binomial two-sided p-value, and the difference is
printed rather than asserted.

A ninth row, `Lodestar_ClopperPearson`, has no counterpart at all: `Accord` exports no confidence
interval for a proportion. It measures the beta inversion `Internal.BetaQuantile` against nothing
but itself, which is what the interval costs on top of the test.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*VarianceAndFitBenchmarks*'
```

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 55. The feature transformers against ML.NET and `scikit-learn` (issue #1122)

`TransformerIncumbentBenchmarks` races the seven #1122 members against the two ML.NET 5.0.0
estimators that answer the same question, at 1,000 and 20,000 rows and four features.

**Only two of the seven have an incumbent at all, and that is the finding.** ML.NET's
`NormalizeLpNorm` scales a row to unit norm, as `Normalizer` does, and its `NormalizeBinning` cuts
a feature into bins — though it emits a position in `[0, 1]` rather than the bin, so the two price
the same traversal and not the same answer. For the other five — `PolynomialFeatures`,
`QuantileTransformer`, `PowerTransformer`, `KnnImputer` and `LabelEncoder` — there is nothing in
.NET to race, so their rows are Lodestar against itself and the comparison that matters is the
cross-language one below.

**Each family is timed as a fit and as a transform, because the two are not the same work.** The
fitted objects are built in `GlobalSetup` and excluded, so the transform rows price the transform;
the `_Fit` rows price the fit. `PowerTransformer`'s two differ by orders of magnitude — the fit
maximises a likelihood by Brent's method per feature, and the transform is one power per value.

**`KnnImputer` is pinned at 2,000 rows on every parameter.** It compares every receiving row with
every fitted row, so its cost is quadratic where every other row here is linear; at 20,000 rows
over four features that is 1.6 billion distance terms, past the ceiling the type itself refuses
at. Scaling it with the others would have made one row of the table decide the whole run.

`compare-transformers` puts the same operations against `scikit-learn` 1.9.1 at 10,000 and 100,000
rows. No corpus file: both sides build the matrix from the row index by the same rule,
`exp((i mod 97) / 24)` — strictly positive because Box-Cox refuses anything else, and right-skewed
because that is the column a power or a quantile map is called on, so one matrix serves every
operation. Two parameters are pinned away from the reference's defaults, because leaving them
would compare different work: `QuantileTransformer(subsample=None)`, since the reference's default
draws 10,000 rows from numpy's generator and the C# side has no subsample at all, and the imputer's
fixed 2,000-row slice.

Nothing is asserted to agree before timing: `tests/oracles/preprocessing_*.json` replay all seven
against the reference at `1e-9`, `PowerTransformer` at the `1e-5` `docs/equivalence.md` states.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*TransformerIncumbent*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-transformers
python3 bench/python/bench_transformers.py
python3 bench/compare.py transformers
```

### What moved while this was measured

`QuantileTransformer.Transform` built the two negated-and-reversed sequences its backward
interpolation reads **once per value**. At 20,000 rows over four features that is 80,000 pairs of
1,000-element arrays, and the first run of this class is what exposed it: **104.07 ms and
1,254,250 KB allocated** per call. They are built once in the constructor now — **10.81 ms and
625.74 KB**, the size of the output, for the same numbers, since nothing but the allocation
changed. The suite's 10,919 tests are green across both target frameworks either way, which is
the point: no oracle could have caught this.

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 56. The score matrix against FuzzySharp and `rapidfuzz` (issue #1123)

`CdistIncumbentBenchmarks` races `Process.Cdist` against the loop .NET forces today, at 50 × 50
and 200 × 200.

**The incumbent has no matrix call at all, and that is the finding.** Checked by reflection before
the class was written: `Raffinert.FuzzySharp.Process` exports `ExtractAll`, `ExtractTop`,
`ExtractSorted` and `ExtractOne`, and nothing that takes two collections. Its row here is
therefore `ExtractAll` called once per query — the hand-written loop, expressed the way that
library forces. A third row writes the same loop against **this** package's `Fuzz.Ratio`, which
prices what `Cdist` adds over what a caller can already write: one allocation for the matrix
instead of a list per query, and the cutoff applied where the score is produced.

`compare-cdist` puts the same call against `rapidfuzz.process.cdist` at 50, 200 and 500 a side.
Two parameters are pinned away from the reference's defaults, because leaving them would compare
different work: `dtype=np.float64`, since its default `np.float32` carries seven decimal digits
where this package scores in `double` throughout, and `workers=1`, since `Cdist` does not
parallelise — and at these sizes the reference's own pool is measured *slower* than one thread
(8.3 ms against 0.5 at 200 × 200).

**It loses on the cheap scorer and draws on the expensive one, which is the finding.** `cdist`
builds each query's bit-parallel equality table once and scans every choice against it;
`Fuzz.Ratio` takes two strings and rebuilds that table per pair. So the harness runs **two**
scorers per size: `ratio`, where that fixed cost is most of a cell, and `WRatio`, which inspects
its input and computes several sub-ratios and so amortises it. The contrast between the two rows
is what says the deficit is the bulk shape rather than the language — better evidence than any
cross-language loop, because both rows are the same two libraries on the same corpus.
[#1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries the hoisting, which needs new
`Lodestar.Text` API and a release order.

**A cold probe is what nearly went into the record.** One unwarmed `Stopwatch` pass over 200 × 200
read 25.5 ms against the reference's 0.5 — a 51× gap that is JIT. The harness and BenchmarkDotNet
agree with each other at 1.58 ms and 1.97 ms and not with it. Warm every side before believing a
factor.

No corpus file: both sides build the phrases from the row index by the same rule, three words and
an index, so a committed corpus would be a file holding that rule's output. The trailing index
keeps every phrase distinct, so no pair scores 100 by accident and the scorer does the same work
on both sides. Nothing is asserted to agree before timing —
`tests/oracles/process_cdist.json` replays all seven scorers at `1e-9`.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*CdistIncumbent*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-cdist
python3 bench/python/bench_cdist.py
python3 bench/compare.py cdist
```

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 57. The factorization applied to unseen rows, against `scikit-learn` (issue #1124)

`DecompositionBenchmarks` gains an `Nmf_Transform` row, and `compare-nmf-transform` puts the same
call against `NMF.transform` at 100, 500 and 2,000 unseen rows over a 1,500-row fit.

**There is no incumbent row beside it.** Neither ML.NET nor NumFlat publishes a non-negative
factorization at all, so what the BenchmarkDotNet row prices is the transform against the fit it
was taken from — the number a reader wants is what fraction of a fit a batch of new rows costs.

`solver='mu'` and `tol=0.0` are pinned on the Python side: the multiplicative update is the only
solver this package implements, and a disabled early stop makes the iteration count an input
rather than a result on both sides. The fit is built outside the timed region on both sides, so
what is measured is the transform.

**Run both losses, because they are two different computations.** scikit-learn's Frobenius update
is three dense products a BLAS parallelises; its Kullback-Leibler update densifies `W H` where
this package computes the ratio `X / WH` only at the stored positions. A single row would report
one of those as though it were the member's cost.

No corpus file: both sides build the matrices from the row index by the same rule, so a committed
corpus would be a file holding that rule's output. Nothing is asserted to agree before timing —
`tests/oracles/decomposition_nmf.json`'s `transform` section replays eight cases at `1e-9`.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*DecompositionBenchmarks*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-nmf-transform
python3 bench/python/bench_nmf_transform.py
python3 bench/compare.py nmf-transform
```

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 58. The cutoff that skips a pair rather than reporting it (issue #1134)

`CdistCutoffBenchmarks` prices the length bound `Process.Cdist` rejects a pair with, as the before
and after of one run: the baseline row is the default scorer, which takes the bound, and the second
row is the same arithmetic through a lambda the gate cannot recognise, which is the call as it was
before this change.

**The gate is the whole measurement.** The bound holds for the Indel ratio and for nothing else —
`partial_ratio("cat", "the cat sat on the mat")` is 100 against a ceiling of 24 — so `Cdist` takes
it only when the scorer is the delegate it defaults to. Passing `(a, b) => Fuzz.Ratio(a, b)`
computes the same scores and is not that delegate, which is what makes the two rows comparable:
one corpus, one scorer, one difference.

**`Widths` is the second half of the finding, and it has to be run.** A bound rejects a pair only
where the lengths differ enough, so the spread of the corpus is what decides whether it fires at
all. `Widths=true` draws phrases of one to six words; `Widths=false` draws the three-word corpus
section 56 uses, which still varies by a word's length and so rejects at a high cutoff and at no
other. Reporting only the first would be choosing the corpus that flatters it.

**The `EveryPair` row is the control, not the comparison.** It carries one more delegate hop than
the baseline, and between-benchmark layout moves both rows by up to 7% on this machine, so the
before and after is the *baseline against itself at `Cutoff=0`* — one method, one delegate, one
thing changed. What `EveryPair` proves is the other half: it is flat across all four cutoffs, so
the cutoff alone buys nothing without a bound behind it.

`Cutoff=0` is the call before this change in a second sense: no score can fall below zero, so the
gate is closed and both rows are the same code path. It is the row that says the branch is free
when it cannot fire.

**`compare-cdist` carries the other half, and it is the one that matters.** Section 56 measured
this package at 0.13× the reference on a cheap scorer; a cutoff is only worth publishing if the
reference does not get the same discount. So the harness runs the varied corpus twice on each
side, with `score_cutoff=90` and without, and the answer is that `rapidfuzz` reads the same
number either way. Its rule for the varied phrases is written out on both sides rather than read
from a file, exactly as section 56's is.

No corpus file: both sides build the phrases from the row index by the same rule, section 56's
through `CdistCorpus.Phrases` and the varied one through `Varied`/`varied`. Nothing is asserted to
agree before timing — `tests/oracles/process_cdist.json` replays its `ratio` cases through the
bounded path, one of them at a cutoff that rejects ten of twelve cells, and
`CdistLengthBoundTests` compares the bounded and unbounded paths over a random corpus at eight
cutoffs.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*CdistCutoff*'
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-cdist
python3 bench/python/bench_cdist.py
python3 bench/compare.py cdist
## 59. One pattern's table, held rather than rebuilt (issue #1130)

`IndelPatternBenchmarks` scores one query against 64 texts three ways: the pairwise loop a caller
writes today, `IndelPattern` holding the query's equality table, and the maintained .NET
incumbent's scorer called once per text.

**The corpus parameter is the finding, and leaving it out would have flattered the result.** The
handle's table spans the whole pattern, so it cannot drop a shared prefix and suffix the way the
pairwise call does. `longAffix` — 45 units shared at each end — is the case that costs it, and it
is the case fuzzy matching is most often pointed at. `oneWordAffix` is the shape that would refute
leaving a one-word pattern unguarded, `suffixOnly` the one a prefix-only probe would miss, and
`beyondTable` the fallback band on its own row. Reporting `unrelated` alone would have been
choosing the corpus that wins.

**Every corpus but `beyondTable` keeps the query inside the table's 128 units, and that is not
incidental.** The first version of this class built a 60 + 60 affix around the middle, which put
the query at 136 and 152 units — past the table, so every scan on the one row meant to measure the
guard was already the pairwise call ([#1137](https://github.com/CyrilB1531/lodestar/issues/1137)).
`beyondTable` uses 70 + 70 because 60 + 60 around a four-unit middle is 124, back inside.

**`Length` starts at 4 for the same reason.** What the pairwise trim buys grows as the middle
shrinks, so a short middle under a long affix is the shape that costs the handle most.

**Every row is checked to compute the same thing before it is timed**, and that check is what
found the defect this benchmark exists to have caught. `Indel.Distance(a, b)` over two
`ReadOnlySpan<char>` does **not** bind to the character overload: C# prefers a candidate whose
parameters all have arguments, so the generic `Distance<char>` wins and takes the dynamic program.
The answers are identical, so no test could see it — the long-affix rows past the table read
3.19× and 9.33× until the comparison unit was named explicitly.

The incumbent scores on the 0-100 scale and rounds, so its row is checked against the same ratio
rather than equated to it. It publishes no held form at all, which is why its row is one call per
text: that absence is the same finding section 56 records for the score matrix.

No corpus file: both the shared affixes and the middle that differs are built from the row index
by a rule written in the class, so a committed corpus would be a file holding that rule's output.
The agreement check above covers the corpus timed here; correctness in general is
`IndelPatternTests`' job, which replays `tests/oracles/indel.json`'s 1,522 pairs through the
handle in both orientations, together with a random corpus that crosses each width band and the
guard in both directions.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*IndelPatternBenchmarks*'
```

The numbers, with their machine and window, are in
[`docs/guides/performance.md`](../docs/guides/performance.md).

## 60. The four laws' tails and quantiles against Math.NET Numerics and Meta.Numerics (issue #1158)

`DistributionIncumbentBenchmarks` times the lower tail and the quantile of Student's t, F,
chi-squared and the standard normal, one call per row, against the two .NET libraries that publish
the same functions: Math.NET Numerics 5.0.0 (MIT) and Meta.Numerics 4.2.0 (MS-PL). Twenty-four
rows, eight operations on three libraries.

**Every value is compared before anything is timed.** The three libraries compute the same
quantity, so a row is a comparison only where their answers agree; `MetaNumericsAgreement`
records every value that differs from Lodestar's past `1e-9` and prints the list, which the
performance page quotes. Meta.Numerics' laws are objects, built once in the setup as a caller
holding a law would hold them; Math.NET's and Lodestar's are static calls. The arguments are the
reference pages' examples, held in instance fields so the JIT cannot fold a closed form into its
answer, as `DistributionTailBenchmarks` measured it would.

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*DistributionIncumbentBenchmarks*'
```

The numbers, with their machine and window, are in
[`src/Lodestar.Stats/performance.md`](../src/Lodestar.Stats/performance.md).

## 61. Instrumental variables against `linearmodels` (issue #1155)

`compare-iv` puts `InstrumentalVariables` against `linearmodels` 7.0's `IV2SLS`, `IVLIML` and
`IVGMM` at 1,000, 10,000 and 100,000 rows: three exogenous regressors and a constant, two endogenous
regressors, four instruments. **No .NET library estimates one**, free or commercial
([decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)), so the
Python reference is the only incumbent.

No corpus file: both sides build each value from its row and column index by one formula, every
column at its own frequency so the blocks are of full rank. Each operation produces the whole table.
`linearmodels` computes the first-stage diagnostics and the overidentification test only when they
are read, so its side reads them; the C# summary always carries them. Agreement is
`tests/oracles/stats_iv.json`'s job, not the harness's.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-iv
python3 bench/python/bench_iv.py
python3 bench/compare.py iv
```

## 62. Panel regression against `linearmodels` (issue #1156)

`compare-panel` puts `PanelRegression` against `linearmodels` 7.0's `PanelOLS`, `BetweenOLS`,
`FirstDifferenceOLS` and `RandomEffects` at 100, 1,000 and 10,000 entities over ten periods — 1,000,
10,000 and 100,000 rows — with three regressors and a constant. **No .NET library estimates one**
([decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)), so the
Python reference is the only incumbent.

No corpus file: both sides build each value from its entity, period and column by one formula, an
entity effect that moves with the regressors so fixed and random effects differ. Each operation
produces the whole table. `linearmodels` computes the R² family, the model tests and the variance
decomposition only when they are read, so its side reads them; the C# summary always carries them.
The frame is built outside the timed region, as the C# arrays are; `linearmodels`' conversion of it
into a panel is inside, since every fit pays it. Agreement is `tests/oracles/stats_panel.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-panel
python3 bench/python/bench_panel.py
python3 bench/compare.py panel
```

## 63. Cross-conformal intervals against MAPIE (issue #1159)

`compare-cross-conformal` puts `CrossConformal` against MAPIE 1.5.0's `CrossConformalRegressor`
(ten unshuffled folds, plus and min-max, the absolute and the gamma score) and
`JackknifeAfterBootstrapRegressor` (thirty bootstrap models), at 1,000 and 10,000 training samples
and 500 test points. **No .NET library computes a conformal interval**
([decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)), so MAPIE is
the only incumbent.

No corpus file: every model is a formula of the row index, shifted by the mean index of the rows it
was fitted on, and K-fold runs unshuffled, so both sides know each fold's model. What is timed is
the interval at every test point from fitted models. MAPIE's `predict_interval` also calls each
model's `predict` on the test block, timed alone as `predict_only` so it can be read off; this
package takes those predictions, computed outside the timed region. The bootstrap's bags come from
MAPIE's generator on its side and from a formula on this one, at the same model count and a similar
out-of-bag share. Agreement is `tests/oracles/conformal_cross.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-cross-conformal
python3 bench/python/bench_cross_conformal.py
python3 bench/compare.py cross-conformal
```

## 64. Sparse one-hot encoding against scikit-learn (issue #1161)

`compare-onehot` puts `OneHotEncoder<int>.TransformSparse` against scikit-learn 1.9.1's
`OneHotEncoder` at its default sparse output, at 10,000 and 100,000 rows of three integer features,
with every category kept and with `min_frequency=5` and `max_categories=200` grouping the tail. No
.NET encoder groups infrequent categories or returns this layout; ML.NET's dense `OneHotEncoding` is
the incumbent the encoders are already measured against.

No corpus file: both sides build each category from its row and column index by one formula, a
hashed index folded twice so that small codes are far more frequent. Each operation is a fit and
the sparse transform of the fitted rows. Agreement is
`tests/oracles/preprocessing_onehot_infrequent.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-onehot
python3 bench/python/bench_onehot.py
python3 bench/compare.py onehot
```

## 65. Weighted k-means, restarts and weighted DBSCAN against scikit-learn (issue #1163)

`compare-cluster-weighted` puts `KMeans.Fit` with sample weights, from one given start and from four
through `KMeansOptions.InitialCentreSets`, and the weighted `Dbscan.Fit` against scikit-learn
1.9.1's `KMeans(algorithm="lloyd")` and `DBSCAN` with `sample_weight`. k-means runs at 10,000 and
100,000 rows of eight features and sixteen clusters, DBSCAN at 5,000 and 20,000 planar rows. No
.NET library weighs a sample in either algorithm, so the reference is the incumbent.

No corpus file: both sides build each value from its row and column index by one formula — sixteen
or ten blobs, each row jittered by a hash of its index — and weigh row `i` `1 + 37i mod 5`. The
starts are rows picked by a stride, so both sides run the same Lloyd iterations; scikit-learn takes
the four starts through a callable `init`, handed the centred matrix, as the corpus does.
Agreement is `tests/oracles/cluster_weighted.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-cluster-weighted
python3 bench/python/bench_cluster_weighted.py
python3 bench/compare.py cluster-weighted
```

## 66. The log-rank family, the restricted mean and the concordance index against lifelines (issue #1170)

`compare-survival-family` puts `LogRank.Test` under the Wilcoxon weighting with subject weights and
under Fleming-Harrington, `LogRank.MultiGroup` and `LogRank.Pairwise` over five groups,
`KaplanMeier.RestrictedMean` after a fit, `KaplanMeier.CompareAt` after two, and `Concordance.Index`
against lifelines 0.30.3's `logrank_test`, `multivariate_logrank_test`, `pairwise_logrank_test`,
`restricted_mean_survival_time(return_variance=True)`,
`survival_difference_at_fixed_point_in_time_test` and `utils.concordance_index`, at 1,000, 10,000
and 100,000 subjects. No .NET library computes any of them, so the reference is the incumbent.

No corpus file: both sides build each subject from its index by one formula — durations in
hundredths with ties, seven in ten observed, five groups, a score loosely following the duration,
weights one to three. Each operation includes the fit it reads, as a caller pays for it. Agreement is
`tests/oracles/survival_logrank_family.json`, `survival_restricted.json` and
`survival_concordance.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-survival-family
python3 bench/python/bench_survival_family.py
python3 bench/compare.py survival-family
```

## 67. The extended Cox model against lifelines (issue #1171)

`compare-cox-extended` puts `CoxProportionalHazards.Fit` stratified and weighted, with a ridge penalty
and the robust variance, and with a lasso penalty, `CoxProportionalHazards.TestProportionalHazards`,
`CoxSummary.PredictSurvivalFunction` and `CoxTimeVarying.Fit` against lifelines 0.30.3's
`CoxPHFitter`, `proportional_hazard_test`, `predict_survival_function` and `CoxTimeVaryingFitter`, at
1,000 and 10,000 subjects of four covariates. Every fit runs at lifelines' defaults, as a caller runs
it. No .NET library fits a Cox model, so the reference is the incumbent.

No corpus file: both sides build each subject from its index by one formula — four covariates in
`[-1, 1)`, each column hashed by its own multiplier so that no two are collinear, durations from a
hashed uniform, seven in ten observed, three strata, weights one to three; the time-varying data
splits each subject at half its duration. Agreement is `tests/oracles/survival_cox_extended.json`'s job.

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-cox-extended
python3 bench/python/bench_cox_extended.py
python3 bench/compare.py cox-extended
```

# Changelog

All notable changes to this project are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and
this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

The fourteen packages (`Lodestar.Text`, `Lodestar.Embeddings`, `Lodestar.Fuzzy`,
`Lodestar.Metrics` — published as `DataNet.*` up to 2026-08-15 — plus
`Lodestar.Conformal`, `Lodestar.Abstractions`, `Lodestar.Decomposition`,
`Lodestar.Onnx`, `Lodestar.Stats`, `Lodestar.Extensions.AI`,
`Lodestar.Extensions.MathNet`, `Lodestar.Preprocessing`, `Lodestar.Cluster` and
`Lodestar.Stats.Regression`, all newer than
that rename)
version and release **independently**, each from its own
`src/<Package>/Version.props`, so entries are grouped per package. Releases up to
and including `0.2.0` predate the split and covered all three at once — see
[`docs/decisions/0012`](docs/decisions/0012-per-package-versioning.md). From the
2026-08-14 release the heading carries the date alone, because the four packages
no longer share a number: `DataNet.Metrics` shipped its first `0.1.0` while the
other three shipped `0.3.0`. Each entry
is one sentence, the issue and the commit; see
[`CONTRIBUTING.md`](CONTRIBUTING.md#releasing) for the shape and why.

## [Unreleased]

### Lodestar.Text

#### Added

- **`Lodestar.Text.Phonetics` adds `DoubleMetaphone`, which answers with two codes rather than one.** A spelling with more than one plausible reading no longer has to be forced into a single verdict: `Smith` encodes to `SM0` with `XMT` alongside it, and `XMT` is what `Schmidt` encodes to, so the two meet without every `S`-word joining them. `Encode` returns a `DoubleMetaphoneCode` whose `Secondary` is empty — not a repeat of the primary — where a word has only one reading, which is 321 of the 423 words the corpus pins. Codes are not truncated to four characters, because the reference does not truncate. Its oracle is **`doublemetaphone` 1.2 rather than `jellyfish`**, which exports no Double Metaphone at all: [decision 0075](docs/decisions/0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md) measured the permissive candidates against each other over the phonetics corpus, found `metaphone` 0.6 and `doublemetaphone` 1.2 agreeing on 401 of 401 primaries — so the corpus freezes an algorithm and not one library's dialect — and excluded `abydos` on GPLv3+ per [decision 0003](docs/decisions/0003-provenance-and-licensing.md). Only ASCII letters are read, so `élan` encodes as `lan` does; that contract is frozen in the corpus rather than asserted in C#. ([#177](https://github.com/CyrilB1531/lodestar/issues/177))

- **`Lodestar.Text.Stemming` adds `DutchSnowballStemmer`, the seventh Snowball language.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `SnowballWorkerBase` the six before it already share. Dutch folds umlauts and acutes to their bare vowel **before** the rules run — `è` excepted, a letter of the alphabet rather than a diacritic — and undoubles a stressed vowel **after** them, which is what makes `maan` and `manen` one key instead of two. That last step is also why `wandeling`, `wandelingen` and `wandelend` all reach `wandel` while `lopend`, too short for `end` to sit in R2, comes back whole. The base class moved with it: R1 and R2 are both measured the standard way before R1 takes its three-letter floor, the order the German and Dutch descriptions state, where measuring R2 from an already-floored R1 kept `ing` out of R2 and stemmed `opening` to itself. No German stem moved — its 88-word corpus replays unchanged. Replays `nltk` 3.10.1's `SnowballStemmer("dutch")` over the 177 words of `tests/oracles/snowball_nl.json`, exactly. ([#304](https://github.com/CyrilB1531/lodestar/issues/304))

- **`Lodestar.Text.Stemming` adds `SwedishSnowballStemmer`, the eighth Snowball language.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `SnowballWorkerBase` the seven before it already share. It is the shortest of the eight — three steps, all of them in R1, with no R2 and no RV region — and the one place it differs from its siblings is where the region is applied: the Scandinavian steps search for the longest suffix **inside R1** rather than for the longest suffix, tested against R1 afterwards. `nyheter` ends with `heter`, which reaches past R1, and with `er`, which does not; `er` is what goes, and the word stems to `nyhet`. Choosing the longest first leaves it whole, which is why the base class gained `LongestSuffixInR1` alongside the `LongestSuffix` the German and Romance steps use — an addition, so no stem in the other seven corpora moved. Nothing is marked before the rules and nothing is folded after them, so `tjänstemännens` stems to `tjänstemän` rather than to `tjansteman`, and `löst` and `fullt` are rewritten rather than stripped so that `hjälplöst` keeps a stem that is still a word. Replays `nltk` 3.10.1's `SnowballStemmer("swedish")` over the 211 words of `tests/oracles/snowball_sv.json`, exactly. ([#306](https://github.com/CyrilB1531/lodestar/issues/306))

- **`Lodestar.Text.Stemming` adds `RussianSnowballStemmer`, the ninth Snowball language and the first Cyrillic one.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `SnowballWorkerBase` the eight before it already share and with no change to it. Its regions are not the ones its siblings use: RV is simply the rest of the word after its first vowel — not the Romance RV, which counts letters two and three — steps 1 and 2 search inside it, R2 is measured the ordinary way and carries step 3 alone, and R1 is never used. Step 1 is four tables tried in order, three of them split into a group that counts only after an `а` or `я`, which is what parts `читаемый` from `любимый`. `ё` is folded to `е` before the rules run, every letter is a single UTF-16 unit so there is no code-point mode to choose, and both of those are settled in [decision 0086](docs/decisions/0086-russian-follows-nltks-table-and-the-descriptions-alphabet.md) rather than left to be inferred. That decision also settles the two places `nltk` — which stems a Roman transliteration rather than Cyrillic — parts from the description: its adjectival table misspells one pair of 234, so `рискующая` stems to `рискующ` and **that is followed**, as [0008](docs/decisions/0008-italian-enza-nltk-divergence.md) followed Italian `enza`; its `ъ` is two apostrophes to `ь`'s one, so step 4 halves a final `ъ` and answers `подь` for `подъём`, and **that is not**, because matching it means modelling `ъ` as two `ь` in every step and shipping `отт^` for `отць`. Replays `nltk` 3.10.1's `SnowballStemmer("russian")` over the 291 words of `tests/oracles/snowball_ru.json`, exactly; the two `ъ` words are pinned by `RussianSnowballStemmerTests` instead. ([#305](https://github.com/CyrilB1531/lodestar/issues/305))
- **`Lodestar.Text.Stemming` adds `ArabicSnowballStemmer`, the fifteenth Snowball language and the last of the nine.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)). [#176](https://github.com/CyrilB1531/lodestar/issues/176) took Arabic last because its algorithm shares least with the others, and it does: it uses **neither R1 nor R2**, carrying an explicit character count on every rule instead, so `ArabicSnowballStemmer` is the one stemmer here that does not derive from `SnowballWorkerBase` and adds nothing to it. Normalisation is half the work and runs first — vocalisation marks and the kasheeda dropped, the lam-alef ligatures written back as two letters, the Arabic-Indic digits folded to ASCII, so `مُحَمَّد` and `محمد` are one word and `١٢٣` comes back as `123` — while the hamza carriers are resolved **last**, surviving the affix steps before they fold. Affixes then come off both ends: the possessives, case and verb endings and the nisba `ي` from the right, and the conjunctions `و`/`ف`, the definite article and the future markers from the left, so `كتاب`, `الكتاب`, `بالكتاب`, `كتابه` and `كتابها` all reach `كتاب`. **Its corpus is frozen from `snowballstemmer` 3.1.1 rather than `nltk`**, the second to leave `nltk` after Hungarian and on a stronger ground: `nltk`'s Arabic stemmer keeps its classification flags on the instance, so `كتبوا` stems to `كتب` on a fresh stemmer and `كتبو` after other words — a corpus frozen from a shared one would encode the order of the word list where nothing downstream could see it, and no stemmer that documents Thread-safe can reproduce it ([decision 0094](docs/decisions/0094-arabic-takes-snowballstemmer-and-stands-outside-the-worker-base.md)). Replays `snowballstemmer` 3.1.1's `stemmer("arabic")` over the 146 words of `tests/oracles/snowball_ar.json`, exactly. ([#312](https://github.com/CyrilB1531/lodestar/issues/312))

- **`Lodestar.Text.Stemming` adds `DanishSnowballStemmer`, the tenth Snowball language.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `SnowballWorkerBase` the nine before it already share, and reusing the `LongestSuffixInR1` search [#306](https://github.com/CyrilB1531/lodestar/issues/306) added for Swedish unchanged. #307 asked the third of the three Scandinavian languages to either reuse the shared shape or say why it cannot, and with two of them written there is one to share: **`ScandinavianSnowballWorker`** now holds what Swedish and Danish state the same way — the lowercase-and-compose entry point, R1's three-letter floor, the first step's group (a)/(b) search with its bare `s`, and the consonant-pair step — the way `RomanceSnowballWorker` already holds what Spanish, Portuguese and Italian share. No suffix table moved into it: each language keeps its vowels, its s-endings and its rules in its own file, so that file still reads one-to-one against the published description. `SwedishSnowballStemmer` is rewritten onto it and its 211-word corpus replays unchanged. Danish is Swedish with a step added and a step that repeats: a fourth step undoubles a final `bb`…`tt` when the letter it removes lies in R1, so `bestemmelse` loses its `e`, then its `els`, and ends at `bestem` rather than `bestemm`; and step 3 runs step 2 again afterwards, because deleting `ig`, `lig`, `elig` or `els` can uncover a `gd`/`dt`/`gt`/`kt` that was not there before — `fjendtligt` reaches `fjend` through three deletions in that order. A word ending `igst` loses the `st` before the third step searches at all, which puts `hurtigst`, `hurtigt` and `hurtig` on one key. The one place the implementation departs from the published description is its second definition of R1, the only one in the ten that starts the region after an apostrophe: `nltk` does not implement it, so neither does this, and `pc'er` comes back whole where the prose stems it to `pc'` — [decision 0089](docs/decisions/0087-danish-follows-nltk-on-the-apostrophe.md), the same call as [0008](docs/decisions/0008-italian-enza-nltk-divergence.md), with the six apostrophe words in the corpus so the divergence is pinned rather than latent. No stem in the other nine corpora moved: nothing outside this language changed. Replays `nltk` 3.10.1's `SnowballStemmer("danish")` over the 240 words of `tests/oracles/snowball_da.json`, exactly. ([#307](https://github.com/CyrilB1531/lodestar/issues/307))

- **`Lodestar.Text.Stemming` adds `FinnishSnowballStemmer`, the twelfth Snowball language and the longest of them.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)). Finnish is agglutinative, so one noun carries fifteen cases and each of those can take a possessive and then a particle on top; the algorithm peels them in that order over six steps and standard R1 and R2, with no floor and no RV region. `talossaan` gives up its `-an` as a possessive and only then its `-ssa` as a case, reaching the same `talo` as `talossa`, `taloon`, `talot`, `taloja` and `talonsa`. Vowel harmony is what no other language here has: `-ssa` after a back vowel and `-ssä` after a front one, with a possessive `-an` admitted only behind the first list and `-än` only behind the second. Three steps read what sits in front of the ending — `hXn` needs its own vowel repeated, `-siin` a vowel and an `i`, `-seen` a long vowel, the partitive `-a` a consonant and a vowel — and the tidying step is what makes a Finnish stem look short, removing a long vowel, then a consonant and one of `a ä e i`, then an `-oj`/`-uj`/`-jo`, then the second letter of a doubled consonant, so `kotimaahan` ends at `kotim` and `kissaa` at `kis`. The base class gained `LongestSuffixInR2` alongside the `LongestSuffixInR1` [#306](https://github.com/CyrilB1531/lodestar/issues/306) added — an addition, so no stem in the other eleven corpora moved. A condition that fails ends the search, except on the four endings that are a longer spelling of the genitive, where the `-n` goes anyway: that follows `nltk` over the published text ([decision 0091](docs/decisions/0090-finnish-falls-back-to-the-genitive-where-nltk-does.md)), `ihmisiin` is the one real word in the corpus that meets it, and four constructed forms pin the boundary from both sides. Replays `nltk` 3.10.1's `SnowballStemmer("finnish")` over the 217 words of `tests/oracles/snowball_fi.json`, exactly. ([#309](https://github.com/CyrilB1531/lodestar/issues/309))

- **`Lodestar.Text.Stemming` adds `NorwegianSnowballStemmer`, the eleventh Snowball language.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `ScandinavianSnowballWorker` [#307](https://github.com/CyrilB1531/lodestar/issues/307) introduced, which this lot extends rather than copies. Written next to Swedish first, Norwegian repeated 34 lines of it and the quality gate measured that — 13.2% duplication on new code against a 3% threshold, with no rule violation and coverage at 96.8%. What repeated was the scaffolding a nested worker needs, not the rules: declaration, vowel field, constructor and `Run` override, identical in every language. `ScandinavianSnowballWorker` now also takes a language as **a vowel test and a sequence of steps**, so one whose steps are static methods adds no scaffolding at all; Norwegian takes that entry and its longest run against Swedish and Danish alike falls from 34 lines to 3. Swedish and Danish are untouched. Each of the three steps takes the longest suffix **lying in R1**, not the longest suffix tested against R1 afterwards. **This is Bokmål**, which is what nltk's `norwegian` implements; Nynorsk has no oracle here and is out of scope for the same reason Turkish is, and the XML documentation and the reference page both say so. `ert` and `erte` are rewritten to `er` rather than stripped, so `servert` and `serverte` meet at `server`; a bare `s` goes only after one of eighteen letters or after a `k` that is not itself preceded by a vowel, which is what parts `folks` → `folk` from `boks`, its own stem. `æ`, `å` and `ø` are letters rather than accented forms and are neither marked nor folded. Replays `nltk` 3.10.1's `SnowballStemmer("norwegian")` over the 244 words of `tests/oracles/snowball_no.json`, exactly — a corpus that exercises every one of the algorithm's step-1 and step-3 suffixes. ([#308](https://github.com/CyrilB1531/lodestar/issues/308))

- **`Lodestar.Text.Stemming` adds `HungarianSnowballStemmer`, the thirteenth Snowball language and the first corpus here frozen from something other than `nltk`.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)): nine steps, all searching R1, with no R2 and no RV region. Hungarian's R1 is neither of the two standard shapes — after the first consonant when the word opens with a vowel, after the first vowel when it opens with a consonant — and a digraph (`cs`, `dzs`, `sz`, `gy` and the rest) counts as one consonant while it is measured. The instrumental and factive steps undouble the consonant their ending doubled, so `vassal` returns to `vas` and `hússá` to `hús`. **Its oracle is `snowballstemmer` 3.1.1, not `nltk`**: nltk 3.10.1's Hungarian omits `ő` and `ű` from its vowel set and `től`, `ről` and `ből` from step 2, which leaves `nő`/`nők`, `szőlő`/`szőlők` and `gyűrű`/`gyűrűk` as two keys each and every `-ből` form unstemmed — the omission covers a letter class rather than the handful of words [decision 0008](docs/decisions/0008-italian-enza-nltk-divergence.md) and [decision 0087](docs/decisions/0087-danish-follows-nltk-on-the-apostrophe.md) chose to follow, so [decision 0091](docs/decisions/0091-hungarian-takes-snowballstemmer-as-its-oracle.md) takes the reference implementation instead. The other eight Snowball corpora are untouched. `SnowballWorkerBase` gains a constructor for a language that computes its own region, and `ReplaceIfInR1` beside the existing `ReplaceIfInR2`; both additive. Replays 211 words exactly. ([#310](https://github.com/CyrilB1531/lodestar/issues/310))
- **`Lodestar.Text.Stemming` adds `FinnishSnowballStemmer`, the twelfth Snowball language and the longest of them.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)). Finnish is agglutinative, so one noun carries fifteen cases and each of those can take a possessive and then a particle on top; the algorithm peels them in that order over six steps and standard R1 and R2, with no floor and no RV region. `talossaan` gives up its `-an` as a possessive and only then its `-ssa` as a case, reaching the same `talo` as `talossa`, `taloon`, `talot`, `taloja` and `talonsa`. Vowel harmony is what no other language here has: `-ssa` after a back vowel and `-ssä` after a front one, with a possessive `-an` admitted only behind the first list and `-än` only behind the second. Three steps read what sits in front of the ending — `hXn` needs its own vowel repeated, `-siin` a vowel and an `i`, `-seen` a long vowel, the partitive `-a` a consonant and a vowel — and the tidying step is what makes a Finnish stem look short, removing a long vowel, then a consonant and one of `a ä e i`, then an `-oj`/`-uj`/`-jo`, then the second letter of a doubled consonant, so `kotimaahan` ends at `kotim` and `kissaa` at `kis`. The base class gained `LongestSuffixInR2` alongside the `LongestSuffixInR1` [#306](https://github.com/CyrilB1531/lodestar/issues/306) added — an addition, so no stem in the other eleven corpora moved. A condition that fails ends the search, except on the four endings that are a longer spelling of the genitive, where the `-n` goes anyway: that follows `nltk` over the published text ([decision 0092](docs/decisions/0090-finnish-falls-back-to-the-genitive-where-nltk-does.md)), `ihmisiin` is the one real word in the corpus that meets it, and four constructed forms pin the boundary from both sides. Replays `nltk` 3.10.1's `SnowballStemmer("finnish")` over the 217 words of `tests/oracles/snowball_fi.json`, exactly. ([#309](https://github.com/CyrilB1531/lodestar/issues/309))

- **`Lodestar.Text.Stemming` adds `NorwegianSnowballStemmer`, the eleventh Snowball language.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), over the `ScandinavianSnowballWorker` [#307](https://github.com/CyrilB1531/lodestar/issues/307) introduced, which this lot extends rather than copies. Written next to Swedish first, Norwegian repeated 34 lines of it and the quality gate measured that — 13.2% duplication on new code against a 3% threshold, with no rule violation and coverage at 96.8%. What repeated was the scaffolding a nested worker needs, not the rules: declaration, vowel field, constructor and `Run` override, identical in every language. `ScandinavianSnowballWorker` now also takes a language as **a vowel test and a sequence of steps**, so one whose steps are static methods adds no scaffolding at all; Norwegian takes that entry and its longest run against Swedish and Danish alike falls from 34 lines to 3. Swedish and Danish are untouched. Each of the three steps takes the longest suffix **lying in R1**, not the longest suffix tested against R1 afterwards. **This is Bokmål**, which is what nltk's `norwegian` implements; Nynorsk has no oracle here and is out of scope for the same reason Turkish is, and the XML documentation and the reference page both say so. `ert` and `erte` are rewritten to `er` rather than stripped, so `servert` and `serverte` meet at `server`; a bare `s` goes only after one of eighteen letters or after a `k` that is not itself preceded by a vowel, which is what parts `folks` → `folk` from `boks`, its own stem. `æ`, `å` and `ø` are letters rather than accented forms and are neither marked nor folded. Replays `nltk` 3.10.1's `SnowballStemmer("norwegian")` over the 244 words of `tests/oracles/snowball_no.json`, exactly — a corpus that exercises every one of the algorithm's step-1 and step-3 suffixes. ([#308](https://github.com/CyrilB1531/lodestar/issues/308))

- **`Lodestar.Text.Stemming` adds `RomanianSnowballStemmer`, the fourteenth Snowball language and the only Romance one of [#176](https://github.com/CyrilB1531/lodestar/issues/176)'s nine.** Written from the published Snowball description rather than a reference implementation ([decision 0003](docs/decisions/0003-provenance-and-licensing.md)), and the one of the nine that reaches `RomanceSnowballWorker` — the RV that Spanish, Portuguese and Italian already share — rather than `SnowballWorkerBase` alone. Five steps, of which step 1 *loops*: `activitate` loses `ivitate` for `iv` and stops at `activ`, and a chain is walked one link a turn. Only step 3's region qualifies its search, so `casem` falls through `asem`, which reaches past RV, to the `em` that does not — everywhere else the longest ending wins outright and one that overshoots stops the step. `i` and `u` between two vowels are marked before the regions are measured and come back as themselves. **`ş` and `ș` are two letters here**: every suffix table carries the cedilla forms the published description writes, so a word spelled the modern way matches only the endings without those letters — `informaţie` stems to `inform`, `informație` to `informaț` — and nothing is folded, deliberately ([decision 0092](docs/decisions/0092-romanian-keeps-the-cedilla-alphabet-the-description-writes.md), which also bounds [0008](docs/decisions/0008-italian-enza-nltk-divergence.md) a second time after [0086](docs/decisions/0086-russian-follows-nltks-table-and-the-descriptions-alphabet.md): the regions are read off the word rather than off `nltk`'s stale copies of them, which parts on 35 of 30 252 swept words and on none of the 349 real ones). Replays `nltk` 3.10.1's `SnowballStemmer("romanian")` over the 349 words of `tests/oracles/snowball_ro.json`, exactly. ([#311](https://github.com/CyrilB1531/lodestar/issues/311))

### Lodestar.Embeddings

#### Fixed

- **A special token written as ordinary text no longer encodes two ways on Llama-2, and no longer becomes a control token in the middle of a sentence.** `tokenizers` normalizes a `normalized: true` added token's **content** with the file's declared normalizer, so Llama-2 — whose whitespace escape is a `Prepend`+`Replace` normalizer — matches on `▁<s>`, not `<s>`. `BpeTokenizer` built that pattern from the Unicode forms alone while escaping the text it searched, which parted two ways: `"<s>"` encoded to `['▁', '<s>']` where the reference answers the single id `1`, and `"the cat<s>"` matched the added token where the reference does **not** — so a caller's own `<s>` silently became the BOS id. The escape now joins the pattern only where the file spelled it as a normalizer; Mistral v0.1, which declares its entries raw under a `Metaspace` pre-tokenizer, is unchanged and was already exact. A `Metaspace` pre-tokenizer carrying a `normalized: true` entry is refused at construction rather than approximated — the escape would depend on the piece's position, which a fixed pattern cannot carry. [Decision 0085](docs/decisions/0085-a-normalized-added-tokens-pattern-carries-the-normalizers-escape.md) amends [0062](docs/decisions/0062-the-two-metaspace-spellings-part-on-the-prepend-twice.md) with the third place the two spellings part, and `sentencepiece_bpe_lineage.json` grows from 16 rows to 26, freezing the five texts [#318](https://github.com/CyrilB1531/lodestar/issues/318) had taken out rather than settle. **Llama-2 ids change**: an embedding produced through this lineage by 0.6.0 carries the extra `▁` and must be regenerated. ([#551](https://github.com/CyrilB1531/lodestar/issues/551))

### Lodestar.Extensions.AI

#### Added

- **`Lodestar.Extensions.AI` 0.1.0 — the ONNX embedding path, behind `Microsoft.Extensions.AI`'s own interface.** `OnnxEmbeddingGenerator` implements `IEmbeddingGenerator<string, Embedding<float>>` over an `OnnxTextEmbedder` and a `BatchEncoder`, so a Semantic Kernel pipeline or any `Microsoft.Extensions.AI` chain can hold Lodestar embeddings without knowing Lodestar. It adds **no arithmetic**: every vector is what `OnnxTextEmbedder.EmbedBatch` returned for that text, which is why the suite asserts identity with that overload exactly rather than within a tolerance, and why this package carries no oracle corpus of its own. It is the **second satellite**, and it exists rather than being a second dependency on `Lodestar.Onnx` because [decision 0076](docs/decisions/0076-a-core-package-carries-no-external-dependency.md) says an external dependency earns its own package named for it — so a caller who wants inference and not the AI abstractions still restores nothing extra. Three things are stated rather than left to be discovered: the returned task is **already completed**, since the model runs in the calling process and `Task.Run` would move the same CPU without telling the caller anything true; `EmbeddingGenerationOptions.Dimensions` is **checked, not honoured**, because an ONNX model's width is fixed at export, and a width the model provably does not produce is refused instead of silently ignored; and the generator **takes ownership** of the embedder, because `IEmbeddingGenerator` is `IDisposable` and a consumer holding it through the interface cannot see that disposing would otherwise leave a native session open. `GetService` answers for the metadata, for the generator itself, and for the underlying `OnnxTextEmbedder` — the only way to reach the single-sequence entry point through an interface that has no shape for it. ([#570](https://github.com/CyrilB1531/lodestar/issues/570))

### Lodestar.Extensions.MathNet

#### Added

- **`Lodestar.Extensions.MathNet` 0.1.0 — `CsrMatrix` to and from Math.NET's sparse matrix, in one pass over the stored values.** `MathNetInterop.ToSparseMatrix` and `MathNetInterop.ToCsrMatrix` move the three compressed-row arrays rather than visiting `rows x columns` cells, because both sides store a matrix the same way and both expose it; neither result shares an array with its source, since Math.NET's storage is mutable through `At`. The dense pair is deliberately absent: `CsrMatrix.ToDense()` already returns a `double[,]` and Math.NET builds a `DenseMatrix` from one unaided, so the sparse pair is the one conversion neither side can do for itself. **The conversion sorts.** `CsrMatrix` validates four things and the order of column indices within a row is not among them, while Math.NET reaches a cell by searching that row — so a hand-built matrix handed over unsorted would convert without complaint and then answer zero for values it holds. Each row is sorted and duplicate columns are added together, after a pass that detects the already-sorted case and copies straight through, which is what every vectorizer here produces. [Decision 0089](docs/decisions/0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md) records that, and two things beside it: the interop family is now `Lodestar.Extensions.*` — so `Lodestar.Extensions.AI` becomes an instance of a convention rather than a one-off — and an interop satellite **may** take a dependency a core package refused, because converting to a caller's own types is not the same decision as computing with them. `Lodestar.Decomposition`'s refusal of Math.NET stands unchanged, re-measured on 2026-09-09: 5.0.0 is still the only stable release, dated 2022-04-03. ([#571](https://github.com/CyrilB1531/lodestar/issues/571))

### Lodestar.Preprocessing

#### Added

- **`Lodestar.Preprocessing` 0.1.0 — `StandardScaler`, at scikit-learn parity, with spans instead of an `IDataView`.** `Fit` takes a row-major span and a feature count — the shape `Lodestar.Metrics` already uses — and returns a scaler whose `Mean`, `Variance` and `Scale` are readable and **nullable exactly where the reference reports `None`**: `with_mean=False` still fits a mean, and only turning both steps off drops it. `Transform` and `InverseTransform` return new arrays and never write to the input. **A near-constant feature scales by 1, and the test is not `variance == 0`**: scikit-learn compares the variance against the two-pass error bound of Chan, Golub and LeVeque, `var <= n·eps·var + (n·mean·eps)²`, so a feature with a large mean and a tiny variance is constant to within what the computation could resolve. `tests/oracles/preprocessing_standard_scaler.json` freezes eight cases against scikit-learn 1.9.0, including the pair that separates the two readings — `1e8 ± 1e-8`, whose variance is `1.48e-16` and whose scale is 1, against `1e8 ± 1e-7`, scaled by `8.5e-08`. An implementation testing `variance == 0` passes every other case and fails those two by eight orders of magnitude. The threshold is read from `sklearn.preprocessing._data._is_constant_feature` (BSD-3, allowed by [decision 0003](docs/decisions/0003-provenance-and-licensing.md)) and attributed in the source: the papers give the error analysis, not the number. Core tier — no external dependency, no inter-package edge. ([#568](https://github.com/CyrilB1531/lodestar/issues/568))

### Lodestar.Cluster

#### Added

- **`Lodestar.Cluster` 0.1.0 — k-means by Lloyd's algorithm, at scikit-learn parity, with the starting centres as an input.** `KMeans.Fit` takes a row-major span and returns `Centres`, `Labels`, `Inertia` and `Iterations`, the four things scikit-learn reports; `Predict` assigns unseen rows without refitting. `Labels` is the shape `Lodestar.Metrics` already scores, so a clustering arrives with silhouette, adjusted Rand, AMI and V-measure on day one rather than needing them built — which is the argument [#442](https://github.com/CyrilB1531/lodestar/issues/442) made for this domain, and it held. The loop is the reference's: assign, update, stop on unchanged labels or on a centre shift within the tolerance, and when it stops on the shift a final assignment runs so `Labels` matches `Centres` rather than trailing one update behind. `Tolerance` is scaled by the mean feature variance, as `_tolerance` scales it, so the same number means the same thing at any scale. An empty cluster is relocated onto the sample furthest from its own centre. **`KMeansOptions.InitialCentres` is an input, not a seed** — [decision 0072](docs/decisions/0072-omega-is-an-input-not-a-seed.md)'s move, applied here: given a starting block the run is an ordinary parity target, and `tests/oracles/cluster_kmeans.json` compares every centre, label, inertia and iteration count across eight cases rather than comparing distributions. Left alone, k-means++ draws from this package's own generator and reproduces a run of Lodestar, never one of scikit-learn. **One measured divergence, and it is recorded rather than papered over**: a sample exactly equidistant from two centres takes the lowest-indexed one here, where the reference was observed choosing the second on one configuration and the first on another; [decision 0093](docs/decisions/0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md) has both, and the two corpus fixtures that hinged on a tie were replaced so no frozen case rests on a convention. Core tier — no external dependency, no inter-package edge. ([#567](https://github.com/CyrilB1531/lodestar/issues/567))

### Lodestar.Stats.Regression

#### Changed

- **The variance inflation factors are read off one decomposition instead of one regression per regressor.** `Fit` ran an auxiliary least squares for every regressor to reach its VIF, which put five Householder QRs of the design behind a four-regressor fit; for standardised regressors a VIF is a diagonal entry of the inverse correlation matrix, so a single QR of the standardised block answers all of them. Same numbers — the frozen corpus agrees to 1.3e-12 relative on its near-collinear case and to 3.4e-15 or better on the other five, inside the 1e-9 it is compared at — and the twelfth significant digit of the reference page's 6e4 example moved with it. Measured on 10 000 rows and four regressors, an AMD Ryzen 7 8700G on Ubuntu 26.04.1 with .NET SDK 10.0.401: 3 628 µs to 1 646 µs and 6 490 KB to 2 893 KB, which turns 0.75× against `Accord.Statistics` into 1.63×. ([#591](https://github.com/CyrilB1531/lodestar/issues/591))

#### Added

- **A fourteenth package: ordinary least squares with the table that makes it inference.** `OrdinaryLeastSquares.Fit` takes a row-major design and a response and returns an `OlsSummary` carrying the coefficients, their standard errors, t statistics, two-sided p-values and confidence intervals at a stated level, alongside R-squared and its adjusted form, the overall *F* and its p-value, the residual standard error and degrees of freedom, and a variance inflation factor per regressor — at `statsmodels` 0.15.0 parity, replayed over six frozen cases and compared **relatively**, as [decision 0081](docs/decisions/0081-the-stats-numerical-layer-stays-internal.md) established for p-values. Solved through the Householder QR `Lodestar.Decomposition` 0.2.0 publishes rather than the normal equations, whose `XᵀX` squares the condition number of exactly the near-collinear designs a VIF exists to report; the tails come from `Lodestar.Stats` 0.2.0. Core tier holds: two Lodestar edges and nothing external. **The #427 protocol was run before the code, and it replaced the gap claim rather than confirming it** — read through a `MetadataLoadContext`, `Accord.Statistics` 3.8.0 exports the whole inference table and has been archived under LGPL-2.1 since 2017, while `MathNet.Numerics` 5.0.0 carries coefficient standard errors only in `Optimization.NonlinearMinimizationResult`, for the non-linear minimisers, unreachable from `LinearRegression` and followed by no t, no p-value, no interval, no adjusted R-squared, no *F* and no VIF across 5 333 exported members. [Decision 0096](docs/decisions/0096-ordinary-least-squares-earns-its-own-package.md) has the reading, and records that **`statsmodels` 0.15.0 changed what a VIF is**: `standardize=True` centres and scales each column before the auxiliary regression, which is a no-op with an intercept and moves the no-intercept answer from `87.43` to `21.0`. One divergence, in `docs/equivalence.md`: a single regressor with no intercept leaves the auxiliary design empty, where the reference raises and this reports `NaN`. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))

### Lodestar.Stats

#### Added

- **`Distributions.ChiSquaredSf` joins the four tails `Lodestar.Stats` already publishes.** [Decision 0095](docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) published four members and wrote that *publishing later is always available*; [#569](https://github.com/CyrilB1531/lodestar/issues/569)'s log-rank test is the second caller to ask, so [decision 0097](docs/decisions/0097-the-chi-squared-tail-joins-the-published-four.md) applies that rule rather than amending it. The member is the same tail `ChiSquare.GoodnessOfFit` and `ChiSquare.Contingency` already report, so a statistic routed either way gives the same p-value to the last bit — asserted by a test, and the reason re-deriving a tail inside the new package was refused on [decision 0081](docs/decisions/0081-the-stats-numerical-layer-stays-internal.md)'s own ground. Unlike the three before it, **`x` is not validated**: the distribution has no mass below zero, so a non-positive statistic returns one instead of surfacing an internal helper's parameter name from a public method. Six cases join `tests/oracles/stats_distributions.json`, reaching `7.7e-26` and compared relatively. Everything else in the numerical layer stays internal. ([#569](https://github.com/CyrilB1531/lodestar/issues/569))

- **`Distributions.NormalQuantile` joins them, and `Lodestar.Stats` reaches 0.4.0.** [#569](https://github.com/CyrilB1531/lodestar/issues/569)'s Kaplan-Meier confidence bounds are built on the log-log transform of the estimate, and their multiplier is `scipy.stats.norm.ppf`. The obvious way to avoid publishing anything — a Student quantile at a very large degrees of freedom, which is already public — was tried and **measured**: its accuracy has an optimum near 1e8 degrees of freedom and a floor around **9e-9**, because below that the convergence to the normal is incomplete and above it the bisection loses more than it gains. The transform amplifies that into the seventh digit of a bound, past the `1e-9` the corpora compare at, and the frozen corpus is what caught it. This member answers to about `1e-15`. Two details carry over from publishing `StudentQuantile`: it is the **quantile, not the inverse survival function**, so the published member negates the internal helper — symmetry, not a correction — and the median returns **positive** zero rather than `-0`. A test pins the substitute's failure alongside the member's success, so the measurement cannot rot silently. Everything else in the numerical layer stays internal ([decision 0098](docs/decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)). ([#569](https://github.com/CyrilB1531/lodestar/issues/569))

### Lodestar.Survival

#### Added

- **A fifteenth package, and the first survival analysis on NuGet.** `KaplanMeier.Estimate` returns the survival function with its step table — a time, the risk set before it, and what happened at it — plus Greenwood's variance and confidence bounds on the **log-log transform**, which is lifelines' default and not the plain Greenwood interval: that one reaches `1.0067` at `S = 0.857` on 21 subjects, outside what a probability can be. `NelsonAalen.Estimate` returns the cumulative hazard over the same step table, and **its tie increment is not `d/n`** — with `d` events at one time it is `Σ 1/(n - i)`, so three events among 21 at risk give `0.150251` rather than `0.142857`, a 5% difference at the first step of the trial every survival text opens with. `LogRank.Test` compares two arms with the hypergeometric variance under ties, and takes its p-value from `Distributions.ChiSquaredSf` rather than re-deriving a tail. Right censoring only; left truncation, interval censoring, Cox regression and the accelerated-failure-time models are each their own lot.

  **Core tier holds**: one Lodestar edge, to `Lodestar.Stats` 0.4.0 for the two members [decision 0097](docs/decisions/0097-the-chi-squared-tail-joins-the-published-four.md) and [decision 0098](docs/decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md) published for it, and nothing external.

  **There is no incumbent to compare against, and that is recorded rather than assumed.** A NuGet search on 2026-09-09 returned 0 packages for `survival analysis` and 0 for `kaplan meier`, so [decision 0099](docs/decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md) discharges [ADR 0074](docs/decisions/0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s protocol by recording the searches — there was no assembly to read through a `MetadataLoadContext` — and `bench/README.md` section 20 is that absence rather than a blank. **`scikit-survival` is refused** on GPL-3.0-or-later per [decision 0003](docs/decisions/0003-provenance-and-licensing.md); `lifelines` 0.30.3 is MIT, read from the wheel because `autograd` and `autograd-gamma` both report `License: UNKNOWN` in legacy metadata. Adding it drops `pandas` to 2.3.3 with **all 127 corpora unmoved**, and pins `autograd-gamma` to 0.4.2 because 0.5.0 ships no wheel. Replays `tests/oracles/survival_curves.json` (8 samples) and `survival_logrank.json` (5 comparisons), with ties on purpose in half of them. ([#569](https://github.com/CyrilB1531/lodestar/issues/569))

## Released — 2026-09-10

Two minor bumps and nothing else: the numerical members
[decision 0095](docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
published so that `Lodestar.Stats.Regression` could floor on them. `src/` reaches its
neighbours through published packages, so this cut is what stands between the two pull
requests [#566](https://github.com/CyrilB1531/lodestar/issues/566) needed.

### Lodestar.Stats — 0.2.0

#### Added

- **`Distributions` publishes three tails, and no more.** `StudentSf`, `StudentQuantile` and `FisherSf` are what an OLS table needs — a p-value per coefficient, a multiplier per interval, and the overall *F* — and they become public so `Lodestar.Stats.Regression` can reach them rather than carry a second copy. [Decision 0081](docs/decisions/0081-the-stats-numerical-layer-stays-internal.md) kept this layer internal and named the condition for changing that; [decision 0095](docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) exercises it and holds the line at four members: the log-gamma, the incomplete beta and gamma, the normal tail and the Kolmogorov machinery stay internal. **Paying 0081's stated price found a defect.** It asked for "a corpus at the tolerance a general-purpose caller would need, not the one the ten tests above it happen to need" — and that corpus, reaching `3.1e-24` and compared relatively, showed that the internal `Beta.StudentQuantile` solves `P(T > x) = p` and therefore carries the **opposite sign to a quantile**. Nothing internal noticed, because its one caller wanted exactly that; published unchanged it would have handed a reader `-2.1788` where every table prints `+2.1788`, and an interval built on it would have been reflected through its own estimate. `Distributions.StudentQuantile` publishes `scipy.stats.t.ppf`'s meaning, and the negation is the distribution's symmetry rather than a correction. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))

### Lodestar.Decomposition — 0.2.0

#### Added

- **`QrDecomposition` publishes the thin Householder QR this package already writes.** `Householder` returns `Q` and `R` row-major with the shape a least-squares solve wants — `m × n` and `n × n` — beside `RowCount` and `ColumnCount`. It is published rather than copied because `Lodestar.Stats.Regression` needs the same kernel, and two hand-written QRs in one repository disagree eventually. The LU and the one-sided Jacobi SVD beside it stay internal. No new corpus: `tests/oracles/decomposition_qr.json` already exists and the internal kernel replays it, so what the public wrapper owes is its shape and its refusals — a wide matrix has no thin QR and is refused rather than answered with a factorization of a different shape. The signs are the reflections', not a convention, and the page says so where a reader comparing against `numpy.linalg.qr` would otherwise expect entry-for-entry agreement. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))

## Released — 2026-09-08

Five deliverables in one cut. `Lodestar.Embeddings` reaches 0.6.0 rather than 0.5.0:
that number went to the feed on 2026-09-03 and nuget.org is immutable, so the two
public removals below could only land above it — the section for 0.5.0 is reconstructed
further down. `Lodestar.Onnx` publishes the satellite it was split into, against that
same published 0.5.0, and `Lodestar.Stats` its first release. `Lodestar.Decomposition`
takes a patch: `Nmf.Fit`'s widened bound removes nothing a caller could hold.

### Lodestar.Text — 0.5.0

#### Added

- **`Lodestar.Text.Phonetics` adds `MatchRatingApproach`, the Match Rating Approach (Western Airlines, 1977).** `Codex` reduces a name to its Match Rating code, and `Compare` decides whether two names' codes rate as a match — a `bool?`, `null` when the codes are too far apart in length to rate at all, rather than the codex either could give a caller on its own: the minimum rating a comparison must clear is read from a table keyed by their **combined** length. Written from the published algorithm description rather than a reference implementation (ADR 0003), and unlike `Soundex`, `Metaphone` and `Nysiis` next door, a character that is neither a Unicode letter nor a space is refused (`ArgumentException`) rather than ignored. Replays `jellyfish` 1.2.1's `match_rating_codex` (420 words) and `match_rating_comparison` (212 pairs); [decision 0080](docs/decisions/0080-match-rating-approach-comparison-uses-character-length-not-byte-length.md) records the one divergence found and not reproduced — jellyfish measures a codex's length in UTF-8 bytes rather than characters for a handful of non-Latin inputs. ([#313](https://github.com/CyrilB1531/lodestar/issues/313))

- **`BkTree`** is a metric index over the integer distances, worth building only at a radius of 1 — [`docs/guides/dictionary-lookup.md`](docs/guides/dictionary-lookup.md) has the measurement. ([#526](https://github.com/CyrilB1531/lodestar/issues/526))

- **`Lodestar.Text.Keywords` adds two unsupervised keyword extractors, `Rake` and `TextRank`.** Neither trains or downloads a model: `Rake` scores the runs between stop words by their word co-occurrence, and `TextRank` ranks a co-occurrence graph of the document's own stems the way PageRank ranks a link graph. Each replays a frozen oracle against its Python reference — `rake-nltk` 1.0.6 in `tests/oracles/keywords_rake.json`, `summa` 1.2.0 in `tests/oracles/keywords_textrank.json` — and [decision 0077](docs/decisions/0077-the-keyword-extractors-take-their-oracles-lists-and-not-their-own.md) records where each is measured to diverge from it. [`docs/guides/keyword-extraction.md`](docs/guides/keyword-extraction.md) has both, plus the KeyBERT-style composition with `Mmr.Select`. ([#525](https://github.com/CyrilB1531/lodestar/issues/525))

#### Changed

- **`CsrMatrix` and `SparseNorm` moved to `Lodestar.Abstractions`.** Consuming code adds `using Lodestar.Abstractions;`; the vectorizers still return the same type, and the seven reference pages moved with it. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))

- `Lcs.SubsequenceLength` takes the bit-parallel route from a pattern of 2 characters and `Levenshtein.Distance` from 5, while a pattern holding a character above U+00FF is refused below 6 and 10 instead. ([#411](https://github.com/CyrilB1531/lodestar/issues/411), [`a5c0d52`](https://github.com/CyrilB1531/lodestar/commit/a5c0d52))

#### Fixed

- **The TextRank oracle no longer freezes a tie order that floating-point noise decides.** Two adjacent entries of `tests/oracles/keywords_textrank.json` sat one or two units in the last place apart, so the reference's descending sort ordered them by noise and a runner's BLAS broke the near-tie the other way — failing *Oracles are reproducible* on pull requests that touch nothing near it. The generator now sorts each run of scores tied within `1e-9` by phrase, which is the model `TextRankOracleTests` already applies on replay, so corpus and test agree by construction rather than by the test quietly absorbing one machine's order. No score moved. ([#541](https://github.com/CyrilB1531/lodestar/issues/541), [decision 0079](docs/decisions/0079-tied-textrank-scores-canonicalize-by-phrase-not-blas.md))

- The blocked bit-parallel equality table is sized from the pattern's characters above U+00FF rather than from its length, and a pattern too long to tabulate takes the dynamic program instead of wrapping the table's length in `int`. ([#413](https://github.com/CyrilB1531/lodestar/issues/413), [`52d68cc`](https://github.com/CyrilB1531/lodestar/commit/52d68cc))

### Lodestar.Embeddings — 0.6.0

#### Added

- **`Mmr.Select` (`Lodestar.Embeddings.Search`) picks a diverse, relevance-weighted subset of candidate vectors — Maximal Marginal Relevance**, knowing nothing about text: the candidates are vectors and the result is their indices, in selection order. It replays `keybert` 0.9.0's own selection step, `keybert._mmr.mmr`, compared as a set rather than a sequence (`tests/oracles/mmr.json`) — [decision 0077](docs/decisions/0077-the-keyword-extractors-take-their-oracles-lists-and-not-their-own.md) has the three divergences, and [decision 0078](docs/decisions/0078-keybert-is-declared-nodeps-not-compiled-into-the-lock.md) why `keybert` itself stays out of the oracle lock file. Composes with `Rake` and `OnnxTextEmbedder` into a KeyBERT-style pipeline, walked through in [`docs/guides/keyword-extraction.md`](docs/guides/keyword-extraction.md). ([#525](https://github.com/CyrilB1531/lodestar/issues/525))

- **`LoadBpe` reads a `TemplateProcessing` post-processor instead of refusing the file, which is what let Llama-2 and Mistral v0.1 load at last.** Neither `Metaspace` nor `byte_fallback` was the obstacle — both shipped earlier — but a `post_processor` section was refused outright, and all three reference files carry one. `BpeVocabulary.PrefixTokens` and `BpeVocabulary.SuffixTokens` now carry what the `single` template puts around the text, `["<s>"]` and nothing for both models; they are public because the caller composes the `SpecialTokenTemplate` itself, that type needing a pad token the file does not declare. The `pair` template is read and discarded — [decision 0083](docs/decisions/0083-the-pair-template-is-read-and-discarded.md) has the three files measured before any code was written, and why reproducing a two-sequence encoding would mean inventing a type to express it. ([#548](https://github.com/CyrilB1531/lodestar/issues/548))

#### Removed

- **The `SentencePieceTokenizer(IReadOnlyList<SentencePiece>, int)` constructor is gone.** It guessed which pieces were controls from their ids being 0, 1 or 2, which is wrong for any model laying them out differently; `SentencePieceTokenizer(SentencePieceVocabulary)` is told instead, by the file. It carried an `[Obsolete]` since 0.4.0 — **whose message named v2.0.0, and this removal lands in 0.6.0 instead**: v2.0.0 is not a release this project has planned, and waiting for one would mean shipping a constructor that is wrong by construction indefinitely. A caller still on it passes `SentencePieceModelLoader.Load` or `TokenizerJsonLoader.LoadUnigram` output to the remaining constructor, or builds a `SentencePieceVocabulary` with the piece types it knows. ([#539](https://github.com/CyrilB1531/lodestar/issues/539))

- **`OnnxTextEmbedder` moved to the new `Lodestar.Onnx` package, and this one now carries no external dependency at all.** `Microsoft.ML.OnnxRuntime` 1.28.0 was the repository's only external dependency and was reached by one file of 407 lines, while the four sub-word tokenizers, the batch encoder, the pooling, the `.npy` reader and the SIMD kNN index could not be had without it — `dotnet add package Lodestar.Embeddings` restored a native runtime for a caller who only tokenizes. Migration is one `using`: the type is `Lodestar.Onnx.OnnxTextEmbedder`, with the same members and the same behaviour, in a package that depends on this one. [Decision 0076](docs/decisions/0076-a-core-package-carries-no-external-dependency.md) states the rule it settles — a core package carries no external dependency, an external dependency earns its own satellite package — supersedes [0069](docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md), and records what was refused. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))

### Lodestar.Onnx — 0.1.0

#### Added

- **First release, 0.1.0: ONNX inference, and the satellite tier's first member.** One type, `OnnxTextEmbedder`, moved verbatim from `Lodestar.Embeddings` into namespace `Lodestar.Onnx` — every package sets `RootNamespace` equal to its `PackageId`, and the rename is also what let the split land without colliding with the copy published in `Lodestar.Embeddings` 0.4.0 and 0.5.0. It depends on `Lodestar.Embeddings` 0.5.0 for the tokenizers, the encoding options and the pooling it feeds a session with, and on `Microsoft.ML.OnnxRuntime` 1.28.0, which no other package in the repository now references. Ships `net10.0;netstandard2.0` like the rest. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))

### Lodestar.Decomposition — 0.1.1

#### Changed

- **`Nmf.Fit(matrix, k)` accepts `k == min(rows, columns)`**, scikit-learn's own bound, where it refused any `k` at or above the column count — a bound inherited from the validation `TruncatedSvd` needs rather than from anything NMF does, so a square matrix at full rank was a fit there and an `ArgumentOutOfRangeException` here. The oracle corpus now freezes a `24 × 8` fit at `k = 8` against `NMF` itself, and `TruncatedSvd`'s own bound is untouched: `n_components >= n_features` is what scikit-learn refuses there too. ([#519](https://github.com/CyrilB1531/lodestar/issues/519))

### Lodestar.Stats — 0.1.0

#### Added

- **`Lodestar.Stats` is a new package: ten families of classical hypothesis

  test at `scipy.stats` 1.18.0 parity.** Student and Welch *t*, Mann-Whitney
  *U*, Wilcoxon signed-rank, χ² goodness-of-fit and contingency, Fisher exact,
  two-sample Kolmogorov-Smirnov, one-way ANOVA, Kruskal-Wallis, Shapiro-Wilk,
  and the Bonferroni, Benjamini-Hochberg and Benjamini-Yekutieli corrections.
  Core tier: the tail probabilities come from the package's own internal
  log-gamma, incomplete beta and incomplete gamma rather than from a numerical
  dependency. `TTest.Independent` defaults to Welch where `scipy` defaults to
  Student, which is the one deliberate divergence and has its row in
  [`docs/equivalence.md`](docs/equivalence.md). Ten frozen corpora, each case
  carrying the full argument set it was generated with, and p-values compared
  at `1e-9` **relative** — ordinary cases reach `2.38e-53`, where the
  repository's absolute tolerance would accept a zero.
  ([#442](https://github.com/CyrilB1531/lodestar/issues/442), decision
  [0081](docs/decisions/0081-the-stats-numerical-layer-stays-internal.md))

## Released — 2026-09-03

One package, published to nuget.org and never tagged, so it had no section here — the
same gap the 2026-09-01 wave was reconstructed for, and filled the same way: each entry
is filed under the release its own commit is an ancestor of. `Lodestar.Embeddings` 0.5.0
is what `Lodestar.Onnx` 0.1.0 depends on, and `src/Directory.Packages.props` pins.

### Lodestar.Embeddings — 0.5.0

#### Added

- **`BatchEncoder.EncodeAll` and `BatchEncoder.Pad` are public**, so a caller that groups rows itself no longer needs a second copy of the padding. `EncodeAll` returns one unpadded row per text, template applied and truncation done; `Pad` lays a **window** of those rows out as one rectangle, widened to the longest row in that window rather than in the corpus — which is what makes grouping by length worth anything. `EncodeBatch` is unchanged, and is still the two of them over the whole corpus at once. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))

- `EmbeddingIndex.FromBlock` and `EmbeddingIndex.FromOwnedBlock` build an index from a contiguous block of vectors in one copy or none, where replaying the block through `Add` cost three times the read that produced it — the adopting factory keeps the caller's array for the life of the index, an invariant the caller keeps and [decision 0056](docs/decisions/0056-a-block-may-be-adopted-and-the-invariant-is-the-callers-to-keep.md) argues for. ([#474](https://github.com/CyrilB1531/lodestar/issues/474), [`13bdacc`](https://github.com/CyrilB1531/lodestar/commit/13bdacc))

- `bench/Lodestar.Text.Benchmarks -- sidecar` prices a binary sidecar against the JSON artifact in bytes and in time, and [decision 0055](docs/decisions/0055-the-artifact-gets-a-binary-sidecar-once-a-block-can-be-ingested-whole.md) takes one — conditional on a bulk ingest into `EmbeddingIndex`, without which the sidecar route is slower than what it replaces. No shipped behaviour changes yet. ([#436](https://github.com/CyrilB1531/lodestar/issues/436), [`7ab80d1`](https://github.com/CyrilB1531/lodestar/commit/7ab80d1))

- **numpy's `.npy` reads and writes, for the vector block only.** `NpyFile.Read` and `NpyFile.Write` carry a contiguous `float32` block in numpy's own format, returning an `NpyBlock` of the values and the shape; the header is parsed against a fixed grammar and never evaluated, so `descr: '|O'` — numpy's pickle-backed dtype — is refused by name before the payload is touched. It is interop and not a second artifact format: a `.npy` carries no ids, no normalize flag and no schema, so `EmbeddingIndex.Save` is untouched and [decision 0011](docs/decisions/0011-persistence-format.md) is not reopened. ([#450](https://github.com/CyrilB1531/lodestar/issues/450), [`0f05972`](https://github.com/CyrilB1531/lodestar/commit/0f05972))

- **`byte_fallback` resolves an uncovered symbol into `<0xXX>` byte pieces instead of the unknown token, so Llama-2 and Mistral v0.1 both load.** `BpeVocabulary.ByteFallback` and `TokenizerJsonLoader.LoadBpe` require the vocabulary to carry all 256 pieces, refusing by name a file that does not rather than reproduce the silent degradation — or, with no unknown token declared, the dropped symbol — `tokenizers` 0.23.1 falls back to; the expansion runs before the merges, on the decorated symbol, so a `continuing_subword_prefix` or `end_of_word_suffix` on it is itself encoded as bytes. `BpeTokenizer.Decode` now reproduces such a file's `decoder` block too, a bare `ByteFallback` or Llama-2's own `Sequence[Replace, ByteFallback, Fuse, Strip]`, round-tripping the byte pieces and the whitespace escape together — [decision 0063](docs/decisions/0063-byte-fallback-requires-the-whole-alphabet-and-its-decoder-is-read-strictly-too.md) has the measurements against the reference, including an upstream ordering bug found and not reproduced. ([#317](https://github.com/CyrilB1531/lodestar/issues/317), [`6b4f2b6`](https://github.com/CyrilB1531/lodestar/commit/6b4f2b6))

#### Changed

- **The `.npy` read copies the block once, and a second entry point copies it none.** `NpyFile.Read(Stream)` reads the payload straight into the `float[]` the returned block keeps, where it used to stage the same bytes through two buffers first, and names that array as `NpyBlock.OwnedArray` so `EmbeddingIndex.FromOwnedBlock` can adopt it rather than copy the block once more; `NpyFile.Read(ReadOnlyMemory<byte>)` serves a caller already holding the file by **aliasing** those bytes, which must not change while the block lives, and leaves `OwnedArray` null because a borrowed block has no array to hand on. Reading the same 15 360 128 bytes against `np.load` measured 0.21–0.23× of numpy's wall time with three copies between the stream and the block, and a fourth into the index that held it; on the adopting route it now measures **1.00–1.13× cpu and 1.21–1.25× wall** — parity in the first round and slightly ahead in the other two on cpu, the column this project trusts, where it was four to five times behind. The stream read is one copy on `net10.0` and two on `netstandard2.0`, which has no `Stream.Read(Span<byte>)` to read into a caller's array — one API and one behaviour at two speeds, as [decision 0057](docs/decisions/0057-the-npy-read-serves-a-stream-and-a-buffer-differently.md) records with the view on every path it refused. ([#466](https://github.com/CyrilB1531/lodestar/issues/466), [`a3d3145`](https://github.com/CyrilB1531/lodestar/commit/a3d3145))
- **The payload buffer is rented, not allocated.** `EmbeddingIndex.Load(Stream)` takes its artifact buffer from `ArrayPool<byte>.Shared` and returns it once parsing is done, which removes 20.5 MB of allocation and three of the four collections a load provoked: renting is **42× the allocation and 1.74 ms a load**, about a tenth of one, because what cost was never the allocation but the large-object collection it triggered. The pool holds 33.5 MB for the life of the process in exchange — see [decision 0054](docs/decisions/0054-the-payload-buffer-is-pooled-after-all-because-the-collection-is-the-cost.md), which amends [0053](docs/decisions/0053-the-payload-buffer-is-not-pooled-because-residency-outlives-the-load.md) for refusing that trade without ever timing it. ([#470](https://github.com/CyrilB1531/lodestar/issues/470), [`f8de2ba`](https://github.com/CyrilB1531/lodestar/commit/f8de2ba))
- **Half the allocation, same bytes on disk.** `EmbeddingIndex.Save` and `SaveAsync` write the vector block a slice at a time instead of handing `Utf8JsonWriter.WriteBase64String` the whole thing, so the writer's buffer no longer doubles its way up to the 20.48 MB the encoding occupies: `EmbeddingIndexSave` allocates **19.87 MB against 39.64**, with a third fewer collections in every generation, and the row against `numpy.save` moves **0.29× to 0.39×**. Slices are 245 760 bytes — a multiple of 12, so a whole number of base64 groups and of floats — which is what makes the artifact byte-for-byte what it was; `SaveAsync` loses its intermediate `MemoryStream` with it. The load pays part of it back, having been subsidised by the buffer the save used to leave behind — see [decision 0051](docs/decisions/0051-the-save-paths-cost-is-the-buffer-not-the-encoding.md), which also records why parallelising the base64 was refused: it runs at `memcpy` speed already. ([#430](https://github.com/CyrilB1531/lodestar/issues/430), [`2a50cc1`](https://github.com/CyrilB1531/lodestar/commit/2a50cc1))

#### Fixed

- `NpyFile.Read` bounds a block by `ArtifactLoadOptions.MaxTotalBytes` rather than by `MaxArrayLength`, which that option documents as not applying to a vector block: a 2 605 × 384 block — small for embeddings — was refused at the default options while the same vectors loaded from an index artifact. ([#468](https://github.com/CyrilB1531/lodestar/issues/468), [`c480c1f`](https://github.com/CyrilB1531/lodestar/commit/c480c1f))

## Released — 2026-09-01

Four tags on one day, and none of them had a section here: the three packages below
kept their entries under *Unreleased* while their releases were already on the feed.
Each entry is filed under the tag its own commit is an ancestor of, which is how the
2026-08-16 wave was reconstructed too. `Nmf.Fit`'s component bound is not here because
it landed after `Lodestar.Decomposition/v0.1.0` was cut; it ships in 0.1.1 above.

### Lodestar.Abstractions — 0.1.0

#### Added

- **The sparse primitive the packages share.** `CsrMatrix` and `SparseNorm` ship in a package of their own, with two new products — `Multiply(block, columnCount)` and `TransposeMultiply(block, columnCount)` — that read the matrix once per non-zero rather than once per block column. `Lodestar.Text` still declares its own copy until its next release; [decision 0071](docs/decisions/0071-csrmatrix-moves-to-an-abstractions-package.md) amends [0069](docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md) and records the sequence. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))

### Lodestar.Conformal — 0.1.0

#### Added

- **Split conformal prediction, at MAPIE 1.5.0 parity.** `SplitConformal` turns a point prediction into an interval or a class into a prediction set, with a finite-sample coverage guarantee: `AbsoluteResiduals` and `LeastAmbiguousScores` score a calibration set, `Quantile` reduces the scores to the one number that carries the guarantee, and `Interval` and `PredictionSet` apply it. Static and dependency-free, the fifth package under [decision 0069](docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md)'s first rule. The empty LAC prediction set is reproduced rather than repaired, and `k > n` returns an infinite interval instead of MAPIE's clamp to the widest score — [decision 0070](docs/decisions/0070-k-greater-than-n-returns-an-infinite-interval.md). The guarantee assumes exchangeability, which the guide leads with. ([#441](https://github.com/CyrilB1531/lodestar/issues/441))

### Lodestar.Decomposition — 0.1.0

#### Added

- **`TruncatedSvd` — `sklearn.decomposition.TruncatedSVD(algorithm="randomized")` at parity, over a `CsrMatrix` and without centring it.** Fit, transform, components, singular values, explained variance and its ratio; all three power-iteration normalizers, including `Auto`'s rule. Ω is an input rather than a seed, which is what makes a randomized algorithm an ordinary parity target — [decision 0072](docs/decisions/0072-omega-is-an-input-not-a-seed.md) has the measurement and what it refuses. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))
- **`Nmf` — `sklearn.decomposition.NMF(solver="mu")` at parity, on both β losses, from the NNDSVD family.** The dense kernels it needs — thin Householder QR, LU with partial pivoting, one-sided Jacobi SVD — are written here, so the package's only dependency is `Lodestar.Abstractions`. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))

### Lodestar.Abstractions — 0.1.1

#### Fixed

- **The shared internal helpers are no longer compiled into this package.** `src/Shared/Guard.cs` and its siblings are compiled into every library, and this one grants `InternalsVisibleTo` to `Lodestar.Text`, which compiles them too — one internal type in both assemblies is CS0436 at every call site on the consuming side, 96 of them across the two target frameworks. `CsrMatrix` carries the two argument guards it needs instead; behaviour and exception types are unchanged. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))

## Released — 2026-08-21

Four deliverables, cut in two steps on the same day. `Lodestar.Text`,
`Lodestar.Embeddings` and `Lodestar.Metrics` went first; `Lodestar.Fuzzy` followed once
`Lodestar.Text 0.4.0` was served by nuget.org, which is what
`src/Directory.Packages.props` requires before its floor may move — and moving that
floor is the whole of what Fuzzy publishes here, its source being untouched since
`0.3.1`.

### Lodestar.Text — 0.4.0

#### Changed

- **CJK and emoji take the bit-parallel path.** A pattern holding a character above U+00FF sent `Levenshtein.Distance`, `Lcs.SubsequenceLength` and therefore `Indel` and `fuzz.ratio` back to the dynamic program in the UTF-16 mode, because the equality table was indexed by the character. A side table now carries those symbols on both the single-word and the blocked route, so the kernels no longer refuse an alphabet — see [decision 0043](docs/decisions/0043-the-equality-table-is-sized-to-the-pattern.md). ([#302](https://github.com/CyrilB1531/lodestar/issues/302), [`649b8e6`](https://github.com/CyrilB1531/lodestar/commit/649b8e6))
- **Faster, same answers.** The blocked bit-parallel LCS kernel no longer threads a borrow between its 64-bit words. It never needed one: the subtrahend is `v & peq`, a bit-subset of `v`, and subtracting a subset cannot borrow — the chain had been carrying a provably zero value since #273. Measured **1.56×** at length 512 and **1.43×** at 128 on the pair corpus, interleaved over four replications with `Levenshtein` as an untouched control, which moves the two long buckets from roughly 2× behind rapidfuzz to 1.38× and 1.53×. ([#357](https://github.com/CyrilB1531/lodestar/issues/357), [`5a448a9`](https://github.com/CyrilB1531/lodestar/commit/5a448a9))
- **Faster, same answers.** The blocked bit-parallel LCS kernel — which `Indel` and therefore `fuzz.ratio` reach on long inputs — no longer calls a helper once per text character, and no longer re-tests inside its inner loop whether that character is one the equality table can hold. Measured **1.10×** on the pair corpus's length-512 bucket, interleaved over four replications with `Levenshtein` as an untouched control; the length-128 bucket is unchanged, its patterns spanning two machine words against eight. ([#320](https://github.com/CyrilB1531/lodestar/issues/320), [`1fa65f3`](https://github.com/CyrilB1531/lodestar/commit/1fa65f3))
- **Faster, same answers.** `Levenshtein.Distance`, `Lcs.SubsequenceLength` and therefore `Indel` and `fuzz.ratio` take the bit-parallel route from a pattern of 8 characters rather than 16, and neither kernel clears an equality table that `stackalloc` has already zeroed: on the pair corpus's length-32 bucket that is **2.09×** for Levenshtein (427.6 → 204.8 ns/pair) and **2.19×** for Indel (318.7 → 145.6 ns/pair), with every other bucket within noise and the 3 905-test suite unchanged. ([#208](https://github.com/CyrilB1531/lodestar/issues/208), [`cae6236`](https://github.com/CyrilB1531/lodestar/commit/cae6236))
- **Breaking.** `Soundex.Encode(string)`, `Nysiis.Encode(string)` and `Metaphone.Encode(string)` now throw `ArgumentNullException` on a `null` word instead of silently returning `""`, matching the stemmers next door — see [decision 0042](docs/decisions/0042-phonetic-encoders-refuse-a-null-word.md). ([#342](https://github.com/CyrilB1531/lodestar/issues/342), [`4000a05`](https://github.com/CyrilB1531/lodestar/commit/4000a05))

### Lodestar.Embeddings — 0.4.0

#### Added

- The embeddings guide documents how to compress an artifact, and what it costs: the caller wraps the stream on both sides, which works today and needed no library change. Deflate takes back the format's 1.33× base64 expansion almost exactly, at **26.67× the save and 7.19× the load**, and 76.8× and 14.8× on the benchmark corpus's larger index — so the library declines to do it by default, and [decision 0044](docs/decisions/0044-compression-belongs-to-the-caller.md) records why. `compare-persistence` now reports the bytes each row wrote or read, next to its time. ([#378](https://github.com/CyrilB1531/lodestar/issues/378), [`01642c9`](https://github.com/CyrilB1531/lodestar/commit/01642c9))
- `EmbeddingIndex.Load(ReadOnlyMemory<byte>)` reads an index from bytes the caller already holds — a blob, a cache entry, an embedded resource — where handing them to the `Stream` overload made the loader copy them back out first. Measured **1.40×** on processor time against that overload, both rows in the same run, which is the read phase [#324](https://github.com/CyrilB1531/lodestar/issues/324) profiled at about a third of the load. It checks `MaxTotalBytes` before parsing rather than while reading, the length being known up front, and has no `Async` counterpart because nothing is waited on. It is the only loader to gain one: the saving scales with the artifact and no other is large enough. ([#336](https://github.com/CyrilB1531/lodestar/issues/336), [`27fa908`](https://github.com/CyrilB1531/lodestar/commit/27fa908))

#### Changed

- **`EmbeddingIndex.LoadAsync` no longer refuses an index that `Load` accepts.** The segmented read #377 gave the synchronous path stopped there, so the same artifact past the CLR's array ceiling loaded one way and threw the other — a disagreement between two overloads of the same method rather than a missing feature. Both now take the same decision on the same threshold, and the chain they build is one implementation so they cannot drift apart again; a cancelled read throws instead of parsing a partial chain. ([#396](https://github.com/CyrilB1531/lodestar/issues/396), [`3a89dde`](https://github.com/CyrilB1531/lodestar/commit/3a89dde))
- **An index is no longer capped by the text encoding of its vectors.** An artifact past the CLR's array ceiling was read into one `byte[]` and could not be, so the format's 1.34× expansion came straight off the largest index that could exist — about 1.04 million vectors at 384 dimensions where the raw block allowed 1.40 million. `EmbeddingIndex.Load` now reads such an artifact in segments and hands the parser a `ReadOnlySequence<byte>`, which `Utf8JsonReader` reads natively. **The bytes on disk do not change**, so an artifact written by any earlier version still loads. ([#377](https://github.com/CyrilB1531/lodestar/issues/377), [`cfc1945`](https://github.com/CyrilB1531/lodestar/commit/cfc1945))
- **Faster, same answers.** Loading an artifact no longer has the runtime zero the two large buffers it overwrites in full — the payload the stream fills and the vector block the base64 decoder fills. Measured **1.18×** on `embedding_index_load`, with a write-only operation re-run as an untouched control; a small artifact such as a fitted vectorizer sees nothing, its buffers never reaching the large-object heap. ([#324](https://github.com/CyrilB1531/lodestar/issues/324), [`359d889`](https://github.com/CyrilB1531/lodestar/commit/359d889))
- **Faster, same bytes.** `EmbeddingIndex.Save` no longer allocates and copies the whole vector block before encoding it: on a little-endian machine the bytes to base64 are the ones already in the span, and the copy existed only to carry an endianness swap that is a no-op there. Measured **1.46×** on processor time at the benchmark's size, with the load direction re-run as an untouched control, and the encoding pinned byte for byte by a new test. ([#323](https://github.com/CyrilB1531/lodestar/issues/323), [`4359d32`](https://github.com/CyrilB1531/lodestar/commit/4359d32))

### Lodestar.Metrics — 0.3.0

#### Added

- `docs/guides/metrics.md` answers which metric to reach for, which the per-member reference pages deliberately cannot: a router across the four families, and the four things true of all of them — row-major input with a count, `sampleWeight` as a weighted mean, `ZeroDivision` as an argument rather than a warning, and the answers that look like bugs and are scikit-learn's. ([#203](https://github.com/CyrilB1531/lodestar/issues/203), [`8aaa19a`](https://github.com/CyrilB1531/lodestar/commit/8aaa19a))

#### Added — ranking

- `Dcg`, `Ndcg` and `TopKAccuracy` score an ordered list of documents at scikit-learn parity, tie handling included: equal scores have their discounted gain averaged over the permutations of the tie by default, which on a row whose four scores are equal is `0.8069…` against `0.6138…` for `ignoreTies: true`. ([#173](https://github.com/CyrilB1531/lodestar/issues/173), [`8f3fda1`](https://github.com/CyrilB1531/lodestar/commit/8f3fda1))
- `ReciprocalRank` scores rankings by the position of their first relevant document — the one member of this package **not verified against a reference**, because `sklearn.metrics` has no counterpart to freeze; its definition is pinned by tests under [`docs/decisions/0036`](docs/decisions/0036-a-member-may-ship-without-an-oracle-if-it-says-so.md), which also says what would retire the exception. ([#173](https://github.com/CyrilB1531/lodestar/issues/173), [`8f3fda1`](https://github.com/CyrilB1531/lodestar/commit/8f3fda1))
- `CoverageError`, `LabelRankingLoss` and `LabelRankingAveragePrecision` score a boolean label matrix at scikit-learn parity, the two places the reference disagrees with itself included: a single label column is accepted by the average precision and refused by the other two, and a weight vector summing to zero gives `NaN` there where the other two raise. ([#201](https://github.com/CyrilB1531/lodestar/issues/201), [`eec79dd`](https://github.com/CyrilB1531/lodestar/commit/eec79dd))
- A sample with no relevant label contributes `0` to `CoverageError` rather than the label count, so its mean can sit below `1` — measured, `0.5` on two samples one of which is empty; a tie between a relevant and an irrelevant label counts as an error in `LabelRankingLoss`, so a sample whose scores are all equal scores `1`. ([#201](https://github.com/CyrilB1531/lodestar/issues/201), [`eec79dd`](https://github.com/CyrilB1531/lodestar/commit/eec79dd))
- `Dcg.Score`, `Ndcg.Score` and `TopKAccuracy.Score` take a `sampleWeight`, which the reference has always had and these three did not — three rows of `docs/equivalence.md` called them identical anyway. With weights `TopKAccuracy`'s `normalize: false` returns the **sum of the weights** of the hits rather than how many there are, measured `7.0` against the unweighted `3.0`, and because that path never divides it does not refuse a zero-sum vector at all, where the fraction does — what it returns there is the weighted sum of the hits, `3.0` on weights `[1, 1, 1, -3]` whose total is zero. ([#216](https://github.com/CyrilB1531/lodestar/issues/216), [`e2b62e3`](https://github.com/CyrilB1531/lodestar/commit/e2b62e3))

#### Changed

- **Faster, same answers.** `MeanSquaredError`, `MeanAbsoluteError` and `RootMeanSquaredError` accumulate through `Vector<double>` on `net10.0` when there is a single output, which is the rule [decision 0027](docs/decisions/0027-r2-and-explainedvariance-vectorize-only-a-single-output.md) already set for `R2` and `ExplainedVariance`: **1.65×** on `mse` and **1.60×** on `mae` at a million rows, with `r2` re-run as an untouched control. The lanes reduce in a different order from a scalar loop, so the values can differ in their last bits; the frozen scikit-learn corpora pass unchanged at their `1e-9` comparison. ([#321](https://github.com/CyrilB1531/lodestar/issues/321), [`36ec36e`](https://github.com/CyrilB1531/lodestar/commit/36ec36e))
- **Numerical change, under `1e-14`.** `NormalizedMutualInformation`, `Homogeneity`, `Completeness` and `VMeasure` return slightly different values on inputs where one labelling is a single cluster: the shared mutual-information term now zeroes each contribution below the machine epsilon before summing and returns `0.0` outright when either side has one label, both of which the reference does. The old values were up to `5.13e-15` from scikit-learn's and the new ones are exact, so anything comparing at the corpus tolerance of `1e-9` is unaffected — this is recorded because the values moved, not because a caller should have to react. ([#191](https://github.com/CyrilB1531/lodestar/issues/191), [`43b4368`](https://github.com/CyrilB1531/lodestar/commit/43b4368))

#### Fixed — ranking

- `Dcg.Score` refuses a `logBase` outside `(0, ∞)` instead of returning a silent `NaN`: zero, a negative, `NaN` and infinity now raise `ArgumentOutOfRangeException`, which is where `dcg_score` raises too. A base below `1` is still accepted, and still takes the score negative. ([#215](https://github.com/CyrilB1531/lodestar/issues/215), [`1eff5e5`](https://github.com/CyrilB1531/lodestar/commit/1eff5e5))

### Lodestar.Fuzzy — 0.4.0

#### Changed

- **`fuzz.ratio` and `process.extract` now require the kernels they were made faster by.** The floor on `Lodestar.Text` moves from `0.3.1` to `0.4.0`, so a caller who references only `Lodestar.Fuzzy` stops resolving a `Lodestar.Text` that predates #208, #320, #357 and #302. No source file changes; `Lodestar.Text 0.4.0` also refuses a `null` word in the phonetic encoders, which a consumer of both packages meets here. ([#415](https://github.com/CyrilB1531/lodestar/issues/415), [`8a1573c`](https://github.com/CyrilB1531/lodestar/commit/8a1573c))

## Released — 2026-08-16

The rename from `DataNet.*` to `Lodestar.*`, and the reference pages that went out
with it. Five tags were cut and none of them had a section here until the 0.4.0
release went looking for one; each entry below is filed under the tag its own commit
is an ancestor of.

### Lodestar.Text — 0.3.1

#### Added

- `docs/reference/text/distances.md` documents every type of `Lodestar.Text.Distances` in the layout of the .NET API reference, and a test checks each declaration, parameter list and `Applies to` against the assembly. ([#181](https://github.com/CyrilB1531/data.net/issues/181), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))

#### Changed

- The package is `Lodestar.Text`, and its namespaces are `Lodestar.Text.*`. `DataNet.Text 0.3.0` and `Lodestar.Text 0.3.1` hold the same code: the id changed, nothing else did. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`3a9931a`](https://github.com/CyrilB1531/lodestar/commit/3a9931a))
- `DamerauLevenshtein`'s documented summary no longer says "Not a proper metric": unit-cost unrestricted Damerau-Levenshtein satisfies the triangle inequality and is a true metric; `Osa` is the one that does not. ([#181](https://github.com/CyrilB1531/data.net/issues/181), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))
- The reference is one page per member, with a type page and a namespace index above it: `docs/reference/text/distances.md` becomes 9 type pages and 22 member pages, and the index a reader lands on is 64 lines rather than 1034. ([#189](https://github.com/CyrilB1531/data.net/issues/189), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))

### Lodestar.Text — 0.3.2

#### Changed

- The toolkit is `Lodestar`: the tags no longer say `datanet`, and every package carries an embedded icon rather than none. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`ec421f6`](https://github.com/CyrilB1531/lodestar/commit/ec421f6))

### Lodestar.Embeddings — 0.3.1

#### Changed

- The package is `Lodestar.Embeddings`, and its namespaces are `Lodestar.Embeddings.*`. `Lodestar.Embeddings 0.3.1` holds the same code as `DataNet.Embeddings 0.3.0`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

### Lodestar.Fuzzy — 0.3.1

#### Changed

- The package is `Lodestar.Fuzzy`, and its namespaces are `Lodestar.Fuzzy.*`. `Lodestar.Fuzzy 0.3.1` holds the same code as `DataNet.Fuzzy 0.3.0`, and its floor names `Lodestar.Text 0.3.1`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

### Lodestar.Metrics — 0.2.0

#### Added

- `docs/reference/metrics/classification.md` and `docs/reference/metrics/regression.md` document every type of `Lodestar.Metrics` in the layout of the .NET API reference, and the same test checks each declaration, parameter list and `Applies to` against the assembly. ([#181](https://github.com/CyrilB1531/data.net/issues/181), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))

#### Added — clustering

- `AdjustedRand`, `NormalizedMutualInformation`, `Homogeneity`, `Completeness` and `VMeasure` score a clustering against a reference partition at scikit-learn parity, degenerate cases included: an empty input and a single sample both score `1`, and two independent partitions score `-0.5` on adjusted Rand. ([#172](https://github.com/CyrilB1531/data.net/issues/172), [`3d10674`](https://github.com/CyrilB1531/lodestar/commit/3d10674))
- `Silhouette` scores a clustering with no reference partition, from the samples with the euclidean distance or from a distance matrix already computed, per sample or as their mean. ([#172](https://github.com/CyrilB1531/data.net/issues/172), [`714dd80`](https://github.com/CyrilB1531/lodestar/commit/714dd80))

#### Changed

- The reference is one page per member, with a type page and a namespace index above it: the two documents above become 31 type pages and 42 member pages, and the index a reader lands on is 102 lines rather than 1646. ([#189](https://github.com/CyrilB1531/data.net/issues/189), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))
- The package is `Lodestar.Metrics`, and its namespaces are `Lodestar.Metrics.*`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

## Released — 2026-08-14

### DataNet.Text — 0.3.0

#### Added

- Stop-word lists for French, German, Italian, Portuguese and Spanish join the existing English list, one per language with a Snowball stemmer. ([#13](https://github.com/CyrilB1531/data.net/issues/13), [`58c5ed5`](https://github.com/CyrilB1531/data.net/commit/58c5ed5))
- `TfidfVectorizer`, `CountVectorizer` and `HashingVectorizer` gain `Save`/`Load` so a fitted model survives the process. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `ArtifactLoadOptions` bounds what a loaded artifact may declare, so a malformed or hostile file raises `InvalidDataException` instead of `OutOfMemoryException`. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))

#### Changed

- The idf vector is stored as base64 raw IEEE-754 bits instead of JSON numbers. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- Artifacts are written with the relaxed JSON encoder instead of escaping every non-ASCII character. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- Single doubles use the shortest round-trippable form on `net8.0` and later, keeping `"G17"` on `netstandard2.0`. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- Measured against scikit-learn with `pickle`, `Save` is now 2.09× faster and `Load` matches it on elapsed time. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- Loading an artifact stopped copying the payload around: the read path sizes one buffer from the stream's length and decodes straight into the destination array. ([#100](https://github.com/CyrilB1531/data.net/issues/100), [`114245f`](https://github.com/CyrilB1531/data.net/commit/114245f))
- `CsrMatrix`'s public constructor now validates its arrays — `RowPointers` non-decreasing and in range, every column index in range. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- Stop-word removal no longer allocates the tokens it discards, since a dropped token is checked as a span rather than materialised. ([#80](https://github.com/CyrilB1531/data.net/issues/80), [`74f741b`](https://github.com/CyrilB1531/data.net/commit/74f741b))
- `DataNet.Text` declares `System.Text.Json` on `netstandard2.0`, where it is not in-box until `net8.0`. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))

### DataNet.Embeddings — 0.3.0

#### Added

- Vocabulary loaders cover the three formats a pretrained tokenizer ships in: `vocab.txt`, `tokenizer.json` and `spiece.model`. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `WordPieceVocabulary` and `SentencePieceVocabulary` carry the settings that change tokenization: the unknown token, the continuation prefix, lowercasing, and piece type. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `SentencePieceTokenizer(SentencePieceVocabulary)` decides what may match text from each piece's declared type. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- The loaders refuse a file whose pipeline they do not reproduce — an `NFKC` or precompiled normalizer, a `BertPreTokenizer`, a `post_processor` inserting `[CLS]`/`[SEP]` — naming what they found. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `added_tokens` are read rather than dropped, reaching both tokenizers instead of tokenizing to the unknown token. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- The four `added_tokens` matching flags that decide where an entry matches now apply on both tokenizers. ([#104](https://github.com/CyrilB1531/data.net/issues/104), [`21f808b`](https://github.com/CyrilB1531/data.net/commit/21f808b))
- WordPiece added tokens are matched as text, not folded into the vocabulary, changing tokenization for any `tokenizer.json` carrying a non-empty `added_tokens` table. ([#104](https://github.com/CyrilB1531/data.net/issues/104), [`96b1b6b`](https://github.com/CyrilB1531/data.net/commit/96b1b6b))
- `BpeTokenizer`, `BpeVocabulary`, `BpeFilesLoader` and `TokenizerJsonLoader.LoadBpe` add a third sub-word tokenizer, matching `tokenizers.models.BPE` in both its classic and byte-level lineages, with byte-level `Encode`/`Decode` round-tripping any well-formed string exactly. ([`b46c474`](https://github.com/CyrilB1531/data.net/commit/b46c474))
- `continuing_subword_prefix` loads instead of being refused, applied to every symbol after the first of each pre-tokenized piece on the classic, non-byte-level lineage. ([#120](https://github.com/CyrilB1531/data.net/issues/120), [`dfa7639`](https://github.com/CyrilB1531/data.net/commit/dfa7639))
- `fuse_unk` loads instead of being refused: a run of consecutive uncovered characters becomes one unknown token rather than one each. ([#119](https://github.com/CyrilB1531/data.net/issues/119), [`c91f3ef`](https://github.com/CyrilB1531/data.net/commit/c91f3ef))
- The merge loop threads symbols on a doubly-linked list and a hand-rolled priority queue, replacing a rescan-and-shift loop that was quadratic on a token with no split point. ([`b46c474`](https://github.com/CyrilB1531/data.net/commit/b46c474))
- A batch encoding pipeline — `BatchEncoder`, `EncodingOptions`, `SpecialTokenTemplate`, `EncodedBatch`, `ISubwordTokenizer` — now owns matching a model's special-token wrapping instead of leaving it to the caller. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `OnnxTextEmbedder.EmbedBatch` takes text in and returns one normalized vector per text out, in input order, mirroring `SentenceTransformer.encode`. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `CancellationToken` is now accepted on every batch entry point. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `Pooler.MeanPoolBatch` and `MeanPoolAndNormalizeBatch` pool a `[batch, seq, dim]` tensor with each row against its own mask slice. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `EmbeddingIndex.Save`/`Load`, with `SaveAsync`/`LoadAsync` counterparts, round-trip a built index so embedding a corpus is not lost with the process. ([#62](https://github.com/CyrilB1531/data.net/issues/62), [`7e093c9`](https://github.com/CyrilB1531/data.net/commit/7e093c9))
- `EmbeddingIndex.Add(vector, id)`, `GetId` and `HasIds` attach an opaque id to each vector, kept off `SearchResult`. ([#62](https://github.com/CyrilB1531/data.net/issues/62), [`c06b472`](https://github.com/CyrilB1531/data.net/commit/c06b472))

#### Changed

- A `Sequence`'s `Split` step whose `pattern` declares both `Regex` and `String` is now refused, where it loaded by silently reading the first. ([#167](https://github.com/CyrilB1531/data.net/issues/167), [`01c0de1`](https://github.com/CyrilB1531/data.net/commit/01c0de1))
- `EmbeddingIndex.Load` now moves a vector block in three passes instead of five. ([#100](https://github.com/CyrilB1531/data.net/issues/100), [`114245f`](https://github.com/CyrilB1531/data.net/commit/114245f))
- `OnnxTextEmbedder.Embed` takes `ReadOnlySpan<long>` where it took `IReadOnlyList<long>`, a source break that removes two defensive copies per call. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- The default output is chosen deterministically instead of by dictionary key order. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- An output of unexpected rank now throws instead of producing an out-of-range access or a silently wrong result. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- The zero `token_type_ids` buffer is thread-static and never written to, instead of being allocated per call. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- An added token is a token, not a vocabulary entry: a single-character added token `model.vocab` does not declare no longer makes that character look covered. ([#130](https://github.com/CyrilB1531/data.net/issues/130), [`d785b86`](https://github.com/CyrilB1531/data.net/commit/d785b86))
- `BpeVocabulary.PreSplitPattern` becomes `PreSplit`, a `BpeSplitStep` carrying the pattern, the `behavior` and the `invert` flag together. ([#145](https://github.com/CyrilB1531/data.net/issues/145), [`9546b1c`](https://github.com/CyrilB1531/data.net/commit/9546b1c))
- A `BpeVocabulary` has to say how its text is split, and is refused when it declares none of `PreSplit`, `PreTokenizerPattern` or `NoPreTokenizer`. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`545c51e`](https://github.com/CyrilB1531/data.net/commit/545c51e))

#### Deprecated

- `SentencePieceTokenizer(IReadOnlyList<SentencePiece>, int)`, the id-based constructor, is deprecated in favor of building a `SentencePieceVocabulary` with a loader. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))

#### Fixed

- A merge pair listed twice in `model.merges` now keeps its last occurrence instead of its first, changing the tokens produced for a file that repeats one. ([#160](https://github.com/CyrilB1531/data.net/issues/160), [`708982f`](https://github.com/CyrilB1531/data.net/commit/708982f))
- A `Sequence` of `Split` then `ByteLevel` now applies both patterns instead of only the `Split` step's, changing the tokens produced for Llama-3 and Qwen2 on ordinary text. ([#143](https://github.com/CyrilB1531/data.net/issues/143), [`9a8d15c`](https://github.com/CyrilB1531/data.net/commit/9a8d15c))
- A `Sequence`'s `Split` step now honours its `behavior` and `invert` fields instead of always acting as `Removed` with `invert: true`. ([#145](https://github.com/CyrilB1531/data.net/issues/145), [`9546b1c`](https://github.com/CyrilB1531/data.net/commit/9546b1c))
- A `tokenizer.json` declaring no `pre_tokenizer`, or a bare `ByteLevel` step with `use_regex` off, now loads as `BpeVocabulary.NoPreTokenizer` instead of the `Whitespace` split. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`545c51e`](https://github.com/CyrilB1531/data.net/commit/545c51e))
- With a `Sequence` pre-tokenizer and `add_prefix_space` on, the space now goes on every piece the `Split` step produces instead of once per added-token segment, so `"a|b|c|d"` decodes to `" a | b | c | d"` where it decoded to `" a|b|c|d"`. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`26481a9`](https://github.com/CyrilB1531/data.net/commit/26481a9))
- A `Sequence`'s `Split` step whose pattern is spelled `{"String": …}` now loads, the literal escaped into the regex matching exactly it, instead of being refused for declaring no `pattern.Regex`. ([#167](https://github.com/CyrilB1531/data.net/issues/167), [`01c0de1`](https://github.com/CyrilB1531/data.net/commit/01c0de1))

### DataNet.Fuzzy — 0.3.0

#### Changed

- `DataNet.Fuzzy` depends on `DataNet.Text` as a published NuGet package rather than a project reference, so a package can ship without dragging the other two with it. ([#64](https://github.com/CyrilB1531/data.net/issues/64), [`96286ac`](https://github.com/CyrilB1531/data.net/commit/96286ac))

### DataNet.Metrics — 0.1.0

First release of a fourth package.

#### Added

- Classification metrics at scikit-learn parity: `ConfusionMatrix`, `Accuracy`, `Precision`, `Recall`, `F1`, `FBeta`, `ClassificationReport` and `RocAuc`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- All four averaging modes — `Averaging.Binary`, `Micro`, `Macro` and `Weighted` — are an enum instead of a string, with `average=None` becoming a separate `PerClass` method. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `ClassificationReport` comes in both shapes: structured rows a program can read, and `ToText(digits)` reproducing `classification_report`'s printed output character for character. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `RocAuc.Score` mirrors `_binary_clf_curve`'s sort-and-accumulate, and `RocAuc.MultiClass` covers both `ovr` and Hand & Till's `ovo`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `ZeroDivision.Zero`, `One`, `NaN` or `Throw` give an explicit, caller-chosen answer for the 0/0 case scikit-learn silently defaults and warns on. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `sampleWeight` is threaded throughout, which is why matrix cells and support figures are `double` rather than `int`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- All 29 operations are measured at or above 1× scikit-learn's processor time rather than merely asserted, narrowest margin 2.74×. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- Opt-in parallelism for multiclass ROC-AUC: `RocAuc.MultiClass(…, new MultiClassRocOptions { MaxDegreeOfParallelism = … })`, sequential by default and bit-identical either way. ([#86](https://github.com/CyrilB1531/data.net/issues/86), [`a2cae2b`](https://github.com/CyrilB1531/data.net/commit/a2cae2b))
- At n=100 000, k=10, on four physical cores, one-vs-rest drops from 76 ms sequential to 27 ms at eight workers, and one-vs-one from 127 ms to 37 ms at four. ([#86](https://github.com/CyrilB1531/data.net/issues/86), [`a2cae2b`](https://github.com/CyrilB1531/data.net/commit/a2cae2b))
- Balanced accuracy, Matthews correlation and Cohen's kappa — `BalancedAccuracy.Score`, `MatthewsCorrelation.Score` and `CohenKappa.Score` — each from labels or from an already-built `ConfusionMatrix`. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- `confusion_matrix(…, normalize=…)` is a projection: `ConfusionMatrix.ToArray(Normalization.None/True/Pred/All)` returns scaled cells without the matrix itself remembering it was normalized. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- `ZeroDivision` keeps a faithful default per metric rather than one across the package — `Zero` for precision, recall, F1, F-beta, the report and Matthews correlation; `NaN` for Cohen's kappa. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- 18 new cross-language rows — three operations over six shapes — are at or above 1× scikit-learn's processor time, narrowest margin 16.59× on `balanced_accuracy` at n=1 000 000. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- Regression metrics at scikit-learn parity: `MeanSquaredError`, `RootMeanSquaredError`, `MeanAbsoluteError`, `MedianAbsoluteError`, `MeanAbsolutePercentageError`, `MeanSquaredLogError`, `RootMeanSquaredLogError`, `MaxError`, `R2`, `ExplainedVariance` and `PinballLoss`. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- `multioutput=` is spelled by choosing a method: `Score(…)` is `uniform_average`, `PerOutput(…)` is `raw_values`, and `VarianceWeighted(…)` is `variance_weighted` on `R2` and `ExplainedVariance`. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- The undefined cases are two knobs, not one: `forceFinite` answers zero variance over two or more samples, and `R2`'s `ZeroDivision` separately answers fewer than two samples. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- The weighted median averages within one machine epsilon rather than exactly, matching scikit-learn's own overshoot test against `np.finfo(float64).eps`. ([`859da5c`](https://github.com/CyrilB1531/data.net/commit/859da5c))
- Two refusals taken from `check_array` and from `numpy.average`: a `sampleWeight` that is zero throughout, and `outputWeights` that sum to zero. ([`2216d5b`](https://github.com/CyrilB1531/data.net/commit/2216d5b))
- `log(1 + x)` is computed as `log1p`, using Kahan's identity, in `MeanSquaredLogError` and `RootMeanSquaredLogError`. ([`2216d5b`](https://github.com/CyrilB1531/data.net/commit/2216d5b))
- `R2`'s two passes, `ExplainedVariance`'s five accumulations, and `Outputs.WeightedMean` now sum with Neumaier compensation rather than a running total. ([#127](https://github.com/CyrilB1531/data.net/issues/127), [`fcb705b`](https://github.com/CyrilB1531/data.net/commit/fcb705b))
- `mse`, `mae`, `median_ae` and `r2` were benchmarked against scikit-learn over six shapes; `median_ae` is the one operation below the 1× processor-time gate, at 0.80–0.90×. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))

#### Changed

- `DataNet.Metrics`'s long comment blocks became ten decision records, so the reasoning lives where it can be cited instead of duplicated at each call site. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`d4d9326`](https://github.com/CyrilB1531/data.net/commit/d4d9326))
- The Neumaier-versus-Kahan argument for `CompensatedSum` moved into a record of its own instead of living only as comments in the source. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))
- `MultiClassRocOptions`'s doc comments no longer restate `docs/decisions/0018`, and `Normalization`'s comment points at `0020` instead of repeating it. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))
- The rest of the package's remaining long comments were trimmed to their reason, with no behaviour changed. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))

## [0.2.0] — 2026-08-05

Reach, correctness and honesty about performance. Nothing in the public API was
removed or renamed, so upgrading from `0.1.0` is a version bump.

### Added

- `netstandard2.0` becomes a second target framework, reaching .NET Framework 4.6.1+, Mono, Xamarin and Unity through conditional compilation rather than a reduced API. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Four Snowball stemmers join English and French: `SpanishSnowballStemmer`, `PortugueseSnowballStemmer`, `ItalianSnowballStemmer` and `GermanSnowballStemmer`. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Blocked (multi-word) Myers removes the 64-character cap on `Levenshtein.Distance`'s bit-parallel path. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A benchmark suite compares the `net10.0` and `netstandard2.0` builds of the same library. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Mirror test projects replay the entire suite against the `netstandard2.0` assemblies, 339 tests across both builds. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))
- A sample under `samples/DataNet.Sample` consumes the packages by `PackageReference` from a locally packed feed, and runs in CI. ([#50](https://github.com/CyrilB1531/data.net/issues/50), [`391a71c`](https://github.com/CyrilB1531/data.net/commit/391a71c))
- `CONTRIBUTING.md` and this changelog are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- SonarQube Cloud analysis, a `lint` CI job (markdownlint and `dotnet format`), and Dependabot for GitHub Actions are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Changed

- Long-string `Levenshtein.Distance` is 20–33× faster: 684 µs to 21 µs at 512 characters. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Regular expressions are bounded by a match timeout: a pathological pattern now raises `RegexMatchTimeoutException` instead of hanging the calling thread. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Warnings are errors across the whole repository, covering `src`, `tests` and `bench` rather than the libraries alone. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Fixed

- Static-analysis defects fixed and verified against the oracle corpora: an `int` division widened to `double` in `Jaro`, nested classes shadowing their outer type in the Snowball stemmers, unread step-method return values, and nested ternaries in three files. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Code coverage was never collected: CI referenced `coverlet.collector` without depending on it, so the collection step silently did nothing. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Security

- A `workflow_dispatch` input was interpolated directly into a shell command in a job holding `id-token: write`, letting it mint a nuget.org publishing key; values now reach the shell through the environment. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- GitHub Actions are pinned to full commit SHAs, so a moved tag cannot change what runs in CI. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- CI dependency installation is hardened: markdownlint pinned with lifecycle scripts disabled, and `pip install --require-hashes` against a generated lock file pinning all 29 packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Documentation

- Package metadata now attributes the project to Cyril BRUNET (`Authors`, `Company`, `Copyright`). ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))
- `THIRD-PARTY-NOTICES.md` now records the shipped dependencies instead of saying "None yet". ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))

### Notes

- Deliberate analyzer suppressions live in the source as `#pragma warning disable` with their justification, since SonarLint reads neither `.editorconfig` nor workspace settings. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- The `netstandard2.0` build is behavior-verified: the whole suite is replayed against those assemblies, not only compiled. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))

> Entries below predate the per-lot issue convention and this shape: this
> repository had not yet adopted filing one issue per change, so several point
> at the same issue rather than one each. A missing link is a date, not an
> oversight.

## [0.1.0] — 2026-08-01

First release. All four lots of the project brief are delivered, and every
building block is validated by replaying frozen reference outputs captured from
the canonical Python libraries — see [`docs/equivalence.md`](docs/equivalence.md).

### Added

- Lot 1 — string distances and similarity (`DataNet.Text`): Levenshtein (with a Myers bit-parallel fast path), OSA, Damerau-Levenshtein, Hamming, Jaro, Jaro-Winkler, Indel, LCS, Ratcliff-Obershelp, Jaccard, Dice, Overlap, Tversky, Cosine, Soundex, Metaphone, NYSIIS. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Lot 2 — tokenization and sparse vectorization (`DataNet.Text`): CSR matrix, word/char/char_wb tokenizers, `CountVectorizer`, `TfidfVectorizer`, `HashingVectorizer` (MurmurHash3-32), Porter and Snowball EN/FR stemmers, English stop words. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Lot 3 — embeddings and semantic search (`DataNet.Embeddings`): WordPiece and SentencePiece (unigram Viterbi) tokenizers, pooling, SIMD kNN, ONNX inference, with ONNX Runtime isolated to this package. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Lot 4 — applied fuzzy matching (`DataNet.Fuzzy`): `fuzz.*` (ratio / partial / token_sort / token_set / WRatio), `process.extract` and `extractOne`, blocking deduplication. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Migration guides for NumPy, pandas, scikit-learn, statsmodels, PyTorch, matplotlib and seaborn, plus a three-column inventory mapping each need to use / build / decide. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A decision log records the deliberate divergences from the Python references. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Publishing to nuget.org via Trusted Publishing (keyless, OIDC) and to GitHub Packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

[Unreleased]: https://github.com/CyrilB1531/data.net/compare/DataNet.Text/v0.3.0...HEAD
[0.2.0]: https://github.com/CyrilB1531/data.net/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/CyrilB1531/data.net/releases/tag/v0.1.0

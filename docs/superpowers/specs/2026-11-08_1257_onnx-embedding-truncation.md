# A `BatchEncoder` left without a `MaxLength` truncates to the model's configured length

**Issue:** [#1257](https://github.com/CyrilB1531/lodestar/issues/1257).
**Status:** written with the work, 2026-11-08.
**Date:** 2026-11-08.

## The problem

`OnnxEmbeddingGenerator.GenerateAsync` calls `OnnxTextEmbedder.EmbedBatch(texts, encoder)`, which never fills a null
`MaxLength` from the model. The overload taking `EncodingOptions` does. The generator's own README example builds
`new BatchEncoder(tokenizer)`, so a text past the model's position table is refused where sentence-transformers and
`embedder.EmbedBatch(texts)` truncate it.

## The reference

Measured on sentence-transformers 6.2.0 and transformers 5.22.2, over a Bert with a position table of six rows and
no `max_seq_length` configured:

| Input | Tokens fed | Result |
| --- | --- | --- |
| empty batch | none | empty |
| `""` | 2 | a vector |
| four words, the table exactly | 6 | a vector |
| five words | 6, `[SEP]` kept | a vector |
| 600 words | 6, `[SEP]` kept | a vector |
| 600 words, `max_seq_length=4` | 4 | a vector |
| 600 words, `max_seq_length=9`, past the table | 9 | raises `RuntimeError` |
| 3 words, `max_seq_length=1`, below the template | 5, nothing cut | a vector |
| 600 words, `max_seq_length=1`, below the template | 602, nothing cut | raises `RuntimeError` |
| 600 words, the tokenizer stating no maximum, a table of 700 | 602 | a vector |

`Transformer.__init__` caps the tokenizer at `min(model_max_length, config.max_position_embeddings)` unless a length
was configured; the position table is the same number `OnnxTextEmbedder.MaxSequenceLength` reads.

The configured length is `max_seq_length` in `sentence_bert_config.json`, which `Transformer.load_config` reads from
the model directory, trying six legacy names after it (`sentence_roberta_config.json` to
`sentence_xlnet_config.json`); its ONNX backend keeps the graph under that directory's `onnx/`. Measured on the same
versions, over a saved model whose table holds twelve rows and whose tokenizer states 512:

| `max_seq_length` in the file | Result |
| --- | --- |
| `7` | cut at 7 |
| absent, `null`, `{}`, or no file | cut at 12, the tokenizer capped at the table |
| `40`, past the table | 40 fed, raises `RuntimeError` |
| `0`, `true` | loaded, nothing cut, raises `RuntimeError` past the table |
| `"5"`, `"abc"`, `5.0`, `5.5` | refused at load, `TypeError` |
| `-1` | refused at load, `OverflowError` |
| the file not JSON, or a JSON array | refused at load |

## The rule

An encoder whose `MaxLength` is null takes a default length, through every entry that encodes text, both text
overloads of `EmbedBatch` and the generator alike:

1. The default is the `max_seq_length` of the first of sentence-transformers' seven configuration names found in the
   Transformer module's folder: the parent of a directory named `onnx` the model sits in, else the model's directory,
   and only when a `modules.json` lists that folder, its own at its root or its parent's under its name; `onnx/` itself
   is never read. 256 when no module is listed, no file is found, or the field is absent or null. It is read once, at
   the first text call, from the folder the model was opened from.
2. The default never passes `MaxSequenceLength` when the model states one: it is capped at it. A model that states
   none takes the default as it is.
3. A sequence is cut to the default under the encoder's own `Truncation`; `None` refuses the text, and so does a
   configured `only_second`, which cuts none.
4. A `MaxLength` the caller sets is its own: below the limit it wins, past it a longer row stays refused (#1423), and
   the options overload refuses it outright, as before.
5. A file sentence-transformers refuses to load is refused by the two text overloads, with an
   `InvalidOperationException`: one that is a directory, cannot be read, opens with a byte-order mark, is not UTF-8, is
   not JSON, is not a JSON object, holds an integer of more than 4300 digits (Python 3.12's `int_max_str_digits`, sign
   aside; a float has no such limit), or states a `max_seq_length` that is not a JSON integer of at least 1. What
   Python's `json` reads is read: a duplicated field takes its last value, `NaN` and `Infinity` are values, nesting goes
   to 9997 containers, the depth Python 3.12.14 reads before its `RecursionError`, a lone surrogate escape is a string,
   and an integer past `long` is capped as any length past the model's limit is. `Embed` and `EmbedBatch(EncodedBatch)`
   never read the length and are not refused, as on `main`. A file past 256 MiB, the artifacts' byte cap, is refused. A
   name is present as `Path.exists()`, which sentence-transformers' `load_file_path` asks, says: a link is followed, so
   one to nothing is absent and the next name is read, and a directory is present.
6. A `do_lower_case` true by Python's truth lowercases each text before both text overloads encode it, as
   sentence-transformers 6.2.0 prepends the tokenizers' `Lowercase` normalizer then: each code point's own mapping,
   U+0130 to `i` + U+0307, no final sigma (measured equal over every code point on net10.0, tokenizers 0.23.2).
7. Where the template alone is longer than the default, nothing can be cut to fit and the encoder is left as it is:
   a row past the model's limit stays refused under the overload's own argument, and an empty batch stays empty.

`EmbedBatch(EncodedBatch)` and `Embed` take rows already encoded. They cannot cut, and stay refused past the limit,
as `session.run` fails past the table.

## Members touched

- `BatchEncoder.WithDefaultMaxLength(int)`: new.
- `OnnxTextEmbedder`'s constructors: read the configuration; a malformed one is kept for the text overloads to refuse.
- `OnnxTextEmbedder.EmbedBatch(IEnumerable<string>, EncodingOptions, CancellationToken)` and
  `EmbedBatch(IEnumerable<string>, BatchEncoder, CancellationToken)`: apply the rule.
- `OnnxEmbeddingGenerator.GenerateAsync`: unchanged code, inherits it; `None` past the limit now names `values`.

The other members of `Lodestar.Extensions.AI` (`GetService`, `Dispose`, the constructor) encode nothing.

## Decisions

The maintainer kept every decision below, Cyril 2026-11-10.

- **The fix is in the `EmbedBatch` overload, not the generator**, as the issue directs: the generator holds no
  tokenizer to rebuild its encoder with, and a direct caller of that overload has the same defect.
- **A new public member on `BatchEncoder`.** `OnnxTextEmbedder` lives in another package and the encoder exposes no
  tokenizer, so nothing public could yield the capped copy; cutting the rows after encoding would be a second
  truncation that has to know the template. The copy keeps the ids resolved at construction (#1453).
  Approved by the maintainer, Cyril 2026-11-10.
- **`max_seq_length` beside the model, else 256, capped at `MaxSequenceLength`**: the maintainer's rule, Cyril
  2026-11-10. No ONNX graph carries the configured length, but a sentence-transformers export carries the file, so
  all-MiniLM-L6-v2 cuts at 256 as sentence-transformers does. Where none states one, sentence-transformers takes the
  tokenizer's `model_max_length`, which an ONNX file does not carry either; 256 is the rule's fixed answer. The cap
  diverges where a length past the table is configured: sentence-transformers feeds it and the graph fails.
- **The configuration is read where sentence-transformers reads it**, Review A pass 4: the Transformer module's folder
  is the model's, or the parent of an `onnx/` it sits in, the layout the ONNX backend saves and loads, and it counts
  only when a `modules.json` lists it, at `""` in its own or under its name in its parent's; without one
  sentence-transformers builds a fresh module and reads no configuration. The first legacy name whose content Python
  finds true is taken, as `load_config`'s `if config: break`, and a `model_max_length` in `tokenizer_args` or
  `processor_kwargs` replaces `max_seq_length`, as `Transformer.__init__` does, and `processing_kwargs` sets the
  call's `max_length` and `truncation` over both: `preprocess` updates its text kwargs with a `text` bucket Python
  finds true and its common ones with any `common` bucket, an object or a list of pairs as `dict.update` takes them,
  and calls `{**text_kwargs, **common_kwargs}`, so the two merge key by key, `common`'s word winning (passes 5 to 7). A
  bucket `dict.update` refuses, a `null`, `0` or `false` `common` among them, is refused, as every encode then
  fails; so is a strategy transformers does not know (pass 7). `only_second` is read as Python reads it, the
  maintainer's word, Cyril 2026-11-13: it cuts no single text, and a text past the length is refused with an
  `ArgumentException` naming `texts`, as tokenizers 0.23.2 raises "Second sequence not provided" then. A null `truncation` beside a `max_length` and a `false`
  `padding` cuts at that length, as transformers' `_get_padding_truncation_strategies` turns it into `longest_first`
  (pass 10). The files are read when a text overload first runs, so an embedder fed only encoded rows reads none
  (pass 6).
  A module path `.` or `./` is the root, and one leaving the model's folder is refused, as `_validate_relative_path`
  refuses it. A key the constructor does not take is read past, where sentence-transformers raises `TypeError`: a
  reader of two fields has no list of the constructor's to keep in step, and refusing it would refuse a file only a
  newer sentence-transformers wrote. A folder named `ONNX` counts where the file system
  ignores case, as Windows and macOS do by default, where any spelling of `onnx` opens it; a case-sensitive one keeps
  `ONNX` apart from `onnx`. A `modules.json` folder spelled in another case names the module's folder on the same
  terms (pass 7).
- **`do_lower_case` is applied in this PR**, the maintainer's word, Cyril 2026-11-11: the file is already read, and
  a cased tokenizer under an uncased export gave other vectors than sentence-transformers. The texts are lowercased
  before encoding, which diverges only for a special token written in capitals into a text, matched by the
  tokenizer before its normalizer.
- **A zero and `true` are refused**, where sentence-transformers loads them and then cuts nothing: neither is a
  length, and refusing them with the values it refuses is one rule rather than two.
- **The options overload goes through `WithDefaultMaxLength` too**, so the two overloads cannot disagree.
- **A default below the template leaves the row whole**, as sentence-transformers 6.2.0 does, measured in the table
  above. Review A pass 4 read tokenizers 0.23.2 called directly, which cuts the text to such a cap; pass 5 showed the
  chain this PR answers for does not, so the row stays uncut and `MaxLength` keeps bounding every row it cuts.
- **`TruncationStrategy.None` past the limit names `values`/`texts`**, where it named `encoder` on `main`: it is now the
  text the encoder refuses, with its position, as under the options overload.

## Settled

- The reader keeps its emulation of Python's `json` — named literals, the 4300-digit limit, depth 9997, the byte-order
  mark, Python's truth — which Review A passes 2 and 3 asked for and the maintainer approved; a plain `JsonDocument`
  parse would refuse or misread files sentence-transformers loads. Review A pass 4 asked to drop it; declined.

- `EncodingOptions.MaxLength` left null still caps nothing on `BatchEncoder` itself (#1446, #1505); only the embedder
  that knows a model fills it in, on a copy, and the caller's encoder is not changed.
- A template longer than the default is not refused on an empty batch. That is the behaviour of `main`'s overload
  taking a `BatchEncoder` and of `SentenceTransformer.encode([])`. An explicit `MaxLength` below it is still refused at
  construction, as on `main`.
- The generator still names `encoder` when an explicit `MaxLength` past the limit produces a row too long: `encoder`
  is what the caller has to change, and #1423 settled that name.

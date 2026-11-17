using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Lodestar.Internal.Persistence;

namespace Lodestar.Onnx;

/// <summary>
/// Reads sentence-transformers' <c>max_seq_length</c> and <c>do_lower_case</c> from the module configuration exported
/// beside an ONNX model.
/// </summary>
/// <remarks>
/// sentence-transformers loads a Transformer module from the folder <c>modules.json</c> names, its ONNX graph there or
/// in its <c>onnx/</c>, and takes the first of <see cref="FileNames"/> Python finds true. A file Python's json cannot
/// read, or a length or module this reader cannot take, is refused; an unknown key is read past (#1257).
/// </remarks>
internal static class SentenceTransformersConfig
{
    /// <summary>The length taken when no configuration states one: the maintainer's rule, Cyril 2026-11-10 (#1257).</summary>
    public const int DefaultMaxSeqLength = 256;

    private const string Field = "max_seq_length";

    private const string LowerCaseField = "do_lower_case";

    /// <summary>The tokenizer's own cap, which sentence-transformers takes over <c>max_seq_length</c> when the kwargs set it.</summary>
    private const string TokenizerField = "model_max_length";

    /// <summary>The kwargs sentence-transformers merges into each tokenizer call, its <c>text</c> bucket over its <c>common</c> one.</summary>
    private const string ProcessingField = "processing_kwargs";

    private const string TruncationField = "truncation";

    private const string MaxLengthField = "max_length";

    /// <summary>A call's <c>max_length</c> as its refusal names it.</summary>
    private const string CallLengthField = $"{ProcessingField} {MaxLengthField}";

    private const string PaddingField = "padding";

    private const string StrideField = "stride";

    private const string TextBucket = "text";

    private const string CommonBucket = "common";

    /// <summary>
    /// The buckets <c>Transformer.preprocess</c> updates, in its order: common, then the modalities, text first and
    /// the three a tokenizer's call never takes; common's word still wins, as in <c>{**text, **common}</c>.
    /// </summary>
    private static readonly string[] Buckets = [CommonBucket, TextBucket, "audio", "image", "video"];

    /// <summary>The call's kwargs that decide where a text is cut, the only ones read from <c>processing_kwargs</c>.</summary>
    private static readonly string[] CallFields = [TruncationField, MaxLengthField, PaddingField, StrideField];

    private const string OnnxFolder = "onnx";

    private const string ModulesFile = "modules.json";

    /// <summary>Python 3.12's <c>sys.get_int_max_str_digits()</c>: its json refuses an integer of more digits, sign aside.</summary>
    private const int MaxIntegerDigits = 4300;

    /// <summary>
    /// Opens a quoted named literal in the raw JSON text; a file string spelled the same way, U+0001 then a literal's
    /// name, is read as that float: the cost of a parser that refuses the literals, for a string no export writes.
    /// </summary>
    private const string QuotedLiteralStart = "\"\\u0001";

    /// <summary>The literals Python's json reads where a value stands, with the float each is.</summary>
    private static readonly (string Name, double Value)[] NamedFloats =
        [("-Infinity", double.NegativeInfinity), ("Infinity", double.PositiveInfinity), ("NaN", double.NaN)];

    /// <summary>The separators a <c>modules.json</c> path may use, whichever system wrote it.</summary>
    private static readonly char[] PathSeparators = ['/', '\\'];

    // sentence-transformers 6.2.0's _load_init_kwargs renames the legacy key over the new one, so it wins when both appear.
    private static readonly (string Old, string New)[] RenamedFields =
        [("model_args", "model_kwargs"), ("tokenizer_args", "processor_kwargs"), ("config_args", "config_kwargs")];

    private const string RemoteCodeField = "trust_remote_code";

    private const string ModalityField = "modality_config";

    private const string OutputNameField = "module_output_name";

    private const string TaskField = "transformer_task";

    private const string MessageModality = "message";

    private const string ModelPathField = "model_name_or_path";

    private const string TokenizerPathField = "tokenizer_name_or_path";

    private const string ExpansionField = "query_expansion";

    private const string QueryLengthField = "query_length";

    private const string ExpansionStrategyField = "strategy";

    private const string ExpansionAttendField = "attend";

    private const string ExpansionTokenField = "token";

    private const string ExpansionLengthField = "length";

    /// <summary><c>_VALID_QUERY_EXPANSION_KEYS</c>, sentence-transformers 6.2.0.</summary>
    private static readonly string[] ExpansionKeys =
        [ExpansionStrategyField, ExpansionAttendField, ExpansionTokenField, ExpansionLengthField];

    /// <summary>transformers' <c>LARGE_INTEGER</c>, past which a tokenizer's <c>model_max_length</c> is no limit at all.</summary>
    private static readonly BigInteger LargeInteger = BigInteger.Pow(10, 20);

    /// <summary><see cref="LargeInteger"/> as a float, exactly: Python compares a float with it exactly.</summary>
    private static readonly double LargeFloat = (double)LargeInteger;

    /// <summary>The paddings transformers' <c>PaddingStrategy</c> takes by name.</summary>
    private static readonly string[] Paddings = ["longest", MaxLengthField, "do_not_pad"];

    /// <summary><c>load_config</c>'s single modalities, which a <c>+</c> joins into a tuple key.</summary>
    private static readonly string[] Modalities = [TextBucket, "image", "audio", "video", MessageModality];

    /// <summary><c>TRANSFORMER_TASK_TO_AUTO_MODEL</c>'s keys under transformers 5, <c>any-to-any</c> among them.</summary>
    private static readonly string[] Tasks =
        ["feature-extraction", "sequence-classification", "text-generation", "fill-mask", "retrieval", "any-to-any"];

    // Python 3.12.14's json reads 9997 nested containers and refuses 9998, measured; the parser here keeps its own stack.
    private static readonly JsonDocumentOptions ReaderOptions = new() { MaxDepth = 9997 };

    // sentence-transformers 6.2.0, base/modules/transformer.py, Transformer.load_config, in its order.
    private static readonly string[] FileNames =
    [
        "sentence_bert_config.json",
        "sentence_roberta_config.json",
        "sentence_distilbert_config.json",
        "sentence_camembert_config.json",
        "sentence_albert_config.json",
        "sentence_xlm-roberta_config.json",
        "sentence_xlnet_config.json",
    ];

    /// <summary>The configured length and <c>do_lower_case</c>, or why a file sentence-transformers reads is refused.</summary>
    /// <param name="modelPath">The full path of the <c>.onnx</c> file ONNX Runtime has already opened.</param>
    /// <param name="openedFrom">The current directory when the model was opened, which a relative tokenizer path names from; null where it was gone.</param>
    /// <returns>What the configuration states, as <see cref="Stated"/> lists it.</returns>
    public static Stated Read(string modelPath, string? openedFrom)
    {
        (string? module, string? refusal) = ModuleDirectory(Path.GetDirectoryName(modelPath));
        if (refusal is not null || module is null)
        {
            return new Stated(null, false, refusal, false);
        }

        for (int i = 0; i < FileNames.Length; i++)
        {
            string path = Path.Combine(module, FileNames[i]);
            (bool present, JsonDocument? document, string? why) = Open(path);
            if (!present)
            {
                continue;
            }
            if (document is null)
            {
                return Refused(path, why!);
            }

            using (document)
            {
                JsonElement root = document.RootElement;
                // load_config moves past a file Python finds false; the last name's value is what it then loads.
                if (Truthy(root) || i == FileNames.Length - 1)
                {
                    return root.ValueKind == JsonValueKind.Object ? FromObject(path, root, openedFrom) : Refused(path, "is not a JSON object");
                }
            }
        }

        return new Stated(null, false, null, false);
    }

    /// <summary>
    /// The length and the flag one configuration states: a call's <c>processing_kwargs</c> over the tokenizer's
    /// <c>model_max_length</c> over <c>max_seq_length</c>.
    /// </summary>
    private static Stated FromObject(string path, JsonElement root, string? openedFrom)
    {
        Stated configured = Configured(path, root, openedFrom);
        if (configured.Refusal is not null && !configured.AtEncode)
        {
            return configured;
        }
        // `processing_kwargs or {}` passes over a value Python finds false.
        if (Last(root, ProcessingField) is not JsonElement processing || !Truthy(processing))
        {
            return configured;
        }
        if (processing.ValueKind != JsonValueKind.Object)
        {
            return RefusedAtEncode(path, $"states {ProcessingField} {Shown(processing)}, not a JSON object");
        }

        // Transformer.preprocess updates its text kwargs with a truthy text bucket, its common ones with any common
        // bucket, and calls {**text, **common}: the buckets merge key by key, common's word winning (#1257).
        (string? why, Dictionary<string, JsonElement>? merged) = MergedBuckets(processing);
        return merged is null ? RefusedAtEncode(path, why!) : Cut(path, configured, merged);
    }

    /// <summary>Why <c>preprocess</c> refuses a bucket, or the call's fields of those it takes, merged.</summary>
    private static (string? Refusal, Dictionary<string, JsonElement>? Merged) MergedBuckets(JsonElement processing)
    {
        var common = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        var text = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (string bucket in Buckets)
        {
            // Only common skips `if overrides :=`: a falsy modality is passed over, a falsy common reaches update,
            // which refuses null, 0 and false. image, audio and video never reach a tokenizer's call (#1780).
            bool isCommon = bucket == CommonBucket;
            if (Last(processing, bucket) is not JsonElement kwargs || (!isCommon && !Truthy(kwargs)))
            {
                continue;
            }
            string? why = bucket switch
            {
                CommonBucket => Updated(kwargs, common),
                TextBucket => Updated(kwargs, text),
                _ => Updated(kwargs, null),
            };
            if (why is not null)
            {
                return ($"states {ProcessingField}.{bucket} {Shown(kwargs)}, {why}", null);
            }
        }
        // The call takes {**text, **common}: common's word over the text bucket's.
        foreach (KeyValuePair<string, JsonElement> field in common)
        {
            text[field.Key] = field.Value;
        }
        return (null, text);
    }

    /// <summary>
    /// <c>dict.update(kwargs)</c> on the keys read here: an object's entries, or a list's pairs in order; why Python
    /// refuses it otherwise, or, where the bucket reaches the tokenizer's call (<paramref name="merged"/> given), why
    /// <c>**kwargs</c> refuses a key.
    /// </summary>
    private static string? Updated(JsonElement kwargs, Dictionary<string, JsonElement>? merged)
    {
        if (kwargs.ValueKind == JsonValueKind.Object)
        {
            foreach (string field in CallFields)
            {
                if (merged is not null && Last(kwargs, field) is JsonElement value)
                {
                    merged[field] = value;
                }
            }
            return null;
        }
        if (kwargs.ValueKind == JsonValueKind.String && kwargs.GetRawText().Length == 2)
        {
            // update("") adds nothing; a longer string's characters are not pairs.
            return null;
        }
        return kwargs.ValueKind == JsonValueKind.Array
            ? UpdatedFromPairs(kwargs, merged)
            : "which dict.update cannot take";
    }

    /// <summary><c>dict.update</c> over a list: each item a pair, in order; why Python refuses one otherwise.</summary>
    private static string? UpdatedFromPairs(JsonElement kwargs, Dictionary<string, JsonElement>? merged)
    {
        foreach (JsonElement pair in kwargs.EnumerateArray())
        {
            if (Pair(pair) is not (JsonElement key, JsonElement value))
            {
                if (!TwoCharacters(pair))
                {
                    return "whose items are not key-value pairs";
                }
                // A string of two characters is a pair of one-character names, neither a key read here.
                continue;
            }
            if (KeyRefusal(key, merged is not null) is string why)
            {
                return why;
            }
            if (merged is not null && key.ValueKind == JsonValueKind.String && Array.Find(CallFields, field => TextIs(key, field)) is string named)
            {
                merged[named] = value;
            }
        }
        return null;
    }

    /// <summary>
    /// Why a pair's key is refused: Python hashes no list or object, and, where the bucket reaches the tokenizer's call,
    /// <c>update</c> keeps a key that is not a string and the call fails, "keywords must be strings", measured (#1780).
    /// </summary>
    private static string? KeyRefusal(JsonElement key, bool reachesCall)
    {
        if (key.ValueKind is JsonValueKind.Array or JsonValueKind.Object)
        {
            return "whose key is a list or an object, which Python cannot hash";
        }
        return reachesCall && (key.ValueKind != JsonValueKind.String || IsNamedLiteral(key))
            ? "whose key is not a string, which the tokenizer's call cannot take as a keyword"
            : null;
    }

    /// <summary>
    /// The key and value Python unpacks an item into: a two-item list's items, or an object's two distinct keys, the
    /// second standing as the value; null for any other item.
    /// </summary>
    private static (JsonElement Key, JsonElement Value)? Pair(JsonElement item)
    {
        if (item.ValueKind == JsonValueKind.Object)
        {
            List<JsonElement> keys = Keys(item);
            return keys.Count == 2 ? (keys[0], keys[1]) : null;
        }
        if (item.ValueKind != JsonValueKind.Array || item.GetArrayLength() != 2)
        {
            return null;
        }
        using JsonElement.ArrayEnumerator items = item.EnumerateArray();
        items.MoveNext();
        JsonElement key = items.Current;
        items.MoveNext();
        return (key, items.Current);
    }

    /// <summary>Whether the item is a string of two characters, which Python unpacks into two names.</summary>
    private static bool TwoCharacters(JsonElement item) =>
        item.ValueKind == JsonValueKind.String && Units(item.GetRawText()).Count(unit => !unit.Paired) == 2;

    /// <summary>
    /// An object's distinct keys as JSON strings, in the order Python's dict keeps them, up to three: a key Python
    /// decodes to the same text as an earlier one, a lone surrogate's included, is that key again.
    /// </summary>
    private static List<JsonElement> Keys(JsonElement item)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var keys = new List<JsonElement>();
        foreach (JsonProperty property in item.EnumerateObject())
        {
            string raw = RawName(property.ToString());
            if (seen.Add(Decoded(raw)))
            {
                using JsonDocument name = JsonDocument.Parse(raw);
                keys.Add(name.RootElement.Clone());
                if (keys.Count > 2)
                {
                    break;
                }
            }
        }
        return keys;
    }

    /// <summary>The quoted name that opens a property's raw text.</summary>
    private static string RawName(string property)
    {
        int end = 1;
        while (property[end] != '"')
        {
            end += property[end] == '\\' ? 2 : 1;
        }
        return property.Substring(0, end + 1);
    }

    /// <summary>The string Python decodes a JSON string's raw text into.</summary>
    private static string Decoded(string raw) => new([.. Units(raw).Select(unit => unit.Unit)]);

    /// <summary>
    /// The UTF-16 units Python decodes a JSON string's raw text into, each with whether it is the low half of a
    /// surrogate pair, which Python reads as one character with the unit before it.
    /// </summary>
    private static List<(char Unit, bool Paired)> Units(string raw)
    {
        var units = new List<(char Unit, bool Paired)>();
        int i = 1;
        while (i < raw.Length - 1)
        {
            char unit;
            if (raw[i] != '\\')
            {
                unit = raw[i];
                i++;
            }
            else if (raw[i + 1] == 'u')
            {
                unit = (char)Convert.ToInt32(raw.Substring(i + 2, 4), 16);
                i += 6;
            }
            else
            {
                unit = Escaped(raw[i + 1]);
                i += 2;
            }
            bool paired = char.IsLowSurrogate(unit) && units.Count > 0 && char.IsHighSurrogate(units[units.Count - 1].Unit);
            units.Add((unit, paired));
        }
        return units;
    }

    /// <summary>The character a JSON escape other than <c>\u</c> stands for.</summary>
    private static char Escaped(char escape) => escape switch
    {
        'b' => '\b',
        'f' => '\f',
        'n' => '\n',
        'r' => '\r',
        't' => '\t',
        _ => escape,
    };

    /// <summary>
    /// The length the merged kwargs cut at, by transformers' truncation strategies: only <c>False</c>, <c>None</c> and
    /// <c>do_not_truncate</c> cut nothing; <c>True</c>, <c>longest_first</c> and <c>only_first</c> cut a single text at
    /// <c>max_length</c>, else at the configured length. <c>None</c> beside a <c>max_length</c> and a <c>False</c>
    /// padding is <c>longest_first</c>, as <c>_get_padding_truncation_strategies</c> reads it.
    /// </summary>
    /// <remarks>
    /// <c>only_second</c> cuts no single text, and tokenizers refuses one past the length; any other value
    /// <c>TruncationStrategy</c> refuses at every call, as does this reader (#1257), and a padding before it (#1796).
    /// </remarks>
    private static Stated Cut(
        string path, Stated configured, Dictionary<string, JsonElement> merged)
    {
        JsonElement? cut = merged.TryGetValue(MaxLengthField, out JsonElement length) ? length : null;
        JsonElement? padding = merged.TryGetValue(PaddingField, out JsonElement padded) ? padded : null;
        if (padding is JsonElement pad && !PaddingKnown(pad))
        {
            return RefusedAtEncode(path, $"states {ProcessingField} {PaddingField} {Shown(pad)}, which transformers' PaddingStrategy refuses");
        }
        bool callLength = cut is { ValueKind: not JsonValueKind.Null };
        bool lengthOnly = callLength && padding is { ValueKind: JsonValueKind.False };
        Strategy strategy = merged.TryGetValue(TruncationField, out JsonElement given) && !(lengthOnly && given.ValueKind == JsonValueKind.Null)
            ? StrategyOf(given)
            : Strategy.Cuts;
        if (strategy == Strategy.Unknown)
        {
            return RefusedAtEncode(path, $"states {ProcessingField} {TruncationField} {Shown(given)}, which a single text cannot be cut by");
        }
        if (strategy == Strategy.None)
        {
            return Uncut(path, configured, cut, padding is { ValueKind: JsonValueKind.String } named && TextIs(named, MaxLengthField));
        }
        if (!callLength && configured.HandsNone)
        {
            // Handed no length, a null model_max_length or one past LARGE_INTEGER, tokenizers cuts nothing and checks no
            // stride: only_second cuts nothing here (#1793), any other strategy still caps at the model's limit (#1257).
            return configured with { RefusesPast = strategy == Strategy.RefusesPast, Source = path };
        }
        return Cutting(path, configured, merged, cut, strategy == Strategy.RefusesPast);
    }

    /// <summary>The length a truncation that cuts hands tokenizers, and the stride beside it, or why either is refused.</summary>
    private static Stated Cutting(
        string path, Stated configured, Dictionary<string, JsonElement> merged, JsonElement? cut, bool refusesPast)
    {
        Stated length = HandedLength(path, configured, cut, refusesPast);
        if (length.Refusal is not null || !merged.TryGetValue(StrideField, out JsonElement stride))
        {
            // tokenizers' enable_truncation takes max_length before stride, so a length it refuses is met first.
            return length;
        }
        return UsizeOf(stride) is ulong taken
            ? length with { Stride = taken }
            : RefusedAtEncode(path, $"states {ProcessingField} {StrideField} {Shown(stride)}, not an integer from 0 to 2^64 - 1");
    }

    /// <summary>The length a truncation that cuts hands tokenizers, the call's over the configured one, or why it is refused.</summary>
    private static Stated HandedLength(string path, Stated configured, JsonElement? cut, bool refusesPast)
    {
        if (cut is not { ValueKind: not JsonValueKind.Null } stated)
        {
            return configured with { RefusesPast = refusesPast, Source = path };
        }
        (int length, ulong handed, string? why) = TokenizerLength(stated, CallLengthField);
        return why is null
            ? new Stated(length, configured.LowerCase, null, refusesPast, Source: path, Handed: handed)
            : RefusedAtEncode(path, why);
    }

    /// <summary>The refusal of a length that is not an integer of at least 1, as every length refusal words it.</summary>
    private static string NotALength(string field, JsonElement value) => $"states {field} {Shown(value)}, not an integer of at least 1";

    /// <summary>
    /// A length as tokenizers takes it, capped at <c>int</c>, and as it is handed; or why not, an integer from 1 to
    /// 2^64 - 1 being what it takes.
    /// </summary>
    private static (int Length, ulong Handed, string? Refusal) TokenizerLength(JsonElement stated, string field)
    {
        if (PythonInteger(stated) is not BigInteger integer || LengthOf(integer) is not int length)
        {
            return (0, 0, NotALength(field, stated));
        }
        return HandedOf(integer) is ulong handed
            ? (length, handed, null)
            : (0, 0, $"states {field} {Shown(stated)}, past the 2^64 - 1 tokenizers takes");
    }

    /// <summary>
    /// Under a truncation that cuts nothing, only a <c>max_length</c> padding hands tokenizers a length, the call's
    /// else the configured one, and transformers compares a configured string, list or object with each text's length
    /// unless the call states one: what fails there fails each encode, measured on transformers 5.5.4 (#1796). The
    /// call's length padded to is held to what a cut length is.
    /// </summary>
    private static Stated Uncut(string path, Stated configured, JsonElement? cut, bool padsToLength)
    {
        if (cut is { ValueKind: not JsonValueKind.Null } stated)
        {
            // Padded to, the call's length is read as a cut length is: a zero or a boolean refused with what fails (#1257).
            return padsToLength && TokenizerLength(stated, CallLengthField).Refusal is string why
                ? RefusedAtEncode(path, why)
                : Unlimited(configured.LowerCase);
        }
        return configured.AtEncode && (configured.Unordered || padsToLength) ? configured : Unlimited(configured.LowerCase);
    }

    /// <summary>
    /// What hands tokenizers no length — no limit, or no truncation at all — so only the model's own limit then bounds
    /// the rows, and no stride reaches tokenizers.
    /// </summary>
    private static Stated Unlimited(bool lowerCase) => new(int.MaxValue, lowerCase, null, false, HandsNone: true);

    /// <summary>Whether <c>PaddingStrategy</c> takes the padding: a boolean, or one of its three names.</summary>
    private static bool PaddingKnown(JsonElement padding) =>
        padding.ValueKind is JsonValueKind.True or JsonValueKind.False
        || (padding.ValueKind == JsonValueKind.String && Array.Exists(Paddings, name => TextIs(padding, name)));

    /// <summary>
    /// The integer tokenizers takes as a <c>usize</c>: from 0 to 2^64 - 1, a boolean as 0 or 1; null for a value it
    /// refuses at every call, measured on tokenizers 0.22.2 (#1780).
    /// </summary>
    private static ulong? UsizeOf(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.True => 1,
        JsonValueKind.False => 0,
        _ => PythonInteger(value) is BigInteger integer && integer >= BigInteger.Zero ? HandedOf(integer) : null,
    };

    /// <summary>What a single text meets under a truncation strategy, by transformers' <c>TruncationStrategy</c>.</summary>
    private static Strategy StrategyOf(JsonElement strategy) => strategy.ValueKind switch
    {
        JsonValueKind.False or JsonValueKind.Null => Strategy.None,
        JsonValueKind.True => Strategy.Cuts,
        JsonValueKind.String when TextIs(strategy, "do_not_truncate") => Strategy.None,
        JsonValueKind.String when TextIs(strategy, "longest_first") || TextIs(strategy, "only_first") => Strategy.Cuts,
        JsonValueKind.String when TextIs(strategy, "only_second") => Strategy.RefusesPast,
        _ => Strategy.Unknown,
    };

    /// <summary>The length the module is built with: a kwargs <c>model_max_length</c> over <c>max_seq_length</c>.</summary>
    private static Stated Configured(string path, JsonElement root, string? openedFrom)
    {
        if (ConstructorRefusal(root, openedFrom) is string why)
        {
            return Refused(path, why);
        }
        bool lowerCase = Last(root, LowerCaseField) is JsonElement flag && Truthy(flag);
        // ConstructorRefusal leaves the processor kwargs _load_init_kwargs keeps an object.
        JsonElement? kwargs = Kept(root, RenamedFields[1])?.Kwargs;
        if (kwargs is JsonElement table && Last(table, TokenizerField) is JsonElement cap)
        {
            // transformers reads a null model_max_length as no limit at all, which the model's own then bounds.
            return cap.ValueKind == JsonValueKind.Null
                ? Unlimited(lowerCase)
                : FromLength(path, TokenizerField, cap, lowerCase);
        }

        if (Last(root, Field) is not JsonElement element || element.ValueKind == JsonValueKind.Null)
        {
            return new Stated(null, lowerCase, null, false);
        }
        return FromLength(path, Field, element, lowerCase);
    }

    /// <summary>
    /// The module built with the length <paramref name="given"/> states, neither null nor absent. transformers 5.5.4
    /// loads any value, measured (#1796): an integer of at least 1 is a length; a zero or a boolean, which cuts
    /// nothing, is refused at load, the rule #1257 settled; a number past <c>LARGE_INTEGER</c> is no limit; any other
    /// value fails each encode that hands it to tokenizers, and a string, a list or an object every encode transformers
    /// compares it with a text's length in, so they are refused at the first text.
    /// </summary>
    private static Stated FromLength(string path, string field, JsonElement given, bool lowerCase)
    {
        BigInteger? integer = PythonInteger(given);
        bool pastLarge = integer is BigInteger whole ? whole > LargeInteger : FloatOf(given) > LargeFloat;
        if (pastLarge)
        {
            return Unlimited(lowerCase);
        }
        if (integer is BigInteger stated && LengthOf(stated) is not null)
        {
            // Up to LARGE_INTEGER, an integer past the 2^64 - 1 tokenizers takes overflows each encode handing it.
            (int length, ulong handed, string? overflow) = TokenizerLength(given, field);
            return overflow is null
                ? new Stated(length, lowerCase, null, false, Handed: handed)
                : RefusedAtEncode(path, overflow) with { LowerCase = lowerCase };
        }
        string why = NotALength(field, given);
        if (given.ValueKind is JsonValueKind.True or JsonValueKind.False || integer == BigInteger.Zero)
        {
            return Refused(path, why);
        }
        bool unordered = (given.ValueKind is JsonValueKind.String or JsonValueKind.Array or JsonValueKind.Object) && !IsNamedLiteral(given);
        // The flag travels with the refusal: a call's max_length that drops it keeps the configured lowercasing.
        return RefusedAtEncode(path, why) with { LowerCase = lowerCase, Unordered = unordered };
    }

    /// <summary>The length tokenizers is handed for an integer of at least 1, or null past the 2^64 - 1 its <c>usize</c> holds.</summary>
    private static ulong? HandedOf(BigInteger length) => length <= ulong.MaxValue ? (ulong)length : null;

    /// <summary>
    /// Why sentence-transformers 6.2.0 refuses a constructor field at load, measured, in its order (#1780, #1794):
    /// <c>load_config</c> looks <c>trust_remote_code</c> up in each kwargs field and iterates <c>modality_config</c>;
    /// <c>_load_init_kwargs</c> updates the three kwargs a rename leaves, the old name's replacing the new; the call passes
    /// <c>model_name_or_path</c> itself; <c>__init__</c> then checks what <see cref="InitRefusal"/> lists. Null when none
    /// is refused.
    /// </summary>
    private static string? ConstructorRefusal(JsonElement root, string? openedFrom)
    {
        foreach ((string old, string renamed) in RenamedFields)
        {
            if ((RemoteCodeRefusal(root, old) ?? RemoteCodeRefusal(root, renamed)) is string why)
            {
                return why;
            }
        }
        JsonElement? modalities = Last(root, ModalityField);
        if (modalities is { ValueKind: not JsonValueKind.Object } notObject)
        {
            return $"states {ModalityField} {Shown(notObject)}, not a JSON object";
        }
        // Without a modality_config, load_config looks the task's defaults up before any kwargs is updated; with one,
        // __init__ checks the task first.
        string? task = TaskRefusal(root);
        if (modalities is null && task is not null)
        {
            return task;
        }
        foreach ((string old, string renamed) in RenamedFields)
        {
            if (Kept(root, (old, renamed)) is (string name, JsonElement kwargs) && kwargs.ValueKind != JsonValueKind.Object)
            {
                return $"states {name} {Shown(kwargs)}, not a JSON object";
            }
        }
        return Last(root, ModelPathField) is JsonElement model
            ? $"states {ModelPathField} {Shown(model)}, which sentence-transformers passes the module itself, so the call takes it twice"
            : task ?? InitRefusal(root, modalities, openedFrom);
    }

    /// <summary>
    /// Why <c>Transformer.__init__</c> refuses the fields, in its order after the <c>transformer_task</c>: a
    /// <c>processing_kwargs</c> it cannot take as a set of keys, a <c>tokenizer_name_or_path</c> transformers cannot
    /// load from, the <c>modality_config</c> entries and the <c>module_output_name</c> beside them, then the
    /// <c>query_expansion</c> and the <c>query_length</c> beside it (#1780, #1794, #1795).
    /// </summary>
    private static string? InitRefusal(JsonElement root, JsonElement? modalities, string? openedFrom)
    {
        if (Last(root, ProcessingField) is JsonElement processing && Truthy(processing) && !TakenAsSet(processing))
        {
            return $"states {ProcessingField} {Shown(processing)}, which Python cannot take as a set of keys";
        }
        if (Last(root, TokenizerPathField) is { ValueKind: not JsonValueKind.Null } tokenizer && !Loadable(tokenizer, openedFrom))
        {
            return $"states {TokenizerPathField} {Shown(tokenizer)}, neither a folder nor a Hugging Face repository id transformers loads from";
        }
        if (modalities is JsonElement entries && ModalityRefusal(root, entries) is string why)
        {
            return why;
        }
        return ExpansionRefusal(root);
    }

    /// <summary>Why <c>load_config</c> refuses a kwargs field: <c>trust_remote_code</c> cannot be looked up in it and removed.</summary>
    private static string? RemoteCodeRefusal(JsonElement root, string name) =>
        Last(root, name) is JsonElement value && !RemoteCodeLookedUp(value)
            ? $"states {name} {Shown(value)}, in which trust_remote_code cannot be looked up and removed"
            : null;

    /// <summary>Why the <c>transformer_task</c> is refused: not one of the tasks sentence-transformers knows.</summary>
    private static string? TaskRefusal(JsonElement root) =>
        Last(root, TaskField) is JsonElement task
        && !(task.ValueKind == JsonValueKind.String && Array.Exists(Tasks, name => TextIs(task, name)))
            ? $"states {TaskField} {Shown(task)}, not a task sentence-transformers knows"
            : null;

    /// <summary>
    /// The kwargs field <c>_load_init_kwargs</c> keeps of a renamed pair, the old name's replacing the new one's whole, and its
    /// value; null where neither is stated.
    /// </summary>
    private static (string Name, JsonElement Kwargs)? Kept(JsonElement root, (string Old, string New) pair)
    {
        if (Last(root, pair.Old) is JsonElement old)
        {
            return (pair.Old, old);
        }
        return Last(root, pair.New) is JsonElement renamed ? (pair.New, renamed) : null;
    }

    /// <summary>
    /// Whether <c>set(processing_kwargs)</c> runs: an object's keys, a string's characters, a list whose items Python
    /// hashes, none a list or an object; never a number or a boolean, which it cannot iterate (#1795).
    /// </summary>
    private static bool TakenAsSet(JsonElement value) =>
        Iterated(value, _ => true, item => item.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object));

    /// <summary>
    /// Whether Python iterates the value through to the end: an object's keys always, a string
    /// <paramref name="stringTakes"/> takes, a list each of whose items <paramref name="itemTakes"/> takes; never a
    /// number, a boolean, <c>null</c> or a named literal, which it cannot iterate.
    /// </summary>
    private static bool Iterated(JsonElement value, Func<JsonElement, bool> stringTakes, Func<JsonElement, bool> itemTakes) =>
        value.ValueKind switch
        {
            JsonValueKind.Object => true,
            JsonValueKind.String => !IsNamedLiteral(value) && stringTakes(value),
            JsonValueKind.Array => value.EnumerateArray().All(itemTakes),
            _ => false,
        };

    /// <summary>
    /// Whether transformers may load a tokenizer from the value: a string naming a folder, from where the model was
    /// opened, whose content is the caller's tokenizer's business, or one <c>validate_repo_id</c> takes, which only
    /// the Hub can answer; anything else fails at load, measured on transformers 5.5.4 (#1794).
    /// </summary>
    private static bool Loadable(JsonElement value, string? openedFrom)
    {
        if (value.ValueKind != JsonValueKind.String || IsNamedLiteral(value) || Text(value) is not string name)
        {
            return false;
        }
        return RepositoryId(name) || Folder(openedFrom, name);
    }

    /// <summary>
    /// Whether <paramref name="name"/> names a folder from <paramref name="openedFrom"/>, as Python's <c>os.path.isdir</c>
    /// answers; asked when the configuration is read, at the first call to a text overload, an empty batch included, as
    /// every file it names is (#1257). With no directory to name from, only a rooted path can name one.
    /// </summary>
    private static bool Folder(string? openedFrom, string name)
    {
        try
        {
            bool rooted = Path.IsPathRooted(name);
            if (name.Length == 0 || (openedFrom is null && !rooted))
            {
                return false;
            }
            // A rooted name off this machine, as spelled, is read past unprobed: a probe would reach whatever host the
            // untrusted file names. A relative one resolves from the user's own working directory, `..` included.
            if (rooted && OffMachine(name))
            {
                return true;
            }
            return Directory.Exists(openedFrom is null ? name : Path.Combine(openedFrom, name));
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException
            or System.Security.SecurityException)
        {
            return false;
        }
    }

    /// <summary>
    /// Whether Windows may reach another machine for a rooted path as spelled, before anything normalises it: a share or
    /// device path, a root or a drive leaning on whatever is current when probed, or a path on a network drive, which
    /// <c>GetDriveType</c> answers locally. Elsewhere a mounted share is not told apart.
    /// </summary>
    private static bool OffMachine(string path)
    {
        if (Path.DirectorySeparatorChar != '\\')
        {
            return false;
        }
        // Any leading separator: a share or device path (\\host, \\?\, \\.\, \??\), or a root on whichever drive is
        // current when it is probed, which may by then be a network one.
        if (path.Length >= 1 && path[0] is '\\' or '/')
        {
            return true;
        }
        if (path.Length < 2 || path[1] != ':')
        {
            return false;
        }
        // A drive without a root names that drive's current directory, wherever it then is.
        bool driveRelative = path.Length == 2 || path[2] is not ('\\' or '/');
        return driveRelative || new DriveInfo(path.Substring(0, 1)).DriveType == DriveType.Network;
    }

    /// <summary>The current directory a relative tokenizer path names a folder from, or null where it is gone.</summary>
    public static string? OpenedFrom()
    {
        try
        {
            return Directory.GetCurrentDirectory();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or NotSupportedException
            or System.Security.SecurityException)
        {
            return null;
        }
    }

    /// <summary>
    /// huggingface_hub 1.33.0's <c>validate_repo_id</c>, whose pattern takes <c>re.ASCII</c>, bounds the namespace as
    /// the name and is <c>fullmatch</c>ed, measured refusing <c>café</c>, <c>abc\n</c> and a namespace of 97: a name or
    /// a namespace and a name, each 1 to 96 of ASCII letters, digits, <c>_</c>, <c>-</c> and <c>.</c>, opening and
    /// closing on a letter, a digit or <c>_</c>; no <c>--</c>, no <c>..</c>, no closing <c>.git</c>.
    /// </summary>
    private static bool RepositoryId(string name) =>
        name.Split('/') is { Length: <= 2 } parts
        && parts.All(RepositoryPart)
        && name.IndexOf("--", StringComparison.Ordinal) < 0
        && name.IndexOf("..", StringComparison.Ordinal) < 0
        && !name.EndsWith(".git", StringComparison.Ordinal);

    private static bool RepositoryPart(string part) =>
        part.Length is >= 1 and <= 96
        && WordCharacter(part[0])
        && WordCharacter(part[part.Length - 1])
        && part.All(c => WordCharacter(c) || c is '-' or '.');

    /// <summary>A character <c>\w</c> matches under <c>re.ASCII</c>.</summary>
    private static bool WordCharacter(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9') or '_';

    /// <summary>
    /// Why <c>_normalize_query_expansion</c> and the setter refuse a non-null <c>query_expansion</c>, in their order: not
    /// an object, an unknown key, a strategy other than <c>fixed</c> and <c>min</c>, an <c>attend</c> not a boolean, a
    /// length not an integer of at least 1, a token no tokenizer can look up, then a <c>query_length</c> below that
    /// length or no number to compare with it (#1794). A string token is checked against the tokenizer Python loads.
    /// </summary>
    private static string? ExpansionRefusal(JsonElement root)
    {
        if (Last(root, ExpansionField) is not { ValueKind: not JsonValueKind.Null } expansion)
        {
            return null;
        }
        if (ExpansionShapeRefusal(expansion, out BigInteger length) is string why)
        {
            return $"states {ExpansionField} {Shown(expansion)}, {why}";
        }
        return Last(root, QueryLengthField) is { ValueKind: not JsonValueKind.Null } query && Below(query, length) is not false
            ? $"states {QueryLengthField} {Shown(query)}, below the {ExpansionField} length {length} or no number to compare with it"
            : null;
    }

    /// <summary>What <c>_normalize_query_expansion</c> and the setter's tokenizer check refuse in the object itself.</summary>
    /// <param name="expansion">The non-null <c>query_expansion</c>.</param>
    /// <param name="length">Its <c>length</c> where nothing is refused, else 0.</param>
    private static string? ExpansionShapeRefusal(JsonElement expansion, out BigInteger length)
    {
        length = BigInteger.Zero;
        if (expansion.ValueKind != JsonValueKind.Object)
        {
            return "not a JSON object";
        }
        if (expansion.EnumerateObject().Any(property => !Array.Exists(ExpansionKeys, key => NameIs(property, key))))
        {
            return "with a key sentence-transformers does not know";
        }
        if (Last(expansion, ExpansionStrategyField) is not { ValueKind: JsonValueKind.String } strategy
            || !(TextIs(strategy, "fixed") || TextIs(strategy, "min")))
        {
            return "whose strategy is neither fixed nor min";
        }
        if (Last(expansion, ExpansionAttendField) is { ValueKind: not (JsonValueKind.True or JsonValueKind.False) })
        {
            return "whose attend is not a boolean";
        }
        if (Last(expansion, ExpansionLengthField) is not JsonElement stated
            || PythonInteger(stated) is not BigInteger given || given < BigInteger.One)
        {
            return "whose length is not an integer of at least 1";
        }
        if (Last(expansion, ExpansionTokenField) is JsonElement token && !TokenLookedUp(token))
        {
            return "whose token no tokenizer can look up";
        }
        length = given;
        return null;
    }

    /// <summary>
    /// Whether <c>convert_tokens_to_ids</c> takes the token: null, or a string, an object's keys or a list of strings,
    /// each encodable as UTF-8, which a lone surrogate is not and <see cref="Text(JsonElement)"/> reads as null; never
    /// a number or a boolean, which it cannot iterate, measured on transformers 5.5.4 (#1794).
    /// </summary>
    private static bool TokenLookedUp(JsonElement token) => token.ValueKind switch
    {
        JsonValueKind.Null => true,
        JsonValueKind.Object => token.EnumerateObject().All(property => Text(property) is not null),
        _ => Iterated(
            token,
            // No vocabulary holds the empty token, so a string one is answered with the unknown id; a list comes back as a
            // list, never compared with it, so its items need only encode (#1794).
            text => Text(text) is { Length: > 0 },
            item => item.ValueKind == JsonValueKind.String && !IsNamedLiteral(item) && Text(item) is not null),
    };

    /// <summary>
    /// Python's <c>value &lt; length</c> for an integer <paramref name="length"/>: exact for an integer or a float, a
    /// boolean as 0 or 1, false for NaN; null where Python cannot compare the two.
    /// </summary>
    private static bool? Below(JsonElement value, BigInteger length)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.True:
                return BigInteger.One < length;
            case JsonValueKind.False:
                return BigInteger.Zero < length;
            case JsonValueKind.Number when PythonInteger(value) is BigInteger integer:
                return integer < length;
            default:
                if (FloatOf(value) is not double number)
                {
                    return null;
                }
                // A float is below an integer exactly when its floor is; infinities and NaN compare as Python's do.
                return !double.IsNaN(number) && !double.IsPositiveInfinity(number)
                    && (double.IsNegativeInfinity(number) || new BigInteger(Math.Floor(number)) < length);
        }
    }

    /// <summary>The float Python reads a JSON number or a named literal as; null for any other value.</summary>
    private static double? FloatOf(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.Number => PythonFloat(value.GetRawText()),
        JsonValueKind.String => NamedFloat(value.GetRawText()),
        _ => null,
    };

    /// <summary>The float a string's raw text stands for when it is the marker, a literal's name and the closing quote.</summary>
    private static double? NamedFloat(string raw)
    {
        if (!raw.StartsWith(QuotedLiteralStart, StringComparison.Ordinal))
        {
            return null;
        }
        return LiteralAt(raw, QuotedLiteralStart.Length) is (string name, double value)
            && raw.Length == QuotedLiteralStart.Length + name.Length + 1
                ? value
                : null;
    }

    /// <summary>
    /// Whether <c>"trust_remote_code" in value</c> and its <c>pop</c> both run: an object always; a string or a list
    /// that does not hold it; never a number, a boolean or <c>null</c>, which <c>in</c> cannot iterate.
    /// </summary>
    private static bool RemoteCodeLookedUp(JsonElement value) =>
        Iterated(
            value,
            kwargs => Decoded(kwargs.GetRawText()).IndexOf(RemoteCodeField, StringComparison.Ordinal) < 0,
            item => !(item.ValueKind == JsonValueKind.String && TextIs(item, RemoteCodeField)));

    /// <summary>
    /// Why an object <c>modality_config</c> is refused at load, its unknown keys dropped: a <c>message</c> entry that is
    /// not an object, read first, then no <c>module_output_name</c> beside it, then an entry that is not an object
    /// naming <c>method</c> and <c>method_output_name</c>.
    /// </summary>
    private static string? ModalityRefusal(JsonElement root, JsonElement modalities)
    {
        // Python's dict keeps a duplicated key's last value; a key that is not a modality, or a join of them, is dropped.
        var entries = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (JsonProperty property in modalities.EnumerateObject())
        {
            if (Text(property) is string key && key.Split('+').All(part => Array.IndexOf(Modalities, part) >= 0))
            {
                entries[key] = property.Value;
            }
        }
        // __init__ reads the message entry's format before it asks for a module_output_name (#1794).
        if (entries.TryGetValue(MessageModality, out JsonElement message) && message.ValueKind != JsonValueKind.Object)
        {
            return $"states the {ModalityField} entry '{MessageModality}' {Shown(message)}, not an object whose format can be read";
        }
        if (Last(root, OutputNameField) is not { ValueKind: not JsonValueKind.Null })
        {
            return $"states a {ModalityField} without a {OutputNameField}";
        }
        foreach (KeyValuePair<string, JsonElement> entry in entries)
        {
            if (entry.Value.ValueKind != JsonValueKind.Object || Last(entry.Value, "method") is null || Last(entry.Value, "method_output_name") is null)
            {
                return $"states the {ModalityField} entry '{entry.Key}' {Shown(entry.Value)}, not an object naming method and method_output_name";
            }
        }
        return null;
    }

    /// <summary>
    /// Whether the file is present by Python's <c>Path.exists()</c>, which <c>load_file_path</c> asks, and, when it is,
    /// the file read as Python's json reads it, or why not. A directory is present, and a link is followed, so one whose
    /// target is gone is absent.
    /// </summary>
    private static (bool Present, JsonDocument? Document, string? Refusal) Open(string path)
    {
        if (Directory.Exists(path))
        {
            // Python's Path.exists() holds for a directory, and sentence-transformers then fails to open it.
            return (true, null, "is a directory");
        }

        byte[] bytes;
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (stream.Length > ArtifactLimits.DefaultMaxTotalBytes)
            {
                return (true, null, $"is longer than {ArtifactLimits.DefaultMaxTotalBytes} bytes");
            }
            bytes = new byte[stream.Length];
            int read = StreamFill.UpTo(stream, bytes);
            if (read < bytes.Length)
            {
                // A file that shrank while it was read: Python reads what is there, so the rest is no part of it.
                Array.Resize(ref bytes, read);
            }
        }
        catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
        {
            return (false, null, null);
        }
        catch (IOException) when (IsLink(path))
        {
            // A link that cannot be followed, a loop among them: Path.exists() answers False on ELOOP, so it is absent.
            return (false, null, null);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Present but unreadable: sentence-transformers finds it and fails to open it.
            return (true, null, $"cannot be read: {exception.Message}");
        }

        string text;
        try
        {
            // Python's json refuses a BOM and its utf-8 codec invalid bytes; Encoding.UTF8 would drop the one and replace the other.
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                return (true, null, "starts with a byte-order mark");
            }
            text = ArtifactEncoding.Utf8NoBom.GetString(bytes);
        }
        catch (DecoderFallbackException)
        {
            return (true, null, "is not UTF-8");
        }

        try
        {
            string? quoted = NamedLiteralsQuoted(text);
            return quoted is null
                ? (true, null, $"holds an integer of more than {MaxIntegerDigits} digits, which Python's json refuses")
                : (true, JsonDocument.Parse(quoted, ReaderOptions), null);
        }
        catch (JsonException)
        {
            return (true, null, "cannot be read as JSON");
        }
    }

    /// <summary>A field's last value, as Python's json keeps a duplicated key's; null when it is absent.</summary>
    /// <remarks>The value lives in the caller's document, which every caller holds open while it reads it.</remarks>
    private static JsonElement? Last(JsonElement root, string name)
    {
        JsonElement? last = null;
        foreach (JsonProperty property in root.EnumerateObject().Where(property => NameIs(property, name)))
        {
            last = property.Value;
        }
        return last;
    }

    /// <summary>
    /// Whether a key is <paramref name="name"/>. A key holding a lone surrogate, which Python's json reads, cannot be
    /// decoded to compare and cannot equal a field name, so it is no field rather than a failure.
    /// </summary>
    private static bool NameIs(JsonProperty property, string name)
    {
        try
        {
            return property.NameEquals(name);
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    /// <summary>Python's truth of a JSON value: false, null, a zero, an empty string or an empty container are false.</summary>
    private static bool Truthy(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.True => true,
        JsonValueKind.Number => PythonFloat(value.GetRawText()) != 0,
        JsonValueKind.String => value.GetRawText().Length > 2,
        JsonValueKind.Array => value.GetArrayLength() > 0,
        JsonValueKind.Object => value.EnumerateObject().Any(),
        _ => false,
    };

    /// <summary>
    /// The float Python reads a JSON number's raw text as: one that underflows is zero, one that overflows infinite,
    /// which .NET Framework's parser refuses rather than returns.
    /// </summary>
    private static double PythonFloat(string raw)
    {
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
        {
            return value;
        }
        // .NET Framework's parser refuses a float past the double range, and reads one below it as zero, as Python does.
        int exponent = raw.IndexOfAny(['e', 'E']);
        if (!(exponent < 0 ? raw : raw.Substring(0, exponent)).Any(c => c is >= '1' and <= '9'))
        {
            return 0;
        }
        return raw[0] == '-' ? double.NegativeInfinity : double.PositiveInfinity;
    }

    /// <summary>
    /// The int Python reads a JSON number as, however long within the <see cref="MaxIntegerDigits"/> its json reads,
    /// which the parse refuses past: a sign and digits only, a fraction or an exponent making it a float; null for any
    /// other value.
    /// </summary>
    private static BigInteger? PythonInteger(JsonElement value)
    {
        if (value.ValueKind != JsonValueKind.Number)
        {
            return null;
        }
        string raw = value.GetRawText();
        return raw.IndexOfAny(['.', 'e', 'E']) < 0 ? BigInteger.Parse(raw, CultureInfo.InvariantCulture) : null;
    }

    /// <summary>An integer of at least 1, capped at <c>int</c>: past it only the model's own limit, the caller's, bounds it.</summary>
    private static int? LengthOf(BigInteger length) =>
        length >= BigInteger.One ? (int)BigInteger.Min(length, int.MaxValue) : null;

    /// <summary>
    /// Python's json reads NaN, Infinity and -Infinity where a value stands; quoted with a marker, the parser here reads
    /// them there, and a name it refuses, such as one in a key's place, stays a parse error. Null when an integer outside
    /// a string has more digits than Python's json reads.
    /// </summary>
    private static string? NamedLiteralsQuoted(string text)
    {
        var quoted = new StringBuilder(text.Length);
        var containers = new Stack<char>();
        bool valueExpected = true;
        int i = 0;
        while (i < text.Length)
        {
            char c = text[i];
            if (c == '"')
            {
                int end = StringEnd(text, i);
                quoted.Append(text, i, end - i);
                i = end;
                valueExpected = false;
                continue;
            }
            string? literal = valueExpected ? LiteralAt(text, i)?.Name : null;
            if (literal is not null)
            {
                quoted.Append(QuotedLiteralStart).Append(literal).Append('"');
                i += literal.Length;
                valueExpected = false;
                continue;
            }
            if (c is '-' or (>= '0' and <= '9'))
            {
                int end = NumberEnd(text, i, out bool longInteger);
                if (longInteger)
                {
                    return null;
                }
                quoted.Append(text, i, end - i);
                i = end;
                valueExpected = false;
                continue;
            }
            valueExpected = Expected(c, containers, valueExpected);
            quoted.Append(c);
            i++;
        }
        return quoted.ToString();
    }

    /// <summary>The named literal spelled at <paramref name="start"/>, with the float it is, or null where none is.</summary>
    private static (string Name, double Value)? LiteralAt(string text, int start)
    {
        foreach ((string name, double value) in NamedFloats)
        {
            if (string.CompareOrdinal(text, start, name, 0, name.Length) == 0)
            {
                return (name, value);
            }
        }
        return null;
    }

    /// <summary>Just past the string opening at <paramref name="start"/>: an escape takes the character after it.</summary>
    private static int StringEnd(string text, int start)
    {
        int i = start + 1;
        while (i < text.Length && text[i] != '"')
        {
            i += text[i] == '\\' ? 2 : 1;
        }
        return Math.Min(i + 1, text.Length);
    }

    /// <summary>Where the number starting at <paramref name="start"/> ends; whether it is an integer past Python's digits.</summary>
    private static int NumberEnd(string text, int start, out bool longInteger)
    {
        int i = text[start] == '-' ? start + 1 : start;
        int digits = 0;
        while (i < text.Length && text[i] is >= '0' and <= '9')
        {
            i++;
            digits++;
        }
        bool fraction = i < text.Length && text[i] is '.' or 'e' or 'E';
        while (i < text.Length && (text[i] is >= '0' and <= '9' or '.' or 'e' or 'E' or '+' or '-'))
        {
            i++;
        }
        longInteger = !fraction && digits > MaxIntegerDigits;
        return i;
    }

    /// <summary>Whether a value may stand after <paramref name="c"/>: after a colon, an opening bracket, or an array's comma.</summary>
    private static bool Expected(char c, Stack<char> containers, bool before)
    {
        switch (c)
        {
            case '{':
                containers.Push(c);
                return false;
            case '[':
                containers.Push(c);
                return true;
            case '}' or ']':
                if (containers.Count > 0)
                {
                    containers.Pop();
                }
                return false;
            case ':':
                return true;
            case ',':
                return containers.Count > 0 && containers.Peek() == '[';
            case ' ' or '\t' or '\n' or '\r':
                return before;
            default:
                return false;
        }
    }

    /// <summary>
    /// Whether the value is a named literal the parser here reads as a marked string: a float to Python. A file string
    /// spelled as the marker and a name reads as that float too, the cost <see cref="QuotedLiteralStart"/> states.
    /// </summary>
    private static bool IsNamedLiteral(JsonElement value) =>
        value.ValueKind == JsonValueKind.String && IsNamedLiteral(value.GetRawText());

    /// <summary>Whether a string's raw text is the marker, a literal's name and the closing quote, and nothing else.</summary>
    private static bool IsNamedLiteral(string raw) => NamedFloat(raw) is not null;

    /// <summary>The value as the file spells it, every quoted named literal unquoted; never decoded, so no string can throw.</summary>
    private static string Shown(JsonElement element)
    {
        string raw = element.GetRawText();
        if (raw.IndexOf(QuotedLiteralStart, StringComparison.Ordinal) < 0)
        {
            return raw;
        }
        // Every string that is a quoted literal, the value itself or one nested in it, unquoted; other strings kept whole.
        var shown = new StringBuilder(raw.Length);
        int i = 0;
        while (i < raw.Length)
        {
            if (raw[i] != '"')
            {
                shown.Append(raw[i++]);
                continue;
            }
            int end = StringEnd(raw, i);
            string text = raw.Substring(i, end - i);
            shown.Append(IsNamedLiteral(text) ? text.Substring(QuotedLiteralStart.Length, text.Length - QuotedLiteralStart.Length - 1) : text);
            i = end;
        }
        return shown.ToString();
    }

    /// <summary>
    /// The Transformer module's folder, the one holding the graph or its <c>onnx/</c>, when a <c>modules.json</c> names
    /// it: its own, listing the module at its root, or its parent's, listing it under its folder's name. Without one
    /// sentence-transformers builds a fresh module and reads no configuration; only the module folder's own file is
    /// refused.
    /// </summary>
    private static (string? Module, string? Refusal) ModuleDirectory(string? directory)
    {
        if (directory is null)
        {
            return (null, null);
        }

        string module = InOnnxFolder(directory) && Path.GetDirectoryName(directory) is string root ? root : directory;
        (string? Module, string? Refusal) own = Listed(module, module);
        if (own.Module is not null || own.Refusal is not null || Path.GetDirectoryName(module) is not string parent)
        {
            return own;
        }
        // sentence-transformers opens the parent's file only when loading the parent, which a defect in it cannot tell.
        (string? Module, string? Refusal) inParent = Listed(parent, module);
        return inParent.Refusal is null ? inParent : (null, null);
    }

    /// <summary>Whether <paramref name="root"/>'s <c>modules.json</c> lists a Transformer module in <paramref name="module"/>.</summary>
    private static (string? Module, string? Refusal) Listed(string root, string module)
    {
        string path = Path.Combine(root, ModulesFile);
        (bool present, JsonDocument? document, string? why) = Open(path);
        if (!present)
        {
            return (null, null);
        }
        if (document is null)
        {
            return (null, RefusalMessage(path, why!));
        }

        using (document)
        {
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                return (null, RefusalMessage(path, "is not a JSON array"));
            }
            // _load_config_modules reads and validates every entry before it loads any, so one bad entry fails them all.
            var modules = new List<(string Folder, string Name)>();
            foreach (JsonElement entry in document.RootElement.EnumerateArray())
            {
                if (entry.ValueKind != JsonValueKind.Object
                    || Last(entry, "path") is not { ValueKind: JsonValueKind.String } listedPath
                    || Last(entry, "type") is not { ValueKind: JsonValueKind.String } type)
                {
                    return (null, RefusalMessage(path, "lists a module without a string path and type"));
                }
                if (Text(listedPath) is not string folder || Text(type) is not string name)
                {
                    return (null, RefusalMessage(path, "lists a module path or type that is not text"));
                }
                if (!Relative(folder))
                {
                    return (null, RefusalMessage(path, $"lists the module path '{folder}', outside the model's folder"));
                }
                modules.Add((folder, name));
            }

            bool listed = modules.Exists(listing =>
                (string.Equals(listing.Name, "Transformer", StringComparison.Ordinal)
                    || listing.Name.EndsWith(".Transformer", StringComparison.Ordinal))
                && SameFolder(root, listing.Folder, module));
            return (listed ? module : null, null);
        }
    }

    /// <summary>
    /// <c>_validate_relative_path</c>: no Windows anchor — a leading separator or a drive, refused on every system — no
    /// <c>..</c> part in either syntax, and no NUL.
    /// </summary>
    private static bool Relative(string folder)
    {
        bool anchored = folder.Length > 0
            && (folder[0] is '/' or '\\' || (folder.Length > 1 && folder[1] == ':' && char.IsLetter(folder[0])));
        return !anchored && !folder.Split(PathSeparators).Contains("..") && folder.AsSpan().IndexOf('\0') < 0;
    }

    /// <summary>
    /// Whether the model sits in its root's <c>onnx/</c>: a folder of that name, or, on a case-insensitive file system,
    /// one of that name in any case, as Windows opens <c>ONNX</c> as <c>onnx</c>. A case-sensitive one keeps them apart.
    /// </summary>
    private static bool InOnnxFolder(string directory)
    {
        string name = Path.GetFileName(directory);
        if (string.Equals(name, OnnxFolder, StringComparison.Ordinal))
        {
            return true;
        }
        // A spelling no folder holds opens only where case is ignored, so it tells such a file system from the others.
        string other = string.Equals(name, "OnNx", StringComparison.Ordinal) ? "oNnX" : "OnNx";
        return string.Equals(name, OnnxFolder, StringComparison.OrdinalIgnoreCase)
            && Path.GetDirectoryName(directory) is string root
            && Directory.Exists(Path.Combine(root, other));
    }

    /// <summary>Whether <paramref name="path"/> is itself a symbolic link, read off the link rather than its target.</summary>
    private static bool IsLink(string path)
    {
        try
        {
            return (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }

    /// <summary>A property's name, or null when it holds a lone surrogate, which no modality is named with.</summary>
    private static string? Text(JsonProperty property)
    {
        try
        {
            return property.Name;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>A string value's text, or null when it holds a lone surrogate, which no path or module name can carry.</summary>
    private static string? Text(JsonElement value)
    {
        try
        {
            return value.GetString();
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>
    /// Whether two paths name one folder, as <c>Path(root, subfolder)</c> resolves <c>""</c>, <c>.</c> and <c>./</c> to
    /// the root.
    /// </summary>
    /// <remarks>A folder this file system cannot name, as .NET Framework refuses <c>|</c> in a path, names no folder.</remarks>
    private static bool SameFolder(string root, string folder, string module)
    {
        try
        {
            string listed = Path.GetFullPath(Path.Combine(root, folder)).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string own = module.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (string.Equals(listed, own, StringComparison.Ordinal))
            {
                return true;
            }
            // Spelled otherwise, the listing opens the folder only where case is ignored, which a spelling no folder holds tells.
            string swapped = new([.. own.Select(c => char.IsUpper(c) ? char.ToLowerInvariant(c) : char.ToUpperInvariant(c))]);
            return string.Equals(listed, own, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(swapped, own, StringComparison.Ordinal)
                && Directory.Exists(swapped);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return false;
        }
    }

    /// <summary>
    /// Whether a string value decodes to <paramref name="expected"/>; one holding a lone surrogate never does, and
    /// <c>ValueEquals</c> throws on it once it is as long as the name, so that throw is no match either.
    /// </summary>
    private static bool TextIs(JsonElement value, string expected)
    {
        try
        {
            return value.ValueEquals(expected);
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    private static Stated Refused(string path, string reason) =>
        new(null, false, RefusalMessage(path, reason), false);

    /// <summary>A refusal Python meets in <c>preprocess</c>, at each encode, so an empty batch never meets it (#1780).</summary>
    private static Stated RefusedAtEncode(string path, string reason) =>
        new(null, false, RefusalMessage(path, reason), false, AtEncode: true);

    /// <summary>A refusal of the configuration at <paramref name="path"/>, for <paramref name="reason"/>.</summary>
    public static string RefusalMessage(string path, string reason) =>
        $"The sentence-transformers configuration '{path}' beside the model {reason}.";

    /// <summary>What one configuration states for the two text overloads, or why it is refused.</summary>
    /// <param name="Length">The length, null when no module or configuration is found or it states none.</param>
    /// <param name="LowerCase">Whether texts are lowercased, by Python's truth of <c>do_lower_case</c>.</param>
    /// <param name="Refusal">
    /// Why a file is refused: it cannot be read, is not what sentence-transformers loads or encodes under, or states a
    /// length that is neither null, nor an integer from 1 to 2^64 - 1, nor a number past <c>LARGE_INTEGER</c>: at load a
    /// zero or a boolean, which it loads and cuts nothing with, at each encode any other, as <paramref name="AtEncode"/> says.
    /// </param>
    /// <param name="RefusesPast">Whether a text past the length is refused, under <c>only_second</c>, rather than cut.</param>
    /// <param name="Stride">The <c>stride</c> the tokenizer's call takes, where the truncation cuts and one is stated (#1780).</param>
    /// <param name="Source">The file a stated stride was read from, for its refusal.</param>
    /// <param name="Handed">The length tokenizers is handed to cut at; null where it is handed the default, or none.</param>
    /// <param name="AtEncode">Whether <paramref name="Refusal"/> fails each encode rather than the load, so no text meets it.</param>
    /// <param name="Unordered">
    /// Whether <paramref name="Refusal"/> is a configured length transformers compares with each text's, a string, a
    /// list or an object, so it fails every encode whose call states no <c>max_length</c>, whatever the truncation.
    /// </param>
    /// <param name="HandsNone">
    /// Whether tokenizers is handed no length: Python cuts nothing; Lodestar caps at the model's limit, but beside
    /// <paramref name="RefusesPast"/>, <c>only_second</c>, cuts nothing either (#1793).
    /// </param>
    public readonly record struct Stated(
        int? Length,
        bool LowerCase,
        string? Refusal,
        bool RefusesPast,
        ulong? Stride = null,
        string? Source = null,
        ulong? Handed = null,
        bool AtEncode = false,
        bool Unordered = false,
        bool HandsNone = false);

    /// <summary>A truncation strategy as a single text meets it.</summary>
    private enum Strategy
    {
        /// <summary>Cut at the length.</summary>
        Cuts,

        /// <summary>Never cut, so only the model's own limit bounds the row.</summary>
        None,

        /// <summary>Never cut, and refused past the length, as tokenizers refuses <c>only_second</c>.</summary>
        RefusesPast,

        /// <summary>A value <c>TruncationStrategy</c> refuses.</summary>
        Unknown,
    }
}

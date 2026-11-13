using System.Globalization;
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

    private const string PaddingField = "padding";

    /// <summary>The call's kwargs that decide where a text is cut, the only ones read from <c>processing_kwargs</c>.</summary>
    private static readonly string[] CallFields = [TruncationField, MaxLengthField, PaddingField];

    private const string OnnxFolder = "onnx";

    private const string ModulesFile = "modules.json";

    /// <summary>Python 3.12's <c>sys.get_int_max_str_digits()</c>: its json refuses an integer of more digits, sign aside.</summary>
    private const int MaxIntegerDigits = 4300;

    /// <summary>
    /// Opens a quoted named literal in the raw JSON text; a file string spelled the same way is shown as the literal, a
    /// message's only cost, since a string is refused as a length either way.
    /// </summary>
    private const string QuotedLiteralStart = "\"\\u0001";

    private static readonly string[] NamedLiterals = ["-Infinity", "Infinity", "NaN"];

    /// <summary>The separators a <c>modules.json</c> path may use, whichever system wrote it.</summary>
    private static readonly char[] PathSeparators = ['/', '\\'];

    // sentence-transformers 6.2.0's _load_init_kwargs renames the legacy key over the new one, so it wins when both appear.
    private static readonly string[] KwargsFields = ["tokenizer_args", "processor_kwargs"];

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
    /// <returns>
    /// The length, null when no module or configuration is found or it states none; whether texts are lowercased, by
    /// Python's truth of the field; or the refusal when a file cannot be read, is not what sentence-transformers loads, or
    /// states a length that is neither null nor an integer of at least 1, all of which sentence-transformers refuses at
    /// load but a zero, which it loads and then cuts nothing with; whether the length is one a text is refused past, under
    /// <c>only_second</c>, rather than cut at.
    /// </returns>
    public static (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) Read(string modelPath)
    {
        (string? module, string? refusal) = ModuleDirectory(Path.GetDirectoryName(modelPath));
        if (refusal is not null || module is null)
        {
            return (null, false, refusal, false);
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
                    return root.ValueKind == JsonValueKind.Object ? FromObject(path, root) : Refused(path, "is not a JSON object");
                }
            }
        }

        return (null, false, null, false);
    }

    /// <summary>
    /// The length and the flag one configuration states: a call's <c>processing_kwargs</c> over the tokenizer's
    /// <c>model_max_length</c> over <c>max_seq_length</c>.
    /// </summary>
    private static (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) FromObject(string path, JsonElement root)
    {
        (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) configured = Configured(path, root);
        if (configured.Refusal is not null)
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
            return Refused(path, $"states {ProcessingField} {Shown(processing)}, not a JSON object");
        }

        // Transformer.preprocess updates its text kwargs with a truthy text bucket, its common ones with any common
        // bucket, and calls {**text, **common}: the buckets merge key by key, common's word winning (#1257).
        var merged = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (string bucket in new[] { "text", "common" })
        {
            // Only text passes `if overrides :=`: a falsy text is skipped, a falsy common reaches update, which refuses
            // null, 0 and false.
            if (Last(processing, bucket) is not JsonElement kwargs || (bucket == "text" && !Truthy(kwargs)))
            {
                continue;
            }
            if (Updated(kwargs, merged) is string why)
            {
                return Refused(path, $"states {ProcessingField}.{bucket} {Shown(kwargs)}, {why}");
            }
        }

        return Cut(path, configured, merged);
    }

    /// <summary>
    /// <c>dict.update(kwargs)</c> on the keys read here: an object's entries, or a list's pairs in order; why Python
    /// refuses it otherwise.
    /// </summary>
    private static string? Updated(JsonElement kwargs, Dictionary<string, JsonElement> merged)
    {
        if (kwargs.ValueKind == JsonValueKind.Object)
        {
            foreach (string field in CallFields)
            {
                if (Last(kwargs, field) is JsonElement value)
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
        return kwargs.ValueKind == JsonValueKind.Array ? UpdatedFromPairs(kwargs, merged) : "which dict.update cannot take";
    }

    /// <summary><c>dict.update</c> over a list: each item a pair, in order; why Python refuses one otherwise.</summary>
    private static string? UpdatedFromPairs(JsonElement kwargs, Dictionary<string, JsonElement> merged)
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
            if (key.ValueKind is JsonValueKind.Array or JsonValueKind.Object)
            {
                return "whose key is a list or an object, which Python cannot hash";
            }
            if (key.ValueKind == JsonValueKind.String && Array.Find(CallFields, field => TextIs(key, field)) is string named)
            {
                merged[named] = value;
            }
        }
        return null;
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
            if (seen.Add(new string([.. Units(raw).Select(unit => unit.Unit)])))
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
    /// <c>TruncationStrategy</c> refuses at every call, and so does this reader (#1257).
    /// </remarks>
    private static (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) Cut(
        string path, (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) configured, Dictionary<string, JsonElement> merged)
    {
        JsonElement? cut = merged.TryGetValue(MaxLengthField, out JsonElement length) ? length : null;
        bool lengthOnly = cut is { ValueKind: not JsonValueKind.Null }
            && merged.TryGetValue(PaddingField, out JsonElement padding) && padding.ValueKind == JsonValueKind.False;
        Strategy strategy = merged.TryGetValue(TruncationField, out JsonElement given) && !(lengthOnly && given.ValueKind == JsonValueKind.Null)
            ? StrategyOf(given)
            : Strategy.Cuts;
        if (strategy == Strategy.Unknown)
        {
            return Refused(path, $"states {ProcessingField} {TruncationField} {Shown(given)}, which a single text cannot be cut by");
        }
        if (strategy == Strategy.None)
        {
            // No truncation at all: only the model's own limit then bounds the rows.
            return (int.MaxValue, configured.LowerCase, null, false);
        }
        bool refusesPast = strategy == Strategy.RefusesPast;

        if (cut is not JsonElement stated || stated.ValueKind == JsonValueKind.Null)
        {
            return (configured.Length, configured.LowerCase, null, refusesPast);
        }
        return Length(stated) is int value
            ? (value, configured.LowerCase, null, refusesPast)
            : Refused(path, $"states {ProcessingField} {MaxLengthField} {Shown(stated)}, not an integer of at least 1");
    }

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
    private static (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) Configured(string path, JsonElement root)
    {
        bool lowerCase = Last(root, LowerCaseField) is JsonElement flag && Truthy(flag);
        JsonElement? kwargs = KwargsFields.Select(name => Last(root, name)).FirstOrDefault(found => found is not null);
        if (kwargs is JsonElement given && given.ValueKind != JsonValueKind.Object)
        {
            return Refused(path, $"states tokenizer kwargs {Shown(given)}, not a JSON object");
        }

        if (kwargs is JsonElement table && Last(table, TokenizerField) is JsonElement cap)
        {
            // transformers reads a null model_max_length as no limit at all, which the model's own then bounds.
            if (cap.ValueKind == JsonValueKind.Null)
            {
                return (int.MaxValue, lowerCase, null, false);
            }
            return Length(cap) is int capped
                ? (capped, lowerCase, null, false)
                : Refused(path, $"states {TokenizerField} {Shown(cap)}, not an integer of at least 1");
        }

        if (Last(root, Field) is not JsonElement element || element.ValueKind == JsonValueKind.Null)
        {
            return (null, lowerCase, null, false);
        }
        return Length(element) is int value
            ? (value, lowerCase, null, false)
            : Refused(path, $"states {Field} {Shown(element)}, not an integer of at least 1");
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
        JsonValueKind.Number => NonZero(value.GetRawText()),
        JsonValueKind.String => value.GetRawText().Length > 2,
        JsonValueKind.Array => value.GetArrayLength() > 0,
        JsonValueKind.Object => value.EnumerateObject().Any(),
        _ => false,
    };

    /// <summary>
    /// Whether Python reads the number as non-zero: a float that underflows is zero, one that overflows is infinite,
    /// and .NET Framework's parser refuses the latter rather than returning infinity.
    /// </summary>
    private static bool NonZero(string raw)
    {
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
        {
            return value != 0;
        }
        int exponent = raw.IndexOfAny(['e', 'E']);
        return (exponent < 0 ? raw : raw.Substring(0, exponent)).Any(c => c is >= '1' and <= '9');
    }

    private static int? Length(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Number)
        {
            return null;
        }
        if (element.TryGetInt64(out long length))
        {
            // Past int, only the model's own limit can bound it, and that is applied by the caller.
            return length < 1 ? null : (int)Math.Min(length, int.MaxValue);
        }
        // Python reads any run of digits as an int, however long; a fraction or an exponent makes a float, refused.
        string raw = element.GetRawText();
        return raw.All(c => c is >= '0' and <= '9') ? int.MaxValue : null;
    }

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
            string? literal = valueExpected
                ? Array.Find(NamedLiterals, name => string.CompareOrdinal(text, i, name, 0, name.Length) == 0)
                : null;
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

    /// <summary>The value as the file spells it, a quoted named literal unquoted; never decoded, so no string can throw.</summary>
    private static string Shown(JsonElement element)
    {
        string raw = element.GetRawText();
        return raw.StartsWith(QuotedLiteralStart, StringComparison.Ordinal)
            ? raw.Substring(QuotedLiteralStart.Length, raw.Length - QuotedLiteralStart.Length - 1)
            : raw;
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

    /// <summary>Whether a string value decodes to <paramref name="expected"/>; one holding a lone surrogate never does.</summary>
    private static bool TextIs(JsonElement value, string expected) => value.ValueEquals(expected);

    private static (int? Length, bool LowerCase, string? Refusal, bool RefusesPast) Refused(string path, string reason) =>
        (null, false, RefusalMessage(path, reason), false);

    private static string RefusalMessage(string path, string reason) =>
        $"The sentence-transformers configuration '{path}' beside the model {reason}.";

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

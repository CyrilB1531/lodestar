using System.Text;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Tests.Fixtures;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>
/// The length a null <c>MaxLength</c> takes: sentence-transformers' <c>max_seq_length</c> from the configuration exported
/// beside the model, else 256, never past the model's own limit (#1257, the maintainer's rule, Cyril 2026-11-10).
/// Malformed files are refused as sentence-transformers 6.2.0 refuses them at load, measured. One test moves the current
/// directory, so the class runs alone.
/// </summary>
[Collection(nameof(SharedCurrentDirectory))]
public sealed class SentenceTransformersConfigTests
{
    /// <summary>Six positions (<c>tools/build_tiny_models.py</c>).</summary>
    private const string Positional = "tiny_positional.onnx";

    /// <summary>No position table: no limit of its own.</summary>
    private const string Unbounded = "tiny_embedder.onnx";

    private const int Positions = 6;

    /// <summary>602 tokens with [CLS] and [SEP], past every length configured here.</summary>
    private static readonly string[] FarPast = [string.Join(" ", Enumerable.Repeat("the dog", 300))];

    [Fact]
    public void A_configured_length_below_the_table_is_taken_by_both_overloads()
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4, "do_lower_case": false}"""));

        AssertCutsAt(model.Path, 4);
    }

    [Fact]
    public void A_configured_length_past_the_table_is_capped_at_the_table()
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 40}"""));

        AssertCutsAt(model.Path, Positions);
    }

    [Fact]
    public void A_configured_length_on_a_model_with_no_limit_is_taken_as_it_is()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 300}"""));

        AssertCutsAt(model.Path, 300);
    }

    [Fact]
    public void Without_a_configuration_a_model_with_no_limit_cuts_at_256()
    {
        using var model = new ModelDirectory(Unbounded);

        AssertCutsAt(model.Path, 256);
    }

    [Fact]
    public void Without_a_configuration_a_table_shorter_than_256_is_the_cut()
    {
        using var model = new ModelDirectory(Positional);

        AssertCutsAt(model.Path, Positions);
    }

    /// <summary>sentence-transformers' ONNX backend keeps the graph under <c>onnx/</c> and the configuration one level up.</summary>
    [Fact]
    public void A_model_under_onnx_reads_the_configuration_of_its_parent()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 300}"""), "onnx");

        AssertCutsAt(model.Path, 300);
    }

    [Fact]
    public void A_model_under_another_folder_does_not_read_its_parent()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 300}"""), "models");

        AssertCutsAt(model.Path, 256);
    }

    /// <summary>The legacy names <c>Transformer.load_config</c> still tries, the first Python finds true winning.</summary>
    [Fact]
    public void A_legacy_file_name_is_read_after_sentence_bert_config()
    {
        using var legacy = new ModelDirectory(Unbounded, ("sentence_roberta_config.json", """{"max_seq_length": 300}"""));
        using var both = new ModelDirectory(
            Unbounded,
            ("sentence_xlnet_config.json", """{"max_seq_length": 100}"""),
            ("sentence_bert_config.json", """{"max_seq_length": 300}"""));

        AssertCutsAt(legacy.Path, 300);
        AssertCutsAt(both.Path, 300);
    }

    /// <summary>An absent or null field, or an empty object, states nothing: sentence-transformers falls back, measured.</summary>
    [Theory]
    [InlineData("""{"do_lower_case": false}""")]
    [InlineData("""{"max_seq_length": null}""")]
    [InlineData("{}")]
    public void A_configuration_stating_no_length_falls_back_to_256(string json)
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", json));

        AssertCutsAt(model.Path, 256);
    }

    /// <summary>Python's <c>json</c> keeps a duplicated key's last value.</summary>
    [Fact]
    public void A_duplicated_field_takes_its_last_value()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 100, "max_seq_length": 300}"""));

        AssertCutsAt(model.Path, 300);
    }

    [Fact]
    public void A_length_past_int_is_capped_at_the_table()
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 99999999999}"""));

        AssertCutsAt(model.Path, Positions);
    }

    /// <summary>
    /// What sentence-transformers refuses at load is refused by both text overloads; a zero, which it loads and then cuts
    /// nothing with, and <c>true</c>, which Python reads as 1, are refused with them. The model still opens, and the entry
    /// points taking encoded rows, which never read the length, run as on <c>main</c>.
    /// </summary>
    [Theory]
    [InlineData("""{"max_seq_length": "5"}""")]
    [InlineData("""{"max_seq_length": 5.0}""")]
    [InlineData("""{"max_seq_length": 5.5}""")]
    [InlineData("""{"max_seq_length": 1e400}""")]
    [InlineData("""{"max_seq_length": 0}""")]
    [InlineData("""{"max_seq_length": -1}""")]
    [InlineData("""{"max_seq_length": -99999999999999999999}""")]
    [InlineData("""{"max_seq_length": true}""")]
    [InlineData("""{"max_seq_length": NaN}""")]
    [InlineData("""{"max_seq_length": -Infinity}""")]
    [InlineData("""{"max_seq_length": "\ud800"}""")]
    [InlineData("""{NaN: 1, "max_seq_length": 4}""")]
    [InlineData("""{"low": 1, Infinity: 2, "max_seq_length": 4}""")]
    [InlineData("""{"max_seq_length": -NaN}""")]
    [InlineData("[1, 2]")]
    [InlineData("{not json")]
    [InlineData("")]
    public void A_malformed_configuration_is_refused_by_the_text_overloads(string json) =>
        AssertRefused(Encoding.UTF8.GetBytes(json));

    /// <summary>Python's json refuses a byte-order mark, and its utf-8 codec bytes that are not UTF-8.</summary>
    [Fact]
    public void A_byte_order_mark_or_invalid_utf8_is_refused()
    {
        AssertRefused([0xEF, 0xBB, 0xBF, .. Encoding.UTF8.GetBytes("""{"max_seq_length": 7}""")]);
        AssertRefused([.. Encoding.UTF8.GetBytes("{\"name\": \""), 0xFF, .. Encoding.UTF8.GetBytes("\", \"max_seq_length\": 7}")]);
    }

    /// <summary>
    /// What Python's json reads is read: named literals elsewhere, deep nesting, an integer past long; and what
    /// sentence-transformers 6.2.0 loads and encodes under is read past, measured (#1780): a false or pair-list modality
    /// bucket, a stride within the room or beside a truncation that cuts nothing, a length stated past the table leaving
    /// the room tokenizers sees, a new kwargs name the rename discards, a dropped modality key, and values that fail
    /// only on the model or at encode.
    /// </summary>
    [Theory]
    [InlineData("""{"scale": NaN, "low": -Infinity, "high": Infinity, "max_seq_length": 4}""", 4)]
    [InlineData("""{"note": "NaN \" Infinity", "max_seq_length": 4}""", 4)]
    [InlineData("""{"bounds": [NaN, -Infinity,Infinity], "max_seq_length": 4}""", 4)]
    [InlineData("""{"note": "\ud800", "max_seq_length": 4}""", 4)]
    [InlineData("""{"\ud800": 1, "max_seq_length": 4}""", 4)]
    [InlineData("""{"max_seq_length": 99999999999999999999}""", Positions)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"image": {}, "audio": [], "video": false}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"image": 0, "video": null}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"image": [[1, 2]], "audio": [["x", 1]], "video": {"a": 1}}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"common": ["xy"], "text": {"bogus": 1}}}""", 4)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 0}}}""", 5)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 2}}}""", 5)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": true}}}""", 5)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": -0}, "common": {"stride": false}}}""", 5)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 4}, "common": {"stride": 0}}}""", 5)]
    [InlineData("""{"max_seq_length": 40, "processing_kwargs": {"text": {"stride": 30}}}""", Positions)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": "x", "truncation": false}}}""", Positions)]
    [InlineData("""{"max_seq_length": 4, "tokenizer_args": {}, "processor_kwargs": "abc"}""", 4)]
    [InlineData("""{"max_seq_length": 4, "model_args": {}, "model_kwargs": [[1]]}""", 4)]
    [InlineData("""{"max_seq_length": 4, "model_kwargs": {"trust_remote_code": true}, "config_args": {}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "modality_config": {"text": {"method": "forward", "method_output_name": "x"}, "foo": 5, "image+foo": 5}, "module_output_name": "t"}""", 4)]
    [InlineData("""{"max_seq_length": 4, "modality_config": {}, "module_output_name": 5}""", 4)]
    [InlineData("""{"max_seq_length": 4, "transformer_task": "fill-mask"}""", 4)]
    [InlineData("""{"max_seq_length": 4, "transformer_task": "any-to-any"}""", 4)]
    public void What_python_reads_is_read(string json, int expected)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", json));

        AssertCutsAt(model.Path, expected);
    }

    /// <summary>Python 3.12.14's json reads 9997 nested containers and raises <c>RecursionError</c> at 9998, measured.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Nesting_is_read_as_deep_as_python_reads_it(bool arrays)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", Nested(9997, arrays)));

        AssertCutsAt(model.Path, 4);
        AssertRefused(Encoding.UTF8.GetBytes(Nested(9998, arrays)));
    }

    /// <summary>A case-sensitive file system opens no <c>onnx/</c> for a folder named <c>ONNX</c>, so its parent is not read.</summary>
    [Fact]
    public void A_folder_named_ONNX_reads_its_parent_only_where_the_file_system_opens_it_as_onnx()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 300}"""), "ONNX");
        bool opensAsOnnx = Directory.Exists(System.IO.Path.Combine(model.Root, "onnx"));

        AssertCutsAt(model.Path, opensAsOnnx ? 300 : 256);
    }

    /// <summary>
    /// A true <c>do_lower_case</c>, by Python's truth, lowercases each text as sentence-transformers' prepended
    /// <c>Lowercase</c> normalizer does, through both text overloads; a false one leaves it as it is.
    /// </summary>
    [Theory]
    [InlineData("true", true)]
    [InlineData("1", true)]
    [InlineData("NaN", true)]
    [InlineData("\"no\"", true)]
    [InlineData("[0]", true)]
    [InlineData("false", false)]
    [InlineData("0", false)]
    [InlineData("-0.0", false)]
    [InlineData("\"\"", false)]
    [InlineData("[]", false)]
    [InlineData("{}", false)]
    [InlineData("null", false)]
    [InlineData("1e400", true)]
    [InlineData("1e-400", false)]
    public void Do_lower_case_lowercases_the_texts_by_pythons_truth(string flag, bool lowercased)
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", $$"""{"do_lower_case": {{flag}}}"""));
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());
        using var plain = new OnnxTextEmbedder(System.IO.Path.Combine(AppContext.BaseDirectory, "oracles", Positional), BatchCorpus.Tokenizer());
        string[] upper = ["The DOG"];
        float[] expected = plain.EmbedBatch(lowercased ? ["the dog"] : upper, cancellationToken: token)[0];

        Assert.Equal(expected, embedder.EmbedBatch(upper, cancellationToken: token)[0]);
        Assert.Equal(expected, embedder.EmbedBatch(upper, new BatchEncoder(BatchCorpus.Tokenizer()), token)[0]);
        Assert.NotEqual(plain.EmbedBatch(["the dog"], cancellationToken: token)[0], plain.EmbedBatch(upper, cancellationToken: token)[0]);
    }

    private static string Nested(int depth, bool arrays)
    {
        int inner = depth - 1;
        string deep = arrays
            ? string.Concat(Enumerable.Repeat("[", inner)) + string.Concat(Enumerable.Repeat("]", inner))
            : string.Concat(Enumerable.Repeat("""{"a":""", inner)) + "1" + string.Concat(Enumerable.Repeat("}", inner));
        return $$"""{"max_seq_length": 4, "deep": {{deep}}}""";
    }

    /// <summary>Python 3.12's json refuses the whole file for one integer past 4300 digits, sign aside; a float has no limit.</summary>
    [Fact]
    public void An_integer_past_python_s_digits_refuses_the_file()
    {
        string longest = new('7', 4300);
        string past = new('7', 4301);

        AssertRefused(Encoding.UTF8.GetBytes($$"""{"max_seq_length": {{past}}}"""));
        AssertRefused(Encoding.UTF8.GetBytes($$"""{"max_seq_length": 4, "other": [1, {"x": -{{past}}}]}"""));
        using var read = new ModelDirectory(Positional, ("sentence_bert_config.json", $$"""{"max_seq_length": {{longest}}, "x": -{{longest}}}"""));
        AssertCutsAt(read.Path, Positions);
        using var floats = new ModelDirectory(
            Positional, ("sentence_bert_config.json", $$"""{"max_seq_length": 4, "x": [{{past}}.0, {{past}}e0, 0.{{past}}]}"""));
        AssertCutsAt(floats.Path, 4);
    }

    /// <summary>
    /// Python's <c>Path.exists()</c> decides which name is read: a link to nothing is absent, so the next name is
    /// read; a directory is present, and sentence-transformers then fails to open it, so it is refused.
    /// </summary>
    [Fact]
    public void A_dangling_link_is_skipped_and_a_directory_is_refused()
    {
        using var linked = new ModelDirectory(Unbounded, ("sentence_roberta_config.json", """{"max_seq_length": 7}"""));
        try
        {
            File.CreateSymbolicLink(System.IO.Path.Combine(linked.Root, "sentence_bert_config.json"), "gone.json");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Assert.Skip("This host cannot create a symbolic link.");
        }
        AssertCutsAt(linked.Path, 7);

        CancellationToken token = TestContext.Current.CancellationToken;
        using var folder = new ModelDirectory(Positional, ("sentence_roberta_config.json", """{"max_seq_length": 4}"""));
        Directory.CreateDirectory(System.IO.Path.Combine(folder.Root, "sentence_bert_config.json"));
        using var embedder = new OnnxTextEmbedder(folder.Path, BatchCorpus.Tokenizer());
        InvalidOperationException refusal = Assert.Throws<InvalidOperationException>(() => embedder.EmbedBatch(FarPast, cancellationToken: token));
        Assert.Contains("is a directory", refusal.Message, StringComparison.Ordinal);
    }

    /// <summary>sentence-transformers reads the module's folder, the parent of <c>onnx/</c>, and never <c>onnx/</c> itself.</summary>
    [Fact]
    public void A_model_under_onnx_never_reads_its_own_folder()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 7}"""), "onnx");
        model.WriteBeside("sentence_xlnet_config.json", """{"max_seq_length": 100}""");
        using var inside = new ModelDirectory(Unbounded, [], "onnx");
        inside.WriteBeside("sentence_bert_config.json", """{"max_seq_length": 64}""");

        AssertCutsAt(model.Path, 7);
        AssertCutsAt(inside.Path, 256);
    }

    /// <summary>
    /// <c>load_config</c> breaks on the first file Python finds true, so a false one passes to the next name; the last
    /// name's value is what it loads, and one that is not an object fails there (Review A pass 4 on #1257).
    /// </summary>
    [Theory]
    [InlineData("{}")]
    [InlineData("0")]
    [InlineData("[]")]
    [InlineData("null")]
    public void A_configuration_python_finds_false_passes_to_the_next_name(string json)
    {
        using var model = new ModelDirectory(
            Unbounded,
            ("sentence_bert_config.json", json),
            ("sentence_roberta_config.json", """{"max_seq_length": 128}"""));
        using var last = new ModelDirectory(Unbounded, ("sentence_xlnet_config.json", json));

        AssertCutsAt(model.Path, 128);
        if (json == "{}")
        {
            AssertCutsAt(last.Path, 256);
        }
        else
        {
            AssertRefusedAt(last.Path, "sentence_xlnet_config.json");
        }
    }

    /// <summary>
    /// A <c>model_max_length</c> in the tokenizer kwargs is the tokenizer's cap, which sentence-transformers takes over
    /// <c>max_seq_length</c>; the legacy <c>tokenizer_args</c> wins over <c>processor_kwargs</c>, as its rename does.
    /// <c>processing_kwargs</c>' <c>text</c> and <c>common</c> buckets, merged key by key with <c>common</c>'s word
    /// winning, each read as <c>dict.update</c> reads an object or a list of pairs, set the call's <c>max_length</c>
    /// over both (Review A passes 5 and 7).
    /// </summary>
    [Theory]
    [InlineData("""{"max_seq_length": 256, "tokenizer_args": {"model_max_length": 128}}""", 128)]
    [InlineData("""{"max_seq_length": 256, "processor_kwargs": {"model_max_length": 100}}""", 100)]
    [InlineData("""{"processor_kwargs": {"model_max_length": 100}, "tokenizer_args": {"model_max_length": 90}}""", 90)]
    [InlineData("""{"max_seq_length": 64, "tokenizer_args": {"do_lower_case": true}}""", 64)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"text": {"max_length": 8}}}""", 8)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"common": {"max_length": 9}}}""", 9)]
    [InlineData("""{"processing_kwargs": {"common": {"max_length": 9}, "text": {"max_length": 7}}}""", 9)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": null}""", 128)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": []}""", 128)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"text": null, "common": []}}""", 128)]
    [InlineData("""{"max_seq_length": 6, "processing_kwargs": {"text": {"max_length": null}}}""", 6)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"common": {"max_length": null}, "text": {"max_length": 5}}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"common": {"truncation": true}, "text": {"truncation": false}}}""", 4)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": [["max_length", 3]]}}""", 3)]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": [["max_length", 2], ["max_length", 3]]}}""", 3)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"common": "", "text": ["ab", {"a": 1, "b": 2}]}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"text": [{"\n": 1, "n": 2}]}}""", 4)]
    [InlineData("""{"max_seq_length": 4, "processing_kwargs": {"text": {"truncation": "only_first", "max_length": 3}}}""", 3)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"text": {"truncation": null, "padding": false, "max_length": 4}}}""", 4)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"common": {"padding": false, "max_length": 4}, "text": {"truncation": null}}}""", 4)]
    [InlineData("""{"max_seq_length": 128, "processing_kwargs": {"text": [["truncation", null], ["padding", false], ["max_length", 3]]}}""", 3)]
    public void A_tokenizer_model_max_length_replaces_max_seq_length(string json, int expected)
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_bert_config.json", json));

        AssertCutsAt(model.Path, expected);
    }

    [Fact]
    public void A_null_model_max_length_cuts_at_the_model_limit_and_a_bad_one_is_refused()
    {
        using var unlimited = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"tokenizer_args": {"model_max_length": null}}"""));

        AssertCutsAt(unlimited.Path, Positions);
        AssertRefused(Encoding.UTF8.GetBytes("""{"tokenizer_args": {"model_max_length": "5"}}"""));
        AssertRefused(Encoding.UTF8.GetBytes("""{"tokenizer_args": [1]}"""));
    }

    /// <summary>
    /// sentence-transformers reads a module configuration only for a model whose <c>modules.json</c> lists it: without
    /// one it builds a fresh module, and the old layout lists it in a subfolder of the root.
    /// </summary>
    [Fact]
    public void The_configuration_is_read_only_for_a_module_modules_json_lists()
    {
        using var bare = new ModelDirectory(Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 300}"""));
        bare.Delete("modules.json");
        using var layout = new ModelDirectory(Unbounded, [], System.IO.Path.Combine("0_Transformer", "onnx"));
        layout.WriteRoot("modules.json", Encoding.UTF8.GetBytes("""[{"path": "0_Transformer", "type": "sentence_transformers.models.Transformer"}]"""));
        File.WriteAllText(System.IO.Path.Combine(layout.Root, "0_Transformer", "sentence_bert_config.json"), """{"max_seq_length": 300}""");
        using var broken = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        broken.WriteRoot("modules.json", Encoding.UTF8.GetBytes("""[{"type": "sentence_transformers.models.Transformer"}]"""));

        AssertCutsAt(bare.Path, 256);
        AssertCutsAt(layout.Path, 300);
        AssertRefusedAt(broken.Path, "modules.json");
    }

    /// <summary>
    /// A parent's <c>modules.json</c> sentence-transformers cannot load refuses nothing: it opens that file only to load
    /// the parent, so the model in its subfolder is a fresh module (Review A pass 9).
    /// </summary>
    [Theory]
    [InlineData("{}")]
    [InlineData("""[{"path": "/abs", "type": "x"}]""")]
    [InlineData("""[{"path": "minilm", "type": "sentence_transformers.models.Transformer"}, 5]""")]
    public void A_parent_modules_json_that_cannot_load_lists_nothing(string modules)
    {
        using var model = new ModelDirectory(Positional, [("sentence_bert_config.json", """{"max_seq_length": 4}""")], "minilm");
        model.WriteRoot("modules.json", Encoding.UTF8.GetBytes(modules));

        AssertCutsAt(model.Path, Positions);
    }

    [Fact]
    public void A_processing_truncation_that_cuts_nothing_leaves_the_model_limit()
    {
        using var off = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": false}}}"""));
        using var named = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"max_seq_length": 2, "processing_kwargs": {"common": {"truncation": "do_not_truncate"}}}"""));

        // A common max_length leaves the text bucket's truncation in force: the buckets merge key by key (pass 7).
        using var merged = new ModelDirectory(
            Positional,
            ("sentence_bert_config.json", """{"max_seq_length": 4, "processing_kwargs": {"common": {"max_length": 5}, "text": {"truncation": false}}}"""));

        // An object in a list is the pair of its two keys, the second standing as the value (pass 8).
        using var keys = new ModelDirectory(
            Positional,
            ("sentence_bert_config.json", """{"max_seq_length": 4, "processing_kwargs": {"common": [{"truncation": 0, "do_not_truncate": 0}]}}"""));

        AssertCutsAt(off.Path, Positions);
        AssertCutsAt(named.Path, Positions);
        AssertCutsAt(merged.Path, Positions);
        AssertCutsAt(keys.Path, Positions);
        AssertRefused(Encoding.UTF8.GetBytes("""{"processing_kwargs": [1]}"""));
        AssertRefused(Encoding.UTF8.GetBytes("""{"processing_kwargs": {"text": {"max_length": "8"}}}"""));
    }

    /// <summary>A null truncation cuts at <c>max_length</c> only beside a padding of <c>False</c> itself (pass 10).</summary>
    [Theory]
    [InlineData("""{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": null, "max_length": 3}}}""")]
    [InlineData("""{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": null, "padding": 0, "max_length": 3}}}""")]
    [InlineData("""{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": null, "padding": false, "max_length": null}}}""")]
    public void A_null_truncation_without_a_false_padding_cuts_nothing(string json)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", json));

        AssertCutsAt(model.Path, Positions);
    }

    /// <summary>
    /// <c>only_second</c> cuts no single text: tokenizers encodes one within the length whole and refuses a longer one,
    /// "Second sequence not provided", measured on tokenizers 0.23.2; a <c>MaxLength</c> the caller sets still cuts.
    /// </summary>
    [Fact]
    public void Only_second_refuses_a_text_past_the_length_and_cuts_none()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"processing_kwargs": {"text": {"truncation": "only_second", "max_length": 4}}}"""));
        using var plain = new ModelDirectory(Positional);
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());
        using var reference = new OnnxTextEmbedder(plain.Path, BatchCorpus.Tokenizer());
        string[] within = ["the dog"];
        string[] past = ["the dog", "the dog the"];

        Assert.Equal(reference.EmbedBatch(within, cancellationToken: token)[0], embedder.EmbedBatch(within, cancellationToken: token)[0]);
        Assert.Equal("texts", Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(past, cancellationToken: token)).ParamName);
        Assert.Equal(
            "texts",
            Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(past, new BatchEncoder(BatchCorpus.Tokenizer()), token)).ParamName);
        Assert.Equal(
            reference.EmbedBatch(past, new EncodingOptions { MaxLength = 3 }, token)[1],
            embedder.EmbedBatch(past, new EncodingOptions { MaxLength = 3 }, token)[1]);
    }

    /// <summary>
    /// A bucket <c>dict.update</c> refuses, a <c>null</c>, <c>0</c> or <c>false</c> <c>common</c> among them, fails
    /// every encode in sentence-transformers; so does a strategy transformers does not know: each is refused (pass 7).
    /// </summary>
    [Theory]
    [InlineData("""{"processing_kwargs": {"common": null}}""")]
    [InlineData("""{"processing_kwargs": {"common": 0, "text": {"max_length": 5}}}""")]
    [InlineData("""{"processing_kwargs": {"common": false}}""")]
    [InlineData("""{"processing_kwargs": {"text": "ab"}}""")]
    [InlineData("""{"processing_kwargs": {"text": [5]}}""")]
    [InlineData("""{"processing_kwargs": {"text": [[["max_length"], 3]]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [{"max_length": 0, "x": 0}]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [{"a": 1}]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [{"a": 1, "b": 2, "c": 3}]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [{"\u0061": 1, "a": 2}]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [{"\ud800": 1, "\ud800": 2}]}}""")]
    [InlineData("""{"processing_kwargs": {"text": {"truncation": "longest"}}}""")]
    [InlineData("""{"processing_kwargs": {"common": {"truncation": 1}}}""")]
    public void A_bucket_or_a_strategy_sentence_transformers_cannot_use_is_refused(string json)
    {
        AssertRefused(Encoding.UTF8.GetBytes(json));
    }

    /// <summary>A relative model path names the folder it named when the model was opened, not one at the first text (pass 7).</summary>
    [Fact]
    public void A_relative_model_path_is_read_where_the_model_was_opened()
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 3}"""));
        using var other = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 5}"""));
        string before = Environment.CurrentDirectory;
        CancellationToken token = TestContext.Current.CancellationToken;
        try
        {
            Environment.CurrentDirectory = model.Root;
            using var embedder = new OnnxTextEmbedder("model.onnx", BatchCorpus.Tokenizer());
            Environment.CurrentDirectory = other.Root;
            float[] cut = embedder.EmbedBatch(FarPast, new EncodingOptions { MaxLength = 3 }, token)[0];

            Assert.Equal(cut, embedder.EmbedBatch(FarPast, cancellationToken: token)[0]);
        }
        finally
        {
            Environment.CurrentDirectory = before;
        }
    }

    /// <summary>
    /// A key or a module name holding a lone surrogate, which Python's json reads, matches no field and never stops the
    /// embedder opening; a module path or type that cannot be text is refused when the texts are encoded (pass 5).
    /// </summary>
    [Fact]
    public void A_lone_surrogate_in_a_key_is_no_field_and_the_model_still_opens()
    {
        using var keys = new ModelDirectory(
            Positional,
            ("sentence_bert_config.json", """{"do_lower_cas\ud800": 1, "tokenizer_args": {"model_max_lengt\ud800": 2}, "max_seq_length": 4}"""));
        using var types = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        types.WriteRoot("modules.json", Encoding.UTF8.GetBytes("""[{"path": "", "pat\ud800": 1, "type": "x.Transforme\ud800"}]"""));

        AssertCutsAt(keys.Path, 4);
        using var bare = new OnnxTextEmbedder(types.Path);
        Assert.NotEmpty(bare.Embed([5, 9], [1, 1]));
        AssertRefusedAt(types.Path, "modules.json");
    }

    /// <summary>
    /// The truncation is read decoded, as Python reads it; the common bucket's word wins over the text one's, as the
    /// processor call merges them (Review A pass 6 on #1257).
    /// </summary>
    [Fact]
    public void A_common_truncation_that_cuts_nothing_wins_and_is_read_decoded()
    {
        using var escaped = new ModelDirectory(
            Positional,
            ("sentence_bert_config.json", """{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": "do_not_trunc\u0061te"}}}"""));
        using var common = new ModelDirectory(
            Positional,
            ("sentence_bert_config.json", """{"processing_kwargs": {"text": {"max_length": 3}, "common": {"truncation": false}}}"""));

        AssertCutsAt(escaped.Path, Positions);
        AssertCutsAt(common.Path, Positions);
    }

    /// <summary>
    /// Every entry of <c>modules.json</c> is validated before any is matched, as sentence-transformers validates them all
    /// first; a Windows anchor and a NUL are refused on every system, and the model still opens (pass 6).
    /// </summary>
    [Theory]
    [InlineData("""[{"path": "", "type": "sentence_transformers.models.Transformer"}, {"path": "../evil", "type": "sentence_transformers.models.Pooling"}]""")]
    [InlineData("""[{"path": "C:x", "type": "sentence_transformers.models.Transformer"}]""")]
    [InlineData("""[{"path": "\\\\host\\share", "type": "sentence_transformers.models.Transformer"}]""")]
    [InlineData("""[{"path": "a\u0000b", "type": "sentence_transformers.models.Transformer"}]""")]
    public void Every_listed_module_path_is_validated_before_one_is_matched(string modules)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        model.WriteRoot("modules.json", Encoding.UTF8.GetBytes(modules));
        using var bare = new OnnxTextEmbedder(model.Path);

        Assert.NotEmpty(bare.Embed([5, 9], [1, 1]));
        AssertRefusedAt(model.Path, "outside the model's folder");
    }

    /// <summary>A link that loops is absent, as Python's <c>Path.exists()</c> answers on ELOOP, so the next name is read (pass 6).</summary>
    [Fact]
    public void A_looping_link_is_absent()
    {
        using var model = new ModelDirectory(Unbounded, ("sentence_roberta_config.json", """{"max_seq_length": 77}"""));
        string link = System.IO.Path.Combine(model.Root, "sentence_bert_config.json");
        try
        {
            File.CreateSymbolicLink(link, link);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Assert.Skip("This host cannot create a symbolic link.");
        }

        AssertCutsAt(model.Path, 77);
    }

    /// <summary>A module path <c>.</c> or <c>./</c> names the root, as <c>Path(root, ".")</c> does; one leaving it is refused.</summary>
    [Theory]
    [InlineData(".")]
    [InlineData("./")]
    public void A_dot_module_path_is_the_root(string folder)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        model.WriteRoot("modules.json", Encoding.UTF8.GetBytes($$"""[{"path": "{{folder}}", "type": "sentence_transformers.models.Transformer"}]"""));

        AssertCutsAt(model.Path, 4);
    }

    [Theory]
    [InlineData("../elsewhere")]
    [InlineData("/elsewhere")]
    public void A_module_path_leaving_the_model_folder_is_refused(string folder)
    {
        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        model.WriteRoot("modules.json", Encoding.UTF8.GetBytes($$"""[{"path": "{{folder}}", "type": "sentence_transformers.models.Transformer"}]"""));

        AssertRefusedAt(model.Path, "outside the model's folder");
    }

    /// <summary>A file the process may not read is refused for that, not for its encoding (Review A pass 4 on #1257).</summary>
    [Fact]
    public void An_unreadable_configuration_is_refused_as_unreadable()
    {
        if (OperatingSystem.IsWindows())
        {
            Assert.Skip("Unix file modes only.");
            return;
        }

        using var model = new ModelDirectory(Positional, ("sentence_bert_config.json", """{"max_seq_length": 4}"""));
        string path = System.IO.Path.Combine(model.Root, "sentence_bert_config.json");
        File.SetUnixFileMode(path, UnixFileMode.None);
        bool readable = true;
        try
        {
            File.ReadAllBytes(path);
        }
        catch (UnauthorizedAccessException)
        {
            readable = false;
        }

        string? message = readable ? null : RefusalAt(model.Path);
        File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite);
        Assert.SkipWhen(readable, "This process reads a file of mode 000, as root does.");
        Assert.Contains("cannot be read:", message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Under <c>only_second</c>, a length below the template leaves every text whole and the template's own length cuts
    /// every text to it, as sentence-transformers 6.2.0 does, measured: tokenizers' <c>L - 2</c> wraps or is 0 (#1780).
    /// </summary>
    [Fact]
    public void Only_second_at_or_below_the_template_takes_the_ordinary_path()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var below = new ModelDirectory(
            Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 1, "processing_kwargs": {"text": {"truncation": "only_second"}}}"""));
        using var at = new ModelDirectory(
            Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 2, "processing_kwargs": {"text": {"truncation": "only_second"}}}"""));
        using var whole = new OnnxTextEmbedder(below.Path, BatchCorpus.Tokenizer());
        using var template = new OnnxTextEmbedder(at.Path, BatchCorpus.Tokenizer());
        string[] texts = ["", "the", FarPast[0]];

        float[][] uncut = whole.EmbedBatch(texts, new EncodingOptions { MaxLength = 602 }, token);
        float[][] cut = template.EmbedBatch(texts, new EncodingOptions { MaxLength = 2 }, token);
        Assert.Equal(uncut, whole.EmbedBatch(texts, cancellationToken: token));
        Assert.Equal(uncut, whole.EmbedBatch(texts, new BatchEncoder(BatchCorpus.Tokenizer()), token));
        Assert.Equal(cut, template.EmbedBatch(texts, cancellationToken: token));
        Assert.Equal(cut, template.EmbedBatch(texts, new BatchEncoder(BatchCorpus.Tokenizer()), token));
    }

    /// <summary>Below the template a row past the model's table stays refused, under the overload's own argument (rule 7).</summary>
    [Fact]
    public void Only_second_below_the_template_refuses_a_row_past_the_table_under_the_overload_argument()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"max_seq_length": 1, "processing_kwargs": {"text": {"truncation": "only_second"}}}"""));
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());

        Assert.Equal("options", Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(FarPast, cancellationToken: token)).ParamName);
        Assert.Equal(
            "encoder",
            Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(FarPast, new BatchEncoder(BatchCorpus.Tokenizer()), token)).ParamName);
    }

    /// <summary>
    /// <c>only_second</c> stops at the first text past the length, worded as the encoder words a refusal, before the
    /// texts after it are read (#1780).
    /// </summary>
    [Fact]
    public void Only_second_refuses_at_the_first_text_past_the_length_without_reading_the_rest()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"processing_kwargs": {"text": {"truncation": "only_second", "max_length": 4}}}"""));
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());

        ArgumentException byOptions = Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(StopsAfterTheSecond(), cancellationToken: token));
        ArgumentException byEncoder = Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(StopsAfterTheSecond(), new BatchEncoder(BatchCorpus.Tokenizer()), token));

        Assert.Equal("texts", byOptions.ParamName);
        Assert.Equal("texts", byEncoder.ParamName);
        Assert.StartsWith("The text at position 1 is refused: it encodes to 5 tokens", byOptions.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// An <c>image</c>, <c>audio</c> or <c>video</c> bucket Python finds true reaches <c>dict.update</c>, and a key that
    /// is not a string in <c>text</c> or <c>common</c> reaches the tokenizer's call: each fails every encode in
    /// sentence-transformers 6.2.0, measured, and is refused (#1780).
    /// </summary>
    [Theory]
    [InlineData("""{"processing_kwargs": {"image": 1}}""")]
    [InlineData("""{"processing_kwargs": {"audio": "abc"}}""")]
    [InlineData("""{"processing_kwargs": {"video": true}}""")]
    [InlineData("""{"processing_kwargs": {"image": "ab"}}""")]
    [InlineData("""{"processing_kwargs": {"image": [[1, 2, 3]]}}""")]
    [InlineData("""{"processing_kwargs": {"audio": [[[1], 2]]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [[1, 2]]}}""")]
    [InlineData("""{"processing_kwargs": {"common": [[false, 2]]}}""")]
    [InlineData("""{"processing_kwargs": {"text": [[null, 2]]}}""")]
    [InlineData("""{"processing_kwargs": {"text": [[1.5, 2]]}}""")]
    [InlineData("""{"processing_kwargs": {"text": [[NaN, 2]]}}""")]
    public void A_modality_bucket_or_a_key_sentence_transformers_cannot_use_is_refused(string json) =>
        AssertRefused(Encoding.UTF8.GetBytes(json));

    /// <summary>
    /// A stride tokenizers 0.22.2 cannot take, or past the <c>L - 2</c> tokens the length leaves beside the template,
    /// fails every call in sentence-transformers, measured, and is refused (#1780).
    /// </summary>
    [Theory]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 4}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"common": {"stride": 100}}}""")]
    [InlineData("""{"max_seq_length": 2, "processing_kwargs": {"text": {"stride": 1}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": [["stride", 4]]}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 4, "truncation": "only_second"}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": null}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": "1"}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 1.0}}}""")]
    [InlineData("""{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": []}}}""")]
    [InlineData("""{"max_seq_length": 1, "processing_kwargs": {"text": {"stride": -1}}}""")]
    [InlineData("""{"max_seq_length": 1, "processing_kwargs": {"text": {"stride": 18446744073709551616}}}""")]
    public void A_stride_tokenizers_refuses_is_refused(string json) => AssertRefused(Encoding.UTF8.GetBytes(json));

    /// <summary>
    /// Below the template any stride is read past, tokenizers' room wrapping, and the rows stay whole (#1780).
    /// </summary>
    [Fact]
    public void A_stride_below_the_template_is_read_past()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(
            Unbounded, ("sentence_bert_config.json", """{"max_seq_length": 1, "processing_kwargs": {"text": {"stride": 18446744073709551615}}}"""));
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());

        Assert.Equal(
            embedder.EmbedBatch(FarPast, new EncodingOptions { MaxLength = 602 }, token),
            embedder.EmbedBatch(FarPast, cancellationToken: token));
    }

    /// <summary>
    /// A stride equal to the room, from 1, leaves a text within the length whole and refuses one past it, as tokenizers
    /// panics on cutting it, measured; a <c>MaxLength</c> the caller sets still cuts (#1780).
    /// </summary>
    [Fact]
    public void A_stride_equal_to_the_room_refuses_a_text_past_the_length()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(
            Positional, ("sentence_bert_config.json", """{"max_seq_length": 5, "processing_kwargs": {"text": {"stride": 3}}}"""));
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());
        string[] within = ["the dog", "the dog the"];
        string[] past = ["the dog the dog"];

        Assert.Equal(
            embedder.EmbedBatch(within, new EncodingOptions { MaxLength = 5 }, token),
            embedder.EmbedBatch(within, cancellationToken: token));
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(past, cancellationToken: token));
        Assert.Equal("texts", refusal.ParamName);
        Assert.Contains("stride 3", refusal.Message, StringComparison.Ordinal);
        Assert.Equal(
            "texts",
            Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(past, new BatchEncoder(BatchCorpus.Tokenizer()), token)).ParamName);
        Assert.Single(embedder.EmbedBatch(past, new EncodingOptions { MaxLength = 4 }, token));
    }

    /// <summary>
    /// The constructor fields sentence-transformers 6.2.0 refuses at load, measured: <c>load_config</c>'s
    /// <c>trust_remote_code</c> look-up, <c>_load_init_kwargs</c>' <c>update</c> of the field the rename keeps, and
    /// <c>__init__</c>'s modality entries, <c>module_output_name</c> and task (#1780).
    /// </summary>
    [Theory]
    [InlineData("""{"model_kwargs": null}""")]
    [InlineData("""{"config_kwargs": 5}""")]
    [InlineData("""{"config_args": 5}""")]
    [InlineData("""{"model_args": [1]}""")]
    [InlineData("""{"tokenizer_args": "abc"}""")]
    [InlineData("""{"processor_kwargs": 0.5}""")]
    [InlineData("""{"model_kwargs": NaN, "model_args": {}}""")]
    [InlineData("""{"tokenizer_args": {}, "processor_kwargs": 5}""")]
    [InlineData("""{"model_args": {}, "model_kwargs": "xtrust_remote_codex"}""")]
    [InlineData("""{"config_args": {}, "config_kwargs": ["trust_remote_code"]}""")]
    [InlineData("""{"processor_kwargs": {}, "tokenizer_args": true}""")]
    [InlineData("""{"modality_config": 5, "module_output_name": "token_embeddings"}""")]
    [InlineData("""{"modality_config": null, "module_output_name": "token_embeddings"}""")]
    [InlineData("""{"modality_config": {"text": 5}, "module_output_name": "token_embeddings"}""")]
    [InlineData("""{"modality_config": {"text": {"method": "forward"}}, "module_output_name": "token_embeddings"}""")]
    [InlineData("""{"modality_config": {"text": {"method": "forward", "method_output_name": "x"}, "image": []}, "module_output_name": "t"}""")]
    [InlineData("""{"modality_config": {"message": 5}, "module_output_name": "token_embeddings"}""")]
    [InlineData("""{"modality_config": {"text": {"method": "forward", "method_output_name": "last_hidden_state"}}}""")]
    [InlineData("""{"modality_config": {}, "module_output_name": null}""")]
    [InlineData("""{"transformer_task": "nope"}""")]
    [InlineData("""{"transformer_task": null}""")]
    [InlineData("""{"transformer_task": ["feature-extraction"]}""")]
    [InlineData("""{"transformer_task": ""}""")]
    public void A_constructor_field_sentence_transformers_refuses_at_load_is_refused(string json) =>
        AssertRefused(Encoding.UTF8.GetBytes(json));

    /// <summary>Two texts, the second past a length of four, then a failure no refusal at the second lets the caller see.</summary>
    private static IEnumerable<string> StopsAfterTheSecond()
    {
        yield return "the dog";
        yield return "the dog the";
        throw new InvalidOperationException("The texts after the refused one were read.");
    }

    private static void AssertRefusedAt(string modelPath, string fileName) =>
        Assert.Contains(fileName, RefusalAt(modelPath), StringComparison.Ordinal);

    private static string RefusalAt(string modelPath)
    {
        using var embedder = new OnnxTextEmbedder(modelPath, BatchCorpus.Tokenizer());
        return Assert.Throws<InvalidOperationException>(
            () => embedder.EmbedBatch(FarPast, cancellationToken: TestContext.Current.CancellationToken)).Message;
    }

    private static void AssertRefused(byte[] configuration)
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var model = new ModelDirectory(Positional);
        model.WriteRoot("sentence_bert_config.json", configuration);
        using var bare = new OnnxTextEmbedder(model.Path);
        using var embedder = new OnnxTextEmbedder(model.Path, BatchCorpus.Tokenizer());

        InvalidOperationException refusal = Assert.Throws<InvalidOperationException>(() => embedder.EmbedBatch(FarPast, cancellationToken: token));
        Assert.Contains("sentence_bert_config.json", refusal.Message, StringComparison.Ordinal);
        Assert.Throws<InvalidOperationException>(() => bare.EmbedBatch(FarPast, new BatchEncoder(BatchCorpus.Tokenizer()), token));
        Assert.NotEmpty(bare.Embed([5, 9], [1, 1]));
    }

    /// <summary>Both text overloads cut a text far past every limit at <paramref name="expected"/>, and only there.</summary>
    private static void AssertCutsAt(string modelPath, int expected)
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        using var embedder = new OnnxTextEmbedder(modelPath, BatchCorpus.Tokenizer());
        float[] cut = embedder.EmbedBatch(FarPast, new EncodingOptions { MaxLength = expected }, token)[0];
        float[] shorter = embedder.EmbedBatch(FarPast, new EncodingOptions { MaxLength = expected - 1 }, token)[0];

        Assert.Equal(cut, embedder.EmbedBatch(FarPast, cancellationToken: token)[0]);
        Assert.Equal(cut, embedder.EmbedBatch(FarPast, new BatchEncoder(BatchCorpus.Tokenizer()), token)[0]);
        Assert.NotEqual(shorter, cut);
        if (embedder.MaxSequenceLength is null || expected < embedder.MaxSequenceLength)
        {
            float[] longer = embedder.EmbedBatch(FarPast, new EncodingOptions { MaxLength = expected + 1 }, token)[0];
            Assert.NotEqual(longer, cut);
        }
    }

    /// <summary>A fixture model copied into a directory of its own, with the files given written beside it.</summary>
    private sealed class ModelDirectory : IDisposable
    {
        private const string RootModules =
            """[{"idx": 0, "name": "0", "path": "", "type": "sentence_transformers.models.Transformer"}]""";

        private readonly string _root;

        public ModelDirectory(string fixture, params (string Name, string Json)[] files)
            : this(fixture, files, subfolder: null)
        {
        }

        public ModelDirectory(string fixture, (string Name, string Json) file, string subfolder)
            : this(fixture, [file], subfolder)
        {
        }

        public ModelDirectory(string fixture, (string Name, string Json)[] files, string? subfolder)
        {
            _root = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"lodestar-1257-{Guid.NewGuid():N}");
            string modelDirectory = subfolder is null ? _root : System.IO.Path.Combine(_root, subfolder);
            Directory.CreateDirectory(modelDirectory);
            Path = System.IO.Path.Combine(modelDirectory, "model.onnx");
            File.Copy(System.IO.Path.Combine(AppContext.BaseDirectory, "oracles", fixture), Path);
            // Every sentence-transformers export lists its Transformer module, at the root unless a test says otherwise.
            File.WriteAllText(System.IO.Path.Combine(_root, "modules.json"), RootModules);
            foreach ((string name, string json) in files)
            {
                File.WriteAllText(System.IO.Path.Combine(_root, name), json);
            }
        }

        public string Path { get; }

        public string Root => _root;

        public void WriteRoot(string name, byte[] bytes) => File.WriteAllBytes(System.IO.Path.Combine(_root, name), bytes);

        public void WriteBeside(string name, string json) =>
            File.WriteAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path)!, name), json);

        public void Delete(string name) => File.Delete(System.IO.Path.Combine(_root, name));

        public void Dispose() => Directory.Delete(_root, recursive: true);
    }
}

/// <summary>A test that moves the current directory runs with nothing else running beside it.</summary>
[CollectionDefinition(nameof(SharedCurrentDirectory), DisableParallelization = true)]
public sealed class SharedCurrentDirectory;

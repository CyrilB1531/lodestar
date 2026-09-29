using System.Buffers.Binary;
using System.Reflection;
using System.Text;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Pooling;
using Lodestar.Embeddings.Search;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>The Review B findings of <c>Lodestar.Embeddings</c> after #1325, one fact or theory each.</summary>
public sealed class ReviewBFindingsTests
{
    private static MemoryStream Utf8(string text) => new(Encoding.UTF8.GetBytes(text));

    private static SentencePieceVocabulary Unigram(params SentencePiece[] pieces) =>
        new(pieces, [.. pieces.Select(_ => SentencePieceType.Normal)], UnkId: 0, BosId: -1, EosId: -1, PadId: -1);

    [Fact]
    public void A_hand_built_unigram_piece_with_a_non_finite_score_is_refused()
    {
        SentencePieceVocabulary vocabulary = Unigram(new("<unk>", -10, 0), new("▁a", double.NaN, 1));

        ArgumentException error = Assert.Throws<ArgumentException>(() => new SentencePieceTokenizer(vocabulary));

        Assert.Equal("vocabulary", error.ParamName);
    }

    [Fact]
    public void A_hand_built_unigram_piece_with_no_string_is_refused_under_vocabulary()
    {
        SentencePieceVocabulary vocabulary = Unigram(new("<unk>", -10, 0), new(null!, -1, 1));

        ArgumentException error = Assert.Throws<ArgumentException>(() => new SentencePieceTokenizer(vocabulary));

        Assert.Equal("vocabulary", error.ParamName);
    }

    [Fact]
    public void Ids_spread_far_apart_load_without_an_array_the_size_of_the_largest()
    {
        var vocabulary = new BpeVocabulary(
            new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = 0, ["b"] = 2_000_000_000, ["ab"] = int.MaxValue },
            [new MergePair("a", "b")])
        { PreTokenizerPattern = @"\S+" };
        var tokenizer = new BpeTokenizer(vocabulary);

        TokenizationResult result = tokenizer.Encode("ab b");

        Assert.Equal([int.MaxValue, 2_000_000_000], result.Ids);
        Assert.Equal("abb", tokenizer.Decode(result.Ids));
    }

    [Fact]
    public void A_negative_id_is_refused_by_every_loader_and_constructor()
    {
        Assert.Throws<InvalidDataException>(() => BpeFilesLoader.Load(Utf8("""{"a":-1,"b":1}"""), Utf8("")));
        Assert.Throws<ArgumentException>(() => new BpeTokenizer(
            new BpeVocabulary(new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = -1 }, [])
            { PreTokenizerPattern = @"\S+" }));
        Assert.Equal(
            "vocab",
            Assert.Throws<ArgumentException>(() => new WordPieceTokenizer(
                new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0, ["a"] = -3 })).ParamName);
    }

    [Fact]
    public void Decode_names_its_own_parameter_for_an_unknown_id()
    {
        var tokenizer = new BpeTokenizer(
            new BpeVocabulary(new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = 0 }, [])
            { PreTokenizerPattern = @"\S+" });

        Assert.Equal("ids", Assert.Throws<ArgumentOutOfRangeException>(() => tokenizer.Decode([5])).ParamName);
    }

    [Fact]
    public void A_byte_level_added_token_outside_the_byte_alphabet_decodes_whole()
    {
        // tokenizers 0.23.2 decodes [a, b, <日本>, a] to 'ab<日本>a' (#1335).
        var vocabulary = new BpeVocabulary(
            new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = 0, ["b"] = 1 }, [])
        {
            ByteLevel = true,
            PreTokenizerPattern = BpePatterns.Gpt2,
            AddedTokens = [new AddedToken("<日本>", 2)],
        };

        Assert.Equal("ab<日本>a", new BpeTokenizer(vocabulary).Decode([0, 1, 2, 0]));
    }

    [Theory]
    [InlineData("a b\n\na b\n")]
    [InlineData("a b\ra b\n")]
    public void A_merges_file_with_a_blank_line_or_a_bare_carriage_return_is_refused(string merges)
    {
        Assert.Throws<InvalidDataException>(() => BpeFilesLoader.Load(Utf8("""{"a":0,"b":1,"ab":2}"""), Utf8(merges)));
    }

    [Fact]
    public void A_version_line_is_skipped_wherever_it_stands_and_crlf_is_one_terminator()
    {
        BpeVocabulary vocabulary = BpeFilesLoader.Load(
            Utf8("""{"a":0,"b":1,"ab":2,"c":3,"abc":4}"""), Utf8("a b\r\n#version: 0.2\r\nab c\r\n"));

        Assert.Equal([new MergePair("a", "b"), new MergePair("ab", "c")], vocabulary.Merges);
    }

    [Fact]
    public void An_index_one_vector_past_the_largest_array_is_refused_before_it_resizes()
    {
        var index = new EmbeddingIndex(dimension: 2);
        typeof(EmbeddingIndex).GetField("_length", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(index, 0x7FFFFFC6);

        Assert.Throws<InvalidOperationException>(() => index.Add([1f, 0f]));
    }

    [Fact]
    public void Pooling_an_empty_sequence_into_a_dimension_past_the_largest_array_is_refused()
    {
        Assert.Equal(
            "dim",
            Assert.Throws<ArgumentException>(() => Pooler.MeanPool([], 0, int.MaxValue, [])).ParamName);
        Assert.Equal(
            "dim",
            Assert.Throws<ArgumentException>(() => Pooler.MeanPoolBatch([], 3, 0, int.MaxValue, [])).ParamName);
    }

    [Fact]
    public void Records_with_absent_members_hash_as_they_compare()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0 };
        WordPieceVocabulary left = new WordPieceVocabulary(vocab, "[UNK]", "##", false) with { UnkToken = null! };
        WordPieceVocabulary right = new WordPieceVocabulary(vocab, "[UNK]", "##", false) with { UnkToken = null! };
        var template = new SpecialTokenTemplate(null!, ["[SEP]"], null!);
        var token = new AddedToken(null!, 3);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Equal(template, new SpecialTokenTemplate(null!, ["[SEP]"], null!));
        Assert.Equal(template.GetHashCode(), new SpecialTokenTemplate(null!, ["[SEP]"], null!).GetHashCode());
        Assert.Equal(token.GetHashCode(), new AddedToken(null!, 3).GetHashCode());
    }

    [Fact]
    public void A_charsmap_is_copied_so_a_later_write_changes_nothing()
    {
        byte[] blob = [4, 0, 0, 0, 0, 0, 0, 0, 7];
        PrecompiledNormalizer normalizer = PrecompiledNormalizer.FromCharsMap(blob);
        PrecompiledNormalizer twin = PrecompiledNormalizer.FromCharsMap((byte[])blob.Clone());
        int hash = normalizer.GetHashCode();

        blob[^1] = 9;

        Assert.Equal(twin, normalizer);
        Assert.Equal(hash, normalizer.GetHashCode());
    }

    [Fact]
    public void Dot_names_its_second_vector_and_L2Norm_sums_in_double()
    {
        Assert.Equal("b", Assert.Throws<ArgumentException>(() => VectorMath.Dot([1f], [1f, 2f])).ParamName);
        Assert.Equal(1e20f, VectorMath.L2Norm([1e20f]));
        Assert.Equal(1e-23f, VectorMath.L2Norm([1e-23f]));
        Assert.Equal(5e20f, VectorMath.L2Norm([3e20f, 4e20f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f]));
    }

    [Fact]
    public void Mmr_measures_in_double_a_dot_whose_float_products_overflow_both_ways()
    {
        // 1e20² is +inf and -1e20 × 1e20 is -inf in float, whose sum is NaN.
        int[] chosen = Mmr.Select([1e20f, 1e20f], [[1e20f, -1e20f], [1e20f, 1e20f]], count: 2);

        Assert.Equal([1, 0], chosen);
    }

    [Fact]
    public void A_template_holding_a_null_token_is_refused_under_options()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0, ["[PAD]"] = 3 };

        Assert.Equal(
            "options",
            Assert.Throws<ArgumentException>(() => new BatchEncoder(
                new WordPieceTokenizer(vocab),
                new EncodingOptions { Template = new SpecialTokenTemplate([null!], [], "[PAD]") })).ParamName);
    }

    [Theory]
    [InlineData("{'descr': '<f4', 'fortran_order': False, 'shape': (2,), 'extra': 1, }")]
    [InlineData("{'descr': '<f4', 'fortran_order': False, 'shape': (2,), } junk")]
    [InlineData("{'descr': '<f4', 'fortran_order': False x, 'shape': (2,), }")]
    public void An_npy_header_numpy_refuses_is_refused(string dictionary)
    {
        Assert.Throws<InvalidDataException>(() => NpyFile.Read(Npy(dictionary, [1f, 2f])));
    }

    [Fact]
    public void A_repeated_npy_header_key_keeps_its_last_value_as_numpy_does()
    {
        NpyBlock block = NpyFile.Read(
            Npy("{'descr': '<f8', 'descr': '<f4', 'fortran_order': False, 'shape': (2,), }", [1f, 2f]));

        Assert.Equal([2], block.Shape);
    }

    [Fact]
    public void An_npy_header_numpy_writes_is_read()
    {
        NpyBlock block = NpyFile.Read(Npy("{'descr': '<f4', 'fortran_order': False, 'shape': (2,), }", [1f, 2f]));

        Assert.Equal([2], block.Shape);
    }

    [Fact]
    public void A_word_piece_tokenizer_names_a_null_token_or_prefix()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0 };

        Assert.Equal("unkToken", Assert.Throws<ArgumentNullException>(() => new WordPieceTokenizer(vocab, null!)).ParamName);
        Assert.Equal(
            "continuationPrefix",
            Assert.Throws<ArgumentNullException>(() => new WordPieceTokenizer(vocab, "[UNK]", null!)).ParamName);
        Assert.Equal(
            "vocabulary",
            Assert.Throws<ArgumentException>(
                () => new WordPieceTokenizer(new WordPieceVocabulary(vocab, "[UNK]", null!, false))).ParamName);
        Assert.Equal(
            "vocabulary",
            Assert.Throws<ArgumentException>(
                () => new WordPieceTokenizer(new WordPieceVocabulary(vocab, "[MISSING]", "##", false))).ParamName);
    }

    [Fact]
    public void Pad_refuses_an_order_outside_the_sequences_and_a_null_row()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0, ["[CLS]"] = 1, ["[SEP]"] = 2, ["[PAD]"] = 3 };
        var encoder = new BatchEncoder(new WordPieceTokenizer(vocab));
        long[][] sequences = [[1, 2], [1, 2]];

        Assert.Equal("order", Assert.Throws<ArgumentOutOfRangeException>(() => encoder.Pad(sequences, 0, 1, [7])).ParamName);
        Assert.Equal("sequences", Assert.Throws<ArgumentException>(() => encoder.Pad([[1, 2], null!], 0, 2)).ParamName);
        Assert.Equal(
            "options",
            Assert.Throws<ArgumentException>(() => new BatchEncoder(
                new WordPieceTokenizer(vocab), new EncodingOptions { Template = null! })).ParamName);
    }

    /// <summary>A v1.0 file whose header dict is <paramref name="dictionary"/>, padded as numpy pads it.</summary>
    private static byte[] Npy(string dictionary, float[] values)
    {
        int unpadded = 10 + dictionary.Length + 1;
        byte[] header = Encoding.ASCII.GetBytes(dictionary + new string(' ', (64 - (unpadded % 64)) % 64) + "\n");
        var file = new byte[10 + header.Length + (values.Length * sizeof(float))];
        file[0] = 0x93;
        Encoding.ASCII.GetBytes("NUMPY").CopyTo(file, 1);
        file[6] = 1;
        BinaryPrimitives.WriteUInt16LittleEndian(file.AsSpan(8), (ushort)header.Length);
        header.CopyTo(file, 10);
        for (int i = 0; i < values.Length; i++)
        {
            BinaryPrimitives.WriteSingleLittleEndian(file.AsSpan(10 + header.Length + (i * sizeof(float))), values[i]);
        }

        return file;
    }
    [Fact]
    public void Vocabularies_with_absent_members_hash_and_compare_as_equal()
    {
        var types = new[] { SentencePieceType.Normal };
        var pieces = new SentencePieceVocabulary(null!, types, 0, -1, -1, -1);
        var bpe = new BpeVocabulary(null!, null!) { AddedTokens = null!, NormalizationForms = null! };

        Assert.Equal(pieces, new SentencePieceVocabulary(null!, types, 0, -1, -1, -1));
        Assert.Equal(pieces.GetHashCode(), new SentencePieceVocabulary(null!, types, 0, -1, -1, -1).GetHashCode());
        Assert.Equal(bpe, new BpeVocabulary(null!, null!) { AddedTokens = null!, NormalizationForms = null! });
        Assert.Equal(bpe.GetHashCode(), new BpeVocabulary(null!, null!) { AddedTokens = null!, NormalizationForms = null! }.GetHashCode());

        // Every list absent at once, and one present against one absent.
        var bare = new SentencePieceVocabulary(null!, null!, 0, -1, -1, -1) { PrefixTokens = null!, SuffixTokens = null! };
        var bareBpe = new BpeVocabulary(null!, null!)
        {
            AddedTokens = null!,
            NormalizationForms = null!,
            PrefixTokens = null!,
            SuffixTokens = null!,
        };
        Assert.Equal(bare, bare with { });
        Assert.Equal(bare.GetHashCode(), (bare with { }).GetHashCode());
        Assert.NotEqual(bare, bare with { Types = types });
        Assert.NotEqual(bare with { Types = types }, bare);
        Assert.Equal(bareBpe, bareBpe with { });
        Assert.Equal(bareBpe.GetHashCode(), (bareBpe with { }).GetHashCode());
        Assert.NotEqual(bareBpe, bareBpe with { NormalizationForms = [NormalizationForm.FormC] });
        Assert.NotEqual(bareBpe with { Vocab = new Dictionary<string, int>(StringComparer.Ordinal) }, bareBpe);
    }

    [Fact]
    public void A_bpe_vocabulary_missing_a_list_or_holding_a_null_token_is_refused_under_vocabulary()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = 0 };

        foreach (BpeVocabulary broken in new[]
        {
            new BpeVocabulary(null!, []) { PreTokenizerPattern = @"\S+" },
            new BpeVocabulary(vocab, null!) { PreTokenizerPattern = @"\S+" },
            new BpeVocabulary(vocab, [new MergePair(null!, "a")]) { PreTokenizerPattern = @"\S+" },
            new BpeVocabulary(vocab, []) { PreTokenizerPattern = @"\S+", AddedTokens = [null!] },
            new BpeVocabulary(vocab, []) { PreTokenizerPattern = @"\S+", NormalizationForms = null! },
        })
        {
            Assert.Equal("vocabulary", Assert.Throws<ArgumentException>(() => new BpeTokenizer(broken)).ParamName);
        }
    }

    [Fact]
    public void A_word_piece_added_token_with_no_string_or_a_negative_id_is_refused()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0 };

        foreach (AddedToken added in new[] { null!, new AddedToken("<x>", -1), new AddedToken(null!, 1) })
        {
            var vocabulary = new WordPieceVocabulary(vocab, "[UNK]", "##", false) { AddedTokens = [added] };

            Assert.Equal("vocabulary", Assert.Throws<ArgumentException>(() => new WordPieceTokenizer(vocabulary)).ParamName);
        }
    }

    [Fact]
    public void Pooling_an_empty_batch_wider_than_the_largest_array_is_refused()
    {
        Assert.Equal(
            "batchSize",
            Assert.Throws<ArgumentException>(() => Pooler.MeanPoolBatch([], int.MaxValue, 0, 0, [])).ParamName);
    }
    [Fact]
    public void A_split_with_no_pattern_or_an_undefined_normalization_form_is_refused_under_vocabulary()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["a"] = 0 };

        foreach (BpeVocabulary broken in new[]
        {
            new BpeVocabulary(vocab, []) { PreSplit = new BpeSplitStep(null!, SplitBehavior.Isolated, false) },
            new BpeVocabulary(vocab, []) { PreTokenizerPattern = @"\S+", NormalizationForms = [(NormalizationForm)99] },
        })
        {
            Assert.Equal("vocabulary", Assert.Throws<ArgumentException>(() => new BpeTokenizer(broken)).ParamName);
        }
    }

    [Fact]
    public void A_unigram_piece_whose_id_is_not_its_position_is_refused()
    {
        SentencePieceVocabulary vocabulary = Unigram(new("<unk>", -10, 0), new("▁a", -1, 5));

        Assert.Equal("vocabulary", Assert.Throws<ArgumentException>(() => new SentencePieceTokenizer(vocabulary)).ParamName);
    }

    [Fact]
    public void Records_with_absent_lists_print_Count_0_as_empty_ones_do()
    {
        // Printing does not tell absent from empty; equality does, which the next test asserts.
        var types = new[] { SentencePieceType.Normal };

        Assert.Contains("Count = 0", new SentencePieceVocabulary(null!, types, 0, -1, -1, -1).ToString(), StringComparison.Ordinal);
        Assert.Contains("Count = 0", new BpeVocabulary(null!, []).ToString(), StringComparison.Ordinal);
        Assert.Contains("Count = 0", (new WordPieceVocabulary(null!, "[UNK]", "##", false)).ToString(), StringComparison.Ordinal);
        Assert.Contains("SpecialTokenCount = 0", new SpecialTokenTemplate(null!, null!, "[PAD]").ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Absent_and_differing_lists_compare_unequal_and_a_record_equals_itself()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0 };
        var pieces = Unigram(new("<unk>", -10, 0), new("▁a", -1, 1));
        var bpe = new BpeVocabulary(vocab, []) { NormalizationForms = [NormalizationForm.FormC] };
        var wordPiece = new WordPieceVocabulary(vocab, "[UNK]", "##", false);
        var template = new SpecialTokenTemplate(["[CLS]"], ["[SEP]"], "[PAD]");

        Assert.True(pieces.Equals(pieces));
        Assert.True(bpe.Equals(bpe));
        Assert.True(wordPiece.Equals(wordPiece));
        Assert.True(template.Equals(template));
        Assert.NotEqual(pieces, pieces with { Types = [SentencePieceType.Normal] });
        Assert.NotEqual(bpe, bpe with { NormalizationForms = [NormalizationForm.FormD] });
        Assert.Equal(wordPiece with { Vocab = null! }, wordPiece with { Vocab = null! });
        Assert.NotEqual(wordPiece with { Vocab = null! }, wordPiece);
    }

    [Fact]
    public void The_trie_builder_grows_to_what_it_is_asked_and_refuses_past_the_largest_array()
    {
        // The growth a large vocabulary reaches, driven directly: no committed vocabulary is big enough to need it.
        Type builderType = typeof(CharTrie).GetNestedType("Builder", BindingFlags.NonPublic)!;
        object builder = Activator.CreateInstance(builderType, (IReadOnlyList<string>)["ab"], new int[128], 2)!;
        MethodInfo growToReach = builderType.GetMethod("GrowToReach", BindingFlags.NonPublic | BindingFlags.Instance)!;
        int before = ((int[])builderType.GetProperty("Check")!.GetValue(builder)!).Length;

        growToReach.Invoke(builder, [(long)before + 1]);

        Assert.Equal(before * 2, ((int[])builderType.GetProperty("Check")!.GetValue(builder)!).Length);
        TargetInvocationException refused = Assert.Throws<TargetInvocationException>(
            () => growToReach.Invoke(builder, [0x7FFFFFC7L + 1]));
        Assert.IsType<InvalidOperationException>(refused.InnerException);
    }
}

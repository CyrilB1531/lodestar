using System.Collections;
using Lodestar.Internal.Persistence;
using Lodestar.Text.Persistence;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Persistence;

/// <summary>A TF-IDF save refuses, before its first byte, idf weights no writer could hold in one array (#1617).</summary>
public sealed class IdfBlockBoundTests
{
    [Fact]
    public void The_idf_block_fits_the_writer_up_to_201_129_983_weights()
    {
        // Two mebibytes under int.MaxValue. System.Text.Json 10's writer, flushed before the block, wrote 201,326,561
        // weights and failed at 201,326,562 (#1618); the margin stands for the builds not measured.
        Assert.True(Base64Numbers.WritableAsProperty(201_129_983, sizeof(double)));
        Assert.False(Base64Numbers.WritableAsProperty(201_129_984, sizeof(double)));
        Assert.False(Base64Numbers.WritableAsProperty(268_435_456, sizeof(double)));
    }

    [Fact]
    public void Weights_past_it_are_refused_with_the_length_of_their_block()
    {
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => FeatureVocabularyJson.EnsureWritableIdf(new Zeros(201_129_984)));
        Assert.Contains("2145386496 characters, within two mebibytes of the most the JSON writer holds", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_non_finite_weight_is_still_refused_first()
    {
        var weights = new Zeros(201_129_984) { NaNAt = 3 };

        Assert.Throws<InvalidDataException>(() => FeatureVocabularyJson.EnsureWritableIdf(weights));
    }

    [Fact]
    public async Task An_unfitted_asynchronous_save_refuses_through_its_task_before_writing()
    {
        var vectorizer = new TfidfVectorizer();
        var stream = new MemoryStream();
        try
        {
            // Taken before it is awaited: a refusal thrown synchronously would never return a task at all.
            Task unfitted = vectorizer.SaveAsync(stream);
            Task nullStream = vectorizer.SaveAsync(null!);
            Assert.True(unfitted.IsFaulted);
            Assert.True(nullStream.IsFaulted);
            await Assert.ThrowsAsync<InvalidOperationException>(() => unfitted);
            await Assert.ThrowsAsync<ArgumentNullException>(() => nullStream);
            Assert.Equal(0, stream.Length);
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }

    [Fact]
    public void A_non_finite_weight_is_refused_before_a_byte_is_written()
    {
        var vectorizer = new TfidfVectorizer();
        vectorizer.Fit(["apple banana", "banana cherry", "cherry date"]);
        string path = Path.GetTempFileName();
        try
        {
            vectorizer.Save(path);

            // Idf hands out the fitted array, the one way to plant a weight no fit produces.
            vectorizer.FittedIdf![0] = double.NaN;

            using var stream = new MemoryStream();
            Assert.Throws<InvalidDataException>(() => vectorizer.Save(stream));
            Assert.Equal(0, stream.Length);
            // A stream the writer refuses is refused first, as main's writer refused it (#1618).
            using var readOnly = new MemoryStream(new byte[16], writable: false);
            Assert.Throws<ArgumentException>(() => vectorizer.Save(readOnly));
            // Refused once the file is open and before its first byte, where main's write refused it; a path opening
            // refuses is refused first, as main refused it (#1618).
            Assert.Throws<InvalidDataException>(() => vectorizer.Save(path));
            Assert.Equal(0, new FileInfo(path).Length);
            Assert.Equal("path", Assert.Throws<ArgumentNullException>(() => vectorizer.Save((string)null!)).ParamName);
            Assert.Equal("path", Assert.Throws<ArgumentException>(() => vectorizer.Save(string.Empty)).ParamName);
            Assert.Throws<DirectoryNotFoundException>(
                () => vectorizer.Save(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "x.json")));
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>A list of zeros that reports a length without holding one value per entry.</summary>
    private sealed class Zeros(int count) : IReadOnlyList<double>
    {
        public int NaNAt { get; init; } = -1;

        public int Count => count;

        public double this[int index] => index == NaNAt ? double.NaN : 0.0;

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Sample;

/// <summary>An in-process collection: the records are the state, the indexes are caches.</summary>
internal static class LodestarVectorStoreCollectionSample
{
    public static async Task RunAsync()
    {
        Console.WriteLine("Vector store collection (Lodestar.Extensions.VectorData)");

        var notes = new LodestarVectorStoreCollection<string, VectorDataCorpus.Note>("notes");
        Console.WriteLine($"  name              : {notes.Name}");
        Console.WriteLine($"  exists before     : {await notes.CollectionExistsAsync().ConfigureAwait(false)}");

        await notes.EnsureCollectionExistsAsync().ConfigureAwait(false);
        await notes.UpsertAsync(VectorDataCorpus.Notes[0]).ConfigureAwait(false);
        await notes.UpsertAsync(VectorDataCorpus.Notes[1..]).ConfigureAwait(false);
        Console.WriteLine($"  exists after      : {await notes.CollectionExistsAsync().ConfigureAwait(false)}");

        VectorDataCorpus.Note? one = await notes.GetAsync("a").ConfigureAwait(false);
        Console.WriteLine($"  by key            : {one?.Text}");

        await foreach (VectorDataCorpus.Note note in notes.GetAsync(["b", "c"]).ConfigureAwait(false))
        {
            Console.WriteLine($"  by keys           : {note.Id}");
        }

        await foreach (VectorDataCorpus.Note note in notes.GetAsync(n => n.Text.Contains("river") || n.Text.Contains("park"), 1,
            new FilteredRecordRetrievalOptions<VectorDataCorpus.Note> { Skip = 1 }).ConfigureAwait(false))
        {
            Console.WriteLine($"  by filter, skip 1 : {note.Id}");
        }

        Console.WriteLine($"  orderBy refused   : {await OrderByIsRefused(notes).ConfigureAwait(false)}");

        Console.WriteLine($"  metadata          : {notes.GetService(typeof(VectorStoreCollectionMetadata)) is not null}");

        await foreach (VectorSearchResult<VectorDataCorpus.Note> hit in notes.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]), 2).ConfigureAwait(false))
        {
            Console.WriteLine($"  nearest           : {hit.Record.Id} at {Inv.F4(hit.Score ?? 0.0)}");
        }

        await foreach (VectorSearchResult<VectorDataCorpus.Note> hit in notes.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 2).ConfigureAwait(false))
        {
            Console.WriteLine($"  fused             : {hit.Record.Id} at {Inv.F4(hit.Score ?? 0.0)}");
        }

        await notes.DeleteAsync("a").ConfigureAwait(false);
        await notes.DeleteAsync(["b", "c"]).ConfigureAwait(false);
        await notes.EnsureCollectionDeletedAsync().ConfigureAwait(false);
        Console.WriteLine($"  exists deleted    : {await notes.CollectionExistsAsync().ConfigureAwait(false)}");
        notes.Dispose();

        Console.WriteLine($"  no full text      : {await HybridSearchNeedsFullText().ConfigureAwait(false)}");
        Console.WriteLine();
    }

    // A dictionary-backed collection has no order of its own, so GetAsync's OrderBy is
    // refused rather than silently ignored.
    private static async Task<bool> OrderByIsRefused(LodestarVectorStoreCollection<string, VectorDataCorpus.Note> notes)
    {
        try
        {
            IAsyncEnumerator<VectorDataCorpus.Note> hits = notes.GetAsync(
                n => n.Text.Contains("cat"), 1,
                new FilteredRecordRetrievalOptions<VectorDataCorpus.Note> { OrderBy = o => o.Ascending(n => n.Id) })
                .GetAsyncEnumerator();
            await using (hits.ConfigureAwait(false))
            {
                return !await hits.MoveNextAsync().ConfigureAwait(false);
            }
        }
        catch (NotSupportedException)
        {
            return true;
        }
    }

    // Fusion needs a keyword half to fuse with; a record type marking no
    // [VectorStoreData(IsFullTextIndexed = true)] property has none to build one from.
    private static async Task<bool> HybridSearchNeedsFullText()
    {
        var plain = new LodestarVectorStoreCollection<string, VectorDataCorpus.PlainNote>("plain");
        float[] vector = [1f, 0f, 0f];
        await plain.EnsureCollectionExistsAsync().ConfigureAwait(false);
        await plain.UpsertAsync(new VectorDataCorpus.PlainNote { Id = "x", Embedding = vector }).ConfigureAwait(false);

        try
        {
            IAsyncEnumerator<VectorSearchResult<VectorDataCorpus.PlainNote>> hits =
                plain.HybridSearchAsync(new ReadOnlyMemory<float>(vector), ["x"], 1).GetAsyncEnumerator();
            await using (hits.ConfigureAwait(false))
            {
                return !await hits.MoveNextAsync().ConfigureAwait(false);
            }
        }
        catch (NotSupportedException)
        {
            return true;
        }
        finally
        {
            plain.Dispose();
        }
    }
}

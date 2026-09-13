using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Sample;

/// <summary>An in-process store: named collections, held for the store's lifetime.</summary>
internal static class LodestarVectorStoreSample
{
    private const string Collection = "notes";

    public static async Task RunAsync()
    {
        Console.WriteLine("Vector store (Lodestar.Extensions.VectorData)");

        using var store = new LodestarVectorStore();
        VectorStoreCollection<string, VectorDataCorpus.Note> notes =
            store.GetCollection<string, VectorDataCorpus.Note>(Collection);
        await notes.EnsureCollectionExistsAsync().ConfigureAwait(false);
        await notes.UpsertAsync(VectorDataCorpus.Notes).ConfigureAwait(false);

        Console.WriteLine($"  store knows it    : {await store.CollectionExistsAsync(Collection).ConfigureAwait(false)}");
        Console.WriteLine($"  unknown name      : {await store.CollectionExistsAsync("ghost").ConfigureAwait(false)}");

        await foreach (string name in store.ListCollectionNamesAsync().ConfigureAwait(false))
        {
            Console.WriteLine($"  listed            : {name}");
        }

        Console.WriteLine($"  store metadata    : {store.GetService(typeof(VectorStoreMetadata)) is not null}");

        // A dynamic record is a dictionary with no properties to compile a filter over.
        try
        {
            store.GetDynamicCollection("dynamic", new VectorStoreCollectionDefinition());
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"  dynamic refused   : {ex.Message.Length > 0}");
        }

        await store.EnsureCollectionDeletedAsync(Collection).ConfigureAwait(false);
        Console.WriteLine($"  deleted           : {!await store.CollectionExistsAsync(Collection).ConfigureAwait(false)}");
        Console.WriteLine();
    }
}

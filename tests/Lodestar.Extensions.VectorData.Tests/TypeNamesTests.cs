using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>How a refusal names a type: CLR names, own type arguments, element suffixes (#1804, #1807).</summary>
public sealed class TypeNamesTests
{
    private static string Alone(Type type) => TypeNames.Names(type)[0].ToString();

    public static TheoryData<Type, string> Named => new()
    {
        { typeof(int), "Int32" },
        { typeof(List<int>), "List<Int32>" },
        { typeof(Dictionary<string, List<float>>), "Dictionary<String, List<Single>>" },
        { typeof(List<float>[]), "List<Single>[]" },
        { typeof(ReadOnlyMemory<float>[][]), "ReadOnlyMemory<Single>[][]" },
        { typeof(int[,]), "Int32[,]" },
        { typeof(int).MakeArrayType(1), "Int32[*]" },
        { typeof(List<int>).MakeByRefType(), "List<Int32>&" },
        { typeof(int).MakePointerType(), "Int32*" },
        { typeof(List<>), "List<T>" },
        { typeof(Dictionary<int, string>.KeyCollection), "KeyCollection" },
        { typeof(GenericOuter<int>.Record), "Record" },
        { typeof(GenericOuter<int>.Inner<string>), "Inner<String>" },
    };

    [Theory]
    [MemberData(nameof(Named))]
    public void A_type_is_named_by_its_CLR_name_with_its_own_arguments(Type type, string expected) =>
        Assert.Equal(expected, Alone(type));

    [Fact]
    public void A_backtick_with_no_arity_after_it_is_part_of_the_name()
    {
        // C# cannot name a type so; IL, which another compiler may emit, can.
        Type emitted = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("TypeNamesTests"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("TypeNamesTests").DefineType("Foo`bar", TypeAttributes.Public).CreateType();

        Assert.Equal("Foo`bar", Alone(emitted));
    }

    [Fact]
    public void Digits_that_count_no_argument_are_part_of_the_name_and_the_last_backtick_is_the_suffix()
    {
        ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ArityNames"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("ArityNames");
        Type notGeneric = module.DefineType("Foo`1", TypeAttributes.Public).CreateType();
        TypeBuilder ticked = module.DefineType("A`B`1", TypeAttributes.Public);
        ticked.DefineGenericParameters("T");
        Type closed = ticked.CreateType().MakeGenericType(typeof(int));

        Assert.Equal("Foo`1", Alone(notGeneric));
        Assert.Equal("A`B<Int32>", Alone(closed));
    }

    private const string Here = "Lodestar.Extensions.VectorData.Tests";

    public static TheoryData<Type[], string[]> NamedTogether => new()
    {
        { [typeof(int), typeof(string)], ["Int32", "String"] },
        { [typeof(List<int>), typeof(int)], ["List<Int32>", "Int32"] },
        { [typeof(SameName.First.Id), typeof(SameName.Second.Id)], [$"{Here}.SameName+First+Id", $"{Here}.SameName+Second+Id"] },
        { [typeof(SameName.First.Id?), typeof(SameName.Second.Id)], [$"Nullable<{Here}.SameName+First+Id>", $"{Here}.SameName+Second+Id"] },
        { [typeof(List<SameName.First.Id>), typeof(List<SameName.Second.Id>)], [$"List<{Here}.SameName+First+Id>", $"List<{Here}.SameName+Second+Id>"] },
        { [typeof(SameName.First.Id[]), typeof(SameName.Second.Id)], [$"{Here}.SameName+First+Id[]", $"{Here}.SameName+Second+Id"] },
        { [typeof(GenericOuter<int>.Record), typeof(SameName.First.Record)], [$"{Here}.GenericOuter<Int32>+Record", $"{Here}.SameName+First+Record"] },
        { [typeof(GenericOuter<int>.Record), typeof(GenericOuter<long>.Record)], [$"{Here}.GenericOuter<Int32>+Record", $"{Here}.GenericOuter<Int64>+Record"] },
        { [typeof(GenericOuter<int>.Inner<string>), typeof(GenericOuter<long>.Inner<string>)], [$"{Here}.GenericOuter<Int32>+Inner<String>", $"{Here}.GenericOuter<Int64>+Inner<String>"] },
        { [typeof((int, int, int)), typeof((int, int))], ["ValueTuple<Int32, Int32, Int32>", "ValueTuple<Int32, Int32>"] },
        { [typeof(Task), typeof(Task<int>)], ["Task", "Task<Int32>"] },
        { [typeof(List<>), typeof(List<SameName.T>)], ["List<T of System.Collections.Generic.List<>>", $"List<{Here}.SameName+T>"] },
        {
            [typeof(GenericOuter<>.Inner<>), typeof(GenericOuter<SameName.T>.Inner<SameName.T>)],
            [$"{Here}.GenericOuter<T of {Here}.GenericOuter<>+Inner<>>+Inner<TInner>", $"{Here}.GenericOuter<{Here}.SameName+T>+Inner<{Here}.SameName+T>"]
        },
        { [typeof(GenericOuter<SameName.First.Id>.Record), typeof(SameName.Second.Id)], ["Record", "Id"] },
        {
            [typeof(GenericOuter<SameName.First.Id>.Record), typeof(GenericOuter<SameName.Second.Id>.Record)],
            [$"{Here}.GenericOuter<{Here}.SameName+First+Id>+Record", $"{Here}.GenericOuter<{Here}.SameName+Second+Id>+Record"]
        },
        { [typeof(Dictionary<SameName.First.Id, SameName.Second.Id>)], [$"Dictionary<{Here}.SameName+First+Id, {Here}.SameName+Second+Id>"] },
        { [typeof(int), typeof(SameName.First.Id), typeof(string), typeof(SameName.Second.Id)], ["Int32", $"{Here}.SameName+First+Id", "String", $"{Here}.SameName+Second+Id"] },
    };

    [Theory]
    [MemberData(nameof(NamedTogether))]
    public void Types_named_together_are_named_short_but_where_two_components_would_read_alike(Type[] types, string[] expected) =>
        Assert.Equal(expected, TypeNames.Names(types).Select(name => name.ToString()));

    [Fact]
    public void Two_types_of_one_full_name_from_two_assemblies_are_named_with_their_assembly()
    {
        Type first = Twin("TwinA");
        Type second = Twin("TwinB");

        Assert.Equal(
            ["Twin.Id [TwinA, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]", "Twin.Id [TwinB, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"],
            TypeNames.Names(first, second).Select(name => name.ToString()));

        static Type Twin(string assembly) =>
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assembly), AssemblyBuilderAccess.RunAndCollect)
                .DefineDynamicModule(assembly).DefineType("Twin.Id", TypeAttributes.Public).CreateType();
    }

    [Fact]
    public void Two_global_types_of_one_name_from_two_assemblies_are_named_with_their_assembly()
    {
        string[] names = [.. TypeNames.Names(Emitted("GlobalA", "Foo"), Emitted("GlobalB", "Foo")).Select(name => name.ToString())];

        Assert.Equal(
            ["Foo [GlobalA, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]", "Foo [GlobalB, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"],
            names);
    }

    [Fact]
    public void A_name_that_prints_as_a_generic_closed_over_the_same_arguments_is_told_apart_from_it()
    {
        ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Lookalike"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Lookalike");
        Type plain = module.DefineType("Foo<Int32>", TypeAttributes.Public).CreateType();
        TypeBuilder generic = module.DefineType("Foo`1", TypeAttributes.Public);
        generic.DefineGenericParameters("T");
        Type closed = generic.CreateType().MakeGenericType(typeof(int));

        string[] names = [.. TypeNames.Names(plain, closed).Select(name => name.ToString())];

        Assert.NotEqual(names[0], names[1]);
        Assert.All(names, name => Assert.StartsWith("Foo<Int32> [Lookalike, ", name, StringComparison.Ordinal));
    }

    [Fact]
    public void Three_types_alike_only_pair_by_pair_take_three_ranks()
    {
        // The plain lookalike prints as the first closure, and the two closures share a head: alike, but not all three.
        (Type plain, Type first) = Lookalikes(typeof(int));
        (_, Type second) = Lookalikes(typeof(long));

        string[] names = [.. TypeNames.Names(plain, first, second).Select(name => name.ToString())];

        Assert.Equal(3, names.Distinct(StringComparer.Ordinal).Count());

        static (Type Plain, Type Closed) Lookalikes(Type argument)
        {
            ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("X"), AssemblyBuilderAccess.RunAndCollect)
                .DefineDynamicModule("X");
            Type plain = module.DefineType("Foo<Int32>", TypeAttributes.Public).CreateType();
            TypeBuilder generic = module.DefineType("Foo`1", TypeAttributes.Public);
            generic.DefineGenericParameters("T");
            return (plain, generic.CreateType().MakeGenericType(argument));
        }
    }

    [Fact]
    public void A_name_that_prints_as_an_array_is_told_apart_from_the_array()
    {
        ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Brackets"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Brackets");
        Type literal = module.DefineType("Foo[]", TypeAttributes.Public).CreateType();
        Type array = module.DefineType("Foo", TypeAttributes.Public).CreateType().MakeArrayType();

        string[] names = [.. TypeNames.Names(literal, array).Select(name => name.ToString())];

        Assert.NotEqual(names[0], names[1]);
    }

    [Fact]
    public void A_nested_type_does_not_read_like_a_type_of_a_namespace()
    {
        ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Nesting"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Nesting");
        TypeBuilder outer = module.DefineType("A.B", TypeAttributes.Public);
        Type nested = outer.DefineNestedType("Foo", TypeAttributes.NestedPublic).CreateType();
        outer.CreateType();
        Type spaced = module.DefineType("A.B.Foo", TypeAttributes.Public).CreateType();

        Assert.Equal(["A.B+Foo", "A.B.Foo"], TypeNames.Names(nested, spaced).Select(name => name.ToString()));
    }

    [Fact]
    public void Ranked_arguments_part_the_type_they_are_passed_to_without_a_rank_of_its_own()
    {
        Type first = Twin();
        Type second = Twin();

        string[] names = [.. TypeNames.Names(typeof(GenericOuter<>.Record).MakeGenericType(first), typeof(GenericOuter<>.Record).MakeGenericType(second))
            .Select(name => name.ToString())];

        Assert.StartsWith($"{Here}.GenericOuter<Twin.Id [Twin, ", names[0], StringComparison.Ordinal);
        Assert.EndsWith(" #1>+Record", names[0], StringComparison.Ordinal);
        Assert.EndsWith(" #2>+Record", names[1], StringComparison.Ordinal);
    }

    [Fact]
    public void A_member_is_named_before_what_tells_its_type_apart()
    {
        RefusalName[] names = TypeNames.Names(Emitted("MemberA", "Doc"), Emitted("MemberB", "Doc"));

        Assert.Equal("Doc.Id [MemberA, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]", names[0].Member("Id"));
    }

    private static Type Emitted(string assembly, string type) =>
        AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assembly), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule(assembly).DefineType(type, TypeAttributes.Public).CreateType();

    private static Type Twin() => Emitted("Twin", "Twin.Id");

    [Fact]
    public void Two_types_nothing_else_tells_apart_are_named_with_their_rank()
    {
        string[] names = [.. TypeNames.Names(Twin(), Twin()).Select(name => name.ToString())];

        Assert.EndsWith(" #1", names[0], StringComparison.Ordinal);
        Assert.EndsWith(" #2", names[1], StringComparison.Ordinal);
        Assert.StartsWith("Twin.Id [Twin, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, ", names[0], StringComparison.Ordinal);
    }

    [Fact]
    public void A_record_hiding_the_key_of_a_base_of_its_own_name_is_told_apart_from_it()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, SameName.Second.Doc>("docs"));

        Assert.StartsWith(
            $"{Here}.SameName+Second+Doc carries no [VectorStoreKey] property of its own; {Here}.SameName+First+Doc.Id is marked,",
            error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void One_type_loaded_in_two_load_contexts_is_named_with_its_context()
    {
        var context = new AssemblyLoadContext("second", isCollectible: true);
        try
        {
            Type loaded = context.LoadFromAssemblyPath(typeof(PlainVectorRecord).Assembly.Location)
                .GetType(typeof(PlainVectorRecord).FullName!, throwOnError: true)!;

            string[] names = [.. TypeNames.Names(typeof(PlainVectorRecord), loaded).Select(name => name.ToString())];

            Assert.NotEqual(names[0], names[1]);
            Assert.StartsWith($"{Here}.PlainVectorRecord [{typeof(PlainVectorRecord).Assembly.FullName}, ", names[0], StringComparison.Ordinal);
            Assert.Contains("\"Default\"", names[0], StringComparison.Ordinal);
            Assert.Contains("\"second\"", names[1], StringComparison.Ordinal);
        }
        finally
        {
            context.Unload();
        }
    }

    private static readonly ReadOnlyMemory<float> OneWide = new float[] { 1f };

    private static readonly ReadOnlyMemory<float> UnitX = new float[] { 1f, 0f, 0f };

    [Fact]
    public void A_key_whose_ToString_throws_leaves_the_width_refusal_standing()
    {
        using var collection = new LodestarVectorStoreCollection<UnprintableKey, UnprintablyKeyed>("keys");

        // Not awaited: the refusal comes out of the call itself, as the documentation says.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => { _ = collection.UpsertAsync(new UnprintablyKeyed { Id = new UnprintableKey(), Embedding = OneWide }); });

        Assert.StartsWith("The record keyed (of type UnprintableKey, whose ToString threw) carries a vector of 1", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_key_whose_ToString_returns_null_is_named_by_its_type()
    {
        using var collection = new LodestarVectorStoreCollection<SilentKey, SilentlyKeyed>("keys");

        // Not awaited: the refusal comes out of the call itself, as the documentation says.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => { _ = collection.UpsertAsync(new SilentlyKeyed { Id = new SilentKey(), Embedding = OneWide }); });

        Assert.StartsWith("The record keyed (of type SilentKey, whose ToString returned null) carries", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void An_empty_key_is_quoted_in_the_width_refusal()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("keys");

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => { _ = collection.UpsertAsync(new PlainVectorRecord { Id = string.Empty, Embedding = OneWide }); });

        Assert.StartsWith("The record keyed \"\" carries a vector of 1", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_parameters_of_two_generic_methods_of_one_name_read_apart_by_their_arity()
    {
        MethodInfo[] selects = [.. typeof(Enumerable).GetMethods().Where(method => method.Name == nameof(Enumerable.Zip))];
        Type one = selects.First(method => method.GetGenericArguments().Length == 2).GetGenericArguments()[0];
        Type other = selects.First(method => method.GetGenericArguments().Length == 3).GetGenericArguments()[0];

        Assert.Equal(
            ["TFirst of System.Linq.Enumerable.Zip<,>", "TFirst of System.Linq.Enumerable.Zip<,,>"],
            TypeNames.Names(one, other).Select(name => name.ToString()));
    }

    [Fact]
    public void A_blank_collection_name_is_quoted_in_the_one_schema_refusal()
    {
        using var store = new LodestarVectorStore();
        _ = store.GetCollection<string, PlainVectorRecord>(" ");

        ArgumentException error = Assert.Throws<ArgumentException>(() => store.GetCollection<string, ArrayVector>(" "));

        Assert.StartsWith("The collection \" \" is already held over String keys and PlainVectorRecord records", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_nullable_vector_search_value_is_searched_as_its_vector()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("vectors");
        await collection.UpsertAsync(new PlainVectorRecord { Id = "a", Embedding = UnitX });
        ReadOnlyMemory<float>? query = UnitX;

        // CS8714: the abstraction declares TInput notnull, a nullability warning a caller may still compile past.
#pragma warning disable CS8714
        List<VectorSearchResult<PlainVectorRecord>> found = await collection.SearchAsync(query, 1).ToListAsync();
#pragma warning restore CS8714

        Assert.Equal("a", Assert.Single(found).Record.Id);
    }

    [Fact]
    public async Task A_value_type_search_value_that_is_not_a_vector_is_named_by_its_type()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("vectors");

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync(42, 1).ToListAsync());

        Assert.StartsWith("Int32 is not a vector", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_record_nested_in_a_generic_type_is_named_without_its_declaring_type_s_arguments()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, GenericOuter<int>.Record>("records"));

        Assert.StartsWith("Record.Id is Int32, and this collection is keyed by String;", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_vector_property_typed_as_an_array_of_a_generic_type_is_named_with_its_arguments()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, ArrayVector>("vectors"));

        Assert.Contains("ArrayVector.Embedding is ReadOnlyMemory<Single>[]; this store holds ReadOnlyMemory<Single>,", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_search_value_of_an_array_of_a_generic_type_is_named_with_its_arguments()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("vectors");

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync(new[] { new List<float>() }, 1).ToListAsync());

        Assert.StartsWith("List<Single>[] is not a vector", error.Message, StringComparison.Ordinal);
        Assert.Contains("supply a ReadOnlyMemory<Single> or a Single[],", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_search_value_declared_object_is_named_by_its_runtime_type_and_any_other_by_its_declared_one()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("vectors");
        object boxed = new List<float>();
        IEnumerable<float> sequence = new List<float>();

        NotSupportedException ofObject = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync(boxed, 1).ToListAsync());
        NotSupportedException ofInterface = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync(sequence, 1).ToListAsync());

        Assert.StartsWith("List<Single> is not a vector", ofObject.Message, StringComparison.Ordinal);
        Assert.StartsWith("IEnumerable<Single> is not a vector", ofInterface.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_function_pointer_whose_name_is_empty_is_named_by_its_full_name()
    {
        Type pointer = typeof(FunctionPointerVector).GetProperty(nameof(FunctionPointerVector.Embedding))!.PropertyType;

        Assert.Empty(pointer.Name);
        Assert.Equal("System.Void()", Alone(pointer));
    }

    [Fact]
    public void Two_function_pointers_whose_texts_read_alike_are_told_apart()
    {
        Type managed = typeof(FunctionPointerVector).GetProperty(nameof(FunctionPointerVector.Embedding))!.PropertyType;
        Type unmanaged = typeof(FunctionPointerVector).GetProperty(nameof(FunctionPointerVector.Unmanaged))!.PropertyType;

        string[] names = [.. TypeNames.Names(managed, unmanaged).Select(name => name.ToString())];

        Assert.NotEqual(managed, unmanaged);
        Assert.NotEqual(names[0], names[1]);
    }

    [Fact]
    public async Task A_null_search_value_is_refused_by_name_whatever_type_it_is_declared()
    {
        using var collection = new LodestarVectorStoreCollection<string, PlainVectorRecord>("vectors");
        float[]? array = null;
        object? boxed = null;

        ArgumentNullException ofArray = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await collection.SearchAsync(array!, 1).ToListAsync());
        ArgumentNullException ofObject = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await collection.HybridSearchAsync(boxed!, ["a"], 1).ToListAsync());

        Assert.Equal("searchValue", ofArray.ParamName);
        Assert.Equal("searchValue", ofObject.ParamName);

        ReadOnlyMemory<float>? none = null;
        // CS8714: the abstraction declares TInput notnull, a nullability warning a caller may still compile past.
#pragma warning disable CS8714
        ArgumentNullException ofNullable = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await collection.SearchAsync(none, 1).ToListAsync());
#pragma warning restore CS8714
        Assert.Equal("searchValue", ofNullable.ParamName);
    }
}

/// <summary>A generic declaring type, whose argument a nested type's own name must not carry.</summary>
/// <typeparam name="T">Unused: the declaring type only has to be generic.</typeparam>
// S2326: T is unused on purpose. CA1034: the nested types are the subject of the test, so they have to be nested.
#pragma warning disable S2326, CA1034
public static class GenericOuter<T>
{
    /// <summary>A record nested in a generic type, keyed by an <see cref="int"/> a <see cref="string"/> collection refuses.</summary>
    public sealed class Record
    {
        [VectorStoreKey]
        public int Id { get; set; }

        [VectorStoreVector(3)]
        public ReadOnlyMemory<float> Embedding { get; set; }
    }

    /// <summary>A generic type nested in a generic one, whose own argument alone is named.</summary>
    /// <typeparam name="TInner">What <see cref="Value"/> holds.</typeparam>
    public sealed class Inner<TInner>
    {
        /// <summary>A member, so the type is not empty.</summary>
        public TInner? Value { get; set; }
    }
}
#pragma warning restore S2326, CA1034

/// <summary>Two types named alike, one nested in each of two holders.</summary>
// CA1034: the nested types are the subject of the test, so they have to be nested.
#pragma warning disable CA1034
public static class SameName
{
    /// <summary>A type named as the parameter of <see cref="List{T}"/> is.</summary>
    public sealed class T
    {
        /// <summary>A member, so the type is not empty.</summary>
        public int Value { get; set; }
    }

    /// <summary>The first holder.</summary>
    public static class First
    {
        /// <summary>An <c>Id</c> nested here.</summary>
        public enum Id { None }

        /// <summary>A <c>Record</c> nested here, named as <see cref="GenericOuter{T}.Record"/> is.</summary>
        public enum Record { None }

        /// <summary>A base whose key <see cref="Second.Doc"/>, of the same name, hides.</summary>
        public class Doc
        {
            [VectorStoreKey]
            public string Id { get; set; } = string.Empty;
        }
    }

    /// <summary>The second holder.</summary>
    public static class Second
    {
        /// <summary>An <c>Id</c> nested here.</summary>
        public enum Id { None }

        /// <summary>A record whose unmarked <c>new</c> member hides the key of <see cref="First.Doc"/>.</summary>
        public sealed class Doc : First.Doc
        {
            public new string Id { get; set; } = string.Empty;

            [VectorStoreVector(3)]
            public ReadOnlyMemory<float> Embedding { get; set; }
        }
    }
}
#pragma warning restore CA1034

/// <summary>A vector property typed as an array of vectors, which the store refuses.</summary>
public sealed class ArrayVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    // CA1819: the array-typed property is what the refusal names.
#pragma warning disable CA1819
    [VectorStoreVector(3)]
    public ReadOnlyMemory<float>[] Embedding { get; set; } = [];
#pragma warning restore CA1819
}

/// <summary>A property typed as a function pointer, the one type reflection gives an empty name.</summary>
// S6640: a function-pointer type can only be declared in unsafe code, and only its type is read.
#pragma warning disable S6640
public static unsafe class FunctionPointerVector
#pragma warning restore S6640
{
    /// <summary>Never called: only its type is read.</summary>
    public static delegate*<void> Embedding => null;

    /// <summary>Never called: an unmanaged function pointer of the same signature, whose text reads alike.</summary>
    public static delegate* unmanaged<void> Unmanaged => null;
}

/// <summary>A well-formed record, searched with a value that is not a vector.</summary>
public sealed class PlainVectorRecord
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A key whose text cannot be written.</summary>
public sealed class UnprintableKey
{
    // CA1065, S3877: a ToString that throws is the case under test, a key whose text cannot be written.
#pragma warning disable CA1065, S3877
    /// <inheritdoc />
    public override string ToString() => throw new InvalidOperationException("No text.");
#pragma warning restore CA1065, S3877
}

/// <summary>A record keyed by <see cref="UnprintableKey"/>.</summary>
public sealed class UnprintablyKeyed
{
    [VectorStoreKey]
    public UnprintableKey Id { get; set; } = new();

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A key whose text is null.</summary>
public sealed class SilentKey
{
    /// <inheritdoc />
    public override string? ToString() => null;
}

/// <summary>A record keyed by <see cref="SilentKey"/>.</summary>
public sealed class SilentlyKeyed
{
    [VectorStoreKey]
    public SilentKey Id { get; set; } = new();

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

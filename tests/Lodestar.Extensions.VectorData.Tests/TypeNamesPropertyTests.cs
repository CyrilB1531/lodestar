using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

// CA5394: a seeded Random draws reproducible cases, not anything security-sensitive.
#pragma warning disable CA5394

/// <summary>
/// The invariant <see cref="TypeNames.Names(Type[])"/> holds whatever it is given: distinct types print as distinct texts.
/// Cases are drawn from homonyms on purpose, across same-named assemblies, a second load context, namespaces, nesting
/// and generics, so most draws need a longer form than the short name. A member printed beside them, and function pointers, read apart too.
/// </summary>
public sealed class TypeNamesPropertyTests
{
    private const int Draws = 20000;

    [Fact]
    public void Distinct_types_always_print_as_distinct_texts()
    {
        var context = new AssemblyLoadContext("homonyms", isCollectible: true);
        try
        {
            Pool pool = Pool.Build(context);
            var random = new Random(1805);
            for (int draw = 0; draw < Draws; draw++)
            {
                // One family of names a draw, so its types collide often enough to need every level.
                Type[] family = pool.Family(random);
                Type[] drawn = [.. Enumerable.Range(0, random.Next(2, 7)).Select(_ => pool.Top(random, family)).Distinct()];

                // A third of the draws print a member Id of a type named as a namespace of the family is.
                bool mentions = random.Next(3) == 0;
                Type[] types = mentions ? [pool.Owner(random), .. drawn] : drawn;
                RefusalName[] named = mentions ? TypeNames.Names(types, 0, "Id") : TypeNames.Names(types);
                string[] names = [.. named.Select(name => name.ToString()), .. mentions ? [named[0].Member("Id")] : Array.Empty<string>()];

                Assert.True(
                    names.Distinct(StringComparer.Ordinal).Count() == names.Length,
                    $"Draw {draw} printed two of its types alike: {string.Join(" | ", names)}");
            }
        }
        finally
        {
            context.Unload();
        }
    }

    /// <summary>The leaves and generic shapes the draws are built from.</summary>
    private sealed class Pool
    {
        private readonly List<Type> _leaves = [];
        private readonly List<Type> _generics = [];
        private readonly List<Type> _owners = [];

        internal static Pool Build(AssemblyLoadContext context)
        {
            var pool = new Pool();

            // Five emitted assemblies, four of them named alike, each declaring the same names.
            foreach (string assembly in new[] { "Twin", "Twin", "Twin", "Twin", "Other" })
            {
                ModuleBuilder module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assembly), AssemblyBuilderAccess.RunAndCollect)
                    .DefineDynamicModule(assembly);
                pool._leaves.Add(module.DefineType("Id", TypeAttributes.Public).CreateType());
                pool._owners.Add(module.DefineType("N", TypeAttributes.Public).CreateType());
                pool._leaves.Add(module.DefineType("N.Id", TypeAttributes.Public).CreateType());
                TypeBuilder outer = module.DefineType("N.Outer", TypeAttributes.Public);
                Type nested = outer.DefineNestedType("Id", TypeAttributes.NestedPublic).CreateType();
                outer.CreateType();
                pool._leaves.Add(nested);
                pool._leaves.Add(module.DefineType("N.Outer+Id", TypeAttributes.Public).CreateType());
                TypeBuilder box = module.DefineType("N.Box`1", TypeAttributes.Public);
                box.DefineGenericParameters("T");
                pool._generics.Add(box.CreateType());
            }

            // The fixtures of this assembly, as loaded by default and as loaded a second time in another context.
            Assembly again = context.LoadFromAssemblyPath(typeof(SameName).Assembly.Location);
            foreach (Assembly assembly in new[] { typeof(SameName).Assembly, again })
            {
                pool._leaves.Add(assembly.GetType(typeof(SameName.First.Id).FullName!, throwOnError: true)!);
                pool._leaves.Add(assembly.GetType(typeof(SameName.Second.Id).FullName!, throwOnError: true)!);
                pool._leaves.Add(assembly.GetType(typeof(SameName.T).FullName!, throwOnError: true)!);
                pool._generics.Add(assembly.GetType(typeof(GenericOuter<>.Record).FullName!, throwOnError: true)!);
            }

            pool._leaves.Add(typeof(int));
            pool._generics.Add(typeof(List<>));
            pool._generics.Add(typeof(GenericOuter<>.Inner<>));
            pool._generics.Add(typeof(Dictionary<,>));
            return pool;
        }

        // The leaves of one name, whatever their namespace, declaring type, assembly or context, or, every other draw,
        // of one full name, which only an assembly, a context or a rank tells apart.
        internal Type[] Family(Random random)
        {
            Func<Type, string> key = random.Next(2) == 0 ? leaf => leaf.Name : leaf => leaf.FullName ?? leaf.Name;
            string[] names = [.. _leaves.Select(key).Distinct(StringComparer.Ordinal)];
            string name = names[random.Next(names.Length)];
            return [.. _leaves.Where(leaf => string.Equals(key(leaf), name, StringComparison.Ordinal))];
        }

        internal Type Owner(Random random) => _owners[random.Next(_owners.Count)];

        // What a refusal may name: a drawn type, a by-ref or pointer to one, or an open definition.
        internal Type Top(Random random, Type[] family) => random.Next(10) switch
        {
            0 => Draw(random, family, depth: 2).MakeByRefType(),
            1 => Draw(random, family, depth: 2).MakePointerType(),
            2 => _generics[random.Next(_generics.Count)],
            3 => FunctionPointers[random.Next(FunctionPointers.Length)],
            _ => Draw(random, family, depth: 2),
        };

        // Two function pointers whose texts read alike, which no generic may take as an argument.
        private static readonly Type[] FunctionPointers =
        [
            typeof(FunctionPointerVector).GetProperty(nameof(FunctionPointerVector.Embedding))!.PropertyType,
            typeof(FunctionPointerVector).GetProperty(nameof(FunctionPointerVector.Unmanaged))!.PropertyType,
        ];

        internal Type Draw(Random random, Type[] family, int depth)
        {
            int shape = depth == 0 ? 0 : random.Next(6);
            return shape switch
            {
                1 => Draw(random, family, depth - 1).MakeArrayType(),
                2 => Draw(random, family, depth - 1).MakeArrayType(random.Next(1, 3)),
                3 or 4 => Close(_generics[random.Next(_generics.Count)], random, family, depth),
                _ => family[random.Next(family.Length)],
            };
        }

        private Type Close(Type generic, Random random, Type[] family, int depth) =>
            generic.MakeGenericType([.. generic.GetGenericArguments().Select(_ => Draw(random, family, depth - 1))]);
    }
}
#pragma warning restore CA5394

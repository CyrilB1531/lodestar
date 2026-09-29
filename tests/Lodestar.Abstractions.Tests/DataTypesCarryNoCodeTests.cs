using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Lodestar.Cluster;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>Decision 0003(e) on the assembly as compiled: no code in a data type — no logic, no validation (#1381 to #1386).</summary>
/// <remarks>
/// Read from the IL, as 0003 says it is: no method, accessor or non-public member the compiler did not write but a
/// structural <c>Equals</c>/<c>GetHashCode</c>, whose own IL is read too (#1418); no nested type; no interface body;
/// constructors that only load, build the values their defaults name, call their base constructor and store. Every
/// namespace is scanned, a polyfill the compiler or PolySharp wrote aside (#1419). The shared helpers are named below.
/// </remarks>
public sealed class DataTypesCarryNoCodeTests
{
    private const BindingFlags Declared =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    /// <summary>The sparse primitive 0003 names by exception, code included.</summary>
    private static readonly HashSet<string> SparsePrimitive = new(StringComparer.Ordinal)
    {
        "Lodestar.Abstractions.CsrMatrix",
        "Lodestar.Abstractions.SparseNorm",
    };

    /// <summary>The shared helpers compiled in: ValueEquality for the records, the rest for the sparse primitive.</summary>
    private static readonly HashSet<string> SharedHelpers = new(StringComparer.Ordinal)
    {
        "Lodestar.Internal.ElementWise",
        "Lodestar.Internal.Guard",
        "Lodestar.Internal.ValueEquality",
    };

    /// <summary>What a record's compiler writes, not every member of it marked as generated.</summary>
    private static readonly HashSet<string> RecordMembers = new(StringComparer.Ordinal)
    {
        "get_EqualityContract", "PrintMembers", "ToString", "op_Equality", "op_Inequality", "<Clone>$", "Deconstruct",
    };

    /// <summary>The instructions a constructor that only stores is made of.</summary>
    private static readonly HashSet<string> StoringOpCodes = new(StringComparer.Ordinal)
    {
        "nop", "ret", "dup", "pop", "ldloc", "ldloc.s", "ldloc.0", "ldloc.1", "ldloc.2", "ldloc.3", "ldloca",
        "ldloca.s", "stloc", "stloc.s", "stloc.0", "stloc.1", "stloc.2", "stloc.3",
        "ldarg", "ldarg.s", "ldarg.0", "ldarg.1", "ldarg.2", "ldarg.3", "ldarga", "ldarga.s",
        "ldc.i4", "ldc.i4.s", "ldc.i4.m1", "ldc.i4.0", "ldc.i4.1", "ldc.i4.2", "ldc.i4.3", "ldc.i4.4",
        "ldc.i4.5", "ldc.i4.6", "ldc.i4.7", "ldc.i4.8", "ldc.i8", "ldc.r4", "ldc.r8", "ldnull", "ldstr",
        "ldtoken", "ldsfld", "ldfld", "stfld", "stsfld", "initobj", "newarr", "stelem", "stelem.ref",
        "stelem.i1", "stelem.i2", "stelem.i4", "stelem.i8", "stelem.r4", "stelem.r8", "ldflda", "conv.i8", "conv.r4",
        "conv.r8", "call", "newobj",
    };

    /// <summary>
    /// The instructions structural equality never needs: arithmetic but combining, ordering by comparison or by branch,
    /// a float constant or conversion, a store, a throw, an allocation (#1482). <c>cgt.un</c> stays allowed: it is how C#
    /// compiles <c>x is not null</c>. The scan reads opcodes and callees, not dataflow: a float product of two fields passes.
    /// </summary>
    private static readonly HashSet<string> NonStructuralOpCodes = new(StringComparer.Ordinal)
    {
        "sub", "sub.ovf", "sub.ovf.un", "div", "div.un", "rem", "rem.un", "neg", "clt", "clt.un", "cgt",
        "blt", "blt.s", "blt.un", "blt.un.s", "bgt", "bgt.s", "bgt.un", "bgt.un.s",
        "ble", "ble.s", "ble.un", "ble.un.s", "bge", "bge.s", "bge.un", "bge.un.s",
        "ldc.r4", "ldc.r8", "conv.r4", "conv.r8", "conv.r.un",
        "stfld", "stsfld", "stobj", "stelem", "stelem.ref", "stelem.i", "stelem.i1", "stelem.i2", "stelem.i4",
        "stelem.i8", "stelem.r4", "stelem.r8", "stind.ref", "stind.i", "stind.i1", "stind.i2", "stind.i4",
        "stind.i8", "stind.r4", "stind.r8",
        "throw", "rethrow", "newobj", "newarr", "localloc", "calli",
    };

    /// <summary>
    /// The members a structural <c>Equals</c>/<c>GetHashCode</c> may call, by declaring type, beside the data types' own
    /// accessors and equality; <see langword="null"/> admits every member of the shared equality helper (#1482).
    /// </summary>
    private static readonly Dictionary<string, HashSet<string>?> EqualityCallees = new(StringComparer.Ordinal)
    {
        ["System.Object"] = ["Equals", "GetHashCode", "ReferenceEquals", "GetType"],
        ["System.Double"] = ["Equals", "GetHashCode", "IsNaN"],
        ["System.Single"] = ["Equals", "GetHashCode", "IsNaN"],
        ["System.Int32"] = ["Equals", "GetHashCode"],
        ["System.Int64"] = ["Equals", "GetHashCode"],
        ["System.Boolean"] = ["Equals", "GetHashCode"],
        ["System.String"] = ["Equals", "GetHashCode", "op_Equality", "op_Inequality"],
        ["System.Type"] = ["op_Equality", "op_Inequality", "GetTypeFromHandle"],
        ["System.HashCode"] = ["Add", "ToHashCode", "Combine"],
        ["System.Nullable`1"] = ["get_HasValue", "get_Value", "GetValueOrDefault", "Equals", "GetHashCode"],
        ["System.Linq.Enumerable"] = ["SequenceEqual"],
        ["System.Collections.Generic.EqualityComparer`1"] = ["get_Default", "Equals", "GetHashCode"],
        ["System.Collections.Generic.IReadOnlyList`1"] = ["get_Item"],
        ["System.Collections.Generic.IReadOnlyCollection`1"] = ["get_Count"],
        ["System.Collections.Generic.IReadOnlyDictionary`2"] = ["TryGetValue", "ContainsKey", "get_Item"],
        ["System.Collections.Generic.IEnumerable`1"] = ["GetEnumerator"],
        ["System.Collections.Generic.IEnumerator`1"] = ["get_Current"],
        ["System.Collections.IEnumerator"] = ["MoveNext"],
        ["System.IDisposable"] = ["Dispose"],
        ["System.Collections.Generic.KeyValuePair`2"] = ["get_Key", "get_Value"],
        ["System.StringComparer"] = ["get_Ordinal", "Equals", "GetHashCode"],
        ["System.ReadOnlyMemory`1"] = ["get_Span", "get_Length", "get_IsEmpty"],
        ["System.ReadOnlySpan`1"] = ["get_Length", "get_Item", "get_IsEmpty"],
        ["System.MemoryExtensions"] = ["SequenceEqual"],
        ["Lodestar.Internal.ValueEquality"] = null,
    };

    private static readonly Dictionary<short, OpCode> OpCodesByValue = typeof(OpCodes)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => (OpCode)field.GetValue(null)!)
        .ToDictionary(code => code.Value);

    private static Assembly Abstractions => typeof(KMeansOptions).Assembly;

    [Fact]
    public void No_data_type_carries_code()
    {
        List<string> offenders = [];
        foreach (Type type in DataTypes())
        {
            offenders.AddRange(Offences(type));
        }

        Assert.Empty(offenders);
    }

    [Fact]
    public void The_shared_helpers_compiled_in_are_exactly_the_named_ones()
    {
        HashSet<string> compiled = Abstractions.GetTypes()
            .Where(type => type.Namespace == "Lodestar.Internal" && !type.IsNested && !IsCompilerGenerated(type))
            .Select(type => type.FullName!)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Equal(SharedHelpers.Order(StringComparer.Ordinal), compiled.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void The_scan_sees_the_code_it_is_meant_to_refuse()
    {
        List<string> offences = [.. Offences(typeof(Carrier))];

        Assert.Contains(offences, offence => offence.Contains("g__", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith(".Total", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith(".ToString", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith(".Cache", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith("..ctor(1)", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith("..ctor(2)", StringComparison.Ordinal));
        Assert.Contains(offences, offence => offence.EndsWith("..ctor(3)", StringComparison.Ordinal));
        Assert.False(OnlyStores(typeof(Named).GetConstructors().Single()));
        Assert.Contains(Offences(typeof(IDefaulted)), offence => offence.EndsWith(".get_Count", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Printed)), offence => offence.EndsWith(".ToString", StringComparison.Ordinal));
        Assert.Empty(Offences(typeof(KMeansOptions)));
        Assert.Equal(4, new Carrier(2).Total());
        Assert.Equal(3, new Carrier(1, 2).Value);
        Assert.Equal(6, new Carrier(2, 3, "six").Value);
        Assert.Null(new Carrier(1) { Cache = null }.Cache);
        Assert.Equal("n", new Named("n").Name);
        Assert.Equal("1", new Printed(1).ToString());
    }

    [Fact]
    public void Every_type_outside_Lodestar_is_one_the_compiler_or_a_polyfill_wrote()
    {
        string[] strays = [.. Abstractions.GetTypes()
            .Where(type => !type.IsNested && !IsLodestars(type) && !IsWrittenForUs(type))
            .Select(type => type.FullName!)];

        Assert.Empty(strays);
    }

    [Fact]
    public void The_scan_refuses_an_equality_that_is_not_structural()
    {
        Assert.Contains(Offences(typeof(Tolerant)), offence => offence.EndsWith(".Equals", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Refusing)), offence => offence.EndsWith(".Equals", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Hasher)), offence => offence.EndsWith(".GetHashCode", StringComparison.Ordinal));
        // Each of these trips one check alone: an allowed callee with a forbidden opcode, or the reverse (#1483).
        Assert.Contains(Offences(typeof(Ordered)), offence => offence.EndsWith(".Equals", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Cached)), offence => offence.EndsWith(".GetHashCode", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Folding)), offence => offence.EndsWith(".Equals", StringComparison.Ordinal));
        Assert.Contains(Offences(typeof(Summed)), offence => offence.EndsWith(".Equals", StringComparison.Ordinal));
        Assert.True(IsLodestars(typeof(Generated)));
        Assert.False(new Ordered(1.0).Equals(new Ordered(2.0)));
        Assert.True(new Ordered(2.0).Equals(new Ordered(1.0)));
        var cached = new Cached(3);
        Assert.Equal(cached.GetHashCode(), cached.Hash);
        Assert.True(new Folding("A").Equals(new Folding("a")));
        Assert.True(new Summed([1.0, 2.0]).Equals(new Summed([3.0])));
        Assert.Equal(1, new Generated(1).Twice() / 2);
        Assert.True(new Tolerant(1.0).Equals(new Tolerant(1.0)));
        Assert.Throws<InvalidOperationException>(() => new Refusing().Equals(null));
        Assert.Equal(2, Hasher.GetHashCode([1.0, 2.0]));
    }

    private static IEnumerable<Type> DataTypes() =>
        Abstractions.GetTypes().Where(type => IsLodestars(type)
            && type.Namespace != "Lodestar.Internal"
            && !type.IsNested
            && !SparsePrimitive.Contains(type.FullName!)
            && !type.IsEnum);

    /// <summary>
    /// A type in a namespace of Lodestar's, the root and the global namespace included (#1419), whatever attribute it
    /// carries: only a compiler-named type (<c>&lt;Module&gt;</c>, <c>&lt;PrivateImplementationDetails&gt;</c>) is left out (#1481).
    /// </summary>
    private static bool IsLodestars(Type type) =>
        !type.Name.StartsWith('<')
        && (type.Namespace is null or "Lodestar" || type.Namespace.StartsWith("Lodestar.", StringComparison.Ordinal));

    /// <summary>
    /// Outside Lodestar's namespaces: a compiler-named or compiler-generated type, a polyfill PolySharp writes into the
    /// netstandard2.0 build, which it marks <c>[Microsoft.CodeAnalysis.Embedded]</c>, or the tracker Microsoft Code
    /// Coverage injects under its instrumentation namespace when CI collects coverage (#1481).
    /// </summary>
    private static bool IsWrittenForUs(Type type) =>
        type.Name.StartsWith('<')
        || IsCompilerGenerated(type)
        || (type.Namespace?.StartsWith("Microsoft.CodeCoverage.Instrumentation", StringComparison.Ordinal) ?? false)
        || type.GetCustomAttributesData().Any(attribute =>
            attribute.AttributeType.FullName == "Microsoft.CodeAnalysis.EmbeddedAttribute");

    private static IEnumerable<string> Offences(Type type) =>
        type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).Select(nested => nested.FullName!)
            .Concat(type.IsInterface ? InterfaceOffences(type) : MemberOffences(type).Concat(ConstructorOffences(type)));

    /// <summary>An interface member may be abstract; none may have a body, a static one included (#1381).</summary>
    private static IEnumerable<string> InterfaceOffences(Type type) =>
        type.GetMethods(Declared).Where(method => !method.IsAbstract).Select(method => method.Name)
            .Concat(type.GetFields(Declared).Select(field => field.Name))
            .Concat(type.GetConstructors(Declared).Select(_ => ".cctor"))
            .Select(name => $"{type.FullName}.{name}");

    private static IEnumerable<string> MemberOffences(Type type)
    {
        bool record = IsRecord(type);
        IEnumerable<string> fields = type.GetFields(Declared)
            .Where(field => !field.IsPublic && !field.Name.EndsWith("k__BackingField", StringComparison.Ordinal))
            .Select(field => field.Name);

        // A non-public auto-property's accessors are compiler-written, and it is still a non-public member (#1385).
        IEnumerable<string> properties = type.GetProperties(Declared)
            .Where(property => property.GetMethod?.IsPublic != true && property.SetMethod?.IsPublic != true
                && !(record && property.Name == "EqualityContract"))
            .Select(property => property.Name);
        IEnumerable<string> methods = type.GetMethods(Declared)
            .Where(method => !IsAllowedMethod(method))
            .Select(method => method.Name);

        return fields.Concat(properties).Concat(methods).Select(name => $"{type.FullName}.{name}");
    }

    /// <summary>A constructor is public, or a record's copy constructor, or static, and only stores.</summary>
    private static IEnumerable<string> ConstructorOffences(Type type) =>
        type.GetConstructors(Declared)
            .Where(constructor => (!constructor.IsPublic && !constructor.IsStatic && !IsCopyConstructor(constructor))
                || !OnlyStores(constructor))
            .Select(constructor => $"{type.FullName}..ctor({constructor.GetParameters().Length})");

    /// <summary>A record class, known by the compiler-written <c>&lt;Clone&gt;$</c> no hand can declare under that name.</summary>
    private static bool IsRecord(Type type) =>
        type.GetMethods(Declared).Any(method => method.Name == "<Clone>$" && IsCompilerGenerated(method));

    private static bool IsCopyConstructor(ConstructorInfo constructor) =>
        IsRecord(constructor.DeclaringType!)
        && constructor.GetParameters() is [{ } only]
        && only.ParameterType == constructor.DeclaringType;

    private static bool IsAllowedMethod(MethodInfo method)
    {
        if (method.Name is nameof(Equals) or nameof(GetHashCode))
        {
            return IsStructuralEquality(method);
        }

        // A local function or lambda compiles to a '<'-named method on the type itself: never allowed.
        if (method.Name.Contains('<', StringComparison.Ordinal))
        {
            return method.Name == "<Clone>$" && IsCompilerGenerated(method);
        }

        // A record's own members as its compiler wrote them, a hand-written one refused (#1384); an auto-accessor,
        // public, anywhere.
        return (RecordMembers.Contains(method.Name) && IsCompilerGenerated(method))
            || (method.IsPublic && method.IsSpecialName && IsCompilerGenerated(method));
    }

    /// <summary>
    /// A public instance <c>Equals(object)</c>, <c>Equals</c> of this type or a base, or <c>GetHashCode()</c>, whose IL
    /// compares and combines what it reads and nothing else: no arithmetic beyond combining a hash, no ordering, no float
    /// constant, no throw, no allocation, and calls only into the types <see cref="EqualityCallees"/> names or this
    /// assembly's own data types (#1418).
    /// </summary>
    private static bool IsStructuralEquality(MethodInfo method)
    {
        Type type = method.DeclaringType!;
        ParameterInfo[] parameters = method.GetParameters();
        bool shaped = method.IsPublic && !method.IsStatic && (method.Name == nameof(GetHashCode)
            ? parameters.Length == 0 && method.ReturnType == typeof(int)
            : parameters is [{ } other] && method.ReturnType == typeof(bool)
                && (other.ParameterType == typeof(object) || other.ParameterType.IsAssignableFrom(type)));
        if (!shaped)
        {
            return false;
        }

        byte[] il = method.GetMethodBody()?.GetILAsByteArray() ?? [];
        OpCode previous = OpCodes.Nop;
        for (int at = 0; at < il.Length;)
        {
            OpCode code = Decode(il, at);
            int operandAt = at + code.Size;
            if (IsCoverageProbe(method, il, at, out int probeEnd))
            {
                at = probeEnd;
                continue;
            }

            if (NonStructuralOpCodes.Contains(code.Name!)
                || (code.OperandType == OperandType.InlineMethod
                    && !IsEqualityCallee(method, Resolve(method, BitConverter.ToInt32(il, operandAt)), previous)))
            {
                return false;
            }

            previous = code;
            at = operandAt + OperandSize(code, il, operandAt);
        }

        return true;
    }

    /// <summary>
    /// A callee <see cref="EqualityCallees"/> admits, a getter of the method's own type, or a data type's accessor or
    /// equality; <c>string.Equals</c> with a comparison only under <c>Ordinal</c>, the constant loaded just before it.
    /// </summary>
    private static bool IsEqualityCallee(MethodBase method, MethodBase callee, OpCode previous)
    {
        Type? declaring = callee.DeclaringType;
        if (declaring is null)
        {
            return false;
        }

        if (declaring == method.DeclaringType && callee.Name.StartsWith("get_", StringComparison.Ordinal))
        {
            return true;
        }

        string name = (declaring.IsGenericType ? declaring.GetGenericTypeDefinition() : declaring).FullName ?? string.Empty;
        // String.Equals with a StringComparison compares case-folded under anything but Ordinal, which is 4.
        if (name == "System.String" && callee.GetParameters().Any(parameter => parameter.ParameterType == typeof(StringComparison)))
        {
            return callee.Name == nameof(Equals) && previous == OpCodes.Ldc_I4_4 && (int)StringComparison.Ordinal == 4;
        }

        return (EqualityCallees.TryGetValue(name, out HashSet<string>? members) && (members is null || members.Contains(callee.Name)))
            || (name.StartsWith("System.ValueTuple`", StringComparison.Ordinal) && callee.Name is nameof(Equals) or nameof(GetHashCode))
            || (declaring.Assembly == Abstractions && !SparsePrimitive.Contains(name) && declaring.Namespace != "Lodestar.Internal"
                && (callee.Name is nameof(Equals) or nameof(GetHashCode) or "op_Equality" or "op_Inequality"
                    || callee.Name.StartsWith("get_", StringComparison.Ordinal)));
    }

    /// <summary>Whether a constructor only stores what it is given or what the type's defaults name.</summary>
    private static bool OnlyStores(ConstructorInfo constructor)
    {
        byte[] il = constructor.GetMethodBody()?.GetILAsByteArray() ?? [];
        for (int at = 0; at < il.Length;)
        {
            OpCode code = Decode(il, at);
            int operandAt = at + code.Size;
            if (IsCoverageProbe(constructor, il, at, out int probeEnd))
            {
                at = probeEnd;
                continue;
            }

            if (!StoringOpCodes.Contains(code.Name!))
            {
                return false;
            }

            if (code.OperandType == OperandType.InlineMethod
                && !IsStoringCall(constructor, Resolve(constructor, BitConverter.ToInt32(il, operandAt))))
            {
                return false;
            }

            at = operandAt + OperandSize(code, il, operandAt);
        }

        return true;
    }

    private static OpCode Decode(byte[] il, int at) =>
        il[at] == 0xFE ? OpCodesByValue[unchecked((short)(0xFE00 | il[at + 1]))] : OpCodesByValue[il[at]];

    /// <summary>
    /// Whether <paramref name="at"/> starts the hit counter Microsoft Code Coverage writes into every block when CI
    /// collects coverage: <c>ldsfld Tracker::Begin; ldc.i4 n; add; ldc.i4.1; stind.i1</c>, recognised whole and nothing
    /// else, so the scan reads the constructor the compiler wrote.
    /// </summary>
    private static bool IsCoverageProbe(MethodBase constructor, byte[] il, int at, out int end)
    {
        end = at;
        OpCode first = Decode(il, at);
        if (first != OpCodes.Ldsfld
            || constructor.Module.ResolveField(BitConverter.ToInt32(il, at + first.Size))?.DeclaringType?.Namespace is not { } space
            || !space.StartsWith("Microsoft.CodeCoverage", StringComparison.Ordinal))
        {
            return false;
        }

        int next = at + first.Size + 4;
        foreach (Func<OpCode, bool> expected in ProbeTail)
        {
            if (next >= il.Length || !expected(Decode(il, next)))
            {
                return false;
            }

            OpCode step = Decode(il, next);
            next += step.Size + OperandSize(step, il, next + step.Size);
        }

        end = next;
        return true;
    }

    /// <summary>The four instructions after the tracker's <c>ldsfld</c>: the block's offset, its sum, the flag and its store.</summary>
    private static readonly Func<OpCode, bool>[] ProbeTail =
    [
        code => code.Name!.StartsWith("ldc.i4", StringComparison.Ordinal),
        code => code == OpCodes.Add,
        code => code == OpCodes.Ldc_I4_1,
        code => code == OpCodes.Stind_I1,
    ];

    /// <summary>
    /// The calls a storing constructor makes: its base constructor; a constructor of a value its defaults name — a
    /// data type of this assembly, itself scanned, an empty collection, a nullable or a tuple; a compiler-written
    /// getter; <c>Array.Empty</c> and <c>RuntimeHelpers.InitializeArray</c> (#1382, #1383).
    /// </summary>
    private static bool IsStoringCall(ConstructorInfo caller, MethodBase callee)
    {
        Type? declaring = callee.DeclaringType;
        if (declaring is null)
        {
            return false;
        }

        if (callee is ConstructorInfo constructor)
        {
            return declaring == caller.DeclaringType!.BaseType
                || (declaring.Assembly == Abstractions && !SparsePrimitive.Contains(declaring.FullName ?? string.Empty))
                || (declaring.Namespace == "System.Collections.Generic" && constructor.GetParameters().Length == 0)
                || (declaring.IsGenericType && declaring.GetGenericTypeDefinition() == typeof(Nullable<>))
                || (declaring.FullName ?? string.Empty).StartsWith("System.ValueTuple`", StringComparison.Ordinal);
        }

        return (callee.Name.StartsWith("get_", StringComparison.Ordinal) && IsCompilerGenerated(callee))
            || callee is MethodInfo { Name: "Empty", DeclaringType.FullName: "System.Array" }
            || callee is MethodInfo { Name: "InitializeArray", DeclaringType.FullName: "System.Runtime.CompilerServices.RuntimeHelpers" };
    }

    /// <summary>A method token read in its method's generic context, so a generic data type does not throw.</summary>
    private static MethodBase Resolve(MethodBase constructor, int token)
    {
        Type declaring = constructor.DeclaringType!;
        return constructor.Module.ResolveMethod(
            token, declaring.IsGenericType ? declaring.GetGenericArguments() : null, null)!;
    }

    private static bool IsCompilerGenerated(MemberInfo member) =>
        member.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);

    private static int OperandSize(OpCode code, byte[] il, int at) => code.OperandType switch
    {
        OperandType.InlineNone => 0,
        OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
        OperandType.InlineVar => 2,
        OperandType.InlineI8 or OperandType.InlineR => 8,
        OperandType.InlineSwitch => 4 + (4 * BitConverter.ToInt32(il, at)),
        _ => 4,
    };

    /// <summary>An interface with a default body, which the scan must refuse.</summary>
    private interface IDefaulted
    {
        IReadOnlyList<int> Items { get; }

        int Count => Items.Count;
    }

    /// <summary>A constructor that validates through the BCL alone: no branch and no throw in its own IL.</summary>
    private sealed class Named
    {
        public Named(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Name = name;
        }

        public string Name { get; }
    }

    /// <summary>A record whose <c>ToString</c> is written by hand.</summary>
    private sealed record Printed(int Value)
    {
        public override string ToString() => $"{Value}";
    }

    /// <summary>An <c>Equals</c> that compares within a tolerance: numerical logic, not structure.</summary>
    private sealed class Tolerant(double value)
    {
        public double Value { get; } = value;

        public override bool Equals(object? obj) => obj is Tolerant other && Math.Abs(Value - other.Value) < 1e-9;

        public override int GetHashCode() => 0;
    }

    /// <summary>An <c>Equals</c> that validates by throwing.</summary>
    private sealed class Refusing
    {
        // S3877, CA1065: the throw is the very thing this probe exists to put in front of the scan.
#pragma warning disable S3877, CA1065
        public override bool Equals(object? obj) => obj is null ? throw new InvalidOperationException() : ReferenceEquals(this, obj);
#pragma warning restore S3877, CA1065

        public override int GetHashCode() => 0;
    }

    /// <summary>An <c>Equals</c> that orders through a branch, calling only its own getter.</summary>
    private sealed class Ordered(double value)
    {
        public double Value { get; } = value;

        public override bool Equals(object? obj)
        {
            if (obj is not Ordered other || Value < other.Value)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode() => 0;
    }

    /// <summary>A <c>GetHashCode</c> that writes a field, through allowed callees only.</summary>
    private sealed class Cached(int value)
    {
        // S1104, CA1051: the public field is the store this probe exists to put in front of the scan.
#pragma warning disable S1104, CA1051
        public int Hash;
#pragma warning restore S1104, CA1051

        public int Value { get; } = value;

        public override bool Equals(object? obj) => obj is Cached other && Value == other.Value;

        public override int GetHashCode() => Hash = Value.GetHashCode();
    }

    /// <summary>An <c>Equals</c> that folds case, through a String member the list refuses by signature.</summary>
    private sealed class Folding(string name)
    {
        public string Name { get; } = name;

        public override bool Equals(object? obj) =>
            obj is Folding other && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => 0;
    }

    /// <summary>An <c>Equals</c> that compares sums, through a Linq member the list does not name.</summary>
    private sealed class Summed(double[] values)
    {
        public IReadOnlyList<double> Values { get; } = values;

        public override bool Equals(object? obj) => obj is Summed other && Values.Sum().Equals(other.Values.Sum());

        public override int GetHashCode() => 0;
    }

    /// <summary>A Lodestar type that claims generated code: scanned all the same (#1481).</summary>
    [System.CodeDom.Compiler.GeneratedCode("probe", "1")]
    private sealed class Generated(int value)
    {
        public int Twice() => value * 2;
    }

    /// <summary>A static utility that only borrows the name <c>GetHashCode</c>.</summary>
    private static class Hasher
    {
        public static int GetHashCode(double[] values) => values.Length;
    }

    /// <summary>A type with each kind of code the scan must see.</summary>
    private sealed class Carrier
    {
        public Carrier(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("A carrier holds no negative value.", nameof(value));
            }

            Value = value;
        }

        public Carrier(int left, int right) => Value = left + right;

        public Carrier(int value, int scale, string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Value = value * scale;
        }

        public int Value { get; }

        internal double[]? Cache { get; set; }

        public int Total() => Twice(Value);

        public override string ToString() => $"{Value}";

        public override bool Equals(object? obj) => obj is Carrier other && Local(other.Value) == Value;

        public override int GetHashCode() => Value;

        private static int Twice(int value) => value * 2;

        private static int Local(int value)
        {
            return Identity(value);

            static int Identity(int x) => x;
        }
    }
}

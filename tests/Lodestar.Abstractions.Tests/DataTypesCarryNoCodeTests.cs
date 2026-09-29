using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Lodestar.Cluster;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>
/// Decisions 0003(e) and 0010 on the assembly as compiled: a data type carries no code the compiler did not write, but for its
/// structural <c>Equals</c> and <c>GetHashCode</c> (#1365).
/// </summary>
/// <remarks>
/// Read from the IL, as 0003 says it is: no method, accessor or non-public member of its own, no nested type such as
/// a closure, and constructors that only store — no branch, no throw, no call into Lodestar but a base constructor,
/// a constructor or a property getter, which a record's defaults reach.
/// </remarks>
public sealed class DataTypesCarryNoCodeTests
{
    private const BindingFlags Declared =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    /// <summary>The sparse primitive decision 0003 names; 0010 admits the constructors that only store, so no other.</summary>
    private static readonly HashSet<string> Exempt = new(StringComparer.Ordinal)
    {
        "Lodestar.Abstractions.CsrMatrix",
    };

    /// <summary>What a record's compiler writes without marking every one: its contract, printer and operators.</summary>
    private static readonly HashSet<string> RecordMembers = new(StringComparer.Ordinal)
    {
        "get_EqualityContract", "PrintMembers", "ToString", "op_Equality", "op_Inequality", "<Clone>$", "Deconstruct",
    };

    private static readonly Dictionary<short, OpCode> OpCodesByValue = typeof(OpCodes)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => (OpCode)field.GetValue(null)!)
        .ToDictionary(code => code.Value);

    [Fact]
    public void No_data_type_carries_code_the_compiler_did_not_write()
    {
        List<string> offenders = [];
        foreach (Type type in DataTypes())
        {
            offenders.AddRange(type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).Select(nested => nested.FullName!));
            offenders.AddRange(type.GetFields(Declared)
                .Where(field => !field.IsPublic && !field.Name.EndsWith("k__BackingField", StringComparison.Ordinal))
                .Select(field => $"{type.FullName}.{field.Name}"));
            offenders.AddRange(type.GetMethods(Declared)
                .Where(method => !type.IsInterface && !IsAllowedMethod(method))
                .Select(method => $"{type.FullName}.{method.Name}"));
            offenders.AddRange(type.GetConstructors(Declared)
                .Where(constructor => !OnlyStores(constructor))
                .Select(constructor => $"{type.FullName}..ctor({constructor.GetParameters().Length})"));
        }

        Assert.Empty(offenders);
    }

    [Fact]
    public void The_scan_sees_the_code_it_is_meant_to_refuse()
    {
        // Held to the same rules: a local function, a validating constructor and a method of its own.
        Assert.False(IsAllowedMethod(
            typeof(Carrier).GetMethods(Declared).Single(method => method.Name.Contains("g__", StringComparison.Ordinal))));
        Assert.False(OnlyStores(typeof(Carrier).GetConstructors().Single()));
        Assert.False(IsAllowedMethod(typeof(Carrier).GetMethod(nameof(Carrier.Total))!));
        Assert.True(OnlyStores(typeof(KMeansOptions).GetConstructors().Single(constructor => constructor.GetParameters().Length == 0)));
        Assert.Equal(4, new Carrier(2).Total());
    }

    private static IEnumerable<Type> DataTypes() =>
        typeof(KMeansOptions).Assembly.GetTypes().Where(type => type.Namespace is { } space
            // Lodestar's own: netstandard2.0 also compiles PolySharp's System.* polyfills in.
            && space.StartsWith("Lodestar.", StringComparison.Ordinal)
            && !space.StartsWith("Lodestar.Internal", StringComparison.Ordinal)
            && !type.IsNested
            && !Exempt.Contains(type.FullName!)
            && !type.IsEnum);

    private static bool IsAllowedMethod(MethodInfo method)
    {
        if (method.Name is nameof(Equals) or nameof(GetHashCode))
        {
            return true;
        }

        // A local function or lambda compiles to a '<'-named method on the type itself: never allowed.
        if (method.Name.Contains('<', StringComparison.Ordinal))
        {
            return method.Name == "<Clone>$";
        }

        return method.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false) || RecordMembers.Contains(method.Name);
    }

    /// <summary>Whether a constructor only stores what it is given or what the type's defaults name.</summary>
    private static bool OnlyStores(ConstructorInfo constructor)
    {
        byte[] il = constructor.GetMethodBody()?.GetILAsByteArray() ?? [];
        for (int at = 0; at < il.Length;)
        {
            OpCode code = il[at] == 0xFE ? OpCodesByValue[unchecked((short)(0xFE00 | il[at + 1]))] : OpCodesByValue[il[at]];
            int operandAt = at + code.Size;
            if (code.FlowControl is FlowControl.Branch or FlowControl.Cond_Branch or FlowControl.Throw)
            {
                return false;
            }

            if (code.OperandType == OperandType.InlineMethod
                && !IsStoringCall(constructor, constructor.Module.ResolveMethod(BitConverter.ToInt32(il, operandAt))!))
            {
                return false;
            }

            at = operandAt + OperandSize(code, il, operandAt);
        }

        return true;
    }

    private static bool IsStoringCall(ConstructorInfo caller, MethodBase callee) =>
        callee.DeclaringType?.Namespace is not { } space
        || !space.StartsWith("Lodestar.", StringComparison.Ordinal)
        || callee is ConstructorInfo
        || callee.Name.StartsWith("get_", StringComparison.Ordinal)
        || callee.DeclaringType == caller.DeclaringType!.BaseType;

    private static int OperandSize(OpCode code, byte[] il, int at) => code.OperandType switch
    {
        OperandType.InlineNone => 0,
        OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
        OperandType.InlineVar => 2,
        OperandType.InlineI8 or OperandType.InlineR => 8,
        OperandType.InlineSwitch => 4 + (4 * BitConverter.ToInt32(il, at)),
        _ => 4,
    };

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

        public int Value { get; }

        public int Total() => Twice(Value);

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

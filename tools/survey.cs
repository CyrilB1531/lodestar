#:package System.Reflection.MetadataLoadContext@10.0.0
// Reading an arbitrary assembly's surface cannot satisfy the trim and single-file analysers
// by construction: naming the types it reflects over is the one thing it must not do.
#:property EnableTrimAnalyzer=false
#:property EnableSingleFileAnalyzer=false
#:property EnableAotAnalyzer=false

// long-comment: what this reads, why it exists and why it has no project file.
// Reads a NuGet package's exported surface, the way decision 0074 requires a gap claim to be
// checked: against what the assembly exports, never against its README. Five decisions ran
// that protocol as a throwaway console project written again each time (#619), so the numbers
// reached the records and the means of producing them did not -- 0096 reports "336 exported
// types, 5 333 members" with no command a reviewer can re-run. A file-based app rather than a
// project, because tools/ is otherwise Python and a .csproj here is either in Lodestar.slnx
// for no reason or outside it, which is what #649 had just finished paying for.
//
//   dotnet run tools/survey.cs -- <package> <version> [regex] [--assembly <name>]
//   dotnet run tools/survey.cs -- Microsoft.ML.TimeSeries 5.0.0 'Arima|Acf|Stationar'
//   dotnet run tools/survey.cs -- Microsoft.ML 5.0.0 'Pca' --assembly Microsoft.ML.PCA

using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

// long-comment: why the assembly is nameable and not derived from the package.
// A package id is not an assembly name. Microsoft.ML 5.0.0 installs no Microsoft.ML.dll at
// all: its surface is spread over Microsoft.ML.Data, Microsoft.ML.PCA and six more, and
// Microsoft.ML.PCA is not a package id anyone can install. Deriving the target from the
// package id made that surface unreadable by this tool, found on its second use (#685).
// The error below lists the closure, so the name to pass is the one it printed.
string[] positional = [.. args.Where(a => !a.StartsWith("--", StringComparison.Ordinal))];
string? assemblyName = null;
for (int i = 0; i < args.Length - 1; i++)
{
    if (args[i] == "--assembly")
    {
        assemblyName = args[i + 1];
        positional = [.. positional.Where(a => a != assemblyName)];
    }
}

if (positional.Length is < 2 or > 3)
{
    Console.Error.WriteLine(
        "usage: dotnet run tools/survey.cs -- <package> <version> [regex] [--assembly <name>]");
    return 2;
}

string package = positional[0];
string version = positional[1];
Regex? wanted = positional.Length == 3 ? new Regex(positional[2], RegexOptions.IgnoreCase) : null;

string work = Directory.CreateTempSubdirectory("lodestar-survey-").FullName;
try
{
    // long-comment: why the package is published rather than read where it sits.
    // A bare lib/*.dll cannot be opened -- GetExportedTypes needs every assembly its
    // signatures mention -- and that trap has been paid twice, on Mosaik.Core (0074) and on
    // Microsoft.ML.TimeSeries, which drags Microsoft.ML, an MKL redistributable and
    // Newtonsoft.Json behind it. The SDK resolves the closure; doing it by hand is the
    // thing that made five readings unreproducible.
    if (!Run("dotnet", $"new console -o \"{work}\"") ||
        !Run("dotnet", $"add \"{work}\" package {package} --version {version}") ||
        !Run("dotnet", $"publish \"{work}\" -c Release -o \"{work}/out\""))
    {
        Console.Error.WriteLine($"could not resolve {package} {version}; the output above says why.");
        return 1;
    }

    string closure = Path.Combine(work, "out");
    string[] assemblies =
    [
        .. Directory.GetFiles(closure, "*.dll"),
        // The running runtime's reference assemblies, so System.* resolves without the
        // target having to carry it.
        .. Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "*.dll"),
    ];

    string target = Path.Combine(closure, (assemblyName ?? package) + ".dll");
    if (!File.Exists(target))
    {
        // A package that installs no assembly is a finding, not a blank: 0104 records
        // cs-glm 1.0.1 as exactly that.
        Console.Error.WriteLine(
            $"{assemblyName ?? package}.dll is not in the published closure. Pass one of these "
            + "to --assembly:");
        foreach (string candidate in Directory.GetFiles(closure, "*.dll").Order())
        {
            Console.Error.WriteLine($"  {Path.GetFileName(candidate)}");
        }

        return 1;
    }

    using var context = new MetadataLoadContext(new PathAssemblyResolver(assemblies));
    Assembly assembly = context.LoadFromAssemblyPath(target);
    Type[] exported = assembly.GetExportedTypes();

    // long-comment: the second trap, and why three counts are printed instead of one.
    // Type counts reproduce across records exactly; member counts did not, and the
    // difference is what each run happened to include. Measured on MathNet.Numerics 5.0.0,
    // whose 336 types decision 0096 reproduces on the nose: its "5 333 members" is the
    // third line below (5 335 here, two apart on a different SDK), while decision 0105's
    // "124 members" for Microsoft.ML.TimeSeries is the first. Neither is wrong; neither
    // said which it was. Printing all three lets an older figure be matched to its basis
    // rather than argued about, and the first line is the one a new record should quote.
    MemberInfo[] declared = [.. exported.SelectMany(type => type.GetMembers(
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))];
    bool Accessor(MemberInfo member) => member is MethodInfo method && method.IsSpecialName;
    MemberInfo[] callable = [.. declared.Where(member => !Accessor(member))];

    Console.WriteLine($"{package} {version}");
    Console.WriteLine($"  assembly       : {Path.GetFileName(target)}");
    Console.WriteLine($"  exported types : {exported.Length}");
    Console.WriteLine($"  public members : {callable.Length}  <- quote this one");
    Console.WriteLine($"                   {declared.Length}  with accessors and operators");
    Console.WriteLine($"                   {callable.Count(m => m is not ConstructorInfo)}"
                      + "  without constructors either");

    if (wanted is null)
    {
        return 0;
    }

    string[] hits = [.. declared
        .Where(member => wanted.IsMatch($"{member.DeclaringType?.FullName}.{member.Name}"))
        .Select(member => $"{member.DeclaringType?.FullName}.{member.Name}")
        .Distinct()
        .Order()];

    Console.WriteLine($"  matching '{wanted}' : {hits.Length}");
    foreach (string hit in hits)
    {
        Console.WriteLine($"    {hit}");
    }

    // An empty result is the finding 0074 asks for -- "zero matches across N members" is a
    // statement, where "we looked and saw nothing" is not.
    if (hits.Length == 0)
    {
        Console.WriteLine(
            $"    none, across {declared.Length} members of {exported.Length} exported types");
    }

    return 0;
}
finally
{
    Directory.Delete(work, recursive: true);
}

static bool Run(string file, string arguments)
{
    using var process = Process.Start(new ProcessStartInfo(file, arguments)
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
    })!;
    string output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
    process.WaitForExit();
    if (process.ExitCode != 0)
    {
        Console.Error.WriteLine(output);
    }

    return process.ExitCode == 0;
}

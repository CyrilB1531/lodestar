# Lodestar.Sample

A console application that consumes the packages exactly as a user would: by `PackageReference`,
restored from the packages this working tree just packed into `./artifacts`
(`samples/NuGet.config` maps `Lodestar.*` there). It is **the packaging gate**: a type that is
public but unreachable from a package, or a package missing a dependency, fails here rather than
in a user's project.

Every public type has a `<TypeName>Sample.cs` calling it, which `tools/check_sample_coverage.py`
asserts for the packages it covers; the rest still live in `Lot*.cs`.
[`CONTRIBUTING.md`](../../CONTRIBUTING.md#definition-of-done) has the rule.

## Run it

Pack the eighteen packages first (the loop is in the root [`README.md`](../../README.md)), then
restore into an isolated package folder, so the run judges this tree's packages rather than the
ones already on nuget.org:

```bash
NUGET_PACKAGES=/tmp/lodestar-sample-packages/ dotnet run -c Release --project samples/Lodestar.Sample
```

It is outside `Lodestar.slnx`, so `dotnet build Lodestar.slnx` does not reach it; CI's
`Sample consumes the packages` job runs it.

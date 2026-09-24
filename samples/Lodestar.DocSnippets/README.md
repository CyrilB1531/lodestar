# Lodestar.DocSnippets

**The doc-snippets gate.** `tools/extract_doc_snippets.py` turns every `csharp` fence in the root
`README.md`, each package's `src/<Package>/README.md`, `docs/guides/` and `docs/reference/` into a
method under `Generated/`, and this project compiles them against the packed packages. A renamed
method fails CI instead of a reader. The reference pages' fences are also **executed**, and a
trailing `// =>` comment on a declaration becomes an assertion on its value.

`SnippetContext.cs` declares, by hand, the symbols the prose uses without showing them; it copies
no snippet. The opt-out markers (`docs-compile: skip`, `docs-declaration`, `docs-run: skip`) are
described in `tools/extract_doc_snippets.py` and in
[`CONTRIBUTING.md`](../../CONTRIBUTING.md#definition-of-done), items 5 and 6.

## Run it

```bash
python3 tools/extract_doc_snippets.py
NUGET_PACKAGES=/tmp/lodestar-snippet-packages/ dotnet build samples/Lodestar.DocSnippets -c Release
NUGET_PACKAGES=/tmp/lodestar-snippet-packages/ dotnet run -c Release --project samples/Lodestar.DocSnippets
```

Pack the packages into `./artifacts` first, as for `Lodestar.Sample`.

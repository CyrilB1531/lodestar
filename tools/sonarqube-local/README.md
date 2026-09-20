# A local SonarQube server — the half the build cannot see

`dotnet build` enforces the Sonar rules that live in the root `.globalconfig`
([`CONTRIBUTING.md`](../../CONTRIBUTING.md#analyzers) has how), but it has no view of three things
the quality gate on the pull request still judges: the Python rules over `tools/`, duplication, and
coverage. [`compose.yaml`](compose.yaml) here runs a disposable SonarQube Community server that
covers all three, for whoever wants that answer before pushing rather than after.

It is optional. Nothing in CI uses it, and no gate asks whether you ran it.

## Running it

```bash
# POSIX (bash/zsh)
cd tools/sonarqube-local && docker compose up -d
# Podman instead of Docker Engine: podman compose up -d
# Wait for the server rather than sleeping blind:
until curl -s http://localhost:9000/api/system/status | grep -q '"status":"UP"'; do sleep 5; done
```

```powershell
# PowerShell — split, not chained with `&&`, which needs PowerShell 7+
cd tools/sonarqube-local
docker compose up -d
# Podman instead of Docker Engine: podman compose up -d
# Wait for the server rather than sleeping blind. -ErrorAction SilentlyContinue does not
# stop Invoke-RestMethod's connection-refused error from aborting a do/until, so this
# catches it explicitly instead — verified against a port with nothing listening yet:
while ($true) { try { if ((Invoke-RestMethod http://localhost:9000/api/system/status).status -eq 'UP') { break } } catch { }; Start-Sleep -Seconds 5 }
```

SonarQube Community bundles Elasticsearch, which wants `vm.max_map_count >= 262144`.
`SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` in the compose file suppresses the startup check, not the
underlying requirement, so a container that exits immediately on a machine at a lower distribution
default (many ship `65530`) needs that raised — `sudo sysctl -w vm.max_map_count=262144`, or the
persistent form in `/etc/sysctl.conf` — before trying again.

Then, from the repository root, with a token created in the local server's UI (*My Account →
Security*, `local` is a fine name) exported as `SONAR_TOKEN`:

```bash
dotnet tool install --global dotnet-sonarscanner   # once, if absent
dotnet sonarscanner begin /k:"datanet-local" \
  /d:sonar.host.url="http://localhost:9000" /d:sonar.token="$SONAR_TOKEN" \
  /d:sonar.python.version="3.12" \
  /d:sonar.exclusions="tests/oracles/**,samples/Lodestar.DocSnippets/Generated/**"
dotnet build Lodestar.slnx -c Release --no-incremental
dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
```

The image is pinned by digest rather than by tag: `community` moves, and a scanner run whose
analyser versions changed underneath it explains a finding that was not there yesterday as a code
change.

## What one run cost, on one machine

Measured with Podman on the maintainer's machine in
[`130f3b3`](https://github.com/CyrilB1531/lodestar/commit/130f3b33), 2026-08-12, against a tree of
17 192 lines of code (13 361 C#, 3 815 Python, 16 XML). Every figure below belongs to that desk and that tree size;
they are here to say what order of magnitude to expect, not to be reproduced.

| step | cost |
| --- | --- |
| pulling `sonarqube:community` (≈1.4 GB) | 38 s |
| `docker compose up -d` | 2 s |
| launch → `"status":"UP"` | ≈56 s |
| `sonarscanner begin` | 5 s |
| `dotnet build --no-incremental` | 39 s |
| `sonarscanner end` | 31 s |

That run reported **0 findings for C# and 0 for Python** — a clean tree, not a light one.
Duplication and coverage sensors both ran (2.0 % duplicated lines, 28 duplicated blocks). Coverage
read 0.0 % because the commands above do not feed it a coverage report; CI's job does.

## It is not a rehearsal of CI

Saying otherwise would make this page worse than not writing it. Four differences, each of which
can make a finding appear here that the real gate does not raise, or the reverse:

- the Community edition has no branch or pull-request analysis, so the verdict is over the whole
  project and never over the diff — which is the axis the real gate judges on;
- the custom `No new issue` gate and its seven conditions are not there — a fresh local server
  starts with only the default `Sonar way` gate, and the run above was evaluated against that one
  instead;
- its analyser versions move independently of the server's, so a rule firing (or not) here does not
  pin down which version fired it on SonarCloud;
- the Community edition carries no taint-analysis engine, so `PythonSecuritySensor` and its
  injection-class vulnerabilities (SSRF, path traversal, and the like) are invisible to it — a
  script that reads an argument into `Path.read_text` or hands one to `urlopen` looks clean here
  and can still fail the real gate ([#131](https://github.com/CyrilB1531/lodestar/issues/131)).

**A finding it reports is real; a clean run promises nothing.**

# Changelog

The eighteen packages version and release **independently**, each from its own
`src/<Package>/Version.props`, so each keeps its own changelog beside its project, in the
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) format, and this repository follows
[Semantic Versioning](https://semver.org/spec/v2.0.0.html). This file only lists them
([#1133](https://github.com/CyrilB1531/lodestar/issues/1133)).

An entry is one sentence, the issue and the commit, under the package's own `## [Unreleased]`:
[`CONTRIBUTING.md`](CONTRIBUTING.md#definition-of-done)'s item 7 has the shape and why. The
four packages first published as `DataNet.*` keep those releases in their files, marked
*published as*, and the joint `0.1.0` and `0.2.0` releases appear in each of the three packages
they covered.

## Packages

- [`Lodestar.Abstractions`](src/Lodestar.Abstractions/CHANGELOG.md)
- [`Lodestar.Text`](src/Lodestar.Text/CHANGELOG.md)
- [`Lodestar.Embeddings`](src/Lodestar.Embeddings/CHANGELOG.md)
- [`Lodestar.Fuzzy`](src/Lodestar.Fuzzy/CHANGELOG.md)
- [`Lodestar.Metrics`](src/Lodestar.Metrics/CHANGELOG.md)
- [`Lodestar.Conformal`](src/Lodestar.Conformal/CHANGELOG.md)
- [`Lodestar.Decomposition`](src/Lodestar.Decomposition/CHANGELOG.md)
- [`Lodestar.Cluster`](src/Lodestar.Cluster/CHANGELOG.md)
- [`Lodestar.Preprocessing`](src/Lodestar.Preprocessing/CHANGELOG.md)
- [`Lodestar.Stats`](src/Lodestar.Stats/CHANGELOG.md)
- [`Lodestar.Stats.Regression`](src/Lodestar.Stats.Regression/CHANGELOG.md)
- [`Lodestar.Stats.TimeSeries`](src/Lodestar.Stats.TimeSeries/CHANGELOG.md)
- [`Lodestar.Survival`](src/Lodestar.Survival/CHANGELOG.md)
- [`Lodestar.Onnx`](src/Lodestar.Onnx/CHANGELOG.md)
- [`Lodestar.Gpu`](src/Lodestar.Gpu/CHANGELOG.md)
- [`Lodestar.Extensions.AI`](src/Lodestar.Extensions.AI/CHANGELOG.md)
- [`Lodestar.Extensions.MathNet`](src/Lodestar.Extensions.MathNet/CHANGELOG.md)
- [`Lodestar.Extensions.VectorData`](src/Lodestar.Extensions.VectorData/CHANGELOG.md)

# MoBi contributor guide

This file applies to the entire repository.

## Repository overview

MoBi is a C#/.NET 10 solution for multiscale physiological modeling. The full application is Windows-specific and includes native and licensed UI dependencies.

- `src/MoBi.Core`: domain logic, commands, serialization, repositories, and services.
- `src/MoBi.Engine`: simulation and calculation orchestration.
- `src/MoBi.Presentation`: presenters, DTOs, mappers, tasks, and UI commands.
- `src/MoBi.UI`: WinForms views and UI-specific services.
- `src/MoBi`: the main desktop application.
- `src/MoBi.CLI.Core`, `src/MoBi.CLI`, and `src/MoBi.BatchTool`: command-line and batch entry points.
- `src/MoBi.R`: R-facing integration.
- `src/MoBi.Assets` and `src/Data`: resources and runtime data.
- `tests/MoBi.Tests`, `tests/MoBi.UI.Tests`, and `tests/MoBi.R.Tests`: automated tests.
- `tests/MoBi.HelpersForTests`: shared test infrastructure.

Keep changes in the narrowest appropriate layer. Domain behavior belongs in `MoBi.Core`; presentation behavior belongs in presenters rather than views; UI-only behavior belongs in `MoBi.UI`.

## Setup

Use Windows for full builds and tests because several projects target `net10.0-windows`. Install the .NET 10 SDK and initialize the repository's submodules:

```shell
git submodule update --init --recursive
```

The submodules provide dimensions, PK parameters, documentation, examples, and shared Rake scripts. Do not edit submodule contents as part of an ordinary MoBi change.

Restore may require access to the Open Systems Pharmacology GitHub Packages NuGet feed at `https://nuget.pkg.github.com/Open-Systems-Pharmacology/index.json`. Configure credentials outside the repository and never commit tokens or license keys. CI supplies required DevExpress and diagram licenses.

## Build and test

Run commands from the repository root. The CI-equivalent development sequence is:

```shell
dotnet build MoBi.sln -p:ExcludeDesigner=true
dotnet test MoBi.sln --no-build -v normal
```

`ExcludeDesigner=true` avoids the licensed DevExpress design-time package and is used by all build workflows. Run the most relevant test project while iterating, for example:

```shell
dotnet test tests/MoBi.R.Tests/MoBi.R.Tests.csproj --no-build -v normal
dotnet test tests/MoBi.UI.Tests/MoBi.UI.Tests.csproj --no-build -v normal
dotnet test tests/MoBi.Tests/MoBi.Tests.csproj --no-build -v normal
```

Use `rake cover` when coverage is required; coverage settings are in `coverage.runsettings`. Installer, portable package, signing, and publishing tasks in `rakefile.rb` and `.github/workflows` are release operations and may require additional tools and secrets.

There is no repository-local formatter or standalone lint command. Follow the existing formatting and treat a clean build and relevant tests as the primary validation.

## C# conventions

- Follow the linked [OSP coding standards](https://dev.open-systems-pharmacology.org/setup/coding_standards) and the style of the surrounding file.
- Existing code generally uses three-space indentation, braces on separate lines, block-scoped namespaces, PascalCase members, and `_camelCase` fields.
- Preserve established interfaces, dependency-injection registrations, mappers, tasks, and service boundaries instead of bypassing them.
- Avoid unrelated formatting or modernization in focused changes.
- Do not edit generated `*.Designer.cs` files directly. Keep `.resx`, designer, and project metadata changes synchronized when UI resources genuinely change.
- Shared assembly metadata is defined in `SolutionInfo.cs`.
- Treat files under `src/Data`, `dimensions`, and `pkparameters` as runtime inputs; preserve their schemas and copy-to-output behavior.
- Maintain backward compatibility when changing project, snapshot, or PKML serialization, and add converter or round-trip coverage where appropriate.

## Test conventions

Tests use NUnit, FakeItEasy, and `OSPSuite.BDDHelper`.

- Match the existing BDD structure: an abstract `concern_for_*` context, scenario classes such as `When_*`, `Context()`/`Because()` setup, and `[Observation]` methods named `should_*`.
- Put tests in the project and folder corresponding to the production layer.
- Reuse `MoBi.HelpersForTests` for shared fixtures and builders.
- Add regression coverage for behavior changes and include boundary, error, and serialization cases when relevant.
- Keep test data declared in the test project when it must be copied to the output directory.

## Before finishing

- Keep the diff scoped to the requested change.
- Build the affected projects and run their tests; run the full solution tests when feasible.
- State clearly when Windows-only dependencies, private package access, licenses, or unavailable submodules prevent validation.
- Follow `.github/PULL_REQUEST_TEMPLATE.md` when preparing a pull request and document the exact validation performed.

# Fantomas for Team Auth

This fork of [Fantomas](https://github.com/fsprojects/fantomas) formats F# the way Team Auth at Svea
Bank prefers. It ships as the `fantomas` dotnet tool on this repository owner's GitHub Packages feed:

```
https://nuget.pkg.github.com/shazmodan/index.json
```

The package id has to stay `fantomas`. Ionide (VS Code) and the F# Formatting extension (Visual
Studio) find the tool through `Fantomas.Client`, which accepts no other name.

## Releasing

`.github/workflows/release-svea.yml` runs on every push to `main`. It releases when the first
version heading in `CHANGELOG.md` has the fork's suffix and has no tag yet:

```markdown
## [8.1.1-svea.1] - 2026-11-10

### Changed

- Describe the rule.
```

1. Add that section to the top of `CHANGELOG.md` and merge to `main`.
2. The workflow builds, runs every test, packs `src/Fantomas`, pushes `fantomas.8.1.1-svea.1.nupkg`
   to the feed, and creates the tag `v8.1.1-svea.1` with a GitHub release.

For the next release, bump the last number (`-svea.2`). After merging a new upstream version,
start again from it (`8.2.0-svea.1`). A version without `-svea.` is never released, so merging
upstream's changelog publishes nothing until you add your own section.

Use `-svea.N`, not a four part version such as `8.1.1.1`. `Fantomas.Client` reads the version as
semver and ignores a tool it cannot parse.

## Using it in a team repository

### Authentication

GitHub Packages asks for a token even to read a public package. Each developer creates a personal
access token (classic) with the `read:packages` scope, and stores it once per machine:

```bash
dotnet nuget update source svea --username <github-user> --password <token> --store-password-in-clear-text
```

(Run it after the `NuGet.config` below is in the repo. On Windows the password is encrypted and the
last flag can be left out.)

In GitHub Actions in the same organisation, `secrets.GITHUB_TOKEN` with `packages: read` is enough,
once the package's settings give that repository access.

### NuGet.config

Add the feed, and map `fantomas` to it alone so the official package on nuget.org can never be
restored in its place:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="svea" value="https://nuget.pkg.github.com/shazmodan/index.json" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="svea">
      <package pattern="fantomas" />
    </packageSource>
    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
  </packageSourceMapping>
</configuration>
```

### Install the tool

```bash
dotnet tool install fantomas --version 8.1.1-svea.1
```

Commit the `.config/dotnet-tools.json` this writes. Everyone else, and CI, runs:

```bash
dotnet tool restore
```

Prefer this local tool over a global one: the editors look for a local tool first, and the version
is pinned in the repository.

### Editors

- **VS Code:** Ionide, with `"[fsharp]": { "editor.defaultFormatter": "Ionide.Ionide-fsharp" }`.
  Reload the window after changing the tool version.
- **Visual Studio:** the F# Formatting extension (`asti.fantomas-vs22`). Restart Visual Studio
  after changing the tool version.

`dotnet fantomas --version` in the repository shows which build the editors will start.

### CI

```bash
dotnet tool restore
dotnet fantomas --check .
```

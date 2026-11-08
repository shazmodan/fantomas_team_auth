# Fantomas for Team Auth

This fork of [Fantomas](https://github.com/fsprojects/fantomas) formats F# the way Team Auth at Svea
Bank prefers. It ships as the `fantomas` dotnet tool on this repository owner's GitHub Packages feed:

```
https://nuget.pkg.github.com/shazmodan/index.json
```

The package id has to stay `fantomas`. Ionide (VS Code) and the F# Formatting extension (Visual
Studio) find the tool through `Fantomas.Client`, which accepts no other name.

## The team's settings

Put this in the `.editorconfig` at the root of a team repository. The last three settings and the
`patterns_only` value exist only in this fork; the official Fantomas reports them as unknown.

```ini
[*.{fs,fsi,fsx}]
max_line_length = 200
fsharp_space_before_colon = true
fsharp_space_before_uppercase_invocation = true
fsharp_blank_lines_around_nested_multiline_expressions = false
fsharp_max_infix_operator_expression = 90
fsharp_max_value_binding_width = 200
fsharp_multiline_bracket_style = stroustrup
fsharp_record_multiline_formatter = number_of_items
fsharp_max_record_number_of_items = 1
fsharp_multi_line_lambda_closing_newline = true
fsharp_space_around_delimiter = patterns_only
fsharp_align_rows = true
fsharp_align_match_arrows = true
fsharp_hanging_infix_operators = true
```

`docs/docs/end-users/Configuration.fsx` describes every setting. What this fork adds:

| Setting | Values | Default | What it does |
| --- | --- | --- | --- |
| `fsharp_space_around_delimiter` | `true`, `false`, `patterns_only` (new) | `true` | `patterns_only`: no space inside the brackets of a list or array expression, `[x1; x0]`, while a list or array pattern keeps it, `\| [ "m", m ] ->`. Records and every other pair of delimiters keep their space. |
| `fsharp_align_rows` (new) | `true`, `false` | `false` | Rows of the same shape are laid out as a table: consecutive one-line match clauses, consecutive one-line `let`s of names as wide, and the operands of a hanging chain. A narrower name, number, character or `A[i]` is centred in its column, the odd space on the left (`>>>  5`, `(x0,  x1, 0uy)`, `[ '=' ; A[a1]]`). A row that lacks the right-hand side of an infix operator the others have gets a blank there (`(q &&& 0x1FL)        \|> int`). |
| `fsharp_align_match_arrows` (new) | `true`, `false` | `false` | Consecutive one-line match clauses pad their patterns so that `->` lines up. A guard, a multiline clause, or a blank line or comment ends the run. |
| `fsharp_hanging_infix_operators` (new) | `true`, `false` | `false` | A `let` whose right-hand side is a chain of one operator wider than `fsharp_max_infix_operator_expression` keeps its first operand on the `=` line; each further line starts with the operator one column after the `=`. Never for `=`, `<`, `>`, `%`, `%%`, `^`, `:=` or `?`, which cannot start such a line. |

`src/Fantomas.Core.SnapshotTests/cases/settings/fsharp_align_rows/true/ModuleDecl/NestedModule/team-auth-base32-and-hash-helper.fs`
holds a whole file formatted with these settings, and its gold what it formats to.

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

The build is versioned from that section alone. Read from the whole changelog, the build would take
the highest version in it, and `8.1.1-svea.1` is a prerelease of `8.1.1`, below it.

For the next release, bump the last number (`-svea.2`). After merging a new upstream version,
start again from it (`8.2.0-svea.1`). A version without `-svea.` is never released, so merging
upstream's changelog publishes nothing until you add your own section.

Use `-svea.N`, not a four part version such as `8.1.1.1`. `Fantomas.Client` reads the version as
semver and ignores a tool it cannot parse.

### The release runner

The release job runs on the owner's own Windows machine, registered with this repository as a
self-hosted runner, rather than on one of GitHub's: a release then costs no Actions minutes. The
pull request checks in `main.yml` still run on GitHub's runners. A release waits in the queue until
the machine is online, and GitHub cancels a job that has waited 24 hours.

The machine needs:

- **Git for Windows**, installed in its default folder, `C:\Program Files\Git`. The workflow's steps
  run in its `bash`, and the checkout needs `git`. The job's first step puts `C:\Program Files\Git\bin`
  at the front of its `PATH`, so WSL's `bash.exe` in `C:\Windows\System32` is never picked up
  instead, and the machine's own `PATH` can stay as it is.
- **The GitHub CLI**, `gh`, which creates the tag and the release. GitHub's own runners have it
  installed; a machine of your own does not.
- Nothing else: the workflow installs the .NET SDK itself, into the runner's tool cache.

To register the machine:

1. On GitHub, open the repository, then **Settings**, **Actions**, **Runners**,
   **New self-hosted runner**, and choose **Windows** and **x64**.
2. In PowerShell on the machine, run the commands that page shows, in order. They download the
   runner into a folder such as `C:\actions-runner`, then register it with `config.cmd` and a token
   that expires after an hour, so register it soon after opening the page.
3. Keep the default runner group, name and labels when `config.cmd` asks: the workflow asks for
   the labels `self-hosted`, `Windows` and `X64`, which every Windows x64 runner gets.
4. Answer yes when it offers to run the runner as a service, so it starts with Windows. Otherwise
   start it with `run.cmd` whenever a release is due.

The runner appears as **Idle** under **Settings**, **Actions**, **Runners** once it is connected.

To start a release again without a new commit, use **Actions**, **Release (Team Auth)**,
**Run workflow** on `main`. **Re-run jobs** on an old run does not do it: a re-run uses the
workflow as it was at that run's commit, so it would ask for the runner that commit named.

## Adding it to an F# repository

The steps below assume a release has been published (see "Releasing"): until the release workflow
has run, there is no package to install.

1. **Give people access to the package.** This repository is private, so the package it publishes
   is private too: GitHub Packages gives a package the visibility of the repository it is linked
   to. The settings page exists once the first release is out: on the owner's profile,
   **Packages**, `fantomas`, **Package settings**.

   - **People:** under **Manage access**, add each team member with the **Read** role.
   - **Other repositories' CI:** under **Manage Actions access**, add each repository whose
     workflows restore the tool, with the **Read** role.

   A token on its own is not enough. Someone without access gets a `401` or `403` from the feed,
   even with a token that has the `read:packages` scope.

2. **Each developer creates a token.** A personal access token (classic) with the `read:packages`
   scope. GitHub Packages asks for one even to read a public package.

3. **Add a `NuGet.config` to the repository root.** It adds the feed, and maps `fantomas` to it
   alone, so the official package on nuget.org can never be restored in its place. A repository
   that already has a `NuGet.config` gets the `svea` source and the `packageSourceMapping` block
   added to it.

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

4. **Each developer stores their token, once per machine.** In the repository root, after step 3.
   On Windows the password is encrypted, and the last flag can be left out.

   ```bash
   dotnet nuget update source svea --username <github-user> --password <token> --store-password-in-clear-text
   ```

5. **Install the tool as a local tool.** The first command only where the repository has no
   `.config/dotnet-tools.json` yet.

   ```bash
   dotnet new tool-manifest
   ```

   ```bash
   dotnet tool install fantomas --version 8.1.1-svea.1
   ```

   A repository that already uses the official Fantomas runs
   `dotnet tool update fantomas --version 8.1.1-svea.1` instead. Commit
   `.config/dotnet-tools.json`, which should now say `8.1.1-svea.1`; everyone else runs
   `dotnet tool restore`. Prefer this local tool over a global one: the editors look for a local
   tool first, and the version is pinned in the repository.

6. **Add the team's settings to the `.editorconfig` at the repository root.** The block in "The
   team's settings" above, with `root = true` at the top of a new file.

7. **Check which build runs.** This is also the build the editors start. It should print
   `v8.1.1-svea.1`; anything else means the tool came from somewhere other than the feed, so
   steps 3 to 5 are worth going over again.

   ```bash
   dotnet fantomas --version
   ```

8. **Leave out what should not be formatted.** List generated or vendored code in a
   `.fantomasignore` at the root, which uses the syntax of `.gitignore`. Indent any file that uses
   tabs with spaces first: F# rejects tabs, so Fantomas cannot read such a file at all.

9. **Format the whole repository once, and commit that on its own.**

   ```bash
   dotnet fantomas .
   ```

   Read the diff before committing. Keeping it in a commit, ideally a pull request, of its own
   keeps it out of real changes, and listing that commit in a `.git-blame-ignore-revs` file lets
   `git blame` skip it.

10. **Set up the editors.** Both find the local tool from step 5 on their own.

    - **VS Code:** Ionide, with `"[fsharp]": { "editor.defaultFormatter": "Ionide.Ionide-fsharp" }`.
      Reload the window after changing the tool version.
    - **Visual Studio:** the F# Formatting extension (`asti.fantomas-vs22`). Restart Visual
      Studio after changing the tool version.

11. **Check the formatting in CI.** In the repository's GitHub Actions workflow, its own
    `secrets.GITHUB_TOKEN` is enough once the repository has been given access in step 1. The job
    needs `packages: read`, and the token has to be stored before the restore. `--check` changes
    nothing and fails the job when a file is not formatted.

    ```yaml
    permissions:
      contents: read
      packages: read
    steps:
      - uses: actions/checkout@v7.0.1
      - uses: actions/setup-dotnet@v6
      - run: dotnet nuget update source svea --username ${{ github.actor }} --password ${{ secrets.GITHUB_TOKEN }} --store-password-in-clear-text
      - run: dotnet tool restore
      - run: dotnet fantomas --check .
    ```

12. **Upgrade with each new release.** Run
    `dotnet tool update fantomas --version <new version>`, commit
    `.config/dotnet-tools.json`, and reload or restart the editors.

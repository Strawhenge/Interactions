# CLAUDE.md

Guidance for Claude when working in this repository.

## What this is

**Strawhenge Interactions** (`com.strawhenge.interactions`) — a Unity package for character interactions: emotes, sitting, sleeping, furniture (chairs, beds, emote spots), barks (one-shot audio), and position placement (smoothly moving/turning a character to a target before an interaction). It is one of the Strawhenge packages, alongside Strawhenge Common, FunctionalUtilities and Strawhenge Inventory, and is consumed by the Strawhenge game.

Consumers install it via UPM from the `release` branch:
`https://github.com/Strawhenge/Interactions.git?path=/Strawhenge.Interactions.Unity/Assets/Package#release`

## Repository layout

```
Strawhenge.Interactions/          Pure .NET core library + tests (no UnityEngine)
  Strawhenge.Interactions/        netstandard2.0 class library -> Strawhenge.Interactions.dll
    OneAtATime/                   OneAtATime + OneAtATimeManager<T> (queue/replace a single active action)
    Furniture/                    Furniture, FurnitureUser, NullFurniture, UserScope/ (service lookup)
  Strawhenge.Interactions.Tests/  net9.0 xUnit tests
Strawhenge.Interactions.Unity/    Unity 2021.3.17f1 project used to develop and test the package
  Assets/Package/                 THE PACKAGE (package.json, Runtime/, Editor/)
    Runtime/                      Strawhenge.Interactions.Unity asmdef
      Barks/ Emotes/ Furniture/ PositionPlacement/ Sit/ Sleep/
      Strawhenge.Interactions.dll (copied build output — git-ignored, see below)
    Editor/                       Strawhenge.Interactions.Unity.Editor asmdef
      ScriptEditors/              Custom inspectors with play-mode test buttons
      Tools/                      ScriptableWizards that generate Animator layers/states + assets
  Assets/Development/             Dev-only scene, Y Bot rig, Mixamo animations, sample assets (not shipped)
.github/workflows/release.yml     Manual release pipeline
```

## Two-layer architecture

1. **Core (.NET, `Strawhenge.Interactions` namespace)** — engine-agnostic logic, unit tested. Depends on `FunctionalUtilities` (`Maybe<T>`) and `Strawhenge.Common` (logging `ILogger`, helpers) via NuGet from the Strawhenge GitHub Packages feed.
2. **Unity (`Strawhenge.Interactions.Unity` namespace)** — references the core as a precompiled DLL, plus the Common, FunctionalUtilities and Inventory Unity packages.

Put logic that doesn't need UnityEngine in the core library so it can be tested with xUnit.

### Unity-side conventions (follow these when adding a feature)

Each feature is split into the same pieces:

| Piece | Example | Role |
|---|---|---|
| `XScript : MonoBehaviour` | `EmotesScript`, `SitScript` | Serialized fields; lazily builds the controller (`Controller => _c ??= Create()`, also in `Awake`). Optional refs marked `[Tooltip("Optional.")]`. Falls back to `new UnityLogger(gameObject)` when no `LoggerScript`. Uses `ComponentRefHelper.EnsureRootHierarchyComponent` for required refs like `Animator`. |
| `XController` | `EmoteController`, `SitController` | Plain C# public API + `Action` events (e.g. `Sitting`/`Standing`, `EmoteBegan`/`EmoteEnded`). |
| `XAnimationHandler` (internal) | `SitAnimationHandler` | Sets animator parameters, listens to the state machine behaviour via `animator.AddEvents<T>()` / `StateMachineEvents<T>` from Common. |
| `XStateMachine : StateMachineBehaviour, IHasDestroyedEvent` | `SitStateMachine` | Lives on the Animator layer; raises callbacks on state enter/exit. |
| `AnimatorParameters` / `AnimatorStates` (static, internal) | | Single source of truth for parameter/state names — editor tools reuse them. |
| `XTypeScriptableObject` / `XScriptableObject` | `SitTypeScriptableObject`, `EmoteScriptableObject` | Data assets holding an animator Id etc. Field-name constants (e.g. `IdFieldName`) let editor tools set private serialized fields. |
| `XScriptEditor` | `SitScriptEditor` | Custom inspector; adds buttons enabled only in play mode for manual testing. |
| `Tools/CreateX/` | `CreateEmoteWizard` + `CreateEmote` | `ScriptableWizard` under `Assets/Create/Strawhenge/Interactions/...`; wizard gathers input, static class edits the `AnimatorController` and creates the asset. Ids come from `ParameterIdHelper.Generate` (Common). |

`Runtime/Assembly.cs` exposes internals to the Editor assembly (`InternalsVisibleTo`).

### Key concepts

- **OneAtATimeManager<T>** — only one `OneAtATime` runs at once. Starting a new one requests the current to stop; when it stops, only the most recent pending one starts, and skipped ones still get their callbacks invoked. Used by emotes. Subclasses implement `OnStart` / `OnStopRequested` and call `InvokeStopped()` when actually finished.
- **Furniture / FurnitureUser** — a user uses one piece of furniture at a time; furniture has one user. Furniture gets the user's services through `IFurnitureUserScope.Get<T>()` (`FurnitureUserScript` registers `EmoteController`, `SitController`, `SleepController`, `PositionPlacementController`; extend via `FurnitureUserScopeConfigurationScript`). Implementations (`Chair`, `Bed`, `EmoteFurniture`) implement `OnUse`/`OnEndUse` and must call `Ended()` when done — including on failure paths.
- Interactions are **asynchronous and animation-driven**: completion is signalled by Animator state machine callbacks, so most APIs take an optional `Action` callback. Always make sure callbacks fire even on early-exit/error paths.
- Use `Maybe<T>` (FunctionalUtilities) instead of null for optional values/dependencies; log via Strawhenge.Common `ILogger` rather than `Debug.Log` in runtime code.

## Build, test, release

Developer environment is Windows (Rider / Visual Studio, Unity 2021.3.17f1). Package `unity` minimum is 2020.3.

- **Build core:** `dotnet build Strawhenge.Interactions`
  A post-build step `xcopy`s the DLL into `Strawhenge.Interactions.Unity/Assets/Package/Runtime/` (Windows-only command). That DLL is git-ignored — after changing core code, rebuild so Unity picks it up.
- **Test core:** `dotnet test Strawhenge.Interactions`
- **Restore** needs the Strawhenge GitHub Packages NuGet source (`https://nuget.pkg.github.com/Strawhenge/index.json`) for `Strawhenge.Common`.
- **Unity code** has no automated tests; verify in `Assets/Development/Development.unity` using the inspector buttons.
- **Release:** `release.yml` is run manually (workflow_dispatch). It builds/tests, publishes the NuGet package as `1.0.<run number>`, force-adds the DLL, stamps `package.json` version, and recreates the `release` branch. Don't hand-edit the version in `package.json` or commit the DLL on `master`.

## Coding style

- C# with no explicit `private` modifier; `_camelCase` private fields; `readonly` where possible.
- Unity runtime code uses block-scoped namespaces; tests use file-scoped namespaces, nullable enabled, test names like `Start_should_stop_previous_emote`.
- Core library targets netstandard2.0 (C# features limited accordingly); Unity side can use `??=` and target-typed `new`.
- Keep `.meta` files with any added/moved/renamed asset or script under `Assets/` — Unity GUIDs are referenced by asmdefs, prefabs and scenes.
- Don't edit `Library/`, `Temp/`, `obj/`, `bin/`, `.vs/`, `.idea/` or generated `.csproj`/`.sln` files in the Unity project.

## Git workflow

- `master` is the main branch; `release` is generated by CI — never commit to it directly.
- When work is split into branches, open a pull request for each one for review rather than merging directly into the feature branch.

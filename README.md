# Strawhenge Interactions

A Unity package for character interactions: **emotes**, **sitting**, **sleeping**, **furniture** (chairs, beds and emote spots), **barks** (short voice or sound clips) and **position placement** (moving and turning a character smoothly into place before an interaction).

Interactions are driven by the character's Animator. Editor wizards create the Animator layers, states and transitions for you, so you mostly work with assets and components rather than wiring Animator graphs by hand.

## Requirements

- Unity 2020.3 or later (developed on 2021.3)
- A humanoid character with an `Animator` and an `AnimatorController`
- These Strawhenge packages, which Unity's Package Manager can't install automatically from Git:
  - [Strawhenge Common](https://github.com/Strawhenge/Common)
  - [Strawhenge FunctionalUtilities](https://github.com/Strawhenge/FunctionalUtilities)
  - [Strawhenge Inventory](https://github.com/Strawhenge/Inventory) (lets emotes hold items)

## Installation

Add the dependencies and the package to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.strawhenge.common": "https://github.com/Strawhenge/Common.git?path=/Strawhenge.Common.Unity/Assets/Package#release",
    "com.strawhenge.functionalutilities": "https://github.com/Strawhenge/FunctionalUtilities.git?path=/Package#release",
    "com.strawhenge.inventory": "https://github.com/Strawhenge/Inventory.git?path=/Strawhenge.Inventory.Unity/Assets/Package#release",
    "com.strawhenge.interactions": "https://github.com/Strawhenge/Interactions.git?path=/Strawhenge.Interactions.Unity/Assets/Package#release"
  }
}
```

Or use **Window → Package Manager → + → Add package from git URL…** and add each URL above, dependencies first.

## Quick start

### 1. Add the components to your character

The package includes an **`Interactions` prefab** (in the package's `Runtime` folder). It holds every character-side component. Add it as a child of your character, or add the components yourself:

| Component | What it does |
|---|---|
| `EmotesScript` | Plays emotes. You can also give it a `BarkScript` and an `InventoryScript`. |
| `SitScript` | Sits and stands. Needs a **default Sit Type**. |
| `SleepScript` | Lies down, sleeps and wakes up. Needs a **default Sleep Type**. |
| `BarkScript` | Plays barks through an `AudioSource`. |
| `PositionPlacementScript` | Moves and turns the character to a target. Set **Root** to the character's root transform, or it uses its own. |
| `FurnitureUserScript` | Lets the character use furniture. Link the Emotes, Sit, Sleep and Position Placement components. |

If no `Animator` is assigned, these components look for one in the character's hierarchy. `LoggerScript` (from Strawhenge Common) is optional on all of them.

### 2. Set up the Animator Controller

From the **Strawhenge → Interactions** menu, run each wizard and pick your character's Animator Controller:

- **Emotes Animator Layer**
- **Sit Animator Layer**
- **Sleep Animator Layer**

Each wizard adds a layer and the parameters it needs. A wizard won't add a layer that already exists.

### 3. Create interaction assets

In the Project window, use **Create → Strawhenge → Interactions**:

| Asset | Created with | Notes |
|---|---|---|
| **Emote** | `Emote...` wizard | Pick the controller, emote layer, a name and an animation clip. Options: *Is Repeating* (plays until ended), *Use Root Motion*, an inventory **Item** to hold and a **Bark** to play. |
| **Sit Type** | `Sit Type...` wizard | Sit, sitting (loop) and stand animation clips. |
| **Sleep Type** | `Sleep Type...` wizard | Lie-down, sleeping (loop) and get-up animation clips. |
| **Bark** | `Barks → Single Clip Bark` / `Random Clip Bark` | One audio clip, or one picked at random from a list. |
| **Emote Collection** | `Emote Collection` | A list of emotes, for example for a UI menu. |
| **Position Placement Args** | `Position Placement Args` | Move and turn speeds, how close counts as "arrived", and a timeout. |

The wizards add the states and transitions to the Animator Controller and save a matching asset in the selected folder. Each asset stores the Animator ID it was given, so create these assets with the wizards rather than duplicating existing ones.

### 4. Add furniture to the scene

Add one of these components to a scene object:

- **`ChairScript`**: walk to the start position, sit, then stand and move to the end position. Uses a Sit Type.
- **`BedScript`**: the same flow, using a Sleep Type.
- **`EmoteFurnitureScript`**: move into place and perform an emote. Useful for fishing spots, workbenches and so on.

Each position has an optional target transform (defaults to the furniture's own transform) and Position Placement Args.

## Using it from code

Each component exposes a plain C# controller:

```csharp
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.Sit;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] EmotesScript _emotes;
    [SerializeField] SitScript _sit;
    [SerializeField] FurnitureUserScript _furnitureUser;
    [SerializeField] EmoteScriptableObject _wave;
    [SerializeField] FurnitureScript _chair;

    void Wave() =>
        _emotes.EmoteController.Perform(_wave, () => Debug.Log("Wave finished"));

    void SitDown() => _sit.SitController.Sit(); // uses the default Sit Type

    void UseChair() =>
        _furnitureUser.User.Use(_chair.Furniture, () => Debug.Log("Left the chair"));

    void LeaveChair() => _furnitureUser.User.EndUse();
}
```

Things to know:

- **Interactions are asynchronous.** They finish when the animation does. Pass a callback or subscribe to events such as `EmoteBegan`/`EmoteEnded`, `Sitting`/`Standing`, `GoingToSleep`/`Sleeping`/`WakingUp`/`WokenUp`, and `BeganUsingFurniture`/`EndedUsingFurniture`.
- **Only one emote plays at a time.** Calling `Perform` while an emote is playing ends the current one first. If several are requested in a row, only the latest plays, but every callback still runs.
- **Furniture is one user at a time.** `Use` is ignored, and its callback runs immediately, if the character is already using furniture or the furniture is taken or deactivated. Check `furniture.IsAvailable` first.
- **Custom furniture:** subclass `Strawhenge.Interactions.Furniture.Furniture`, implement `OnUse` and `OnEndUse`, and call `Ended()` when finished. Get the character's controllers from the `IFurnitureUserScope` passed to `OnUse`. To register your own services in that scope, add a `FurnitureUserScopeConfigurationScript` subclass to the character.

### Testing in the editor

In Play mode, the inspectors for the Emotes, Sit, Sleep, Bark, Position Placement and Furniture User components have buttons for triggering interactions by hand.

## Repository layout

```
Strawhenge.Interactions/         .NET core library (netstandard2.0) + xUnit tests
Strawhenge.Interactions.Unity/   Unity project for developing the package
  Assets/Package/                The package itself (Runtime + Editor)
  Assets/Development/            Sample scene and assets (not included in the package)
```

The core library builds to `Strawhenge.Interactions.dll`, which is copied into the package. Releases are published by the **Release** GitHub Actions workflow to the `release` branch, which the install URLs above point to.

### Building from source

```bash
dotnet build Strawhenge.Interactions   # also copies the DLL into the Unity package (Windows)
dotnet test Strawhenge.Interactions
```

Restoring requires access to the Strawhenge GitHub Packages NuGet feed (`https://nuget.pkg.github.com/Strawhenge/index.json`).

Open `Strawhenge.Interactions.Unity` in Unity and load `Assets/Development/Development.unity` to try things out.

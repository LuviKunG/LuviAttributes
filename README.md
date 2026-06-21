# LuviAttributes

[![Unity](https://img.shields.io/badge/Unity-2022.1%2B-black)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-CC%20BY--NC--ND%204.0-orange)](LICENSE)

A collection of custom Unity inspector attributes for enforcing field validation and display rules directly in code.

## Requirements

- Unity 2022.1 or later

## Installation

### via `manifest.json`

Navigate to your project's `Packages` folder and open `manifest.json` in a text editor. Add the following entry inside `"dependencies"`:

**Pinned release** — use a specific version tag:

```json
{
  "dependencies": {
    "com.luvikung.attributes": "https://github.com/LuviKunG/LuviAttributes.git#1.0.0"
  }
}
```

**Latest** — always tracks the `upm` branch:

```json
{
  "dependencies": {
    "com.luvikung.attributes": "https://github.com/LuviKunG/LuviAttributes.git#upm"
  }
}
```

### via Package Manager (Unity 2019.3+)

1. Open **Window → Package Manager**.
2. Click the **+** button in the top-left corner and choose **Add package from git URL…**
3. Paste one of the URLs below and click **Add**:

**Pinned release:**

```text
https://github.com/LuviKunG/LuviAttributes.git#1.0.0
```

**Latest:**

```text
https://github.com/LuviKunG/LuviAttributes.git#upm
```

## Attributes

### `[ReadOnly]`

Disables the field in the Inspector so it appears grayed out and cannot be edited at edit time. Useful for exposing runtime-only or debug values without allowing accidental edits.

Applies to both **fields** and **properties**.

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    [ReadOnly]
    public int score = 0;

    [ReadOnly]
    public string currentState = "Idle";
}
```

---

### `[NotNull]`

Displays a red error help box when an object reference field is empty in the Inspector. The field itself is also highlighted in red.

Namespace: `LuviKunG.Attributes`

Applies to **fields** only. Validation activates only when the target object is a `Component`.

```csharp
using UnityEngine;
using LuviKunG.Attributes;

public class Example : MonoBehaviour
{
    [NotNull]
    public Transform target;

    [NotNull]
    public AudioClip clip;
}
```

---

### `[PrefabAsset]`

Enforces that an object reference must point to a prefab asset from the asset database — not a scene instance or any other object type. Provides two levels of feedback in the Inspector:

| State | Indicator |
| --- | --- |
| Field is null | Red error box — *"This field cannot be null. Please assign a prefab asset."* |
| Non-prefab or scene instance assigned | Yellow warning box — *"This field is intended for prefab references from the asset database. Please assign a valid prefab asset."* |
| Valid prefab asset assigned | No indicator |

Namespace: `LuviKunG.Attributes`

Applies to **fields** only (`GameObject` or `Component` references). Supports both `Regular` and `Variant` prefab types. Validation activates only when the target object is a `Component`.

```csharp
using UnityEngine;
using LuviKunG.Attributes;

public class Example : MonoBehaviour
{
    [PrefabAsset]
    public GameObject enemyPrefab;

    [PrefabAsset]
    public ParticleSystem effectPrefab;
}
```

## License

Copyright (c) 2026 Thanut Panichyotai ([@LuviKunG](https://github.com/LuviKunG))

This library is licensed under [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International](LICENSE.md).

**You are free to:** Share — copy and redistribute the material in any medium or format.

**Under the following terms:**

- **Attribution** — You must give appropriate credit and provide a link to the license.
- **NonCommercial** — You may not use the material for commercial purposes.
- **NoDerivatives** — You may not remix, transform, or build upon the material and distribute the modified version.

For permissions beyond the scope of this license, contact: [luvikung@gmail.com](mailto:luvikung@gmail.com)

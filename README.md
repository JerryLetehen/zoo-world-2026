# Zoo World 2026

## Unity version

Unity **6000.3.28f1** (URP).

## Custom packages used

- **VContainer** (`jp.hadashikick.vcontainer`, via OpenUPM): dependency injection.
- **R3** (`com.cysharp.r3`, via OpenUPM, plus the R3 NuGet package under `Assets/Packages`): reactive streams and the game ticker.
- **NuGetForUnity** (`com.github-glitchenzo.nugetforunity`): restores the R3 NuGet dependencies.

## Architecture patterns used

- Composition root: `GameLifetimeScope` is the only place concretes are bound.
- Dependency Injection and Dependency Inversion: consumers own small interfaces.
- State machine: level select, then gameplay.
- Strategy: movement behaviours, spawn placement, camera input.
- Factory and Object Pool: animals are created by `AnimalFactory` and pooled per prefab.
- Observer: R3 streams for the game tick and domain events (animal died, score changed).
- MVP: score UI.
- Provider: arenas and configuration sit behind provider interfaces.
- Data-driven ScriptableObjects: animals, configs, arena catalog.

## How to add new animals

1. Create an animal prefab with an `AnimalView`, a Rigidbody, a collider and one movement component (see below).
2. Create an `AnimalDefinition` asset (`Create > ZooWorld > Animal Definition`) and assign the prefab.
3. Add the asset to the `Animal Definitions` list on `GameLifetimeScope`.

No code changes are needed in the spawner, factory, collision resolver or UI.

## How to add new movement types

1. Create a config POCO and a `ConfigAsset<T>` ScriptableObject for its parameters (see `LinearMovementConfig` and `LinearMovementConfigAsset`).
2. Create a class deriving from `AnimalMovement` and implement `Move(float fixedDeltaTime)`. Use `Body` and `Heading` from the base class (see `LinearMovement` and `JumpMovement`).
3. Add the component to an animal prefab and assign its config asset.

The base class handles ticking and steering back into the visible area.

## How to set up food chain

The food chain is defined by the `Diet` list on each `AnimalDefinition`.

- An empty diet makes the animal prey.
- A non-empty diet makes it a predator that eats every animal listed.
- An animal may list itself to fight its own kind. In that case one of the two survives.
- Prey meeting prey simply bounce off each other.

For example, to make a Hawk that eats Frogs and Snakes, add Frog and Snake to the Hawk definition's diet. Nothing else needs to change.

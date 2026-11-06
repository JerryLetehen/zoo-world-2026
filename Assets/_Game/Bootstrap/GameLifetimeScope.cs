using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Animals;
using ZooWorld.App;
using ZooWorld.Arena;
using ZooWorld.CameraControl;
using ZooWorld.Collision;
using ZooWorld.Config;
using ZooWorld.Core;
using ZooWorld.Labels;
using ZooWorld.LevelSelect;
using ZooWorld.Pooling;
using ZooWorld.Scoring;
using ZooWorld.Spawning;

namespace ZooWorld.Bootstrap
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [Header("Config sources (ScriptableObject)")]
        [SerializeField] private SpawnConfigAsset spawnConfig;
        [SerializeField] private CameraConfigAsset cameraConfig;
        [SerializeField] private LabelConfigAsset labelConfig;
        [SerializeField] private PhysicsConfigAsset physicsConfig;

        [Header("Arena")]
        [SerializeField] private ArenaCatalog arenaCatalog;

        [Header("Camera")]
        [SerializeField] private Camera gameCamera;

        [Header("Core")]
        [SerializeField] private GameTicker gameTicker;

        [Header("UI")]
        [SerializeField] private TastyLabelView tastyLabelPrefab;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private LevelSelectView levelSelectView;
        [SerializeField] private GameplayHud gameplayHud;

        [Header("Animals")]
        [SerializeField] private List<AnimalDefinition> animalDefinitions = new();

        protected override void Configure(IContainerBuilder builder)
        {
            ApplyPhysicsTimestep();
            RegisterConfig(builder);
            RegisterCore(builder);
            RegisterPooling(builder);
            RegisterArena(builder);
            RegisterAnimals(builder);
            RegisterSpawning(builder);
            RegisterCollision(builder);
            RegisterLabels(builder);
            RegisterScoring(builder);
            RegisterCamera(builder);
            RegisterApp(builder);
        }

        private void ApplyPhysicsTimestep()
        {
            if (physicsConfig != null)
            {
                Time.fixedDeltaTime = physicsConfig.Value.FixedDeltaTime;
            }
        }

        private void RegisterConfig(IContainerBuilder builder)
        {
            builder.RegisterInstance<IConfigProvider<SpawnConfig>>(
                new ScriptableObjectConfigProvider<SpawnConfig>(spawnConfig));
            builder.RegisterInstance<IConfigProvider<CameraConfig>>(
                new ScriptableObjectConfigProvider<CameraConfig>(cameraConfig));
            builder.RegisterInstance<IConfigProvider<LabelConfig>>(
                new ScriptableObjectConfigProvider<LabelConfig>(labelConfig));
            builder.RegisterInstance<IConfigProvider<PhysicsConfig>>(
                new ScriptableObjectConfigProvider<PhysicsConfig>(physicsConfig));
        }

        private void RegisterCore(IContainerBuilder builder)
        {
            if (gameTicker != null)
            {
                builder.RegisterInstance<IGameTicker>(gameTicker);
            }
        }

        private static void RegisterPooling(IContainerBuilder builder)
        {
            builder.Register<IPoolContainers, PoolContainers>(Lifetime.Singleton);
        }

        private void RegisterArena(IContainerBuilder builder)
        {
            builder.Register<IActiveArena, ActiveArena>(Lifetime.Singleton);

            if (arenaCatalog != null)
            {
                builder.RegisterInstance<IArenaCatalog>(arenaCatalog);
                builder.RegisterInstance<IArenaProvider>(new LocalArenaProvider(arenaCatalog));
            }
        }

        private void RegisterAnimals(IContainerBuilder builder)
        {
            if (animalDefinitions.Count > 0)
            {
                builder.RegisterInstance<IAnimalDefinitionRegistry>(
                    new AnimalDefinitionRegistry(animalDefinitions));
            }

            builder.Register<IPrefabPool<AnimalView>>(
                resolver => new PrefabPool<AnimalView>(resolver.Resolve<IPoolContainers>().Get(PoolCategory.Animals)),
                Lifetime.Singleton);
            builder.Register<IAnimalFactory, AnimalFactory>(Lifetime.Singleton);
        }

        private void RegisterSpawning(IContainerBuilder builder)
        {
            builder.Register<ISpawnTimer, RandomSpawnTimer>(Lifetime.Singleton);
            builder.Register<ISpawnPositionProvider, ArenaSpawnPositionProvider>(Lifetime.Singleton);
            builder.Register<AnimalSpawner>(Lifetime.Singleton)
                .As<AnimalSpawner>()
                .As<IGameplayController>();
        }

        private void RegisterCollision(IContainerBuilder builder)
        {
            builder.Register<ICollisionResolver, FoodChainCollisionResolver>(Lifetime.Singleton);
            builder.Register<CollisionDispatcher>(Lifetime.Singleton);
            builder.Register<AnimalLifecycleService>(Lifetime.Singleton);
            builder.RegisterBuildCallback(container => container.Resolve<CollisionDispatcher>());
            builder.RegisterBuildCallback(container => container.Resolve<AnimalLifecycleService>());
        }

        private void RegisterLabels(IContainerBuilder builder)
        {
            builder.RegisterInstance(tastyLabelPrefab);
            builder.Register<IPrefabPool<TastyLabelView>>(
                resolver => new PrefabPool<TastyLabelView>(resolver.Resolve<IPoolContainers>().Get(PoolCategory.UI)),
                Lifetime.Singleton);
            builder.Register<ITastyLabelSpawner, TastyLabelSpawner>(Lifetime.Singleton);
            builder.RegisterBuildCallback(container => container.Resolve<ITastyLabelSpawner>());
        }

        private void RegisterScoring(IContainerBuilder builder)
        {
            builder.Register<GameStateService>(Lifetime.Singleton)
                .As<IGameStateService>()
                .As<IGameStateReset>();
            builder.RegisterBuildCallback(container => container.Resolve<IGameStateService>());

            if (scoreView != null)
            {
                builder.RegisterComponent(scoreView);
                builder.RegisterEntryPoint<ScorePresenter>();
            }
        }

        private void RegisterCamera(IContainerBuilder builder)
        {
            builder.Register<ICameraPanInput, PointerPanInput>(Lifetime.Singleton);

            builder.RegisterInstance<IVisibleArea>(new CameraVisibleArea(gameCamera));

            if (gameCamera != null)
            {
                builder.RegisterInstance(gameCamera);
                builder.Register<CameraPanController>(Lifetime.Singleton);
                builder.RegisterBuildCallback(container => container.Resolve<CameraPanController>());
            }
        }

        private void RegisterApp(IContainerBuilder builder)
        {
            if (levelSelectView != null)
            {
                builder.RegisterComponent<ILevelSelectView>(levelSelectView);
            }

            if (gameplayHud != null)
            {
                builder.RegisterComponent<IGameplayHud>(gameplayHud);
            }

            builder.Register<ILevelSelection, LevelSelection>(Lifetime.Singleton);
            builder.Register<IStateResolver, ContainerStateResolver>(Lifetime.Singleton);
            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
            builder.Register<LevelSelectState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}

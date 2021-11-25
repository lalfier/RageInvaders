using UnityEngine;
using Zenject;

/// <summary>
/// Bind all classes, interfaces, factories to DI-Container.
/// </summary>
public class GameInstaller : MonoInstaller
{
    [Inject]
    AssetRefs _assetRefs = null;

    public override void InstallBindings()
    {
        InstallEnemies();
        InstallPlayer();
        InstallLevel();
        InstallSignals();
    }

    void InstallEnemies()
    {
        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
        Container.Bind<EnemyRegistry>().AsSingle();

        Container.BindFactory<Enemy, Enemy.Factory>()
            .FromPoolableMemoryPool<Enemy, EnemyPool>(poolBinder => poolBinder
                .WithInitialSize(20)    // Add 20 enemies on int for object pooling
                .FromSubContainerResolve()
                .ByMethod(CreateEnemy));

        Container.BindFactory<float, float, ProjectileTypes, ProjectileEnemy, ProjectileEnemy.Factory>()
            .FromPoolableMemoryPool<float, float, ProjectileTypes, ProjectileEnemy, ProjectileEnemyPool>(poolBinder => poolBinder
                .WithInitialSize(10)    // Add 10 projectiles on int for object pooling
                .FromComponentInNewPrefab(_assetRefs.projectileEnemyRef)
                .UnderTransformGroup("ProjectilesEnemy"));
    }

    void InstallPlayer()
    {
        Container.Bind<PlayerStateFactory>().AsSingle();
        Container.BindFactory<PlayerStateWaiting, PlayerStateWaiting.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateDead, PlayerStateDead.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStatePlaying, PlayerStatePlaying.Factory>().WhenInjectedInto<PlayerStateFactory>();

        Container.BindFactory<float, float, ProjectileTypes, ProjectilePlayer, ProjectilePlayer.Factory>()
            .FromPoolableMemoryPool<float, float, ProjectileTypes, ProjectilePlayer, ProjectilePlayerPool>(poolBinder => poolBinder
                .WithInitialSize(10)    // Add 10 projectiles on int for object pooling
                .FromComponentInNewPrefab(_assetRefs.projectilePlayerRef)
                .UnderTransformGroup("ProjectilesPlayer"));
    }

    void InstallLevel()
    {
        Container.Bind<ScoreManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.Bind<LevelBounds>().AsSingle();
    }

    void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerDeadSignal>();
        Container.DeclareSignal<PlayerLivesSignal>();
        Container.DeclareSignal<EnemyDeadSignal>();
        Container.DeclareSignal<WaveCreatedSignal>();
        Container.DeclareSignal<StartButtonSignal>();
        Container.DeclareSignal<MenuButtonSignal>();
        Container.DeclareSignal<ScoresButtonSignal>();
    }

    // Helper function to create and bind random enemy
    void CreateEnemy(DiContainer subContainer)
    {
        int i = Random.Range(0, _assetRefs.enemiesRef.Count);

        subContainer
            .Bind<Enemy>()
            .FromComponentInNewPrefab(_assetRefs.enemiesRef[i])
            .UnderTransformGroup("Enemies").AsSingle();
    }

    // Memory Pools
    class EnemyPool : MonoPoolableMemoryPool<IMemoryPool, Enemy>
    {
    }

    class ProjectileEnemyPool : MonoPoolableMemoryPool<float, float, ProjectileTypes, IMemoryPool, ProjectileEnemy>
    {
    }

    class ProjectilePlayerPool : MonoPoolableMemoryPool<float, float, ProjectileTypes, IMemoryPool, ProjectilePlayer>
    {
    }
}
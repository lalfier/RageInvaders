using UnityEngine;
using Zenject;

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

        Container.BindFactory<float, float, ProjectileTypes, ProjectileEnemy, ProjectileEnemy.Factory>()
        .FromPoolableMemoryPool<float, float, ProjectileTypes, ProjectileEnemy, ProjectileEnemyPool>(poolBinder => poolBinder
            .WithInitialSize(10)
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
            .WithInitialSize(10)
            .FromComponentInNewPrefab(_assetRefs.projectilePlayerRef)
            .UnderTransformGroup("ProjectilesPlayer"));
    }

    void InstallLevel()
    {
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.Bind<LevelBounds>().AsSingle();
    }

    void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerDeadSignal>();
    }

    class ProjectileEnemyPool : MonoPoolableMemoryPool<float, float, ProjectileTypes, IMemoryPool, ProjectileEnemy>
    {
    }

    class ProjectilePlayerPool : MonoPoolableMemoryPool<float, float, ProjectileTypes, IMemoryPool, ProjectilePlayer>
    {
    }
}
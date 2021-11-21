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
    }

    void InstallPlayer()
    {
        Container.Bind<PlayerStateFactory>().AsSingle();
        Container.BindFactory<PlayerStateWaiting, PlayerStateWaiting.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateDead, PlayerStateDead.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStatePlaying, PlayerStatePlaying.Factory>().WhenInjectedInto<PlayerStateFactory>();
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
}
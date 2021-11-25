using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "RageInvaders/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public PlayerSettings Player;
    public EnemySettings Enemy;
    public EnemyManagerSettings EnemyManager;
    public ScoreManagerSettings ScoreManager;

    [Serializable]
    public class PlayerSettings
    {
        public PlayerStatePlaying.Settings StatePlaying;
        public PlayerStateWaiting.Settings StateStarting;
    }

    [Serializable]
    public class EnemySettings
    {
        public Enemy.Settings StatePlaying;
    }

    [Serializable]
    public class EnemyManagerSettings
    {
        public EnemyManager.Settings StatePlaying;
    }

    [Serializable]
    public class ScoreManagerSettings
    {
        public ScoreManager.Settings HighscoreList;
    }

    public override void InstallBindings()
    {
        Container.Bind<AssetRefs>().FromInstance(FindObjectOfType<AssetManager>().assetRefs).AsSingle();

        Container.BindInstance(Player.StatePlaying);
        Container.BindInstance(Player.StateStarting);
        Container.BindInstance(Enemy.StatePlaying);
        Container.BindInstance(EnemyManager.StatePlaying);
        Container.BindInstance(ScoreManager.HighscoreList);
    }
}

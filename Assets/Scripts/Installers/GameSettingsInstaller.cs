using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "RageInvaders/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public PlayerSettings Player;

    [Serializable]
    public class PlayerSettings
    {
        public PlayerStatePlaying.Settings StatePlaying;
        public PlayerStateWaiting.Settings StateStarting;
    }

    public override void InstallBindings()
    {
        Container.Bind<AssetRefs>().FromInstance(FindObjectOfType<AssetManager>().assetRefs).AsSingle();

        Container.BindInstance(Player.StatePlaying);
        Container.BindInstance(Player.StateStarting);
    }
}

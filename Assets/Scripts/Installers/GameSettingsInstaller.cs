using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "RageInvaders/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<AssetRefs>().FromInstance(FindObjectOfType<AssetManager>().assetRefs).AsSingle();
    }
}

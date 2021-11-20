using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    AssetRefs assetRefs = null;

    public override void InstallBindings()
    {
        
    }
}
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AssetManager : MonoBehaviour
{
    public bool useDlcAssets = true;
    public AssetRefs assetRefs;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartLoadingAssets();
    }

    private void StartLoadingAssets()
    {
        assetRefs = new AssetRefs();

        Addressables.LoadAssetAsync<GameObject>("Player").Completed += handle =>
        {
            assetRefs.playerRef = handle.Result;
        };

        Addressables.LoadAssetAsync<GameObject>("ProjectilePlayer").Completed += handle =>
        {
            assetRefs.projectilePlayerRef = handle.Result;
        };

        Addressables.LoadAssetAsync<GameObject>("ProjectileEnemy").Completed += handle =>
        {
            assetRefs.projectileEnemyRef = handle.Result;
        };

        if (useDlcAssets)
        {
            Addressables.LoadAssetsAsync<GameObject>("enemies", obj => { assetRefs.enemiesRef.Add(obj); }).Completed += LoadSceneAsset;
        }
        else
        {
            Addressables.LoadAssetsAsync<GameObject>(new List<string>(){ "base", "enemies" }, obj => { assetRefs.enemiesRef.Add(obj); }, Addressables.MergeMode.Intersection).Completed += LoadSceneAsset;
        }        
    }

    private void LoadSceneAsset(AsyncOperationHandle<IList<GameObject>> obj)
    {
        Addressables.LoadSceneAsync("Game", LoadSceneMode.Additive, true).Completed += FinishLoadingAssets;
    }

    private void FinishLoadingAssets(AsyncOperationHandle<SceneInstance> obj)
    {
        SceneManager.UnloadSceneAsync(0);   //Initialization is the only scene in BuildSettings
    }
}

public class AssetRefs
{
    public GameObject playerRef;
    public List<GameObject> enemiesRef = new List<GameObject>();
    public GameObject projectilePlayerRef;
    public GameObject projectileEnemyRef;
}

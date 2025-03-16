using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.Data;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Factory
{
  public class CollectableFactory : ICollectableFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly IObjectPool _objectPool;
    private AllCollectablesConfig _configs;

    public CollectableFactory(IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      IObjectPool objectPool)
    {
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _objectPool = objectPool;
    }

    public async UniTask Warmup()
    {
      _configs = _staticDataProvider.GetConfig<AllCollectablesConfig>();
      foreach (var config in _configs.Configs) 
        await _assetProvider.LoadAssetAsync(config.Prefab);
    }

    public Collectable CreateCollectable(Collectable prefab, Vector3 at, Quaternion rotation) => 
      _objectPool.GetGameObject(prefab, at, rotation);
  }

  public interface ICollectableFactory : IWarmupable
  {
    Collectable CreateCollectable(Collectable prefab, Vector3 at, Quaternion rotation);
  }
}
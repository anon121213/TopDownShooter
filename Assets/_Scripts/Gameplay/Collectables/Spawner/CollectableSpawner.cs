using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.Data;
using _Scripts.Gameplay.Collectables.Factory;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Spawner
{
  public class CollectableSpawner : ICollectableSpawner, IDisposable
  {
    private readonly Dictionary<Collectable, Collectable> _collectables = new();
    private readonly ICollectableFactory _collectableFactory;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly IObjectPool _objectPool;
    private AllCollectablesConfig _configs;

    public CollectableSpawner(ICollectableFactory collectableFactory,
      IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      IObjectPool objectPool)
    {
      _collectableFactory = collectableFactory;
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _objectPool = objectPool;
    }

    public void Initialize() => 
      _configs = _staticDataProvider.GetConfig<AllCollectablesConfig>();

    public async UniTask<Collectable> SpawnCollectable(CollectableType type, Vector3 at, Quaternion rotation)
    {
      var config = _configs.GetConfig(type);
      Collectable prefab = await _assetProvider.LoadAssetAsync<Collectable>(config.Prefab);
      Collectable collectable = _collectableFactory.CreateCollectable(prefab, at, rotation);
      _collectables.Add(collectable, prefab);
      collectable.Construct(config.Points);
      collectable.OnCollect += ReturnToPool;
      return collectable;
    }

    private void ReturnToPool(Collectable collectable)
    {
      collectable.OnCollect -= ReturnToPool;
      _objectPool.ReturnGameObject(collectable.gameObject, _collectables[collectable]);
      _collectables.Remove(collectable);
    }
    
    public void Dispose()
    {
      foreach (var projectile in _collectables) 
        projectile.Key.OnCollect -= ReturnToPool;
    }
  }
}
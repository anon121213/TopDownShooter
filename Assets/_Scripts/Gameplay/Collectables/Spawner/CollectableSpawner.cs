using System;
using System.Collections.Generic;
using System.Threading;
using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.Data;
using _Scripts.Gameplay.Collectables.Factory;
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
    private readonly IObjectPool _objectPool;
    private readonly AllCollectablesConfig _configs;

    public CollectableSpawner(ICollectableFactory collectableFactory,
      IStaticDataProvider staticDataProvider,
      IObjectPool objectPool)
    {
      _collectableFactory = collectableFactory;
      _objectPool = objectPool;
      _configs = staticDataProvider.GetConfig<AllCollectablesConfig>();
    }

    public UniTask Warmup(CancellationToken ct)
    {
      foreach (var config in _configs.Configs) 
        _objectPool.Warmup(config.Prefab.gameObject);
      
      return UniTask.CompletedTask;
    }

    public Collectable SpawnCollectable(CollectableType type, Vector3 at, Quaternion rotation, CancellationToken ct)
    {
      var config = _configs.GetConfig(type);
      Collectable collectable = _collectableFactory.CreateCollectable(config.Prefab, at, rotation);
      _collectables.Add(collectable, config.Prefab);
      collectable.Construct(config.Points);
      collectable.OnCollect += ReturnToPool;
      return collectable;
    }

    private void ReturnToPool(Collectable collectable)
    {
      collectable.OnCollect -= ReturnToPool;
      _objectPool.ReturnGameObject(collectable.gameObject, _collectables[collectable].gameObject);
      _collectables.Remove(collectable);
    }

    public void Dispose()
    {
      foreach (var projectile in _collectables) 
        projectile.Key.OnCollect -= ReturnToPool;
    }
  }
}
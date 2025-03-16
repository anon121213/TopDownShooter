using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Projectiles.Spawner
{
  public class ProjectileSpawner : IProjectileSpawner, IDisposable
  {
    private readonly Dictionary<Projectile, Projectile> _projectiles = new();
    private readonly IObjectPool _objectPool;
    private readonly IProjectileFactory _projectileFactory;
    private readonly IAssetProvider _assetProvider;

    public ProjectileSpawner(IObjectPool objectPool,
      IProjectileFactory projectileFactory,
      IAssetProvider assetProvider)
    {
      _objectPool = objectPool;
      _projectileFactory = projectileFactory;
      _assetProvider = assetProvider;
    }

    public async UniTask<Projectile> CreateProjectile(AssetReferenceGameObject reference, Vector3 at,
      Quaternion direction, ProjectileConfig config)
    {
      Projectile prefab = await _assetProvider.LoadAssetAsync<Projectile>(reference);
      Projectile projectile = _projectileFactory.CreateProjectile(prefab, at, direction);
      _projectiles.Add(projectile, prefab);
      projectile.Construct(config);
      projectile.Initialize();
      projectile.OnCollide += ReturnToPool;
      return projectile;
    }

    private void ReturnToPool(Projectile projectile)
    {
      projectile.OnCollide -= ReturnToPool;
      _objectPool.ReturnGameObject(projectile.gameObject, _projectiles[projectile]);
      _projectiles.Remove(projectile);
    }

    public void Dispose()
    {
      foreach (var projectile in _projectiles) 
        projectile.Key.OnCollide -= ReturnToPool;
    }
  }
}
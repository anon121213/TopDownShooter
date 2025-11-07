using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Infrastructure.Services.Pool;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Spawner
{
  public class ProjectileSpawner : IProjectileSpawner, IDisposable
  {
    private readonly Dictionary<Projectile, Projectile> _projectiles = new();
    private readonly IObjectPool _objectPool;
    private readonly IProjectileFactory _projectileFactory;

    public ProjectileSpawner(IObjectPool objectPool,
      IProjectileFactory projectileFactory)
    {
      _objectPool = objectPool;
      _projectileFactory = projectileFactory;
    }

    public Projectile CreateProjectile(Projectile prefab, Vector3 at,
      Quaternion direction, ProjectileConfig config)
    {
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
      _objectPool.ReturnGameObject(projectile.gameObject, _projectiles[projectile].gameObject);
      _projectiles.Remove(projectile);
    }

    public void Dispose()
    {
      foreach (var projectile in _projectiles) 
        projectile.Key.OnCollide -= ReturnToPool;
    }
  }
}
using _Scripts.Infrastructure.Services.Pool;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Factory
{
  public class ProjectileFactory : IProjectileFactory
  {
    private readonly IObjectPool _objectPool;

    public ProjectileFactory(IObjectPool objectPool) => 
      _objectPool = objectPool;

    public Projectile CreateProjectile(Projectile prefab, Vector3 at, Quaternion direction) => 
      _objectPool.GetGameObject(prefab, at, direction);
  }
}
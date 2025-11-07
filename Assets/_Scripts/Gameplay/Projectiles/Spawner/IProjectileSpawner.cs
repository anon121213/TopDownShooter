using _Scripts.Gameplay.Projectiles.Data;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Spawner
{
  public interface IProjectileSpawner
  {
    Projectile CreateProjectile(Projectile prefab, Vector3 at,
      Quaternion direction, ProjectileConfig config);
  }
}
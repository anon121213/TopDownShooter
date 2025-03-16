using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Factory
{
  public interface IProjectileFactory
  {
    Projectile CreateProjectile(Projectile prefab, Vector3 at, Quaternion direction);
  }
}
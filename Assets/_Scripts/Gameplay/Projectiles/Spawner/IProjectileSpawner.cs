using _Scripts.Gameplay.Projectiles.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Projectiles.Spawner
{
  public interface IProjectileSpawner
  {
    UniTask<Projectile> CreateProjectile(AssetReferenceGameObject reference, Vector3 at,
      Quaternion direction, ProjectileConfig config);
  }
}
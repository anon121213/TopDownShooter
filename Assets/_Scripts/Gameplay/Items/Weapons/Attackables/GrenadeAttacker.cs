using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Gameplay.Projectiles.Spawner;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Items.Weapons.Attackables
{
  public class GrenadeAttacker : IAttackable
  {
    private readonly IProjectileSpawner _projectileSpawner;
    private readonly AssetReferenceGameObject _projectile;
    private readonly Transform _owner;
    private readonly GrenadeProjectileConfig _grenadeProjectileConfig;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly Collider[] _results = new Collider[30];

    public GrenadeAttacker(IProjectileSpawner projectileSpawner,
      AssetReferenceGameObject projectile,
      Transform owner,
      GrenadeProjectileConfig grenadeProjectileConfig,
      IPlayerBackpack playerBackpack,
      IPlayerAttacker playerAttacker)
    {
      _projectileSpawner = projectileSpawner;
      _projectile = projectile;
      _owner = owner;
      _grenadeProjectileConfig = grenadeProjectileConfig;
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
    }

    public async UniTask Attack()
    {
      if (_playerAttacker.CurrentWeapon.Value.ItemData.Count.Value <= 0)
        return;
      
      int count = Physics.OverlapSphereNonAlloc(_owner.position, 30, _results);
      if (count == 0)
        return;

      Transform closestTarget = null;
      float minDistance = float.MaxValue;

      for (int i = 0; i < count; i++)
      {
        if (_results[i] == null || _results[i].transform == _owner) continue;

        if (_results[i].TryGetComponent(out IDamageable damageable))
        {
          float distance = Vector3.SqrMagnitude(_results[i].transform.position - _owner.position);
          if (distance < minDistance)
          {
            minDistance = distance;
            closestTarget = _results[i].transform;
          }
        }
      }

      if (closestTarget == null)
        return;

      Vector3 targetPosition = closestTarget.position;
      Vector3 direction = (targetPosition - _owner.position).normalized;

      if (direction == Vector3.zero)
        direction = _owner.forward;

      Quaternion rotation = Quaternion.LookRotation(direction);

      float spawnOffset = 1.5f;
      Vector3 spawnPosition = _owner.position + direction * spawnOffset;

      await _projectileSpawner.CreateProjectile(_projectile, spawnPosition, rotation, 
        _grenadeProjectileConfig);
      
      _playerBackpack.RemoveItem(_playerAttacker.CurrentWeapon.Value.ItemData.Type, 1);
    }
  }
}
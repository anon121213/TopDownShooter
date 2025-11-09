using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Projectiles.Spawner;
using _Scripts.Infrastructure.Scopes.NetCore;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Attackables
{
  public class GrenadeAttacker : IAttackable
  {
    private readonly IProjectileSpawner _projectileSpawner;
    private readonly Transform _owner;
    private readonly WeaponData _weaponData;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IReadOnlyNetworkRoomModel _roomModel;
    private readonly Collider[] _results = new Collider[30];
    
    private readonly float _findTargetRadius;
    private readonly float _spawnProjectileOffset;

    public GrenadeAttacker(IProjectileSpawner projectileSpawner,
      Transform owner,
      WeaponData weaponData,
      IPlayerBackpack playerBackpack,
      IPlayerAttacker playerAttacker,
      IReadOnlyNetworkRoomModel roomModel)
    {
      _projectileSpawner = projectileSpawner;
      _owner = owner;
      _weaponData = weaponData;
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
      _roomModel = roomModel;
      _findTargetRadius = _weaponData.FindTargetRadius;
      _spawnProjectileOffset = _weaponData.SpawnProjectileOffset;
    }

    public void Attack()
    {
      if (_playerAttacker.CurrentWeapon.Value.ItemData.Count.Value <= 0)
        return;
      
      int count = Physics.OverlapSphereNonAlloc(_owner.position, _findTargetRadius, _results);
      if (count == 0)
        return;

      Transform closestTarget = null;
      float minDistance = float.MaxValue;

      for (int i = 0; i < count; i++)
      {
        if (_results[i] == null || _results[i].transform == _owner) continue;

        if (!_results[i].TryGetComponent(out IDamageable damageable))
          continue;
        
        if (damageable.IsDead.Value)
          continue;
        
        float distance = Vector3.SqrMagnitude(_results[i].transform.position - _owner.position);
          
        if (!(distance < minDistance)) 
          continue;
          
        minDistance = distance;
        closestTarget = _results[i].transform;
      }

      if (closestTarget == null)
        return;

      var targetPosition = closestTarget.position;
      var direction = (targetPosition - _owner.position).normalized;

      if (direction == Vector3.zero)
        direction = _owner.forward;

      var rotation = Quaternion.LookRotation(direction);
      var spawnPosition = _owner.position + direction * _spawnProjectileOffset;

      _projectileSpawner.SpawnProjectile(_weaponData.ProjectileType, spawnPosition, rotation);
      _playerBackpack.RemoveItem(_playerAttacker.CurrentWeapon.Value.ItemData.Type, 1);
    }
  }
}
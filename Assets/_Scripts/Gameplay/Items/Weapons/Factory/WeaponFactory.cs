using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Items.Weapons.Attackables;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Projectiles.Spawner;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public class WeaponFactory : IWeaponFactory
  {
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IProjectileSpawner _projectileSpawner;
    private readonly IReadOnlyNetworkRoomModel _roomModel;
    private readonly AllWeaponsConfig _weaponsConfigs;

    public WeaponFactory(IStaticDataProvider staticDataProvider,
      IPlayerBackpack playerBackpack,
      IPlayerAttacker playerAttacker,
      IProjectileSpawner projectileSpawner,
      IReadOnlyNetworkRoomModel roomModel)
    {
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
      _projectileSpawner = projectileSpawner;
      _roomModel = roomModel;
      _weaponsConfigs = staticDataProvider.GetConfig<AllWeaponsConfig>();
    }

    public IWeapon CreateWeapon(ItemType itemType, Transform owner)
    {
      if (!_weaponsConfigs.TryGetWeaponData(itemType, out WeaponData weaponData))
        return null;
      
      return new Weapon(weaponData, GetAttacker(weaponData.AttackerType, owner, weaponData));
    }

    private IAttackable GetAttacker(AttackerType attackerType,
      Transform owner,
      WeaponData weaponData)
    {
      return attackerType switch
      {
        AttackerType.Shooter => new ShootAttacker(_projectileSpawner, owner, weaponData, _roomModel),
        AttackerType.Grenade => new GrenadeAttacker(_projectileSpawner, owner, weaponData, _playerBackpack, _playerAttacker, _roomModel),
        _ => throw new Exception($"Attacker of type {attackerType} does not exist")
      };
    }
  }
}
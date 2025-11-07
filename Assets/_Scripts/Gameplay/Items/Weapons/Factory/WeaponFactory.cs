using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Items.Weapons.Attackables;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Gameplay.Projectiles.Spawner;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public class WeaponFactory : IWeaponFactory
  {
    private readonly IProjectileSpawner _projectileSpawner;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly AllWeaponsConfig _weaponsConfigs;

    public WeaponFactory(IStaticDataProvider staticDataProvider,
      IProjectileSpawner projectileSpawner,
      IPlayerBackpack playerBackpack,
      IPlayerAttacker playerAttacker)
    {
      _projectileSpawner = projectileSpawner;
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
      _weaponsConfigs = staticDataProvider.GetConfig<AllWeaponsConfig>();
    }

    public IWeapon CreateWeapon(ItemType itemType, Transform owner)
    {
      var config = _weaponsConfigs.GetWeaponConfig(itemType);
      return new Weapon(config, GetAttacker(config.AttackerType, owner, config));
    }

    private IAttackable GetAttacker(AttackerType attackerType,
      Transform owner,
      WeaponConfig weaponConfig)
    {
      switch (attackerType)
      {
        case AttackerType.Shooter:
          return new ShootAttacker(_projectileSpawner, weaponConfig.ProjectileConfig.Prefab, owner,
            (ShootProjectileConfig)weaponConfig.ProjectileConfig, weaponConfig.FindTargetRadius,
            weaponConfig.SpawnProjectileOffset);

        case AttackerType.Grenade:
          return new GrenadeAttacker(_projectileSpawner, weaponConfig.ProjectileConfig.Prefab, owner,
            (GrenadeProjectileConfig)weaponConfig.ProjectileConfig, _playerBackpack,
            _playerAttacker, weaponConfig.FindTargetRadius, weaponConfig.SpawnProjectileOffset);
      }

      throw new Exception($"Attacker of type {attackerType} does not exist");
    }
  }
}
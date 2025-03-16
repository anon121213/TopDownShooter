using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Items.Weapons.Attackables;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public class WeaponFactory : IWeaponFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private AllWeaponsConfig _weaponsConfigs;

    public WeaponFactory(IStaticDataProvider staticDataProvider)
    {
      _staticDataProvider = staticDataProvider;
    }

    public async UniTask Warmup()
    {
      _weaponsConfigs = _staticDataProvider.GetConfig<AllWeaponsConfig>();
      await UniTask.CompletedTask;
    }

    public IWeapon CreateWeapon(ItemType itemType)
    {
      var config = _weaponsConfigs.GetWeaponConfig(itemType);
      return new Weapon(config, GetAttacker(config.AttackerType));
    }

    private IAttackable GetAttacker(AttackerType attackerType)
    {
      switch (attackerType)
      {
        case AttackerType.Shooter:
          return new ShootAttacker();
        
        case AttackerType.Thrower:
          return new ThrowAttacker();
      }

      throw new Exception($"Attacker of type {attackerType} does not exist");
    }
  }
}
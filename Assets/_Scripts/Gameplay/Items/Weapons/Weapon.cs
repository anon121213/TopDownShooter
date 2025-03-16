using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Items.Weapons.Attackables;
using UniRx;

namespace _Scripts.Gameplay.Items.Weapons
{
  public class Weapon : IWeapon
  {
    private readonly IAttackable _attacker;
    public ItemData ItemData { get; }
    public float Damage { get; }

    private bool _isReloading;
    private IDisposable _disposable;

    public Weapon(WeaponConfig weaponConfig,
      IAttackable attacker)
    {
      _attacker = attacker;
      ItemData = weaponConfig.ItemData;
      Damage = weaponConfig.Damage;
    }

    public bool TryAttack()
    {
      if (_isReloading)
        return false;
      
      _attacker.Attack();
      Reload();
      return true;
    }

    private void Reload()
    {
      _isReloading = true;
      _disposable = Observable.Timer(TimeSpan.FromSeconds(ItemData.ReloadDelay))
        .Subscribe(_ => _isReloading = false);
    }

    public void Dispose() => 
      _disposable.Dispose();
  }

  public interface IWeapon : IItem, IDisposable
  {
    float Damage { get; }
    bool TryAttack();
  }
}
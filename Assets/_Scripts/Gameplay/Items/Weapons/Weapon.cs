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
    public WeaponData WeaponData { get; }
    public ItemData ItemData => WeaponData.ItemData;

    private bool _isReloading;
    private IDisposable _disposable;

    public Weapon(WeaponData weaponData,
      IAttackable attacker)
    {
      _attacker = attacker;
      WeaponData = weaponData;
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
      _disposable?.Dispose();
      
      _disposable = Observable.Timer(TimeSpan.FromSeconds(WeaponData.ReloadDelay))
        .Subscribe(_ => _isReloading = false);
    }

    public void Dispose() => 
      _disposable.Dispose();
  }

  public interface IWeapon : IItem, IDisposable
  {
    WeaponData WeaponData { get; }
    bool TryAttack();
  }
}
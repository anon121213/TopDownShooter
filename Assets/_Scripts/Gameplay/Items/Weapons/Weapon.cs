using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Data;
using _Scripts.Gameplay.Items.Weapons.Attackables;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons
{
  public class Weapon : IWeapon
  {
    private readonly IAttackable _attacker;
    public ItemData ItemData { get; }

    private bool _isReloading;
    private IDisposable _disposable;

    public Weapon(WeaponConfig weaponConfig,
      IAttackable attacker)
    {
      _attacker = attacker;
      ItemData = weaponConfig.ItemData;
    }

    public async UniTask<bool> TryAttack()
    {
      if (_isReloading)
        return false;
      
      await _attacker.Attack();
      Reload();
      return true;
    }

    private void Reload()
    {
      _isReloading = true;
      _disposable?.Dispose();
      
      _disposable = Observable.Timer(TimeSpan.FromSeconds(ItemData.ReloadDelay))
        .Subscribe(_ => _isReloading = false);
    }


    public void Dispose() => 
      _disposable.Dispose();
  }

  public interface IWeapon : IItem, IDisposable
  {
    UniTask<bool> TryAttack();
  }
}
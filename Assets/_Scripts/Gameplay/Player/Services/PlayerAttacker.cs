using System;
using _Scripts.Gameplay.Items.Weapons;
using _Scripts.Gameplay.Player.Services.Base;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerAttacker : PlayerService, IPlayerAttacker
  {
    private readonly ReactiveProperty<IWeapon> _currentWeapon = new();
    public IReadOnlyReactiveProperty<IWeapon> CurrentWeapon => _currentWeapon;

    public event Action OnAttack;

    public void SwitchWeapon(IWeapon weapon)
    {
      if (_currentWeapon.Value != weapon) 
        _currentWeapon.Value = weapon;
    }

    public bool TryAttack()
    {
      if (!IsEnable)
        return false;

      if (!_currentWeapon.Value.TryAttack()) 
        return false;

      OnAttack?.Invoke();
      return true;
    }
  }
}
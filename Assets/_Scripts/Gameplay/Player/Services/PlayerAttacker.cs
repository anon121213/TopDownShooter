using System;
using _Scripts.Gameplay.Items.Weapons;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerAttacker : IPlayerAttacker
  {
    private readonly ReactiveProperty<IWeapon> _currentWeapon = new();
    public IReadOnlyReactiveProperty<IWeapon> CurrentWeapon => _currentWeapon;

    public event Action OnAttack;

    private bool _isCanAttack;

    public void Enable() => 
      _isCanAttack = true;

    public void SwitchWeapon(IWeapon weapon)
    {
      if (_currentWeapon.Value != weapon) 
        _currentWeapon.Value = weapon;
    }

    public async UniTask<bool> TryAttack()
    {
      if (!_isCanAttack)
        return false;

      if (!await _currentWeapon.Value.TryAttack()) 
        return false;

      OnAttack?.Invoke();
      return true;
    }


    public void Disable() => 
      _isCanAttack = false;
  }
}
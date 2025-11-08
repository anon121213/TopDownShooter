using System;
using _Scripts.Gameplay.Items.Weapons;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public interface IPlayerAttacker
  {
    IReadOnlyReactiveProperty<IWeapon> CurrentWeapon { get; }
    event Action OnAttack;
    void SwitchWeapon(IWeapon weapon);
    bool TryAttack();
  }
}
using System;
using _Scripts.Gameplay.Items.Weapons;
using _Scripts.Infrastructure.Services.Player;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public interface IPlayerAttacker : IPlayerService
  {
    IReadOnlyReactiveProperty<IWeapon> CurrentWeapon { get; }
    event Action OnAttack;
    void SwitchWeapon(IWeapon weapon);
    bool TryAttack();
  }
}
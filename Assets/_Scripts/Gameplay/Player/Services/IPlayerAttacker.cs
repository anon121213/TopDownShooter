using System;
using _Scripts.Gameplay.Items.Weapons;
using _Scripts.Gameplay.Player.Services.Base;
using Cysharp.Threading.Tasks;
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
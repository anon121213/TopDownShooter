using System;
using _Scripts.Gameplay.Player.Services.Base;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerAttackController : PlayerService, IPlayerAttackController
  {
    private readonly IPlayerAttacker _playerAttacker;

    public PlayerAttackController(IPlayerAttacker playerAttacker)
    {
      _playerAttacker = playerAttacker;
    }

    public override void OnInitialize()
    {
      Observable.Interval(TimeSpan.FromSeconds(
          _playerAttacker.CurrentWeapon.Value.WeaponData.ReloadDelay))
        .Subscribe(_ => _playerAttacker.TryAttack()).AddTo(Disposables);
    }
  }

  public interface IPlayerAttackController
  {
  }
}
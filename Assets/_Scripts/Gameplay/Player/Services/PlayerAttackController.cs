using System;
using _Scripts.Gameplay.Player.Services.Base;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerAttackController : PlayerService, IPlayerAttackController
  {
    private readonly IPlayerMover _playerMover;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly SerialDisposable _attackDisposable = new();

    public PlayerAttackController(IPlayerMover playerMover,
      IPlayerAttacker playerAttacker)
    {
      _playerMover = playerMover;
      _playerAttacker = playerAttacker;
    }
    
    public override void OnUpdate() => 
      Attack(_playerMover.IsMoving.Value);

    private void Attack(bool isMoving)
    {
      if (_playerAttacker.CurrentWeapon.Value == null)
        return;
      
      if (isMoving)
      {
        _attackDisposable?.Dispose();
        return;
      }

      _attackDisposable.Disposable = Observable.Interval(TimeSpan.FromSeconds(
          _playerAttacker.CurrentWeapon.Value.ItemData.ReloadDelay))
        .Subscribe(_ => _playerAttacker.TryAttack()); 
    }

    public override void OnDispose() => 
      _attackDisposable?.Dispose();
  }

  public interface IPlayerAttackController
  {
  }
}
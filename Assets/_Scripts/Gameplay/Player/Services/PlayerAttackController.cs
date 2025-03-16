using System;
using _Scripts.Infrastructure.Services.Player;
using UniRx;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerAttackController : IPlayerAttackController
  {
    private readonly IPlayerMover _playerMover;
    private readonly IPlayerAttacker _playerAttacker;
    private IDisposable _moveDisposable;
    private IDisposable _attackDisposable;

    public PlayerAttackController(IPlayerMover playerMover,
      IPlayerAttacker playerAttacker)
    {
      _playerMover = playerMover;
      _playerAttacker = playerAttacker;
    }

    public void Enable() => 
      _moveDisposable = _playerMover.IsMoving.Subscribe(Attack);

    private void Attack(bool isMoving)
    {
      if (_playerAttacker.CurrentWeapon.Value == null)
        return;
      
      if (isMoving)
      {
        _attackDisposable?.Dispose();
        return;
      }

      _attackDisposable = Observable.Interval(TimeSpan.FromSeconds(
          _playerAttacker.CurrentWeapon.Value.ItemData.ReloadDelay))
        .Subscribe(async _ => await _playerAttacker.TryAttack()); 
    }
    
    public void Disable()
    {
      _attackDisposable?.Dispose();
      _moveDisposable?.Dispose();
    }
  }

  public interface IPlayerAttackController : IPlayerService
  {
  }
}
using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Input;
using _Scripts.Infrastructure.Services.Player;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerMover : IPlayerMover, IUpdatable
  {
    private readonly IInputService _inputService;
    private readonly IStaticDataProvider _staticDataProvider;
    private CharacterController _characterController;
    private float _speed;
    
    private readonly ReactiveProperty<bool> _isMoving = new();
    public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

    public PlayerMover(IInputService inputService,
      IStaticDataProvider staticDataProvider)
    {
      _inputService = inputService;
      _staticDataProvider = staticDataProvider;
    }

    public void Construct(CharacterController characterController)
    {
      _characterController = characterController;
      _speed = _staticDataProvider.GetConfig<PlayerConfig>().MoveSpeed;
    }

    public void Enable()
    {
      _inputService.OnStartMove += StartMove;
      _inputService.OnStopMove += StopMove;
    }

    public void OnUpdate() => 
      Move();

    private void Move()
    {
      if (!IsMoving.Value)
        return;

      Vector3 direction = new Vector3(_inputService.MoveDirection.x, 0, _inputService.MoveDirection.y);
      _characterController.Move(direction * _speed);
    }

    private void StopMove() =>
      _isMoving.Value = false;

    private void StartMove() =>
      _isMoving.Value = true;

    public void Disable()
    {
      _inputService.OnStartMove -= StartMove;
      _inputService.OnStopMove -= StopMove;
    }
  }

  public interface IPlayerMover : IPlayerService
  {
    void Construct(CharacterController characterController);
    IReadOnlyReactiveProperty<bool> IsMoving { get; }
  }
}
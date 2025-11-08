using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Input;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerMover : PlayerService, IPlayerMover
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

    public override void OnInitialize()
    {
      _characterController = NetworkPlayerView.CharacterController;
      _speed = _staticDataProvider.GetConfig<PlayerConfig>().MoveSpeed;
    }


    public override void OnEnable()
    {
      base.OnEnable();
      _inputService.OnStartMove += StartMove;
      _inputService.OnStopMove += StopMove;
    }

    public override void OnUpdate() => 
      Move();

    private void Move()
    {
      if (!IsMoving.Value)
        return;

      var direction = new Vector3(_inputService.MoveDirection.x, 0, _inputService.MoveDirection.y);
      _characterController.Move(direction * _speed);
    }

    private void StopMove() =>
      _isMoving.Value = false;

    private void StartMove() => 
      _isMoving.Value = true;

    public override void OnDisable()
    {
      _inputService.OnStartMove -= StartMove;
      _inputService.OnStopMove -= StopMove;
      StopMove();
    }
  }

  public interface IPlayerMover
  {
    IReadOnlyReactiveProperty<bool> IsMoving { get; }
  }
}
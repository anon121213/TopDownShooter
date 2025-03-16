using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.health;
using _Scripts.Infrastructure.Services.Player;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.Player
{
  public class Player : MonoBehaviour, IEnemyTarget
  {
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    private IPlayerServices _playerServices;

    [Inject]
    public void Construct(IPlayerServices playerServices)
    {
      _playerServices = playerServices;
    }

    public void Initialize()
    {
      Health.OnHealthOver += Die;
      _playerServices.InitializeServices();
      _playerServices.EnableServices();
    }

    private void Die()
    {
      _playerServices.DisableServices();
      Debug.LogError("YouDied");
    }

    private void OnDisable()
    {
      Health.OnHealthOver += Die;
    }
  }
}
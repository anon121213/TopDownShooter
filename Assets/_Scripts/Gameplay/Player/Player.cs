using System;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Player.Services;
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
    private IPlayerCollecter _collecter;

    [Inject]
    public void Construct(IPlayerServices playerServices,
      IPlayerCollecter collecter)
    {
      _playerServices = playerServices;
      _collecter = collecter;
    }

    public void Initialize()
    {
      Health.OnHealthOver += Die;
      _playerServices.InitializeServices();
      _playerServices.EnableServices();
    }

    private void OnTriggerEnter(Collider other) => 
      _collecter.OnCollide(other);

    private void Die()
    {
      _playerServices.DisableServices();
      Debug.LogError("YouDied");
    }

    private void OnDisable() => 
      Health.OnHealthOver += Die;
  }
}
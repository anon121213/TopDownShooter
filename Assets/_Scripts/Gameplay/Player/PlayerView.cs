using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.health;
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class PlayerView : MonoBehaviour, IEnemyTarget
  {
    [field: SerializeField] public PlayerScope PlayerScope { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Collider PlayerCollider { get; private set; }
  }
}
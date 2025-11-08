using _Scripts.Gameplay.health;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class PlayerRootView : MonoBehaviour, IDamageable
  {
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Collider PlayerCollider { get; private set; }
    [field: SerializeField] public PlayerModel PlayerModel { get; private set; }
    
    public IReadOnlyReactiveProperty<int> ActorNumber => PlayerModel.ActorNumber;
  }
}
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class LocalPlayerView : MonoBehaviour
  {
    [field: SerializeField] public Collider PlayerCollider { get; private set; }
  }
}
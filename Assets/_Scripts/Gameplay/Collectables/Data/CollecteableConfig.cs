using _Scripts.Gameplay.Collectables.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Data
{
  public class CollectableConfig : ScriptableObject
  {
    [field: SerializeField] public Collectable Prefab { get; private set; }
    [field: SerializeField] public int Points { get; private set; }
    [field: SerializeField] public CollectableType Type { get; private set; }
  }
}
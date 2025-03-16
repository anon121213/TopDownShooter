using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Collectables.Data
{
  public class CollectableConfig : ScriptableObject
  {
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public int Points { get; private set; }
    [field: SerializeField] public CollectableType Type { get; private set; }
  }
}
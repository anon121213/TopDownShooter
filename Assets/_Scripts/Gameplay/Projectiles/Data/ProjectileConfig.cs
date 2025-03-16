using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Projectiles.Data
{
  public abstract class ProjectileConfig : ScriptableObject
  {
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
  }
}
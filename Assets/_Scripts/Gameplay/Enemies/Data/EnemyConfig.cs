using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Enemies.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Enemies/EnemyConfig", fileName = "EnemyConfig")]
  public class EnemyConfig : ScriptableObject
  {
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public float CheckTargetRadius { get; private set; }
    [field: SerializeField] public float WaitPatrolTime { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float AttackRadius { get; private set; }
    [field: SerializeField] public float AttackDelay { get; private set; }
    [field: SerializeField] public float StartHealth { get; private set; }
  }
}
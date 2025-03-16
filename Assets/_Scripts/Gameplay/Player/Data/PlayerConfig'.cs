using _Scripts.Gameplay.health.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.Player.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Player/PlayerConfig", fileName = "PlayerConfig", order = 0)]
  public class PlayerConfig : ScriptableObject
  {
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public PlayerHealthConfig HealthConfig { get; private set; }
    [field: SerializeField] public Vector3 SpawnPosition { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
  }
}
using UnityEngine;

namespace _Scripts.Gameplay.health.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Health/HealthConfig", fileName = "HealthConfig")]
  public class HealthConfig : ScriptableObject
  {
    [field: SerializeField] public float InitHealth { get; private set; }
  }
}
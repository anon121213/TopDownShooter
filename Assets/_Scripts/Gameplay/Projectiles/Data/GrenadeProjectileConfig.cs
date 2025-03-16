using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Projectiles/GrenadeProjectileConfig", fileName = "GrenadeProjectileConfig")]
  public class GrenadeProjectileConfig : ProjectileConfig
  {
    [field: SerializeField] public float Radius { get; private set; }
    [field: SerializeField] public float Delay { get; private set; }
  }
}
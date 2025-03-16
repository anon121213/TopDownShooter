using _Scripts.Gameplay.Projectiles.Data;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Items/WeaponConfig", fileName = "WeaponConfig")]
  public class WeaponConfig : ItemConfig
  {
    [field: SerializeField] public AttackerType AttackerType { get; private set; }
    [field: SerializeField] public ProjectileConfig ProjectileConfig { get; private set; }
  }

  public enum AttackerType
  {
    Unknown = 0,
    Shooter = 1,
    Grenade = 2
  }
}
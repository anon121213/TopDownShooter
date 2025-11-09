using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Projectiles.Data;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Items/AllWeaponsConfig", fileName = "AllWeaponsConfig")]
  public class AllWeaponsConfig : ScriptableObject
  {
    [SerializeField] private List<WeaponData> _weapons;

    public bool TryGetWeaponData(ItemType type, out WeaponData weaponData )
    {
      weaponData = _weapons.FirstOrDefault(x => x.ItemData.Type == type);

      if (weaponData != null)
        return true;
      
      Debug.LogError($"Weapon {type} does not exist");
      return false;
    }
  }

  [Serializable]
  public class WeaponData
  {
    [field: SerializeField] public ItemData ItemData { get; private set; }
    [field: SerializeField] public AttackerType AttackerType { get; private set; }
    [field: SerializeField] public ProjectileTypeEnum ProjectileType { get; private set; }
    [field: SerializeField] public float ReloadDelay { get; private set; }
    [field: SerializeField] public float FindTargetRadius { get; private set; }
    [field: SerializeField] public float SpawnProjectileOffset { get; private set; }
  }
  
  public enum AttackerType
  {
    None = 0,
    Shooter = 1,
    Grenade = 2
  }
}
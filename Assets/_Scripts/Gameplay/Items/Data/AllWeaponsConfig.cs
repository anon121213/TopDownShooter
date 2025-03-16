using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Items.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Items/AllWeaponsConfig", fileName = "AllWeaponsConfig")]
  public class AllWeaponsConfig : ScriptableObject
  {
    [SerializeField] private List<WeaponConfig> _weapons;

    public WeaponConfig GetWeaponConfig(ItemType type) => 
      _weapons.FirstOrDefault(x => x.ItemData.Type == type);
  }
}
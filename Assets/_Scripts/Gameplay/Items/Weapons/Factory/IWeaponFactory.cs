using _Scripts.Gameplay.Items.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public interface IWeaponFactory
  {
    IWeapon CreateWeapon(ItemType itemType, Transform owner);
  }
}
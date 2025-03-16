using _Scripts.Gameplay.Items.Base;
using _Scripts.Infrastructure.Services.Warmup;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public interface IWeaponFactory : IWarmupable
  {
    IWeapon CreateWeapon(ItemType itemType, Transform owner);
  }
}
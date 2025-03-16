using _Scripts.Gameplay.Items.Base;
using _Scripts.Infrastructure.Services.Warmup;

namespace _Scripts.Gameplay.Items.Weapons.Factory
{
  public interface IWeaponFactory : IWarmupable
  {
    IWeapon CreateWeapon(ItemType itemType);
  }
}
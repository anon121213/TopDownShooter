using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Attackables
{
  public class ThrowAttacker : IAttackable
  {
    public void Attack()
    {
      Debug.Log("throw a granade");
    }
  }
}
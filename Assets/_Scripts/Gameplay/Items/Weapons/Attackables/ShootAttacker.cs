using UnityEngine;

namespace _Scripts.Gameplay.Items.Weapons.Attackables
{
  public class ShootAttacker : IAttackable
  {
    public void Attack()
    {
      Debug.Log("shootAttack");
    }
  }
}
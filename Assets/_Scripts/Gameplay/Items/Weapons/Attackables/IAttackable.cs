using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Items.Weapons.Attackables
{
  public interface IAttackable
  {
    UniTask Attack();
  }
}
using UniRx;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IChasingEnemy : INetworkTargetableEnemy, IMoveableEnemy
  {
    IReadOnlyReactiveProperty<bool> IsChasing { get; }
    void SetChasing(bool isChasing);
  }
}
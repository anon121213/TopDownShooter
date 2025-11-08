using UniRx;

namespace _Scripts.Gameplay.health
{
  public interface IDamageable
  {
    IReadOnlyReactiveProperty<int> ActorNumber { get; }
  }
}
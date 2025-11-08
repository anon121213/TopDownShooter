using _Scripts.Gameplay.Enemies.Services;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface ITargetableEnemy
  {
    IReadOnlyReactiveCollection<Transform> Targets { get; }
    IReadOnlyReactiveProperty<Transform> CurrentTarget { get; }
    IEnemyTargetSetter TargetSetter { get; }
    float StoppingDistance { get; }
    void TryAddTarget(Transform target);
    void TryRemoveTarget(Transform target);
    void ResetTargets();
    void SetCurrentTarget(Transform target);
  }
}
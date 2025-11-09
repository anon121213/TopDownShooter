using _Scripts.Gameplay.Enemies.Services;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface INetworkTargetableEnemy
  {
    IReadOnlyReactiveDictionary<int, TargetData> Targets { get; }
    IReadOnlyReactiveProperty<TargetData> CurrentTarget { get; }
    IEnemyTargetSetter TargetSetter { get; }
    float StoppingDistance { get; }
    void TryAddTarget(TargetData data);
    void TryRemoveTarget(int actorNumber);
    void ResetTargets();
    void SetCurrentTarget(int actorNumber, Transform target);
  }

  public readonly struct TargetData
  {
    public readonly int ActorNumber;
    public readonly Transform TargetRoot;

    public TargetData(int actorNumber, Transform targetRoot)
    {
      ActorNumber = actorNumber;
      TargetRoot = targetRoot;
    }
  }
}
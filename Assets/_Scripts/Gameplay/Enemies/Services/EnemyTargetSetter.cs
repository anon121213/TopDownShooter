using _Scripts.Gameplay.Enemies.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Services
{
  public class EnemyTargetSetter : IEnemyTargetSetter {
    private readonly INetworkTargetableEnemy _networkTargetableEnemy;
    private readonly Enemy _enemy;
        
    public EnemyTargetSetter(Enemy enemy, INetworkTargetableEnemy networkTargetableEnemy) {
      _enemy = enemy;
      _networkTargetableEnemy = networkTargetableEnemy;
    }
        
    public void TrySetTarget() {
      foreach (var target in _networkTargetableEnemy.Targets) {
        if (!_networkTargetableEnemy.CurrentTarget.Value.TargetRoot)
          _networkTargetableEnemy.SetCurrentTarget(target.Key, target.Value.TargetRoot);

        var currentDistance = Vector3.Distance(_networkTargetableEnemy.CurrentTarget.Value.TargetRoot.position, _enemy.transform.position);
        var newDistance = Vector3.Distance(target.Value.TargetRoot.position, _enemy.transform.position);

        if (currentDistance > newDistance)
          _networkTargetableEnemy.SetCurrentTarget(target.Key, target.Value.TargetRoot);
      }
    }
  }
  public interface IEnemyTargetSetter
  {
    void TrySetTarget();
  }
}
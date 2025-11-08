using _Scripts.Gameplay.Enemies.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Services
{
  public class EnemyTargetSetter : IEnemyTargetSetter {
    private readonly ITargetableEnemy _targetableEnemy;
    private readonly Enemy _enemy;
        
    public EnemyTargetSetter(Enemy enemy, ITargetableEnemy targetableEnemy) {
      _enemy = enemy;
      _targetableEnemy = targetableEnemy;
    }
        
    public void TrySetTarget() {
      foreach (var target in _targetableEnemy.Targets) {
        if (!_targetableEnemy.CurrentTarget.Value)
          _targetableEnemy.SetCurrentTarget(target);

        if (target == _targetableEnemy.CurrentTarget.Value)
          continue;
                
        var currentDistance = Vector3.Distance(_targetableEnemy.CurrentTarget.Value.position, _enemy.transform.position);
        var newDistance = Vector3.Distance(target.position, _enemy.transform.position);

        if (currentDistance > newDistance)
          _targetableEnemy.SetCurrentTarget(target);
      }
    }
  }
  public interface IEnemyTargetSetter
  {
    void TrySetTarget();
  }
}
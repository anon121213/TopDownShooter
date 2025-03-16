using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class CheckTargetInRange : BehaviorNode
  {
    private readonly IPatrolEnemy _patrolEnemy;
    private readonly Collider[] _results = new Collider[30];

    public CheckTargetInRange(IPatrolEnemy patrolEnemy) =>
      _patrolEnemy = patrolEnemy;

    public override NodeStatus Execute(Enemy enemy)
    {
      int count = Physics.OverlapSphereNonAlloc(enemy.transform.position, _patrolEnemy.CheckTargetRadius, _results);

      if (count > 0)
      {
        for (int i = 0; i < count; i++)
        {
          if (_results[i].TryGetComponent(out IEnemyTarget target))
          {
            _patrolEnemy.Target = _results[i].transform;
            return NodeStatus.Success;
          }
        }
      }

      return NodeStatus.Failure;
    }
  }
}
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class CheckTargetInRangeDelayNode : BehaviorNode
  {
    private readonly IPatrolEnemy _patrolEnemy;
    private float _currentDelay;

    public CheckTargetInRangeDelayNode(IPatrolEnemy patrolEnemy) =>
      _patrolEnemy = patrolEnemy;

    public override NodeStatus Execute(Enemy enemy)
    {
      if (_currentDelay > 0)
      {
        _currentDelay -= Time.deltaTime;
        return NodeStatus.Failure;
      }

      _currentDelay = _patrolEnemy.CheckTargetDelay;
      return NodeStatus.Success;
    }
  }
}
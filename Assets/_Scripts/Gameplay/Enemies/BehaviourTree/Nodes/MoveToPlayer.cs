using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class MoveToPlayer : BehaviorNode
  {
    private readonly IMoveToPlayerEnemy _moveableEnemy;

    public MoveToPlayer(IMoveToPlayerEnemy moveableEnemy)
    {
      _moveableEnemy = moveableEnemy;
    }
    
    public override NodeStatus Execute(Enemy enemy)
    {
      if (_moveableEnemy.Target == null)
        return NodeStatus.Failure;

      float distanceToTarget = Vector3.Distance(enemy.transform.position, _moveableEnemy.Target.position);

      if (distanceToTarget > _moveableEnemy.CheckTargetRadius)
      {
        _moveableEnemy.Target = null;
        return NodeStatus.Failure;
      }
      _moveableEnemy.Mover.Move(_moveableEnemy.Target.position);
      return distanceToTarget < 1f ? NodeStatus.Success : NodeStatus.Running;
    }
  }
}
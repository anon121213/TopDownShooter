using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class MoveToWaypoint : BehaviorNode
  {
    private readonly IPointMoveableEnemy _wayPointEnemy;

    public MoveToWaypoint(IPointMoveableEnemy wayPointEnemy)
    {
      _wayPointEnemy = wayPointEnemy;
    }
    
    public override NodeStatus Execute(Enemy enemy)
    {
      if (_wayPointEnemy.PatrolPoints.Count == 0) 
        return NodeStatus.Failure;

      _wayPointEnemy.Mover.Move(_wayPointEnemy.PatrolPoints[_wayPointEnemy.CurrentPatrolIndex].position);
      
      if (Vector3.Distance(enemy.transform.position, 
            _wayPointEnemy.PatrolPoints[_wayPointEnemy.CurrentPatrolIndex].position) 
          < 1f + enemy.NavMeshAgent.radius)
      { 
        _wayPointEnemy.CurrentPatrolIndex = (_wayPointEnemy.CurrentPatrolIndex + 1) % _wayPointEnemy.PatrolPoints.Count;
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
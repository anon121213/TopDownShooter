using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class WaitAtWaypoint : BehaviorNode
  {
    private readonly IPointMoveableEnemy _pointMoveableEnemy;
    private float _elapsedTime;
    private bool _isWaiting;

    public WaitAtWaypoint(IPointMoveableEnemy pointMoveableEnemy) => 
      _pointMoveableEnemy = pointMoveableEnemy;

    public override NodeStatus Execute(Enemy enemy)
    {
      if (!_isWaiting)
      {
        _isWaiting = true;
        _elapsedTime = 0f;
        return NodeStatus.Running;
      }

      _elapsedTime += Time.deltaTime;
      
      if (_elapsedTime >= _pointMoveableEnemy.WaitTime)
      {
        _isWaiting = false;
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class AttackDelayNode : BehaviorNode
  {
    private readonly IAttackeableEnemy _attackeableEnemy;
    private float _elapsedTime;
    private bool _isWaiting;

    public AttackDelayNode(IAttackeableEnemy attackeableEnemy) => 
      _attackeableEnemy = attackeableEnemy;

    public override NodeStatus Execute(Enemy enemy)
    {
      if (!_isWaiting)
      {
        _isWaiting = true;
        _elapsedTime = 0f;
        return NodeStatus.Running;
      }

      _elapsedTime += Time.deltaTime;
      
      if (_elapsedTime >= _attackeableEnemy.AttackDelay)
      {
        _isWaiting = false;
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
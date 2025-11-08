using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class AttackDelayNode : BehaviorNode {
    private readonly IAttackableEnemy _attackableEnemy;
    private float _elapsedTime;
    private bool _isWaiting;

    public AttackDelayNode(IAttackableEnemy attackableEnemy) =>
      _attackableEnemy = attackableEnemy;

    public override NodeStatus Execute(Enemy enemy) {
      if (!_isWaiting) {
        _isWaiting = true;
        _elapsedTime = 0f;
        return NodeStatus.Running;
      }

      _elapsedTime += Time.deltaTime;

      if (!(_elapsedTime >= _attackableEnemy.AttackDelay))
        return NodeStatus.Running;

      _isWaiting = false;
      return NodeStatus.Success;
    }
  }
}
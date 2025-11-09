using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class AttackNode : BehaviorNode {
    private readonly IAttackableEnemy _attackableEnemy;

    public AttackNode(IAttackableEnemy attackableEnemy) =>
      _attackableEnemy = attackableEnemy;

    public override NodeStatus Execute(Enemy enemy) {
      _attackableEnemy.TargetSetter.TrySetTarget();
            
      if (!_attackableEnemy.CurrentTarget.Value.TargetRoot) {
        _attackableEnemy.SetAttacking(false);
        return NodeStatus.Failure;
      }
            
      var distance = Vector3.Distance(_attackableEnemy.CurrentTarget.Value.TargetRoot.position, enemy.transform.position);
      if (distance > enemy.Config.AttackRadius) {
        _attackableEnemy.SetAttacking(false);
        return NodeStatus.Failure;
      }

      if (_attackableEnemy.Attacker.TryAttack(_attackableEnemy.CurrentTarget.Value.ActorNumber, _attackableEnemy.CurrentTarget.Value.TargetRoot)) {
        _attackableEnemy.SetAttacking(true);
        return NodeStatus.Success;
      }

      _attackableEnemy.SetAttacking(false);
      return NodeStatus.Failure;
    }
  }
}
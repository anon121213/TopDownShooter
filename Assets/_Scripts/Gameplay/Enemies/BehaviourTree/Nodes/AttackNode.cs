using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using _Scripts.Gameplay.health;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class AttackNode : BehaviorNode
  {
    private readonly IAttackeableEnemy _attackeableEnemy;
    private readonly Collider[] _results = new Collider[30];

    public AttackNode(IAttackeableEnemy attackeableEnemy) =>
      _attackeableEnemy = attackeableEnemy;

    public override NodeStatus Execute(Enemy enemy)
    {
      int count = Physics.OverlapSphereNonAlloc(enemy.transform.position, _attackeableEnemy.AttackRadius, _results);

      if (count > 0)
      {
        for (int i = 0; i < count; i++)
          if (_results[i].TryGetComponent(out IDamageable damageable) && _results[i].transform != enemy.transform)
          {
            damageable.TakeDamage(_attackeableEnemy.Damage);
            return NodeStatus.Success;
          }
      }

      for (int i = 0; i < count; i++)
        _results[i] = null;
      return NodeStatus.Failure;
    }
  }
}
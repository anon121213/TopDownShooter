using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes
{
  public class MoveToTargetNode : BehaviorNode {
    private readonly IChasingEnemy _moveableEnemy;
    private readonly SerialDisposable _serialDisposable = new SerialDisposable();

    public MoveToTargetNode(IChasingEnemy moveableEnemy) {
      _moveableEnemy = moveableEnemy;
    }

    public override NodeStatus Execute(Enemy enemy) {
      _moveableEnemy.TargetSetter.TrySetTarget();
            
      if (!_moveableEnemy.CurrentTarget.Value.TargetRoot) {
        _moveableEnemy.Mover.Stop();
        return NodeStatus.Failure;
      }
            
      var distanceToTarget = Vector3.Distance(enemy.transform.position, _moveableEnemy.CurrentTarget.Value.TargetRoot.position);

      if (distanceToTarget <= _moveableEnemy.StoppingDistance) {
        _moveableEnemy.Mover.Stop();
        return NodeStatus.Success;
      }

      _moveableEnemy.Mover.Move(_moveableEnemy.CurrentTarget.Value.TargetRoot.position);
      return NodeStatus.Running;
    }

    public override void OnDisable() {
      _moveableEnemy.Mover.Stop();
      _serialDisposable.Disposable = null;
    }
        
    public override void OnEnable() => 
      _serialDisposable.Disposable = _moveableEnemy.Mover.IsMoving.Subscribe(_moveableEnemy.SetChasing);

    public override void OnDispose() => 
      _serialDisposable.Dispose();
  }
}
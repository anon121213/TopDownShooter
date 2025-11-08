using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Base;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base
{
  public class SelectorNode : BehaviorNode {
    private readonly List<BehaviorNode> _children = new();

    public void AddChild(BehaviorNode node) =>
      _children.Add(node);

    public override NodeStatus Execute(Enemy enemy) {
      foreach (var child in _children) {
        var status = child.Execute(enemy);
        if (status is NodeStatus.Success or NodeStatus.Running)
          return status;
      }
      return NodeStatus.Failure;
    }

    public override void OnEnable() {
      foreach (var child in _children)
        child.OnEnable();
    }

    public override void OnDisable() {
      foreach (var child in _children)
        child.OnDisable();
    }

    public override void OnDispose() {
      foreach (var child in _children)
        child.OnDispose();
    }
  }
}
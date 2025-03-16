using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Base;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base
{
  public class SelectorNode : BehaviorNode
  {
    private readonly List<BehaviorNode> _children = new();

    public void AddChild(BehaviorNode node) =>
      _children.Add(node);

    public override NodeStatus Execute(Enemy enemy)
    {
      foreach (var child in _children)
      {
        NodeStatus status = child.Execute(enemy);
        if (status == NodeStatus.Success || status == NodeStatus.Running)
          return status;
      }
      return NodeStatus.Failure;
    }

    public override void Dispose()
    {
      foreach (var child in _children) 
        child.Dispose();
    }
  }
}
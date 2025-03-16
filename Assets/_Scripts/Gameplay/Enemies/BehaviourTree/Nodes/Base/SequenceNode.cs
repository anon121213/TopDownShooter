using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Base;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base
{
  public class SequenceNode : BehaviorNode
  {
    private readonly List<BehaviorNode> _children = new();
    private int _currentIndex;

    public void AddChild(BehaviorNode node) => 
      _children.Add(node);

    public override NodeStatus Execute(Enemy enemy)
    {
      while (_currentIndex < _children.Count)
      {
        NodeStatus status = _children[_currentIndex].Execute(enemy);

        if (status == NodeStatus.Failure)
        {
          _currentIndex = 0; 
          return NodeStatus.Failure;
        }

        if (status == NodeStatus.Running)
          return NodeStatus.Running;

        _currentIndex++;
      }

      _currentIndex = 0; 
      return NodeStatus.Success;
    }

    public override void Dispose()
    {
      foreach (var child in _children) 
        child.Dispose();
    }
  }
}
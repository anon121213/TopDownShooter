using _Scripts.Gameplay.Enemies.Base;

namespace _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base
{
  public abstract class BehaviorNode
  {
    public abstract NodeStatus Execute(Enemy enemy);

    public virtual void Dispose() { }
  }

  public enum NodeStatus
  {
    Success,
    Failure,
    Running
  }
}
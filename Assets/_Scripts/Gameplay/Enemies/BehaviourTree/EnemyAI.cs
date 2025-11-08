using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;

namespace _Scripts.Gameplay.Enemies.BehaviourTree
{
  public class EnemyAI
  {
    private readonly BehaviorNode _rootNode;
    private readonly Enemy _enemy;
    private bool _isEnabled;

    public EnemyAI(Enemy enemy, BehaviorNode rootNode)
    {
      _rootNode = rootNode;
      _enemy = enemy;
    }
    
    public void SetEnable(bool value) {
      _isEnabled = value;

      if (value)
        _rootNode.OnEnable();
      else
        _rootNode.OnDisable();
    }

    public void Execute()
    {
      if (!_isEnabled)
        return;
      
      _rootNode.Execute(_enemy);
    }

    public void Dispose() => 
      _rootNode.OnDispose();
  }
}
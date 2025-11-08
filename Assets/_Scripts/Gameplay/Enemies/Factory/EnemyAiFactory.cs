using _Scripts.Gameplay.Enemies.BehaviourTree;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;

namespace _Scripts.Gameplay.Enemies.Factory
{
  public class EnemyAiFactory : IEnemyAiFactory
  {
    public EnemyAI CreateSimpleEnemyAI(SimpleEnemy enemy)
    {
      var root = new SequenceNode();

      var attackSequence = new SequenceNode();
      attackSequence.AddChild(new AttackNode(enemy));
      attackSequence.AddChild(new AttackDelayNode(enemy));

      var chaseSequence = new SequenceNode();
      chaseSequence.AddChild(new MoveToTargetNode(enemy));

      root.AddChild(chaseSequence);
      root.AddChild(attackSequence);

      var enemyAI = new EnemyAI(enemy, root);
      return enemyAI;
    }
  }

  public interface IEnemyAiFactory
  {
    EnemyAI CreateSimpleEnemyAI(SimpleEnemy enemy);
  }
}
using System;
using System.Threading;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.BehaviourTree;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Services;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Enemies.Factory
{
  public class EnemyFactory : IEnemyFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IObjectResolver _objectResolver;
    private readonly IObjectPool _objectPool;

    private EnemyConfig _simpleEnemyConfig;
    
    public EnemyFactory(IStaticDataProvider staticDataProvider,
      IObjectResolver objectResolver,
      IObjectPool objectPool)
    {
      _staticDataProvider = staticDataProvider;
      _objectResolver = objectResolver;
      _objectPool = objectPool;
    }

    public UniTask Warmup(CancellationToken ct)
    {
      // TODO MAKE ENEMIES POOL
      _simpleEnemyConfig = _staticDataProvider.GetConfig<EnemyConfig>();
      return UniTask.CompletedTask;
    }

    public SimpleEnemy CreateSimpleEnemy(Vector3 at, Quaternion look)
    {
      SimpleEnemy enemy = (SimpleEnemy)_objectResolver.Instantiate(_simpleEnemyConfig.Prefab, at, look);
      IEnemyMover enemyMover = new EnemyMover(enemy.NavMeshAgent, _simpleEnemyConfig);
      
      enemy.Construct(_simpleEnemyConfig, enemyMover);
      enemy.SetAI(CreateSimpleEnemyAI(enemy));
      enemy.EnableEnemy();
      return enemy;
    }

    public void ReturnEnemyToPool(Enemy enemy, Enemy prefab) => 
      _objectPool.ReturnGameObject(enemy, prefab);

    private EnemyAI CreateSimpleEnemyAI(SimpleEnemy enemy)
    {
      var root = new SelectorNode();
    
      var attackSequence = new SequenceNode();
      attackSequence.AddChild(new AttackNode(enemy));
      attackSequence.AddChild(new AttackDelayNode(enemy));

      var chaseSequence = new SequenceNode();
      chaseSequence.AddChild(new CheckTargetInRangeDelayNode(enemy));
      chaseSequence.AddChild(new CheckTargetInRange(enemy));
      chaseSequence.AddChild(new MoveToPlayer(enemy));

      var patrolSequence = new SequenceNode();
      patrolSequence.AddChild(new MoveToWaypoint(enemy));
      patrolSequence.AddChild(new WaitAtWaypoint(enemy));

      root.AddChild(attackSequence);
      root.AddChild(chaseSequence);  
      root.AddChild(patrolSequence); 

      EnemyAI enemyAI = new EnemyAI(enemy, root);
      enemyAI.SetEnable(true);

      return enemyAI;
    }
  }

  public interface IEnemyFactory : IWarmupable
  {
    SimpleEnemy CreateSimpleEnemy(Vector3 at, Quaternion look);
    void ReturnEnemyToPool(Enemy enemy, Enemy prefab);
  }
}
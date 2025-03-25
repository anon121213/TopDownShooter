using _Scripts.Gameplay.Enemies.BehaviourTree;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes;
using _Scripts.Gameplay.Enemies.BehaviourTree.Nodes.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Services;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
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
    private readonly IAssetProvider _assetProvider;
    private readonly IObjectResolver _objectResolver;
    private EnemyConfig _simpleEnemyConfig;

    public EnemyFactory(IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      IObjectResolver objectResolver)
    {
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _objectResolver = objectResolver;
    }

    public async UniTask Warmup()
    {
      _simpleEnemyConfig = _staticDataProvider.GetConfig<EnemyConfig>();
      await _assetProvider.LoadAssetAsync(_simpleEnemyConfig.Prefab);
    }

    public async UniTask<SimpleEnemy> CreateSimpleEnemy(Vector3 at, Quaternion look)
    {
      SimpleEnemy prefab = await _assetProvider.LoadAssetAsync<SimpleEnemy>(_simpleEnemyConfig.Prefab);
      SimpleEnemy enemy = _objectResolver.Instantiate(prefab, at, look);
      IEnemyMover enemyMover = new EnemyMover(enemy.NavMeshAgent, _simpleEnemyConfig);
      
      enemy.Construct(_simpleEnemyConfig, enemyMover);
      enemy.SetAI(CreateSimpleEnemyAI(enemy));
      enemy.EnableEnemy();
      return enemy;
    }

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
    UniTask<SimpleEnemy> CreateSimpleEnemy(Vector3 at, Quaternion look);
  }
}
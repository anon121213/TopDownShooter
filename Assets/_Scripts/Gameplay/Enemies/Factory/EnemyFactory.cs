using System.Threading;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Services;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Network;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;
using VRShooter.Scopes;

namespace _Scripts.Gameplay.Enemies.Factory
{
  public class EnemyFactory : IEnemyFactory, IInitializable, IWarmupable
  {
    private readonly IObjectPool _pool;
    private readonly IEnemyAiFactory _enemyAiFactory;
    private readonly INetworkDamageService _damageService;
    private readonly IStaticDataProvider _staticDataProvider;

    private EnemiesConfig _enemiesConfig;
    
    public EnemyFactory(IObjectPool objectPool, IEnemyAiFactory enemyAiFactory, 
      INetworkDamageService damageService, IStaticDataProvider staticDataProvider)
    {
      _pool = objectPool;
      _enemyAiFactory = enemyAiFactory;
      _damageService = damageService;
      _staticDataProvider = staticDataProvider;
    }

    public void Initialize()
    {
      _enemiesConfig = _staticDataProvider.GetConfig<EnemiesConfig>();
    }

    public UniTask Warmup(CancellationToken ct)
    {
      foreach (var data in _enemiesConfig.EnemyData) 
        _pool.Warmup(data.Prefab.gameObject, Vector3.zero); 
      
      return UniTask.CompletedTask;
    }

    public Enemy CreateEnemyByType(MobType enemyType, Vector3 at, Quaternion look, Transform parent = null)
    {
      if (!_enemiesConfig.TryGetConfigByType(enemyType, out var config))
        throw new InvalidKeyException($"Cannot find enemy config by type {enemyType}");

      var enemyObj = _pool.GetGameObject(config.Prefab, at, look, parent);
      return InitEnemyAiByType(enemyObj, config);
    }

    private Enemy InitEnemyAiByType(Enemy enemy, EnemyData config)
    {
      switch (config.BehaviourType)
      {
        case MobBehaviourTypes.None:
          return enemy;

        case MobBehaviourTypes.PlayerChase:
          if (!enemy.TryGetComponent(out SimpleEnemy simpleEnemy))
          {
            Debug.LogError($"Enemy prefab {config.MobType} has no component SimpleEnemy");
            return enemy.GetComponent<Enemy>();
          }

          var enemyMover = new EnemyMover(simpleEnemy, config);
          var enemyAttacker = new EnemyAttacker(simpleEnemy, _damageService);
          var enemyTargetSetter = new EnemyTargetSetter(simpleEnemy, simpleEnemy);

          simpleEnemy.Construct(config, enemyMover, enemyAttacker, enemyTargetSetter);
          simpleEnemy.SetAI(_enemyAiFactory.CreateSimpleEnemyAI(simpleEnemy));
          simpleEnemy.OnGetFromPool();
          return simpleEnemy;
        default:
          Debug.LogError($"Enemy by type {config.BehaviourType} does not exist");
          return null;
      }
    }
  }

  public interface IEnemyFactory
  {
    Enemy CreateEnemyByType(MobType enemyType, Vector3 at, Quaternion look, Transform parent = null);
  }
}
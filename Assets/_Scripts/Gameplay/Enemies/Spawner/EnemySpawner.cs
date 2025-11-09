using System;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Network;
using _Scripts.Infrastructure.Services.Pool;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public class EnemySpawner : IEnemySpawner, IInitializable, IDisposable
  {
    private readonly IObjectPool _pool;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly INetworkRoomModel _roomModel;
    private readonly CompositeDisposable _disposables = new();

    private EnemiesConfig _enemiesConfig;
    
    public EnemySpawner(IObjectPool pool, IStaticDataProvider staticDataProvider, 
      INetworkRoomModel roomModel)
    {
      _pool = pool;
      _staticDataProvider = staticDataProvider;
      _roomModel = roomModel;
    }

    public void Initialize() => 
      _enemiesConfig = _staticDataProvider.GetConfig<EnemiesConfig>();

    public void SpawnEnemyByType(MobType enemyType, Vector3 spawnPoint, float startHealth)
    {
      if (!_roomModel.IsServer.Value)
        return;

      if (!_enemiesConfig.TryGetConfigByType(enemyType, out var config))
      {
        Debug.LogError($"Enemy {enemyType} not present in config");
        return;
      }

      int actorNumber = ActorNumberAllocator.GetMobActorNumber(_roomModel);
      _roomModel.AddDtoMob(new MobModelDataDTO(actorNumber, enemyType, config.BehaviourType, spawnPoint, startHealth));
    }

    public void ReturnEnemy(Enemy enemy, MobType enemyType, float delay = 0, Action onReturn = null)
    {
      if (!enemy || !_enemiesConfig.TryGetConfigByType(enemyType, out var enemyConfig))
        return;

      if (delay > 0)
      {
        Observable.Timer(TimeSpan.FromSeconds(delay))
          .Subscribe(_ => ReturnToPool(enemy, enemyConfig.Prefab.gameObject, onReturn))
          .AddTo(_disposables);
        return;
      }

      ReturnToPool(enemy, enemyConfig.Prefab.gameObject, onReturn);
    }

    private void ReturnToPool(Enemy enemy, GameObject prefab, Action onReturn = null)
    {
      enemy.DisableEnemy();
      enemy.OnReturnToPool();
      onReturn?.Invoke();
      _pool.ReturnGameObject(enemy.gameObject, prefab);
    }

    public void Dispose() => 
      _disposables.Dispose();
  }

  public interface IEnemySpawner
  {
    void SpawnEnemyByType(MobType enemyType, Vector3 spawnPoint, float startHealth);
    void ReturnEnemy(Enemy enemy, MobType enemyType, float delay = 0, Action onReturn = null);
  }
}
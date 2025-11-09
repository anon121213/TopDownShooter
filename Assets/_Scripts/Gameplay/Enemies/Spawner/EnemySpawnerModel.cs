using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public class EnemySpawnerModel : IEnemySpawnerModel, IInitializable, ITickable, IDisposable
  {
    private readonly IEnemySpawner _enemySpawner;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly ArenaSceneView _arenaSceneView;
    
    private readonly List<SpawnEntry> _spawnEntries = new();
    private readonly CompositeDisposable _disposables = new();
    
    private EnemiesConfig _enemiesConfig;

    public EnemySpawnerModel(IEnemySpawner enemySpawner, IStaticDataProvider staticDataProvider,
      INetworkRoomModel networkRoomModel, ArenaSceneView arenaSceneView)
    {
      _enemySpawner = enemySpawner;
      _staticDataProvider = staticDataProvider;
      _networkRoomModel = networkRoomModel;
      _arenaSceneView = arenaSceneView;
    }

    public void Initialize()
    {
      _enemiesConfig = _staticDataProvider.GetConfig<EnemiesConfig>();

      if (_networkRoomModel.IsServer.Value) 
        _networkRoomModel.SetIsMobSpawnStarted(false);
    
      DespawnAllEnemies();

      _networkRoomModel.IsMobSpawnStarted
        .DistinctUntilChanged()
        .Subscribe(isStarted =>
        {
          if (isStarted) StartSpawn();
          else StopSpawn();
        })
        .AddTo(_disposables);
    }

    public void Tick()
    {
      if (!_networkRoomModel.IsMobSpawnStarted.Value
          || !_networkRoomModel.IsServer.Value)
        return;

      var deltaTime = Time.deltaTime;

      foreach (var entry in _spawnEntries)
      {
        entry.Timer -= deltaTime;

        if (entry.Timer > 0f)
          continue;

        if (_enemiesConfig.TryGetConfigByType(entry.Config.MobType, out var config)) 
          _enemySpawner.SpawnEnemyByType(config.MobType, entry.Spawner.transform.position, config.StartHealth);

        entry.Timer = entry.Delay;
      }
    }

    public void StartSpawnEnemies()
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
      
      _networkRoomModel.SetIsMobSpawnStarted(true);
    }

    public void StopSpawnEnemies()
    {
      if (!_networkRoomModel.IsMobSpawnStarted.Value || !_networkRoomModel.IsServer.Value)
        return;

      _spawnEntries.Clear();
      _networkRoomModel.SetIsMobSpawnStarted(false);
    }

    public void DespawnAllEnemies()
    {
      if (!_networkRoomModel.IsServer.Value)
        return;

      foreach (var mob in _networkRoomModel.Mobs.ToList())
      {
        _networkRoomModel.RemoveMob(mob.Value.ActorNumber.Value);      
        _networkRoomModel.RemoveDtoMob(mob.Value.ActorNumber.Value);
      }
    }

    private void StartSpawn()
    {
      _spawnEntries.Clear();

      foreach (var spawner in _arenaSceneView.EnemySpawnersViews)
      {
        foreach (var data in spawner.EnemiesData)
        {
          if (!_enemiesConfig.TryGetConfigByType(data.MobType, out var config))
            return;
          
          _spawnEntries.Add(new SpawnEntry
          {
            Spawner = spawner,
            Config = config,
            Delay = data.SpawnDelay,
            Timer = data.FirstSpawnDelay
          });
        }
      }
    }

    private void StopSpawn() => 
      _spawnEntries.Clear();

    public void Dispose() =>
      _disposables.Dispose();

    private class SpawnEntry {
      public EnemySpawnerView Spawner;
      public EnemyData Config;
      public float Timer;
      public float Delay;
    }
  }

  public interface IEnemySpawnerModel
  {
    void StartSpawnEnemies();
    void StopSpawnEnemies();
    void DespawnAllEnemies();
  }
}
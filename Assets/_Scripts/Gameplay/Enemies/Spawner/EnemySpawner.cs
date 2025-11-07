using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Infrastructure.Scopes;
using UnityEngine;
namespace _Scripts.Gameplay.Enemies.Spawner
{
  public class EnemySpawner : IEnemySpawner
  {
    private readonly List<Enemy> _enemies = new();
    
    private readonly IEnemyFactory _enemyFactory;
    private readonly ArenaSceneView _arenaSceneView;

    public EnemySpawner(IEnemyFactory enemyFactory, ArenaSceneView arenaSceneView)
    {
      _enemyFactory = enemyFactory;
      _arenaSceneView = arenaSceneView;
    }
    
    public List<SimpleEnemy> CreateSimpleEnemiesOnSpawnPoints()
    {
      var enemies = new List<SimpleEnemy>();
      
      foreach (var data in _arenaSceneView.EnemySpawnData)
      {
        if (data.spawnPoint == null)
          continue;

        SimpleEnemy enemy = _enemyFactory.CreateSimpleEnemy(data.spawnPoint.position, Quaternion.identity);
        
        enemy.SetPatrolPoints(data.patrolPoints);
        enemy.Initialize();
        _enemies.Add(enemy);
        enemies.Add(enemy);
      }
      
      return enemies;
    }
  }

  [Serializable]
  public class EnemySpawnData
  {
    public Transform spawnPoint;
    public List<Transform> patrolPoints;
  }
}
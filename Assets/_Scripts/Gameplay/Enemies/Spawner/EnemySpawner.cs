using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public class EnemySpawner : MonoBehaviour, IEnemySpawner
  {
    [SerializeField] private List<EnemySpawnData> _enemiesData = new();
    private readonly List<SimpleEnemy> _enemies = new();
    private IEnemyFactory _enemyFactory;
        
    [Inject]
    private void Construct(IEnemyFactory enemyFactory) => 
      _enemyFactory = enemyFactory;

    public async UniTask<List<SimpleEnemy>> CreateSimpleEnemiesOnSpawnPoints()
    {
      foreach (var data in _enemiesData)
      {
        if (data.spawnPoint == null)
          continue;

        SimpleEnemy enemy = await _enemyFactory.CreateSimpleEnemy(data.spawnPoint.position, Quaternion.identity);
                
        enemy.SetPatrolPoints(data.patrolPoints);
        enemy.Initialize();
        _enemies.Add(enemy);
      }
      return _enemies;
    }
  }

  [Serializable]
  public class EnemySpawnData
  {
    public Transform spawnPoint;
    public List<Transform> patrolPoints;
  }
}
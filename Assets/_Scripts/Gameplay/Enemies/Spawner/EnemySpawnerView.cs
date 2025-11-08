using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public class EnemySpawnerView : MonoBehaviour
  {
    [SerializeField] private List<EnemySpawnData> _enemiesData = new();
    public IReadOnlyList<EnemySpawnData> EnemiesData => _enemiesData;
  }
  
  [Serializable]
  public struct EnemySpawnData
  {
    public MobType MobType;
    public float SpawnDelay;
    public float FirstSpawnDelay;
  }
}
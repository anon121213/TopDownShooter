using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Data;
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
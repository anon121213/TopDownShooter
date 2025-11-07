using System.Collections.Generic;
using _Scripts.Gameplay.Collectables.UI;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.health.UI;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes
{
  public class ArenaSceneView : MonoBehaviour
  {
    [field: SerializeField] public HealthPresenter HealthPresenter { get; private set; }
    [field: SerializeField] public CoinsPresenter CoinsPresenter { get; private set; }
    
    [SerializeField] private List<EnemySpawnData> enemiesData = new();

    public IReadOnlyList<EnemySpawnData> EnemySpawnData => enemiesData;

  }
}
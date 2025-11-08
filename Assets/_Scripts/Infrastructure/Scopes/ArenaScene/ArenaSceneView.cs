using System.Collections.Generic;
using _Scripts.Gameplay.Collectables.UI;
using _Scripts.Gameplay.Enemies.Spawner;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.ArenaScene
{
  public class ArenaSceneView : MonoBehaviour
  {
    [field: SerializeField] public CoinsPresenter CoinsPresenter { get; private set; }
    
    [SerializeField] private List<EnemySpawnerView> _enemySpawnersViews = new();

    public IReadOnlyList<EnemySpawnerView> EnemySpawnersViews => _enemySpawnersViews;
  }
}
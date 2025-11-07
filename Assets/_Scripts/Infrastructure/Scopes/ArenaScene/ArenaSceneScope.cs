using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.health.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.ArenaScene
{
  public class ArenaSceneScope : LifetimeScope
  {
    [SerializeField] private ArenaSceneView _arenaSceneView;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterComponent(_arenaSceneView);
      //builder.RegisterComponent(_arenaSceneView.CoinsPresenter);
      builder.RegisterComponent<IHealthPresenter>(_arenaSceneView.HealthPresenter);
      builder.Register<IEnemySpawner, EnemySpawner>(Lifetime.Scoped);
      builder.RegisterEntryPoint<ArenaSceneService>();
    }
  }
}
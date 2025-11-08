using _Scripts.Gameplay.Enemies.Spawner;
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
      builder.Register<ArenaSceneModel>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<EnemySpawnerModel>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.RegisterEntryPoint<ArenaSceneEntryPoint>();
    }
  }
}
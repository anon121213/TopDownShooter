using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Gameplay.Projectiles.Spawner;
using _Scripts.Infrastructure.Services.Network;
using _Scripts.Infrastructure.Services.Scenes;
using _Scripts.Infrastructure.Services.Warmup;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.Game
{
  public class GameScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.Register<EnemyFactory>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<NetworkDamageService>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<ActorNumberAllocator>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<PlayerSpawner>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<EnemySpawner>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<ProjectileFactory>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<ProjectileSpawner>(Lifetime.Singleton).AsImplementedInterfaces();
      
      builder.Register<IWarmupService, WarmupService>(Lifetime.Singleton);
      builder.RegisterEntryPoint<GameService>();
    }
  }
}
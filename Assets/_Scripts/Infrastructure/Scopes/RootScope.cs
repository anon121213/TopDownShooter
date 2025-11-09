using _Scripts.Gameplay.Collectables.Factory;
using _Scripts.Gameplay.Collectables.Spawner;
using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Gameplay.Projectiles.Spawner;
using _Scripts.Infrastructure.Bootstrapper;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Input;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Warmup;
using FishNet.Managing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes
{
  public class RootScope : LifetimeScope
  {
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private NetworkRoomScope _networkRoomScope;

    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterComponent(_networkManager).AsSelf();
      
      // SERVICES
      builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
      builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton);
      builder.Register<IInputService, InputService>(Lifetime.Singleton);
      builder.Register<IObjectPool, ObjectPool>(Lifetime.Singleton);
      
      // FACTORIES
      builder.Register<IEnemyAiFactory, EnemyAiFactory>(Lifetime.Singleton);
      builder.Register<ICollectableFactory, CollectableFactory>(Lifetime.Singleton).As<IWarmupable>();
      
      // SPAWNERS 
      builder.Register<ICollectableSpawner, CollectableSpawner>(Lifetime.Singleton);
      
      builder.RegisterEntryPoint<MainBootstrapper>().WithParameter(_networkRoomScope);
    }
  }
}